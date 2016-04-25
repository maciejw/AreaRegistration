using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace App
{

    public class FileSystemFileContentProvider : IFileContentProvider
    {
        public bool Exists(string relativePath)
        {
            return File.Exists(relativePath);
        }

        public Stream GetContent(string relativePath)
        {
            return File.OpenRead(relativePath);
        }
    }

}
