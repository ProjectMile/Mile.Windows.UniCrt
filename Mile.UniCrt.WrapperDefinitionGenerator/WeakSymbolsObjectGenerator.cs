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

        private const ushort IMAGE_FILE_MACHINE_UNKNOWN = 0x0000;

        /// <summary>
        /// At the beginning of an object file, or immediately after the
        /// signature of an image file, is a standard COFF file header in the
        /// following format. Note that the Windows loader limits the number of
        /// sections to 96.
        /// </summary>
        /// <see cref="https://learn.microsoft.com/en-us/windows/win32/debug/pe-format#coff-file-header-object-and-image"/>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct ImageFileHeader
        {
            /// <summary>
            /// The number that identifies the type of target machine. For more
            /// information, see Machine Types.
            /// </summary>
            public ushort Machine;

            /// <summary>
            /// The number of sections. This indicates the size of the section
            /// table, which immediately follows the headers.
            /// </summary>
            public short NumberOfSections;

            /// <summary>
            /// The low 32 bits of the number of seconds since 00:00 January 1,
            /// 1970 (a C run-time time_t value), which indicates when the file
            /// was created.
            /// </summary>
            public int TimeDateStamp;

            /// <summary>
            /// The file offset of the COFF symbol table, or zero if no COFF
            /// symbol table is present. This value should be zero for an image
            /// because COFF debugging information is deprecated.
            /// </summary>
            public uint PointerToSymbolTable;

            /// <summary>
            /// The number of entries in the symbol table. This data can be used
            /// to locate the string table, which immediately follows the symbol
            /// table. This value should be zero for an image because COFF
            /// debugging information is deprecated.
            /// </summary>
            public uint NumberOfSymbols;

            /// <summary>
            /// The size of the optional header, which is required for
            /// executable files but not for object files. This value should be
            /// zero for an object file. For a description of the header format,
            /// see Optional Header (Image Only).
            /// </summary>
            public ushort SizeOfOptionalHeader;

            /// <summary>
            /// The flags that indicate the attributes of the file. For specific
            /// flag values, see Characteristics.
            /// </summary>
            public ushort Characteristics;
        }

        private const uint IMAGE_SCN_LNK_INFO = 0x00000200;
        private const uint IMAGE_SCN_LNK_REMOVE = 0x00000800;

        /// <summary>
        /// Each row of the section table is, in effect, a section header. This
        /// table immediately follows the optional header, if any. This
        /// positioning is required because the file header does not contain a
        /// direct pointer to the section table. Instead, the location of the
        /// section table is determined by calculating the location of the first
        /// byte after the headers. Make sure to use the size of the optional
        /// header as specified in the file header.
        /// The number of entries in the section table is given by the
        /// NumberOfSections field in the file header. Entries in the section
        /// table are numbered starting from one (1). The code and data memory
        /// section entries are in the order chosen by the linker.
        /// In an image file, the VAs for sections must be assigned by the
        /// linker so that they are in ascending order and adjacent, and they
        /// must be a multiple of the SectionAlignment value in the optional
        /// header.
        /// Each section header (section table entry) has the following format,
        /// for a total of 40 bytes per entry.
        /// </summary>
        /// <see cref="https://learn.microsoft.com/en-us/windows/win32/debug/pe-format#section-table-section-headers"/>
       [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        private struct ImageSectionHeader
        {
            /// <summary>
            /// An 8-byte, null-padded UTF-8 encoded string. If the string is
            /// exactly 8 characters long, there is no terminating null. For
            /// longer names, this field contains a slash (/) that is followed
            /// by an ASCII representation of a decimal number that is an offset
            /// into the string table. Executable images do not use a string
            /// table and do not support section names longer than 8 characters.
            /// Long names in object files are truncated if they are emitted to
            /// an executable file.
            /// </summary>
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] Name;

            /// <summary>
            /// The total size of the section when loaded into memory. If this
            /// value is greater than SizeOfRawData, the section is zero-padded.
            /// This field is valid only for executable images and should be set
            /// to zero for object files.
            /// </summary>
            public uint VirtualSize;

            /// <summary>
            /// For executable images, the address of the first byte of the
            /// section relative to the image base when the section is loaded
            /// into memory. For object files, this field is the address of the
            /// first byte before relocation is applied; for simplicity,
            /// compilers should set this to zero. Otherwise, it is an arbitrary
            /// value that is subtracted from offsets during relocation.
            /// </summary>
            public uint VirtualAddress;

            /// <summary>
            /// The size of the section (for object files) or the size of the
            /// initialized data on disk (for image files). For executable
            /// images, this must be a multiple of FileAlignment from the
            /// optional header. If this is less than VirtualSize, the remainder
            /// of the section is zero-filled. Because the SizeOfRawData field
            /// is rounded but the VirtualSize field is not, it is possible for
            /// SizeOfRawData to be greater than VirtualSize as well. When a
            /// section contains only uninitialized data, this field should be
            /// zero.
            /// </summary>
            public uint SizeOfRawData;

            /// <summary>
            /// The file pointer to the first page of the section within the
            /// COFF file. For executable images, this must be a multiple of
            /// FileAlignment from the optional header. For object files, the
            /// value should be aligned on a 4-byte boundary for best
            /// performance. When a section contains only uninitialized data,
            /// this field should be zero.
            /// </summary>
            public uint PointerToRawData;

            /// <summary>
            /// The file pointer to the beginning of relocation entries for the
            /// section. This is set to zero for executable images or if there
            /// are no relocations.
            /// </summary>
            public uint PointerToRelocations;

            /// <summary>
            /// The file pointer to the beginning of line-number entries for the
            /// section. This is set to zero if there are no COFF line numbers.
            /// This value should be zero for an image because COFF debugging
            /// information is deprecated.
            /// </summary>
            public uint PointerToLinenumbers;

            /// <summary>
            /// The number of relocation entries for the section. This is set to
            /// zero for executable images.
            /// </summary>
            public ushort NumberOfRelocations;

            /// <summary>
            /// The number of line-number entries for the section. This value
            /// should be zero for an image because COFF debugging information
            /// is deprecated.
            /// </summary>
            public ushort NumberOfLinenumbers;

            /// <summary>
            /// The flags that describe the characteristics of the section. For
            /// more information, see Section Flags.
            /// </summary>
            public uint Characteristics;
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

        private static void WriteToStream(
            Stream Stream,
            byte[] Bytes)
        {
            Stream.Write(Bytes, 0, Bytes.Length);
        }

        public static byte[] CreateWeakSymbolObject(
            SortedDictionary<string, string> WeakSymbols)
        {
            MemoryStream ObjectStream = new MemoryStream();

            ImageSectionHeader SectionHeader = new ImageSectionHeader();
            byte[] RawName = Encoding.UTF8.GetBytes(".drectve");
            SectionHeader.Name = new byte[8];
            Array.Copy(RawName, SectionHeader.Name, RawName.Length);
            SectionHeader.Characteristics =
                IMAGE_SCN_LNK_INFO | IMAGE_SCN_LNK_REMOVE;
            ImageFileHeader FileHeader = new ImageFileHeader();
            FileHeader.Machine = IMAGE_FILE_MACHINE_UNKNOWN;
            FileHeader.NumberOfSections = 1;
            FileHeader.TimeDateStamp = Convert.ToInt32(
                new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds());
            FileHeader.PointerToSymbolTable = Convert.ToUInt32(
                Marshal.SizeOf(FileHeader) + Marshal.SizeOf(SectionHeader));
            FileHeader.NumberOfSymbols = Convert.ToUInt32(
                2 + (WeakSymbols.Count * 3));
            FileHeader.SizeOfOptionalHeader = 0;
            FileHeader.Characteristics = 0x0000;
            WriteToStream(ObjectStream, StructureToBytes(FileHeader));
            WriteToStream(ObjectStream, StructureToBytes(SectionHeader));

            ImageSymbol ComponentId = new ImageSymbol();
            ComponentId.ShortName = "@comp.id";
            ComponentId.Value = 0x00FD784B;
            ComponentId.SectionNumber = IMAGE_SYM_ABSOLUTE;
            ComponentId.Type = IMAGE_SYM_TYPE_NULL;
            ComponentId.StorageClass = IMAGE_SYM_CLASS_STATIC;
            ComponentId.NumberOfAuxSymbols = 0;
            WriteToStream(ObjectStream, StructureToBytes(ComponentId));

            ImageSymbol Feature00 = new ImageSymbol();
            Feature00.ShortName = "@feat.00";
            Feature00.Value = 0x00000011;
            Feature00.SectionNumber = IMAGE_SYM_ABSOLUTE;
            Feature00.Type = IMAGE_SYM_TYPE_NULL;
            Feature00.StorageClass = IMAGE_SYM_CLASS_STATIC;
            Feature00.NumberOfAuxSymbols = 0;
            WriteToStream(ObjectStream, StructureToBytes(Feature00));

            int WeakSymbolIndex = 0;
            int StringPoolIndex = sizeof(uint);
            foreach (KeyValuePair<string, string> WeakSymbol in WeakSymbols)
            {
                ImageSymbol TargetSymbol = new ImageSymbol();
                if (WeakSymbol.Value.Length > 8)
                {
                    TargetSymbol.LongName = StringPoolIndex;
                    StringPoolIndex += WeakSymbol.Value.Length + 1;
                }
                else
                {
                    TargetSymbol.ShortName = WeakSymbol.Value;
                }  
                TargetSymbol.Value = 0;
                TargetSymbol.SectionNumber = IMAGE_SYM_UNDEFINED;
                TargetSymbol.Type = IMAGE_SYM_TYPE_NULL;
                TargetSymbol.StorageClass = IMAGE_SYM_CLASS_EXTERNAL;
                TargetSymbol.NumberOfAuxSymbols = 0;
                WriteToStream(ObjectStream, StructureToBytes(TargetSymbol));

                ImageSymbol AliasSymbol = new ImageSymbol();
                if (WeakSymbol.Key.Length > 8)
                {
                    AliasSymbol.LongName = StringPoolIndex;
                    StringPoolIndex += WeakSymbol.Key.Length + 1;
                }
                else
                {
                    AliasSymbol.ShortName = WeakSymbol.Key;
                }
                AliasSymbol.Value = 0;
                AliasSymbol.SectionNumber = IMAGE_SYM_UNDEFINED;
                AliasSymbol.Type = IMAGE_SYM_TYPE_NULL;
                AliasSymbol.StorageClass = IMAGE_SYM_CLASS_WEAK_EXTERNAL;
                AliasSymbol.NumberOfAuxSymbols = 1;
                WriteToStream(ObjectStream, StructureToBytes(AliasSymbol));

                ImageAuxWeakSymbol AuxSymbol = new ImageAuxWeakSymbol();
                AuxSymbol.TagIndex = 2 + (WeakSymbolIndex * 3);
                AuxSymbol.Characteristics =
                    ImageWeakExternSearchType.SearchAlias;
                WriteToStream(ObjectStream, StructureToBytes(AuxSymbol));

                ++WeakSymbolIndex;
            }

            WriteToStream(ObjectStream, BitConverter.GetBytes(StringPoolIndex));
            foreach (KeyValuePair<string, string> WeakSymbol in WeakSymbols)
            {
                if (WeakSymbol.Value.Length > 8)
                {
                    byte[] Encoded = Encoding.UTF8.GetBytes(WeakSymbol.Value);
                    WriteToStream(ObjectStream, Encoded);
                    ObjectStream.WriteByte(0);
                }
                if (WeakSymbol.Key.Length > 8)
                {
                    byte[] Encoded = Encoding.UTF8.GetBytes(WeakSymbol.Key);
                    WriteToStream(ObjectStream, Encoded);
                    ObjectStream.WriteByte(0);
                }
            }

            return ObjectStream.ToArray();
        }

        public static void Test()
        {
            SortedDictionary<string, string> WeakSymbols =
                new SortedDictionary<string, string>();
            WeakSymbols.Add("#access", "#_access");
            byte[] Bytes = CreateWeakSymbolObject(WeakSymbols);

            File.WriteAllBytes("WeakSymbolItem.bin", Bytes);

            Console.ReadKey();
        }
    }
}
