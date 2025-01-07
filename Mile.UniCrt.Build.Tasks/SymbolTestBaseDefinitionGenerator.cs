using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mile.Project.Helpers;
using System.Collections.Generic;

namespace Mile.UniCrt.Build.Tasks
{
    public class SymbolTestBaseDefinitionGenerator : Task
    {
        [Required]
        public string BaseLibraryPath { get; set; }

        [Required]
        public string VcRuntimeLibraryPath { get; set; }

        [Required]
        public string TargetPlatform { get; set; }

        [Required]
        public string OutputLibraryPath { get; set; }

        public override bool Execute()
        {
            {
                ImageArchive.Archive BaseArchive = ImageArchive.Parse(
                    BaseLibraryPath + @"\ucrt.lib");

                ImageArchive.Archive VcRuntimeArchive = ImageArchive.Parse(
                    VcRuntimeLibraryPath + @"\vcruntime.lib");

                SortedSet<string> FinalSymbols = new SortedSet<string>();
                if (BaseArchive.Symbols != null)
                {
                    SortedSet<string> Symbols = ImageArchive.ListSymbols(
                        BaseArchive.Symbols,
                        TargetPlatform == "x86");
                    foreach (string Symbol in Symbols)
                    {
                        FinalSymbols.Add(Symbol);
                    }
                }
                if (VcRuntimeArchive.Symbols != null)
                {
                    SortedDictionary<string, SortedSet<string>> Categories =
                        ImageArchive.CategorizeSymbols(
                            VcRuntimeArchive.Symbols,
                            TargetPlatform == "x86");
                    foreach (var Category in Categories)
                    {
                        string LowerKey = Category.Key.ToLower();
                        if (LowerKey == "vcruntime140.dll" ||
                            LowerKey == "vcruntime140_1.dll")
                        {
                            foreach (string Symbol in Category.Value)
                            {
                                if (Symbol.Contains("__telemetry_main_") ||
                                    Symbol.Contains("__vcrt_"))
                                {
                                    continue;
                                }
                                FinalSymbols.Add(Symbol);
                            }
                        }
                    }
                }
                Utilities.GenerateDefinitionFile(
                    string.Format(
                        @"{0}\Mile.UniCrt.SymbolTestBase.def",
                        OutputLibraryPath),
                    string.Empty,
                    FinalSymbols);

                SortedSet<string> FinalEcSymbols = new SortedSet<string>();
                if (BaseArchive.EcSymbols != null)
                {
                    SortedSet<string> Symbols = ImageArchive.ListSymbols(
                        BaseArchive.EcSymbols);
                    foreach (string Symbol in Symbols)
                    {
                        FinalEcSymbols.Add(Symbol);
                    }
                }
                if (VcRuntimeArchive.EcSymbols != null)
                {
                    SortedDictionary<string, SortedSet<string>> Categories =
                        ImageArchive.CategorizeSymbols(
                            VcRuntimeArchive.EcSymbols);
                    foreach (var Category in Categories)
                    {
                        string LowerKey = Category.Key.ToLower();
                        if (LowerKey == "vcruntime140.dll" ||
                            LowerKey == "vcruntime140_1.dll")
                        {
                            foreach (string Symbol in Category.Value)
                            {
                                if (Symbol.Contains("__telemetry_main_") ||
                                    Symbol.Contains("__vcrt_"))
                                {
                                    continue;
                                }
                                FinalEcSymbols.Add(Symbol);
                            }
                        }
                    }
                }
                if (FinalEcSymbols.Count > 0)
                {
                    Utilities.GenerateDefinitionFile(
                        string.Format(
                            @"{0}\Mile.UniCrt.SymbolTestBase.Arm64EC.def",
                            OutputLibraryPath),
                        string.Empty,
                        FinalEcSymbols);
                }
            }

            {
                ImageArchive.Archive BaseArchive = ImageArchive.Parse(
                    BaseLibraryPath + @"\ucrtd.lib");

                ImageArchive.Archive VcRuntimeArchive = ImageArchive.Parse(
                    VcRuntimeLibraryPath + @"\vcruntimed.lib");

                SortedSet<string> FinalSymbols = new SortedSet<string>();
                if (BaseArchive.Symbols != null)
                {
                    SortedSet<string> Symbols = ImageArchive.ListSymbols(
                        BaseArchive.Symbols,
                        TargetPlatform == "x86");
                    foreach (string Symbol in Symbols)
                    {
                        FinalSymbols.Add(Symbol);
                    }
                }
                if (VcRuntimeArchive.Symbols != null)
                {
                    SortedDictionary<string, SortedSet<string>> Categories =
                        ImageArchive.CategorizeSymbols(
                            VcRuntimeArchive.Symbols,
                            TargetPlatform == "x86");
                    foreach (var Category in Categories)
                    {
                        string LowerKey = Category.Key.ToLower();
                        if (LowerKey == "vcruntime140d.dll" ||
                            LowerKey == "vcruntime140_1d.dll")
                        {
                            foreach (string Symbol in Category.Value)
                            {
                                if (Symbol.Contains("__telemetry_main_") ||
                                    Symbol.Contains("__vcrt_"))
                                {
                                    continue;
                                }
                                FinalSymbols.Add(Symbol);
                            }
                        }
                    }
                }
                Utilities.GenerateDefinitionFile(
                    string.Format(
                        @"{0}\Mile.UniCrt.SymbolTestBaseDebug.def",
                        OutputLibraryPath),
                    string.Empty,
                    FinalSymbols);

                SortedSet<string> FinalEcSymbols = new SortedSet<string>();
                if (BaseArchive.EcSymbols != null)
                {
                    SortedSet<string> Symbols = ImageArchive.ListSymbols(
                        BaseArchive.EcSymbols);
                    foreach (string Symbol in Symbols)
                    {
                        FinalEcSymbols.Add(Symbol);
                    }
                }
                if (VcRuntimeArchive.EcSymbols != null)
                {
                    SortedDictionary<string, SortedSet<string>> Categories =
                        ImageArchive.CategorizeSymbols(
                            VcRuntimeArchive.EcSymbols);
                    foreach (var Category in Categories)
                    {
                        string LowerKey = Category.Key.ToLower();
                        if (LowerKey == "vcruntime140d.dll" ||
                            LowerKey == "vcruntime140_1d.dll")
                        {
                            foreach (string Symbol in Category.Value)
                            {
                                if (Symbol.Contains("__telemetry_main_") ||
                                    Symbol.Contains("__vcrt_"))
                                {
                                    continue;
                                }
                                FinalEcSymbols.Add(Symbol);
                            }
                        }
                    }
                }
                if (FinalEcSymbols.Count > 0)
                {
                    Utilities.GenerateDefinitionFile(
                        string.Format(
                            @"{0}\Mile.UniCrt.SymbolTestBaseDebug.Arm64EC.def",
                            OutputLibraryPath),
                        string.Empty,
                        FinalEcSymbols);
                }
            }

            return true;
        }
    }
}
