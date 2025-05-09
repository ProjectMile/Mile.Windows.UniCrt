﻿using Mile.Project.Helpers;

namespace Mile.UniCrt.WrapperDefinitionGenerator
{
    internal class Program
    {
        private static readonly string ProjectRootPath =
            GitRepository.GetRootPath();

        private static readonly string PackageRootPath =
            ProjectRootPath + @"\Mile.UniCrt";

        private static readonly string ReferencesRootPath =
            ProjectRootPath + @"\Mile.UniCrt.References";

        private static readonly string WrapperRootPath =
            ProjectRootPath + @"\Mile.UniCrt.Wrapper";

        private static SortedSet<string> ImportStringListFromListFile(
            string FilePath)
        {
            SortedSet<string> Result = new SortedSet<string>();

            string[] Candidates = File.ReadAllLines(FilePath);
            foreach (string Candidate in Candidates)
            {
                if (!string.IsNullOrWhiteSpace(Candidate))
                {
                    Result.Add(Candidate);
                }
            }

            return Result;
        }

        private static void GenerateDefinitionFile(
            string OutputFilePath,
            string LibraryName,
            SortedSet<string> Symbols)
        {
            string Content = string.Format(
                "; {0}\r\n\r\nLIBRARY {1}\r\n\r\nEXPORTS\r\n\r\n",
                "Generated by Mile.UniCrt.WrapperDefinitionGenerator",
                LibraryName);

            foreach (string Symbol in Symbols)
            {
                Content += string.Format("{0}\r\n", Symbol);
            }

            FileUtilities.SaveTextToFileAsUtf8Bom(OutputFilePath, Content);
        }

        static void Main(string[] args)
        {
            string[] Platforms = ["arm64", "x64", "x86"];

            (string Key, string Value)[] RuntimeDebuggingWeakSymbols =
            {
                ("_aligned_free_dbg", "_aligned_free"),
                ("_aligned_malloc_dbg", "_aligned_malloc"),
                ("_aligned_msize_dbg", "_aligned_msize"),
                ("_aligned_offset_malloc_dbg", "_aligned_offset_malloc"),
                ("_aligned_offset_realloc_dbg", "_aligned_offset_realloc"),
                ("_aligned_offset_recalloc_dbg", "_aligned_offset_recalloc"),
                ("_aligned_realloc_dbg", "_aligned_realloc"),
                ("_aligned_recalloc_dbg", "_aligned_recalloc"),
                ("_calloc_dbg", "calloc"),
                ("_dupenv_s_dbg", "_dupenv_s"),
                ("_expand_dbg", "_expand"),
                ("_free_dbg", "free"),
                ("_fullpath_dbg", "_fullpath"),
                ("_getcwd_dbg", "_getcwd"),
                ("_getdcwd_dbg", "_getdcwd"),
                ("_malloc_dbg", "malloc"),
                ("_mbsdup_dbg", "_mbsdup"),
                ("_msize_dbg", "_msize"),
                ("_realloc_dbg", "realloc"),
                ("_recalloc_dbg", "_recalloc"),
                ("_strdup_dbg", "_strdup"),
                ("_tempnam_dbg", "_tempnam"),
                ("_wcsdup_dbg", "_wcsdup"),
                ("_wdupenv_s_dbg", "_wdupenv_s"),
                ("_wfullpath_dbg", "_wfullpath"),
                ("_wgetcwd_dbg", "_wgetcwd"),
                ("_wgetdcwd_dbg", "_wgetdcwd"),
                ("_wtempnam_dbg", "_wtempnam")
            };

            foreach (string Platform in Platforms)
            {
                SortedSet<string> ReleaseSymbols = new SortedSet<string>();
                {
                    SortedSet<string> Symbols = ImageArchive.ListSymbols(
                        ImageArchive.Parse(
                            string.Format(
                                @"{0}\Lib\10.0.10240.0\{1}\ucrt.lib",
                                ReferencesRootPath,
                                Platform)).Symbols,
                        Platform == "x86");
                    foreach (string Symbol in Symbols)
                    {
                        ReleaseSymbols.Add(Symbol);
                    }
                }

                SortedSet<string> DebugSymbols = new SortedSet<string>();
                {
                    SortedSet<string> Symbols = ImageArchive.ListSymbols(
                        ImageArchive.Parse(
                            string.Format(
                                @"{0}\Lib\10.0.10240.0\{1}\ucrtd.lib",
                                ReferencesRootPath,
                                Platform)).Symbols,
                        Platform == "x86");
                    foreach (string Symbol in Symbols)
                    {
                        DebugSymbols.Add(Symbol);
                    }
                }

                SortedSet<string> ExtendedSymbols =
                    ImportStringListFromListFile(
                        string.Format(
                            @"{0}\Runtime\10.0.19041.1\{1}\ucrtbase_exports.txt",
                            ReferencesRootPath,
                            Platform));
                foreach (string Symbol in ExtendedSymbols)
                {
                    if (Symbol.StartsWith("_o_"))
                    {
                        string FinalSymbol = string.Format(
                            "{0}={1}",
                            Symbol,
                            Symbol.Substring(3));
                        ReleaseSymbols.Add(FinalSymbol);
                        DebugSymbols.Add(FinalSymbol);
                    }
                    else if (!string.IsNullOrWhiteSpace(Symbol) &&
                        !Symbol.StartsWith("?") &&
                        !Symbol.StartsWith("$"))
                    {
                        ReleaseSymbols.Add(Symbol);
                        DebugSymbols.Add(Symbol);
                    }
                }

                GenerateDefinitionFile(
                    string.Format(
                        @"{0}\Mile.UniCrt.Wrapper.{1}.Release.def",
                        WrapperRootPath,
                        Platform),
                    string.Empty,
                    ReleaseSymbols);

                GenerateDefinitionFile(
                    string.Format(
                        @"{0}\Mile.UniCrt.Wrapper.{1}.Debug.def",
                        WrapperRootPath,
                        Platform),
                    string.Empty,
                    DebugSymbols);

                File.WriteAllBytes(
                    string.Format(
                        @"{0}\Mile.UniCrt.{1}.{2}.obj",
                        PackageRootPath,
                        "RuntimeDebuggingFeatureWeakWrapper",
                        Platform),
                    WeakSymbolsObjectGenerator.CreateWeakSymbolObject(
                        RuntimeDebuggingWeakSymbols,
                        Platform,
                        true));
                if (Platform == "arm64")
                {
                    File.WriteAllBytes(
                    string.Format(
                        @"{0}\Mile.UniCrt.{1}.{2}.obj",
                        PackageRootPath,
                        "RuntimeDebuggingFeatureWeakWrapper",
                        "arm64ec"),
                    WeakSymbolsObjectGenerator.CreateWeakSymbolObject(
                        RuntimeDebuggingWeakSymbols,
                        "arm64ec",
                        true));
                }
            }

            Console.WriteLine(
                "{0} task has been completed.",
                "Mile.UniCrt.WrapperDefinitionGenerator");
            Console.ReadKey();
        }
    }
}
