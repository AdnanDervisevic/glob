﻿using System;
using System.IO;
using System.Linq;
using Xunit;

namespace GlobExpressions.Tests
{
    public class GlobExtensionTests
    {
        private readonly string _sourceRoot = Environment.GetEnvironmentVariable("APPVEYOR_BUILD_FOLDER") ?? Path.Combine("..", "..", "..", "..", "..");

        [Fact]
        public void CanMatchBinFolderGlob()
        {
            var root = new DirectoryInfo(_sourceRoot);
            var allBinFolders = root.GlobDirectories("**/bin");

            Assert.True(allBinFolders.Any(), "There should be some bin folders");
        }

        [Fact]
        public void CanMatchDllExtension()
        {
            var root = new DirectoryInfo(_sourceRoot);
            var allDllFiles = root.GlobFiles("**/*.dll");

            Assert.True(allDllFiles.Any(), "There should be some DLL files");
        }

        [Fact]
        public void CanMatchInfoInFileSystemInfo()
        {
            var root = new DirectoryInfo(_sourceRoot);
            var allInfoFilesAndFolders = root.GlobFileSystemInfos("**/*info");

            Assert.True(allInfoFilesAndFolders.Any(), "There should be some 'allInfoFilesAndFolders'");
        }

        [Fact]
        public void CanMatchConfigFilesInMsDirectory()
        {
            var globPattern = @"**/*.sln";

            var root = new DirectoryInfo(_sourceRoot);
            var result = root.GlobFiles(globPattern).ToList();

            Assert.NotNull(result);
            Assert.True(result.Any(x => x.Name == "Glob.sln"), $"There should be some Glob.sln files in '{root.FullName}'");
        }

        [Fact]
        public void CanMatchStarThenPath()
        {
            var globPattern = @"*/*/*.csproj";

            var root = new DirectoryInfo(_sourceRoot);
            var result = root.GlobFiles(globPattern).OrderBy(x => x.Name).ToList();

            Assert.Collection(
                result,
                file => Assert.Equal("Glob.Benchmarks.csproj", file.Name),
                file => Assert.Equal("Glob.csproj", file.Name),
                file => Assert.Equal("Glob.Tests.csproj", file.Name)
            );
        }

        [Fact]
        public void CanMatchConfigFilesOrFoldersInMsDirectory()
        {
            var globPattern = @"**/*[Tt]est*";

            var root = new DirectoryInfo(_sourceRoot);
            var result = root.GlobFileSystemInfos(globPattern).ToList();

            Assert.NotNull(result);
            Assert.True(result.Any(x => x.Name == "GlobTests.cs"), $"There should be some GlobTests.cs files in '{root.FullName}'");
            Assert.True(result.Any(x => x.Name == "test"), $"There should some folder with 'test' in '{root.FullName}'");
        }

        [Fact]
        public void CanMatchDirectoriesInMsDirectory()
        {
            var globPattern = @"**/*Gl*.Te*";

            var root = new DirectoryInfo(_sourceRoot);
            var result = root.GlobDirectories(globPattern).ToList();

            Assert.NotNull(result);
            Assert.True(result.Any(), $"There should be some directories that match glob: {globPattern} in '{root.FullName}'");
        }
    }
}
