namespace FileTypeChecker.Web.Theorys
{
    using FileTypeChecker.Types;
    using FileTypeChecker.Web.Attributes;
    using FileTypeChecker.Web.Tests;
    using System;
    using Xunit;

    public class AttribuTheoryests
    {
        [Theory]
        [InlineData("365-doc.docx", true)]
        [InlineData("test.pdf", true)]
        [InlineData("test.doc", true)]
        [InlineData("test.png", false)]
        [InlineData("test.zip", false)]
        public void AllowDocumentsAttribute_ShouldAllowOnlyDocuments(string fileName, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowDocumentsAttribute();

            Assert.Equal(expectedResult, attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData(new[] { "365-doc.docx", "test.pdf", "test.doc" }, true)]
        [InlineData(new[] { "365-doc.docx", "test.doc" }, true)]
        [InlineData(new[] { "365-doc.docx", "test.zip" }, false)]
        public void AllowDocumentsAttribute_ShouldAllowOnlyDocumentsIfTheInputIsCollection(string[] files, bool expectedResult)
        {
            var stream = FileHelpers.ReadFiles(files);

            var attributeToTheory = new AllowDocumentsAttribute();

            Assert.Equal(expectedResult, attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData("test.bmp", new[] { Bitmap.TypeExtension }, true)]
        [InlineData("test.bmp", new[] { JointPhotographicExpertsGroup.TypeExtension }, false)]
        [InlineData("test.jpg", new[] { JointPhotographicExpertsGroup.TypeExtension }, true)]
        [InlineData("test.jpg", new[] { Bitmap.TypeExtension, JointPhotographicExpertsGroup.TypeExtension, GraphicsInterchangeFormat89.TypeExtension }, true)]
        [InlineData("test.jpg", new[] { RarArchive.TypeExtension }, false)]
        [InlineData("test.zip", new[] { ZipFile.TypeExtension }, true)]
        public void AllowedTypeAttribute_ShouldAllowOnlyPointedTypes(string fileName, string[] allowedExtensions, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowedTypesAttribute(allowedExtensions);

            Assert.Equal(expectedResult, attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData("test.zip", null)]
        [InlineData("test.zip", new string[] { })]
        public void AllowTypesAttribute_ShouldThrowExceptionIfNoExtensionsProvided(string fileName, string[] allowedExtensions)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowedTypesAttribute(allowedExtensions);

            Assert.Throws<InvalidOperationException>(() => attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData("test.zip", new[] { ZipFile.TypeExtension }, false)]
        [InlineData("test.png", new[] { ZipFile.TypeExtension }, true)]
        [InlineData("test.png", new[] { ZipFile.TypeExtension, ExtensibleArchive.TypeExtension, JointPhotographicExpertsGroup.TypeExtension }, true)]
        [InlineData("test.png", new[] { ZipFile.TypeExtension, PortableNetworkGraphic.TypeExtension, JointPhotographicExpertsGroup.TypeExtension }, false)]
        public void ForbidTypesAttribute_ShouldForbidPointedTypes(string fileName, string[] forbidedExtensions, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new ForbidTypesAttribute(forbidedExtensions);

            Assert.Equal(expectedResult, attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData("test.zip", null)]
        [InlineData("test.zip", new string[] { })]
        public void ForbidTypesAttribute_ShouldThrowExceptionIfNoExtensionsProvided(string fileName, string[] allowedExtensions)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new ForbidTypesAttribute(allowedExtensions);

            Assert.Throws<InvalidOperationException>(() => attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData("test.png", false)]
        [InlineData("test.jpg", false)]
        [InlineData("test.bmp", false)]
        [InlineData("test.zip", true)]
        [InlineData("test.7z", true)]
        [InlineData("test.bz2", true)]
        [InlineData("test.gz", true)]
        [InlineData("365-doc.docx", false)]
        public void OnlyArchiveAttribute_ShouldReturnValidateIfTheFileIsArchive(string fileName, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowArchivesAttribute();

            Assert.Equal(expectedResult, attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData("test.png", true)]
        [InlineData("test.jpg", true)]
        [InlineData("test.bmp", true)]
        [InlineData("test.zip", false)]
        [InlineData("365-doc.docx", false)]
        public void OnlyImageAttribute_ShouldValidateIfTheFileIsImage(string fileName, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowImagesAttribute();

            Assert.Equal(expectedResult, attributeToTheory.IsValid(stream));
        }

        [Theory]
        [InlineData("test.exe")]
        public void ForbidExecutableFilesAttribute_ShouldReturnInvalidIfTheFileIsExecutable(string fileName)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new ForbidExecutablesAttribute();

            Assert.False(attributeToTheory.IsValid(stream));
        }

        [Fact]
        public void OnlyImageAttribute_ShouldValidateMultupleFilesIfTheyAreImages()
        {
            var fileNames = new[] { "test.png", "test.jpg", "test.bmp" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attrebuteToTheory = new AllowImagesAttribute();

            Assert.True(attrebuteToTheory.IsValid(files));
        }

        [Fact]
        public void OnlyImageAttribute_ShouldReturnReturnFalseIfOneOfFilesIsNotImage()
        {
            var fileNames = new[] { "test.png", "test.jpg", "test.bmp", "test.exe" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attrebuteToTheory = new AllowImagesAttribute();

            Assert.False(attrebuteToTheory.IsValid(files));
        }

        [Fact]
        public void AllowArchiveOnlyAttribute_ShouldCanValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.bz2", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowArchivesAttribute();
            Assert.True(attributeToTheory.IsValid(files));
        }

        [Fact]
        public void AllowArchiveOnlyAttribute_ShouldReturnTrueIfInputIsNull()
        {
            var attributeToTheory = new AllowArchivesAttribute();
            Assert.True(attributeToTheory.IsValid(null));
        }

        [Fact]
        public void AllowArchiveOnlyAttribute_ShouldReturnFalseIfOneOfTheFilesIsNotArchive()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.jpg", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowArchivesAttribute();
            Assert.False(attributeToTheory.IsValid(files));
        }

        [Fact]
        public void AllowTypesAttribute_ShouldValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowedTypesAttribute(ZipFile.TypeExtension, SevenZipFile.TypeExtension, Gzip.TypeExtension);
            Assert.True(attributeToTheory.IsValid(files));
        }

        [Fact]
        public void AllowTypesAttribute_ShouldReturnFalseIfOneOfTheFilesIsNotAllowed()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowedTypesAttribute(ZipFile.TypeExtension, SevenZipFile.TypeExtension);
            Assert.False(attributeToTheory.IsValid(files));
        }

        [Fact]
        public void ForbidExecutableAttribute_ShouldValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.jpg", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidExecutablesAttribute();
            Assert.True(attributeToTheory.IsValid(files));
        }

        [Fact]
        public void ForbidExecutableAttribute_ShouldReturnFalseIfOneOfFilesIsExecutable()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.exe", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidExecutablesAttribute();
            Assert.False(attributeToTheory.IsValid(files));
        }

        [Fact]
        public void ForbidTypesAttribute_ShouldValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidTypesAttribute(JointPhotographicExpertsGroup.TypeExtension);
            Assert.True(attributeToTheory.IsValid(files));
        }

        [Fact]
        public void ForbidTypesAttribute_ShouldReturnFalseIfOneOfFilesIsForbiden()
        {
            var fileNames = new[] { "test.zip", "test.jpg", "test.png" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidTypesAttribute(ZipFile.TypeExtension);
            Assert.False(attributeToTheory.IsValid(files));
        }
    }
}
