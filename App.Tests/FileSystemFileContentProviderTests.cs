using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace App.Tests
{
    public class FileSystemFileContentProviderTests
    {

        private readonly FileSystemFileContentProvider sut;
        private readonly ITestOutputHelper output;

        public FileSystemFileContentProviderTests(ITestOutputHelper output)
        {
            sut = new FileSystemFileContentProvider();

            this.output = output;

        }
        [Theory]
        [MemberData("GetTestData", MemberType = typeof(FileContentProviderTestData))]
        public void Should_return_file_stream_given_relative_path(string relativePath, string expectedFirstLine)
        {
            var file = sut.GetContent(relativePath);

            StreamReader sr = new StreamReader(file);

            var firstLine = sr.ReadLine();

            Assert.Equal(expectedFirstLine, firstLine);

        }

        [Fact]
        public void Should_not_find_non_existant_file()
        {
            var exists = sut.Exists("non existent file");

            Assert.False(exists);
        }

        [Fact]
        [PerformanceTrait]
        public void Resource_accessing_should_be_fast_enough()
        {

            var stopwatch = Stopwatch.StartNew();
            var random = new Random();

            Parallel.ForEach(Enumerable.Range(0, FileContentProviderTestData.NumberOfIThreads), i => {
                foreach (var j in Enumerable.Range(0, FileContentProviderTestData.NumberOfIterations))
                {
                    var fileNumber = random.Next(1, 500);

                    output.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId}");

                    var stream = sut.GetContent($@"TestFolder\SpeedTest\file{fileNumber}.txt");

                    var sr = new StreamReader(stream);

                    var s = sr.ReadToEnd();


                }

            });

        }

        [Fact]
        public void Should_throw_exception_for_non_existant_file()
        {
            var exception = Assert.Throws<FileNotFoundException>(() => sut.GetContent("non existant file"));

            Assert.StartsWith("Could not find file", exception.Message);
        }

    }
}
