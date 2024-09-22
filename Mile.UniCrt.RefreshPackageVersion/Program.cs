using Mile.Project.Helpers;
using System.Xml;

namespace Mile.UniCrt.RefreshPackageVersion
{
    internal class Program
    {
        private static string RepositoryRoot = GitRepository.GetRootPath();

        static void Main(string[] args)
        {
            string NuspecFilePath = string.Format(
                @"{0}\Mile.UniCrt.NuGet\Mile.Windows.UniCrt.nuspec",
                RepositoryRoot);

            string VersionTag = string.Empty;
            //string VersionTag = "-preview";

            string VersionString = string.Format(
                "1.0.{0}.0{1}",
                DateTime.Today.Subtract(DateTime.Parse("2024-03-21")).TotalDays,
                VersionTag);

            XmlDocument Document = new XmlDocument();
            Document.PreserveWhitespace = true;
            Document.Load(NuspecFilePath);

            XmlNode? PackageNode = Document["package"];
            if (PackageNode != null)
            {
                XmlNode? MetadataNode = PackageNode["metadata"];
                if (MetadataNode != null)
                {
                    XmlNode? VersionNode = MetadataNode["version"];
                    if (VersionNode != null)
                    {
                        VersionNode.InnerText = VersionString;
                    }
                }
            }

            Document.Save(NuspecFilePath);

            Console.WriteLine(
                "{0} task has been completed.",
                "Mile.UniCrt.RefreshPackageVersion");
            Console.ReadKey();
        }
    }
}
