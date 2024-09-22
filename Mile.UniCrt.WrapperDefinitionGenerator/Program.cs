using Mile.Project.Helpers;

namespace Mile.UniCrt.WrapperDefinitionGenerator
{
    internal class Program
    {
        private static readonly string ProjectRootPath =
            GitRepository.GetRootPath();

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
                "LIBRARY {0}\r\n\r\nEXPORTS\r\n\r\n",
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
                            @"{0}\Runtime\10.0.19041.1\{1}\exports.txt",
                            ReferencesRootPath,
                            Platform));
                foreach (string Symbol in ExtendedSymbols)
                {
                    if (!string.IsNullOrWhiteSpace(Symbol) &&
                        !Symbol.StartsWith("_o_") &&
                        !Symbol.StartsWith("?") &&
                        !Symbol.StartsWith("$") &&
                        !Symbol.StartsWith("__dcrt_"))
                    {
                        string FinalSymbol = Symbol;
                        if (Symbol.Contains("@"))
                        {
                            FinalSymbol = Symbol.Split('@')[0];
                        }
                        ReleaseSymbols.Add(FinalSymbol);
                        DebugSymbols.Add(FinalSymbol);
                    }
                }

                GenerateDefinitionFile(
                    string.Format(
                        @"{0}\Mile.UniversalCRT.Wrapper.{1}.Release.def",
                        WrapperRootPath,
                        Platform),
                    string.Empty,
                    ReleaseSymbols);

                GenerateDefinitionFile(
                    string.Format(
                        @"{0}\Mile.UniversalCRT.Wrapper.{1}.Debug.def",
                        WrapperRootPath,
                        Platform),
                    string.Empty,
                    DebugSymbols);
            }

            Console.WriteLine(
                "{0} task has been completed.",
                "Mile.UniCrt.WrapperDefinitionGenerator");
            Console.ReadKey();
        }
    }
}
