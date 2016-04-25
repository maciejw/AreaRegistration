using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics.Contracts;
using System.Diagnostics.CodeAnalysis;

namespace App
{
    public class ResourceFileContentProvider : IFileContentProvider
    {
        private readonly Assembly assembly;
        private readonly string basePath;
        private readonly ResourcePathReplacer pathReplacer;

        public ResourceFileContentProvider(Assembly assembly, string baseNamespace)
        {
            this.basePath = baseNamespace;
            this.assembly = assembly;
            this.pathReplacer = new ResourcePathReplacer();
        }

        private string GetResourcePath(string relativePath)
        {
            return pathReplacer.ToResourcePath(Path.Combine(basePath, relativePath));
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
