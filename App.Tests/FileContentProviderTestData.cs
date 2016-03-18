using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace App.Tests
{
    [ExcludeFromCodeCoverage]
    public class FileContentProviderTestData
    {
        private static IEnumerable<object[]> GetTestData()
        {
            yield return new object[] { @"TestFolder/file1.txt", "file1" };
            yield return new object[] { @"TestFolder\file1.txt", "file1" };
            yield return new object[] { @"TestFolder\file-2.txt", "file2" };
            yield return new object[] { @"TestFolder\SubFolder\file_3.txt", "file3" };
            yield return new object[] { @"TestFolder\SubFolder\file with space in name.txt", "file with space in name" };

        }
    }
}
