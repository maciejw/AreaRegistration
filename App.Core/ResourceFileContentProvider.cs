using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;

namespace App
{

    public class ResourceFileContentProvider : IFileContentProvider
    {
        private readonly Assembly assembly;
        private readonly string basePath;
        private readonly Regex pathReplacer;

        public ResourceFileContentProvider(Assembly assembly, string baseNamespace)
        {
            Contract.Requires(!string.IsNullOrEmpty(baseNamespace), "baseNamespace is null or empty.");
            Contract.Requires(assembly != null, "assembly is null.");

            this.basePath = baseNamespace;
            this.assembly = assembly;
            this.pathReplacer = new Regex(@"[\\/]", RegexOptions.Compiled);
        }

        private string PathReplacerEvaluator(Match match)
        {
            switch (match.Value)
            {
                case @"\": return ".";
                case @"/": return ".";
                default:
                    throw new NotSupportedException($"Unexpected match {match.Value}");
            }
        }

        private string GetResourcePath(string relativePath)
        {

            var fullPath = Path.Combine(basePath, relativePath);

            return pathReplacer.Replace(fullPath, PathReplacerEvaluator);
        }

        public Stream GetContent(string relativePath)
        {

            string resourcePath = GetResourcePath(relativePath);
            Stream stream = assembly.GetManifestResourceStream(resourcePath);

            if (stream == null)
            {
                throw new FileNotFoundException($"Could not find file {relativePath}");
            }

            return stream;
        }

        public bool Exists(string relativePath)
        {
            string resourcePath = GetResourcePath(relativePath);

            return assembly.GetManifestResourceNames().Any(path => path.Equals(resourcePath));
        }

    }

}
