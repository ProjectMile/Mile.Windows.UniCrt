using System.Runtime.InteropServices;
using System.Text;

namespace Mile.UniCrt.WrapperDefinitionGenerator
{
    public class WeakSymbolsObjectGenerator
    {
        private const short IMAGE_SYM_UNDEFINED = 0;
        private const short IMAGE_SYM_ABSOLUTE = -1;

        private const ushort IMAGE_SYM_TYPE_NULL = 0x0000;

        private const byte IMAGE_SYM_CLASS_STATIC = 0x03;
        private const byte IMAGE_SYM_CLASS_EXTERNAL = 0x02;
        private const byte IMAGE_SYM_CLASS_WEAK_EXTERNAL = 0x69;

        /// <summary>
        /// The symbol table in this section is inherited from the traditional
        /// COFF format. It is distinct from Microsoft Visual C++ debug
        /// information. A file can contain both a COFF symbol table and Visual
        /// C++ debug information, and the two are kept separate. Some Microsoft
        /// tools use the symbol table for limited but important purposes, such
        /// as communicating COMDAT information to the linker. Section names and
        /// file names, as well as code and data symbols, are listed in the
        /// symbol table.
        /// The location of the symbol table is indicated in the COFF header.
        /// The symbol table is an array of records, each 18 bytes long. Each
        /// record is either a standard or auxiliary symbol-table record. A
        /// standard record defines a symbol or name and has the following
        /// format.
        /// </summary>
        /// <see cref="https://learn.microsoft.com/en-us/windows/win32/debug/pe-format#coff-symbol-table"/>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct ImageSymbol
        {
            /// <summary>
            /// The name of the symbol, represented by a union of three
            /// structures. An array of 8 bytes is used if the name is not more
            /// than 8 bytes long.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            private byte[] RawName;

            /// <summary>
            /// An array of the characters. For the symbol name which length is
            /// not more than 8 bytes.
            /// </summary>
            public string ShortName
            {
                get
                {
                    return Encoding.UTF8.GetString(RawName).Split('\0')[0];
                }
                set
                {
                    byte[] RawValue = Encoding.UTF8.GetBytes(value);
                    if (RawValue.Length > 8)
                    {
                        throw new ArgumentException("Invalid short name.");
                    }
                    RawName = new byte[8];
                    Array.Copy(RawValue, RawName, RawValue.Length);
                }
            }

            /// <summary>
            /// An offset into the string table. For the symbol name which more
            /// than 8 bytes.
            /// </summary>
            public int LongName
            {
                get
                {
                    return BitConverter.ToInt32(RawName, 4);
                }
                set
                {
                    RawName = new byte[8];
                    BitConverter.GetBytes(value).CopyTo(RawName, 4);
                }
            }

            /// <summary>
            /// The value that is associated with the symbol. The interpretation
            /// of this field depends on SectionNumber and StorageClass. A
            /// typical meaning is the relocatable address.
            /// </summary>
            public uint Value;

            /// <summary>
            /// The signed integer that identifies the section, using a
            /// one-based index into the section table. 
            /// </summary>
            public short SectionNumber;

            /// <summary>
            /// A number that represents type. Microsoft tools set this field to
            /// 0x20 (function) or 0x0 (not a function). 
            /// </summary>
            public ushort Type;

            /// <summary>
            /// An enumerated value that represents storage class.
            /// </summary>
            public byte StorageClass;

            /// <summary>
            /// The number of auxiliary symbol table entries that follow this
            /// record.
            /// </summary>
            public byte NumberOfAuxSymbols;
        }

        /// <summary>
        /// The enumeration that represents the weak extern search type.
        /// </summary>
        private enum ImageWeakExternSearchType : uint
        {
            /// <summary>
            /// Indicates that no library search for sym1 should be performed.
            /// </summary>
            NoLibrary = 1,

            /// <summary>
            /// Indicates that a library search for sym1 should be performed.
            /// </summary>
            Library = 2,

            /// <summary>
            /// Indicates that sym1 is an alias for sym2.
            /// </summary>
            SearchAlias = 3,
        }

        /// <summary>
        /// "Weak externals" are a mechanism for object files that allows
        /// flexibility at link time. A module can contain an unresolved
        /// external symbol (sym1), but it can also include an auxiliary record
        /// that indicates that if sym1 is not present at link time, another
        /// external symbol (sym2) is used to resolve references instead.
        /// If a definition of sym1 is linked, then an external reference to
        /// the symbol is resolved normally. If a definition of sym1 is not
        /// linked, then all references to the weak external for sym1 refer
        /// to sym2 instead. The external symbol, sym2, must always be linked;
        /// typically, it is defined in the module that contains the weak
        /// reference to sym1.
        /// Weak externals are represented by a symbol table record with
        /// EXTERNAL storage class, UNDEF section number, and a value of zero.
        /// The weak-external symbol record is followed by an auxiliary record
        /// with the following format.
        /// </summary>
        /// <see cref="https://learn.microsoft.com/en-us/windows/win32/debug/pe-format#auxiliary-format-3-weak-externals"/>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct ImageAuxWeakSymbol
        {
            /// <summary>
            /// The symbol-table index of sym2, the symbol to be linked if sym1
            /// is not found.
            /// </summary>
            public int TagIndex;

            /// <summary>
            /// The weak extern search type.
            /// </summary>
            public ImageWeakExternSearchType Characteristics;

            /// <summary>
            /// Unused bytes.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
            public readonly byte[] Unused;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct WeakSymbolItem
        {
            public ImageSymbol TargetSymbol;
            public ImageSymbol AliasSymbol;
            public ImageAuxWeakSymbol AuxSymbol;
        }

        private static byte[] StructureToBytes(object Structure)
        {
            int Size = Marshal.SizeOf(Structure);
            byte[] Result = new byte[Size];
            IntPtr RawResult = Marshal.AllocHGlobal(Size);
            Marshal.StructureToPtr(Structure, RawResult, true);
            Marshal.Copy(RawResult, Result, 0, Size);
            Marshal.FreeHGlobal(RawResult);
            return Result;
        }

        public static void Test()
        {
            WeakSymbolItem Item = new WeakSymbolItem();
            Item.TargetSymbol = new ImageSymbol();
            Item.TargetSymbol.ShortName = "#_access";
            Item.TargetSymbol.Value = 0;
            Item.TargetSymbol.SectionNumber = IMAGE_SYM_UNDEFINED;
            Item.TargetSymbol.Type = IMAGE_SYM_TYPE_NULL;
            Item.TargetSymbol.StorageClass = IMAGE_SYM_CLASS_EXTERNAL;
            Item.TargetSymbol.NumberOfAuxSymbols = 0;
            Item.AliasSymbol = new ImageSymbol();
            Item.AliasSymbol.ShortName = "#access";
            Item.AliasSymbol.Value = 0;
            Item.AliasSymbol.SectionNumber = IMAGE_SYM_UNDEFINED;
            Item.AliasSymbol.Type = IMAGE_SYM_TYPE_NULL;
            Item.AliasSymbol.StorageClass = IMAGE_SYM_CLASS_WEAK_EXTERNAL;
            Item.AliasSymbol.NumberOfAuxSymbols = 1;
            Item.AuxSymbol = new ImageAuxWeakSymbol();
            Item.AuxSymbol.TagIndex = 2 + ((1 - 1) * 3);
            Item.AuxSymbol.Characteristics = ImageWeakExternSearchType.SearchAlias;
            byte[] Bytes = StructureToBytes(Item);

            File.WriteAllBytes("WeakSymbolItem.bin", Bytes);

            Console.ReadKey();
        }
    }
}
