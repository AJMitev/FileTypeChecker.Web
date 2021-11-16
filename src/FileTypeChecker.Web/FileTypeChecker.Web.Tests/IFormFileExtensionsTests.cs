namespace FileTypeChecker.Web.Tests
{
    using FileTypeChecker.Types;
    using Xunit;

    public class IFormFileExtensionsTests
    {
        [Fact]
        public void Is_ShouldReturnTrueIfTheTypesMatch()
        {
            var formFile = FileHelpers.ReadFile("test.bmp");
            var expected = true;
            var actual = formFile.Is<Bitmap>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Is_ShouldReturnFalseIfTypesDidNotMatch()
        {
            var formFile = FileHelpers.ReadFile("test.bmp");
            var expected = false;
            var actual = formFile.Is<Gzip>();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsImage_ShouldReturnTrueIfTheTypesMatch()
        {
            var formFile = FileHelpers.ReadFile("test.jpg");
            var expected = true;
            var actual = formFile.IsImage();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsImage_ShouldReturnFalseIfTypesDidNotMatch()
        {
            var formFile = FileHelpers.ReadFile("test.exe");
            var expected = false;
            var actual = formFile.IsImage();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsArchive_ShouldReturnTrueIfTheTypesMatch()
        {
            var formFile = FileHelpers.ReadFile("test.zip");
            var expected = true;
            var actual = formFile.IsArchive();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsArchive_ShouldReturnFalseIfTypesDidNotMatch()
        {
            var formFile = FileHelpers.ReadFile("test.bmp");
            var expected = false;
            var actual = formFile.IsArchive();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsExecutable_ShouldReturnTrueIfTheTypesMatch()
        {
            var formFile = FileHelpers.ReadFile("test.exe");
            var expected = true;
            var actual = formFile.IsExecutable();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IsExecutable_ShouldReturnFalseIfTypesDidNotMatch()
        {
            var formFile = FileHelpers.ReadFile("test.bmp");
            var expected = false;
            var actual = formFile.IsExecutable();

            Assert.Equal(expected, actual);
        }
    }
}
