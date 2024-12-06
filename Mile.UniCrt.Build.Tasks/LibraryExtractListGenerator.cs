using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mile.Project.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace Mile.UniCrt.Build.Tasks
{
    public class LibraryExtractListGenerator : Task
    {
        [Required]
        public string TargetPlatform { get; set; }

        [Required]
        public string InputLibraryPath { get; set; }

        [Required]
        public string[] FilterList { get; set; }

        [Output]
        public string[] ExtractList { get; set; }

        public override bool Execute()
        {
            SortedSet<string> ExtractObjects = new SortedSet<string>();

            ImageArchive.Archive Archive = ImageArchive.Parse(InputLibraryPath);

            if (Archive.Symbols != null)
            {
                SortedDictionary<string, SortedSet<string>> Categories =
                    ImageArchive.CategorizeSymbols(
                        Archive.Symbols,
                        TargetPlatform == "x86");
                foreach (var Category in Categories)
                {
                    foreach (string Object in FilterList)
                    {
                        if (Category.Key.ToLower().Contains(
                            Object.ToLower()))
                        {
                            ExtractObjects.Add(Category.Key);
                            break;
                        }
                    }
                }
            }

            if (Archive.EcSymbols != null)
            {
                SortedDictionary<string, SortedSet<string>> Categories =
                    ImageArchive.CategorizeSymbols(Archive.EcSymbols);
                foreach (var Category in Categories)
                {
                    foreach (string Object in FilterList)
                    {
                        if (Category.Key.ToLower().Contains(
                            Object.ToLower()))
                        {
                            ExtractObjects.Add(Category.Key);
                            break;
                        }
                    }
                }
            }

            ExtractList = ExtractObjects.ToArray();

            return true;
        }
    }
}
