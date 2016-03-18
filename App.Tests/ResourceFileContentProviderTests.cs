using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests
{
    public class ResourceFileContentProviderTests
    {
        private readonly ResourceFileContentProvider provider;
        public ResourceFileContentProviderTests()
        {
            provider = new ResourceFileContentProvider(assembly: GetType().Assembly, baseNamespace: @"App.Tests");
        }
        [Theory]
        [MemberData("GetTestData", MemberType = typeof(FileContentProviderTestData))]
        public void Should_return_file_stream_given_relative_path(string relativePath, string expectedFirstLine)
        {

            var file = provider.GetContent(relativePath);

            StreamReader sr = new StreamReader(file);

            var firstLine = sr.ReadLine();

            Assert.Equal(expectedFirstLine, firstLine);

        }
        [Fact]
        public void Should_not_find_non_existant_file()
        {

            var exists = provider.Exists("non existant file");

            Assert.False(exists);

        }

        [Fact]
        public void Should_throw_exception_for_non_existant_file()
        {
            var exception = Assert.Throws<FileNotFoundException>(() => provider.GetContent("non existant file"));

            Assert.StartsWith("Could not find file", exception.Message);
        }

    }
}
