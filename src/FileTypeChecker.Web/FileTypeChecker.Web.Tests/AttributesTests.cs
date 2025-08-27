namespace FileTypeChecker.Web.Tests
{
    using FileTypeChecker.Types;
    using FileTypeChecker.Web.Attributes;
    using NUnit.Framework;
    using System;

    [TestFixture]
    public class AttributesTests
    {
        [TestCase("365-doc.docx", true)]
        [TestCase("test.pdf", true)]
        [TestCase("test.doc", true)]
        [TestCase("test.png", false)]
        [TestCase("test.zip", false)]
        public void AllowDocumentsAttribute_ShouldAllowOnlyDocuments(string fileName, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowDocumentsAttribute();

            Assert.That(attributeToTheory.IsValid(stream), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase(new[] { "365-doc.docx", "test.pdf", "test.doc" }, true)]
        [TestCase(new[] { "365-doc.docx", "test.doc" }, true)]
        [TestCase(new[] { "365-doc.docx", "test.zip" }, false)]
        public void AllowDocumentsAttribute_ShouldAllowOnlyDocumentsIfTheInputIsCollection(string[] files, bool expectedResult)
        {
            var stream = FileHelpers.ReadFiles(files);

            var attributeToTheory = new AllowDocumentsAttribute();

            Assert.That(attributeToTheory.IsValid(stream), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("test.bmp", new[] { Bitmap.TypeExtension }, true)]
        [TestCase("test.bmp", new[] { JointPhotographicExpertsGroup.TypeExtension }, false)]
        [TestCase("test.jpg", new[] { JointPhotographicExpertsGroup.TypeExtension }, true)]
        [TestCase("test.jpg", new[] { Bitmap.TypeExtension, JointPhotographicExpertsGroup.TypeExtension, GraphicsInterchangeFormat89.TypeExtension }, true)]
        [TestCase("test.jpg", new[] { RarArchive.TypeExtension }, false)]
        [TestCase("test.zip", new[] { ZipFile.TypeExtension }, true)]
        public void AllowedTypeAttribute_ShouldAllowOnlyPointedTypes(string fileName, string[] allowedExtensions, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowedTypesAttribute(allowedExtensions);

            Assert.That(attributeToTheory.IsValid(stream), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("test.zip", null)]
        [TestCase("test.zip", new string[] { })]
        public void AllowTypesAttribute_ShouldThrowExceptionIfNoExtensionsProvided(string fileName, string[] allowedExtensions)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowedTypesAttribute(allowedExtensions);

            Assert.Throws<InvalidOperationException>(() => attributeToTheory.IsValid(stream));
        }

        [Test]
        [TestCase("test.zip", new[] { ZipFile.TypeExtension }, false)]
        [TestCase("test.png", new[] { ZipFile.TypeExtension }, true)]
        [TestCase("test.png", new[] { ZipFile.TypeExtension, ExtensibleArchive.TypeExtension, JointPhotographicExpertsGroup.TypeExtension }, true)]
        [TestCase("test.png", new[] { ZipFile.TypeExtension, PortableNetworkGraphic.TypeExtension, JointPhotographicExpertsGroup.TypeExtension }, false)]
        public void ForbidTypesAttribute_ShouldForbidPointedTypes(string fileName, string[] forbidedExtensions, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new ForbidTypesAttribute(forbidedExtensions);

            Assert.That(attributeToTheory.IsValid(stream), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("test.zip", null)]
        [TestCase("test.zip", new string[] { })]
        public void ForbidTypesAttribute_ShouldThrowExceptionIfNoExtensionsProvided(string fileName, string[] allowedExtensions)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new ForbidTypesAttribute(allowedExtensions);

            Assert.Throws<InvalidOperationException>(() => attributeToTheory.IsValid(stream));
        }

        [Test]
        [TestCase("test.png", false)]
        [TestCase("test.jpg", false)]
        [TestCase("test.bmp", false)]
        [TestCase("test.zip", true)]
        [TestCase("test.7z", true)]
        [TestCase("test.bz2", true)]
        [TestCase("test.gz", true)]
        [TestCase("365-doc.docx", false)]
        public void OnlyArchiveAttribute_ShouldReturnValidateIfTheFileIsArchive(string fileName, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowArchivesAttribute();

            Assert.That(attributeToTheory.IsValid(stream), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("test.png", true)]
        [TestCase("test.jpg", true)]
        [TestCase("test.bmp", true)]
        [TestCase("test.zip", false)]
        [TestCase("365-doc.docx", false)]
        public void OnlyImageAttribute_ShouldValidateIfTheFileIsImage(string fileName, bool expectedResult)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new AllowImagesAttribute();

            Assert.That(attributeToTheory.IsValid(stream), Is.EqualTo(expectedResult));
        }

        [Test]
        [TestCase("test.exe")]
        public void ForbidExecutableFilesAttribute_ShouldReturnInvalidIfTheFileIsExecutable(string fileName)
        {
            var stream = FileHelpers.ReadFile(fileName);

            var attributeToTheory = new ForbidExecutablesAttribute();

            Assert.That(attributeToTheory.IsValid(stream), Is.False);
        }

        [Test]
        public void OnlyImageAttribute_ShouldValidateMultupleFilesIfTheyAreImages()
        {
            var fileNames = new[] { "test.png", "test.jpg", "test.bmp" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attrebuteToTheory = new AllowImagesAttribute();

            Assert.That(attrebuteToTheory.IsValid(files), Is.True);
        }

        [Test]
        public void OnlyImageAttribute_ShouldReturnReturnFalseIfOneOfFilesIsNotImage()
        {
            var fileNames = new[] { "test.png", "test.jpg", "test.bmp", "test.exe" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attrebuteToTheory = new AllowImagesAttribute();

            Assert.That(attrebuteToTheory.IsValid(files), Is.False);
        }

        [Test]
        public void AllowArchiveOnlyAttribute_ShouldCanValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.bz2", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowArchivesAttribute();
            Assert.That(attributeToTheory.IsValid(files), Is.True);
        }

        [Test]
        public void AllowArchiveOnlyAttribute_ShouldReturnTrueIfInputIsNull()
        {
            var attributeToTheory = new AllowArchivesAttribute();
            Assert.That(attributeToTheory.IsValid(null), Is.True);
        }

        [Test]
        public void AllowArchiveOnlyAttribute_ShouldReturnFalseIfOneOfTheFilesIsNotArchive()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.jpg", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowArchivesAttribute();
            Assert.That(attributeToTheory.IsValid(files), Is.False);
        }

        [Test]
        public void AllowTypesAttribute_ShouldValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowedTypesAttribute(ZipFile.TypeExtension, SevenZipFile.TypeExtension, Gzip.TypeExtension);
            Assert.That(attributeToTheory.IsValid(files), Is.True);
        }

        [Test]
        public void AllowTypesAttribute_ShouldReturnFalseIfOneOfTheFilesIsNotAllowed()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new AllowedTypesAttribute(ZipFile.TypeExtension, SevenZipFile.TypeExtension);
            Assert.That(attributeToTheory.IsValid(files), Is.False);
        }

        [Test]
        public void ForbidExecutableAttribute_ShouldValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.jpg", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidExecutablesAttribute();
            Assert.That(attributeToTheory.IsValid(files), Is.True);
        }

        [Test]
        public void ForbidExecutableAttribute_ShouldReturnFalseIfOneOfFilesIsExecutable()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.exe", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidExecutablesAttribute();
            Assert.That(attributeToTheory.IsValid(files), Is.False);
        }

        [Test]
        public void ForbidTypesAttribute_ShouldValidateMultipleFiles()
        {
            var fileNames = new[] { "test.zip", "test.7z", "test.gz" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidTypesAttribute(JointPhotographicExpertsGroup.TypeExtension);
            Assert.That(attributeToTheory.IsValid(files), Is.True);
        }

        [Test]
        public void ForbidTypesAttribute_ShouldReturnFalseIfOneOfFilesIsForbiden()
        {
            var fileNames = new[] { "test.zip", "test.jpg", "test.png" };
            var files = FileHelpers.ReadFiles(fileNames);

            var attributeToTheory = new ForbidTypesAttribute(ZipFile.TypeExtension);
            Assert.That(attributeToTheory.IsValid(files), Is.False);
        }
    }
}
