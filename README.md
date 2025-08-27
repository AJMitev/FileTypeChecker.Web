# 🛡️ FileTypeChecker.Web

<div align="center">

![FileTypeChecker Logo](https://raw.githubusercontent.com/AJMitev/FileTypeChecker.Web/master/tools/FileTypeCheckerLogo-150.png)

**Secure file type validation for ASP.NET applications using magic number detection**

[![Build Status](https://ci.appveyor.com/api/projects/status/jwd8wyyap0tijs3y?svg=true)](https://ci.appveyor.com/project/AJMitev/filetypechecker-web)
[![NuGet](https://img.shields.io/nuget/v/File.TypeChecker.Web.svg)](https://www.nuget.org/packages/File.TypeChecker.Web/)
[![Downloads](https://img.shields.io/nuget/dt/File.TypeChecker.Web?color=blue)](https://www.nuget.org/packages/File.TypeChecker.Web/)
[![License: MIT](https://img.shields.io/github/license/ajmitev/filetypechecker.web)](https://github.com/AJMitev/FileTypeChecker.Web/blob/master/LICENSE)
[![CodeFactor](https://www.codefactor.io/repository/github/ajmitev/filetypechecker.web/badge)](https://www.codefactor.io/repository/github/ajmitev/filetypechecker.web)
[![Discord](https://img.shields.io/discord/1035464819102470155?logo=discord)](https://discord.gg/your-discord-invite)

</div>

## ✨ Overview

FileTypeChecker.Web is a powerful ASP.NET Core extension that provides reliable file type identification using magic number detection. Built on top of [FileTypeChecker](https://github.com/AJMitev/FileTypeChecker), this library offers validation attributes for `IFormFile` objects, making it easy to secure your web applications from malicious file uploads through simple, declarative validation.

## 📋 Table of Contents

- [🚀 Quick Start](#-quick-start)
- [💡 Why Use FileTypeChecker.Web?](#-why-use-filetypecheckerweb)
- [⚙️ How It Works](#️-how-it-works)
- [📦 Installation](#-installation)
- [🔧 Usage Examples](#-usage-examples)
  - [🏷️ Validation Attributes](#️-validation-attributes)
  - [🎮 Controller Examples](#-controller-examples)
  - [📝 Model Examples](#-model-examples)
  - [⚡ Advanced Usage](#-advanced-usage)
- [🛠️ Configuration](#️-configuration)
- [📄 Supported File Types](#-supported-file-types)
- [🧪 Development](#-development)
- [🤝 Contributing](#-contributing)
- [💖 Support the Project](#-support-the-project)
- [📝 License](#-license)

## 🚀 Quick Start

```csharp
// 1. Install the package
dotnet add package File.TypeChecker.Web

// 2. Register in Program.cs/Startup.cs
builder.Services.AddFileTypesValidation(typeof(Program).Assembly);

// 3. Use in your controller
[HttpPost("upload")]
public IActionResult Upload([AllowImages] IFormFile image)
{
    // File is guaranteed to be a valid image
    return Ok("Image uploaded successfully!");
}
```

## 💡 Why Use FileTypeChecker.Web?

### 🎯 The Problem

Traditional web file validation relies on file extensions and MIME types, both easily manipulated:

- A malicious executable can be renamed from `.exe` to `.jpg`
- HTTP headers can be spoofed to fake MIME types
- `IFormFile.ContentType` property is provided by the client and cannot be trusted
- Basic validation leaves your application vulnerable to malicious file uploads

### ✅ The Solution

FileTypeChecker.Web analyzes the actual file content using magic numbers:

- **Reliable**: Identifies files by their binary signature, not filename or headers
- **Secure**: Prevents malicious files from masquerading as safe formats
- **Easy**: Simple validation attributes that integrate with ASP.NET Core ModelBinding
- **Fast**: Minimal performance overhead with efficient binary analysis
- **Comprehensive**: Built-in support for images, documents, archives, and executables

## ⚙️ How It Works

FileTypeChecker.Web uses **magic numbers** (binary signatures) to identify file types. These are specific byte sequences found at the beginning of files that uniquely identify the format.

### 🔍 Magic Number Examples

```
PDF:  25 50 44 46  (%PDF)
PNG:  89 50 4E 47  (‰PNG)
JPEG: FF D8 FF     (ÿØÿ)
ZIP:  50 4B 03 04  (PK..)
EXE:  4D 5A        (MZ)
```

When you upload a file with a `.jpg` extension, the library reads the first few bytes. If it finds `4D 5A` (the EXE signature) instead of `FF D8 FF` (JPEG signature), it knows the file is actually an executable and rejects it.

> 📖 Learn more about [Magic Numbers on Wikipedia](https://en.wikipedia.org/wiki/File_format#Magic_number)

## 📦 Installation

### Package Manager

```powershell
Install-Package File.TypeChecker.Web
```

### .NET CLI

```bash
dotnet add package File.TypeChecker.Web
```

### PackageReference

```xml
<PackageReference Include="File.TypeChecker.Web" Version="1.2.4" />
```

**Requirements**: .NET Standard 2.1+ (ASP.NET Core 3.1+)

## 🔧 Usage Examples

### 🏷️ Validation Attributes

| Attribute             | Description                                                  |
| --------------------- | ------------------------------------------------------------ |
| `[AllowImages]`       | Restricts to image formats (JPEG, PNG, GIF, BMP, TIFF)       |
| `[AllowArchives]`     | Restricts to archive formats (ZIP, RAR, 7Z, TAR, GZIP, etc.) |
| `[AllowDocuments]`    | Restricts to document formats (PDF, DOC, DOCX, etc.)         |
| `[AllowedTypes]`      | Allows specific file types using `FileExtension` enum        |
| `[ForbidExecutables]` | Prevents executable file uploads (EXE, DLL, ELF, etc.)       |
| `[ForbidTypes]`       | Prevents specific file types using `FileExtension` enum      |

### 🎮 Controller Examples

```csharp
using FileTypeChecker.Web.Attributes;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    [HttpPost("upload-image")]
    public IActionResult UploadImage([AllowImages] IFormFile image)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // File is guaranteed to be a valid image
        // Process your image here...
        return Ok(new { Message = "Image uploaded successfully!",
                        FileName = image.FileName });
    }

    [HttpPost("upload-document")]
    public IActionResult UploadDocument([AllowDocuments] IFormFile document)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid file type. Only documents are allowed.");

        // File is guaranteed to be a valid document
        return Ok("Document uploaded successfully!");
    }

    [HttpPost("upload-safe-file")]
    public IActionResult UploadSafeFile([ForbidExecutables] IFormFile file)
    {
        if (!ModelState.IsValid)
            return BadRequest("Executable files are not allowed for security reasons.");

        // File is guaranteed not to be an executable
        return Ok("File uploaded successfully!");
    }

    [HttpPost("upload-media")]
    public IActionResult UploadMedia(
        [AllowedTypes(FileExtension.Jpeg, FileExtension.Mp4, FileExtension.Mp3)]
        IFormFile media)
    {
        // Accepts only JPEG images, MP4 videos, or MP3 audio files
        return Ok("Media file uploaded successfully!");
    }
}
```

### 📝 Model Examples

```csharp
using FileTypeChecker.Web.Attributes;
using System.ComponentModel.DataAnnotations;

public class FileUploadModel
{
    [Required]
    [AllowImages]
    [Display(Name = "Profile Picture")]
    public IFormFile ProfilePicture { get; set; }

    [AllowArchives]
    [Display(Name = "Backup Archive")]
    public IFormFile BackupFile { get; set; }

    [AllowedTypes(FileExtension.Bitmap, FileExtension.Png)]
    [Display(Name = "Logo (Bitmap/PNG only)")]
    public IFormFile Logo { get; set; }

    [ForbidExecutables]
    [Display(Name = "Safe File Upload")]
    public IFormFile SafeFile { get; set; }

    [ForbidTypes(FileExtension.Doc, FileExtension.Docx)]
    [Display(Name = "Non-Word Document")]
    public IFormFile NonWordDocument { get; set; }
}
```

### ⚡ Advanced Usage

```csharp
// Multiple validation attributes
public class DocumentModel
{
    [Required]
    [AllowDocuments]
    [ForbidExecutables] // Extra safety - redundant but explicit
    public IFormFile Document { get; set; }
}

// Custom validation in action methods
[HttpPost("validate-manually")]
public IActionResult ValidateManually(IFormFile file)
{
    // Manual validation using extension methods
    if (file.IsExecutable())
    {
        return BadRequest("Executable files are not allowed");
    }

    if (!file.IsTypeRecognizable())
    {
        return BadRequest("Unknown file type");
    }

    var fileType = file.GetFileType();
    return Ok($"File type: {fileType.Name} ({fileType.Extension})");
}

// Bulk file validation
[HttpPost("upload-multiple")]
public IActionResult UploadMultiple([AllowImages] IFormFile[] images)
{
    // All files are guaranteed to be valid images
    return Ok($"Uploaded {images.Length} images successfully!");
}
```

> 📚 More examples available in our [Wiki](https://github.com/AJMitev/FileTypeChecker/wiki/How-to-use%3F)

## 🛠️ Configuration

Register the file type validation service in your application startup:

### ASP.NET Core 6.0+ (Program.cs)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register file type validation
builder.Services.AddFileTypesValidation(typeof(Program).Assembly);
builder.Services.AddControllers();

var app = builder.Build();
```

### ASP.NET Core 5.0 and earlier (Startup.cs)

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // Register file type validation with custom types from specified assemblies
    services.AddFileTypesValidation(typeof(Startup).Assembly);

    // Other service registrations...
    services.AddControllers();
}
```

### Custom Error Messages

```csharp
[AllowImages(ErrorMessage = "Please upload only image files (JPEG, PNG, GIF, BMP, TIFF).")]
public IFormFile ProfileImage { get; set; }
```

## 📄 Supported File Types

FileTypeChecker.Web supports **30+ file formats** across multiple categories:

### 🖼️ Images

- **PNG** - Portable Network Graphics
- **JPEG** - Joint Photographic Experts Group
- **GIF** - Graphics Interchange Format
- **BMP** - Bitmap Image File
- **TIFF** - Tagged Image File Format
- **WebP** - WebP Image Format
- **ICO** - Icon File
- **PSD** - Photoshop Document

### 📄 Documents

- **PDF** - Portable Document Format
- **DOC/DOCX** - Microsoft Word Documents
- **XLS/XLSX** - Microsoft Excel Spreadsheets
- **PPT/PPTX** - Microsoft PowerPoint Presentations

### 🗜️ Archives

- **ZIP** - ZIP Archive
- **RAR** - RAR Archive
- **7Z** - 7-Zip Archive
- **TAR** - TAR Archive
- **GZIP** - GNU Zip
- **BZIP2** - BZIP2 Compressed File

### 💻 Executables & Scripts

- **EXE** - Windows Executable
- **DLL** - Dynamic Link Library
- **ELF** - Executable and Linkable Format

### ➕ Extensible

Add your own custom file types by implementing the `IFileType` interface in the core [FileTypeChecker](https://github.com/AJMitev/FileTypeChecker) library.

> 📋 Complete list available in our [Wiki](https://github.com/AJMitev/FileTypeChecker/wiki/What-types-of-file-are-supported%3F)

## 🧪 Development

### Requirements

- .NET SDK 3.1 or later
- Visual Studio 2019+ or VS Code
- Git

### Building the Project

```bash
# Clone the repository
git clone https://github.com/AJMitev/FileTypeChecker.Web.git

# Navigate to the solution
cd FileTypeChecker.Web/src/FileTypeChecker.Web

# Restore packages and build
dotnet restore
dotnet build
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Sample Application

```bash
# Run the sample web application
cd samples/FileTypeChecker.WebApp
dotnet run

# Navigate to https://localhost:5001
```

### Project Structure

```
FileTypeChecker.Web/
├── src/
│   └── FileTypeChecker.Web/
│       ├── FileTypeChecker.Web/          # Main library
│       ├── FileTypeChecker.Web.Tests/    # Unit tests
│       └── FileTypeChecker.Web.sln       # Solution file
├── samples/
│   └── FileTypeChecker.WebApp/           # Sample application
└── tools/
    └── FileTypeCheckerLogo-150.png       # Logo
```

## 🤝 Contributing

We welcome contributions! Please see our [Contributing Guidelines](CONTRIBUTING.md) for details.

### How to Contribute

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Areas for Contribution

- 🐛 Bug fixes and improvements
- 📚 Documentation enhancements
- 🧪 Additional test coverage
- 🎨 Sample applications and examples
- 🌐 Localization support

## 💖 Support the Project

If this library helps you, consider supporting its development:

- ⭐ **Star the repository** and share it with others
- ☕ [**Buy me a coffee**](https://www.buymeacoffee.com/ajmitev) for continued development
- 👥 [**Become a member**](https://buymeacoffee.com/ajmitev/membership) for direct access to maintainers
- 🐛 Report bugs and suggest features via [GitHub Issues](https://github.com/AJMitev/FileTypeChecker.Web/issues)

<a href="https://www.buymeacoffee.com/ajmitev" target="_blank">
  <img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" height="41" width="174">
</a>

## 📝 License

This project is licensed under the [MIT License](LICENSE) - see the LICENSE file for details.

## 🌐 Related Projects

- [**FileTypeChecker**](https://github.com/AJMitev/FileTypeChecker) - The core library for file type detection
- [**FileTypeChecker Console Examples**](https://github.com/AJMitev/FileTypeChecker/tree/master/examples) - Command-line usage examples

## 🙏 Credits

- Built on top of [FileTypeChecker](https://github.com/AJMitev/FileTypeChecker) core library
- Inspired by the need for secure file validation in web applications
- Thanks to all [contributors](https://github.com/AJMitev/FileTypeChecker.Web/graphs/contributors) who help improve this project

---

<div align="center">
  <strong>Made with ❤️ by <a href="https://github.com/AJMitev">Aleksandar J. Mitev</a></strong><br>
  <em>Securing web applications one file upload at a time</em>
</div>
