using QRCodeCore;
using System;
using System.IO;

namespace QRCodeCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateSvgFiles();
        }

        private static void CreateSvgFiles()
        {
            var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
            while (directory.Name != "src")
            {
                directory = directory.Parent;
            }

            directory = directory.Parent;
            CreateSvgFiles(directory);
        }

        private static void CreateSvgFiles(DirectoryInfo root)
        {
            foreach (DirectoryInfo directory in root.GetDirectories())
            {
                if (directory.Name == "src")
                    continue;

                foreach (DirectoryInfo subDirectory in directory.GetDirectories("*.*", SearchOption.AllDirectories))
                {
                    CreateSvgFile(root, subDirectory);
                }
            }
        }

        private static void CreateSvgFile(DirectoryInfo root, DirectoryInfo directory)
        {
            var readme = new FileInfo(Path.Combine(directory.FullName, "Readme.md"));

            if (!readme.Exists)
                return;

            var url = readme.FullName;
            url = url.Replace(root.FullName, string.Empty);
            url = "https://github.com/dlemstra/talks/tree/master" + url.Replace("\\","/");

            var data = new QRCodeData(url);

            SvgQRCode code = new SvgQRCode(data);
            var result = code.Create(512);

            string outputFile = Path.Combine(readme.Directory.FullName, "Readme.svg");
            File.WriteAllText(outputFile, result);
        }
    }
}
