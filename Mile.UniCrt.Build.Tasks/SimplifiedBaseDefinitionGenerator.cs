using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mile.Project.Helpers;

namespace Mile.UniCrt.Build.Tasks
{
    public class SimplifiedBaseDefinitionGenerator : Task
    {
        [Required]
        public string BaseLibraryPath { get; set; }

        [Required]
        public string TargetPlatform { get; set; }

        [Required]
        public string OutputLibraryPath { get; set; }

        public override bool Execute()
        {
            ImageArchive.Archive Archive = ImageArchive.Parse(
               BaseLibraryPath + @"\ucrt.lib");

            if (Archive.Symbols != null)
            {
                Utilities.GenerateDefinitionFile(
                    string.Format(
                        @"{0}\Mile.UniCrt.SimplifiedBase.def",
                        OutputLibraryPath),
                    "ucrtbase.dll",
                    ImageArchive.ListSymbols(
                        Archive.Symbols,
                        TargetPlatform == "x86"));
            }

            if (Archive.EcSymbols != null)
            {
                Utilities.GenerateDefinitionFile(
                   string.Format(
                       @"{0}\Mile.UniCrt.SimplifiedBase.Arm64EC.def",
                       OutputLibraryPath),
                   "ucrtbase.dll",
                   ImageArchive.ListSymbols(
                       Archive.EcSymbols));
            }

            return true;
        }
    }
}
