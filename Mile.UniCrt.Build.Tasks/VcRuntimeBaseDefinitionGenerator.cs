using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mile.Project.Helpers;
using System.Collections.Generic;

namespace Mile.UniCrt.Build.Tasks
{
    public class VcRuntimeBaseDefinitionGenerator : Task
    {
        [Required]
        public string BaseLibraryPath { get; set; }

        [Required]
        public string TargetPlatform { get; set; }

        [Required]
        public string OutputLibraryPath { get; set; }

        public override bool Execute()
        {
            {
                ImageArchive.Archive Archive = ImageArchive.Parse(
                    BaseLibraryPath + @"\vcruntime.lib");

                if (Archive.Symbols != null)
                {
                    SortedSet<string> Symbols = new SortedSet<string>();
                    {
                        SortedDictionary<string, SortedSet<string>> Categories =
                            ImageArchive.CategorizeSymbols(
                                Archive.Symbols,
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
                                    Symbols.Add(Symbol);
                                }
                            }
                        }
                        if (TargetPlatform == "x64")
                        {
                            Symbols.Add("_GetImageBase");
                            Symbols.Add("_GetThrowImageBase");
                            Symbols.Add("_SetImageBase");
                            Symbols.Add("_SetThrowImageBase");
                        }
                    }
                    Utilities.GenerateDefinitionFile(
                        string.Format(
                            @"{0}\Mile.UniCrt.VcRuntimeBase.def",
                            OutputLibraryPath),
                        "ucrtbase.dll",
                        Symbols);
                }

                if (Archive.EcSymbols != null)
                {
                    SortedSet<string> Symbols = new SortedSet<string>();
                    {
                        SortedDictionary<string, SortedSet<string>> Categories =
                            ImageArchive.CategorizeSymbols(
                                Archive.EcSymbols);
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
                                    Symbols.Add(Symbol);
                                }
                            }
                        }
                        Symbols.Add("_GetImageBase");
                        Symbols.Add("_GetThrowImageBase");
                        Symbols.Add("_SetImageBase");
                        Symbols.Add("_SetThrowImageBase");
                    }
                    Utilities.GenerateDefinitionFile(
                       string.Format(
                           @"{0}\Mile.UniCrt.VcRuntimeBase.Arm64EC.def",
                           OutputLibraryPath),
                       "ucrtbase.dll",
                       Symbols);
                }
            }

            {
                ImageArchive.Archive Archive = ImageArchive.Parse(
                    BaseLibraryPath + @"\vcruntimed.lib");

                if (Archive.Symbols != null)
                {
                    SortedSet<string> Symbols = new SortedSet<string>();
                    {
                        SortedDictionary<string, SortedSet<string>> Categories =
                            ImageArchive.CategorizeSymbols(
                                Archive.Symbols,
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
                                    Symbols.Add(Symbol);
                                }
                            }
                        }
                        if (TargetPlatform == "x64")
                        {
                            Symbols.Add("_GetImageBase");
                            Symbols.Add("_GetThrowImageBase");
                            Symbols.Add("_SetImageBase");
                            Symbols.Add("_SetThrowImageBase");
                        }
                    }
                    Utilities.GenerateDefinitionFile(
                        string.Format(
                            @"{0}\Mile.UniCrt.VcRuntimeBaseDebug.def",
                            OutputLibraryPath),
                        "ucrtbased.dll",
                        Symbols);
                }

                if (Archive.EcSymbols != null)
                {
                    SortedSet<string> Symbols = new SortedSet<string>();
                    {
                        SortedDictionary<string, SortedSet<string>> Categories =
                            ImageArchive.CategorizeSymbols(
                                Archive.EcSymbols);
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
                                    Symbols.Add(Symbol);
                                }
                            }
                        }
                        Symbols.Add("_GetImageBase");
                        Symbols.Add("_GetThrowImageBase");
                        Symbols.Add("_SetImageBase");
                        Symbols.Add("_SetThrowImageBase");
                    }
                    Utilities.GenerateDefinitionFile(
                       string.Format(
                           @"{0}\Mile.UniCrt.VcRuntimeBaseDebug.Arm64EC.def",
                           OutputLibraryPath),
                       "ucrtbased.dll",
                       Symbols);
                }
            }

            return true;
        }
    }
}
