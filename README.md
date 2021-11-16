<h1><img src="https://raw.githubusercontent.com/AJMitev/FileTypeChecker.Web/master/tools/FileTypeCheckerLogo-150.png" align="left" alt="FileTypeChecker" width="90">FileTypeChecker.Web - Don't let users to inject you an invalid file</h1>

[![Build status](https://ci.appveyor.com/api/projects/status/jx9bcrxs95srhxsj?svg=true)](https://ci.appveyor.com/project/AJMitev/filetypechecker)  [![NuGet Badge](https://buildstats.info/nuget/File.TypeChecker.Web)](https://www.nuget.org/packages/File.TypeChecker.Web/)

## Project Description

FileTypeChecker.Web is a easy to use library that provides quality identification of a file type. This will help you to secure your applications and validate all files that are provided by external sources. It enables you to easily allow or forbid certain types of files in your controllers or input models.


## Why to use it?

Have you ever had a requirement for users to be able to upload files of a certain type? How do you validate that the file type is allowed? How do you protect your application from uploading a malicious file? It is standard practice to use the [FileSystemInfo](https://docs.microsoft.com/en-us/dotnet/api/system.io.fileinfo?view=netcore-3.1#definition) class provided by Microsoft and its [Extension](https://docs.microsoft.com/en-us/dotnet/api/system.io.filesysteminfo.extension?view=netcore-3.1#System_IO_FileSystemInfo_Extension) property for this kind of job, but is that enough? The answer is simple - No! This is why this small but effective library comes to help.

## How it works?

FileTypeChecker use file's "magic numbers" to identify the type. According to Wikipedia this term ("magic numbers") was used for a specific set of 2-byte identifiers at the beginnings of files, but since any binary sequence can be regarded as a number, any feature of a file format which uniquely distinguishes it can be used for identification. This approach offers better guarantees that the format will be identified correctly, and can often determine more precise information about the file. [See more about Magic Numbers](https://en.wikipedia.org/wiki/File_format#Magic_number)

## How to install?

You can install this library using NuGet into your project.

```nuget
Install-Package File.TypeChecker.Web
```

or by using dotnet CLI

```
dotnet add package File.TypeChecker.Web
```

## How to use?

With this library you will recive access to some powerful validation attributes that will enable you to easily allow or forbid which types of file your web application will accept. For example you can restrict your users to be able to upload only images or only archives just by setting an attribute into your method or class.

All validation attributes should be used over IFormFile interface and can be used in a class over property or with method parameter.

- AllowImages: This validation attribute will restrict IFormFile to be only image format like jpg, gif, bmp, png and tiff
- AllowArchives: This validation attribute will restrict IFormFile to be only archive.
- AllowDocuments: This validation attribute will restrict IFormFile to be only document.
- AllowedTypes: This validation attribute will allow you to specify what types of file you want to receive from user. We advice you to use FileExtension class to specify the extension string.
- ForbidExecutables: This validation attribute will forbid your users to upload executable files.
- ForbidTypes: This validation attribute will allow you to specify what types of file you don't want to recive from user. We advice you to use FileExtension class to specify the extension string.

```c#
[HttpPost("filesUpload")]
public IActionResult UploadFiles([AllowImages] IFormFile imageFile, [AllowArchives] IFormFile archiveFile)
{
    // Some cool code here ...
}
```

```c#
using FileTypeChecker.Web.Attributes;

public class InputModel
{
    [AllowImages]
    public IFormFile FirstFile { get; set; }

    [AllowArchives]
    public IFormFile SecondFile { get; set; }

    [AllowedTypes(FileExtension.Bitmap)]
    public IFormFile ThirdFile { get; set; }

    [ForbidExecutables]
    public IFormFile FourthFile { get; set; }

    [ForbidTypes(FileExtension.Doc)]
    public IFormFile FifthFile { get; set; }
}
```
If you are interested in finding more samples please use our [wiki page](https://github.com/AJMitev/FileTypeChecker/wiki/How-to-use%3F).

## What types of file are supported?

FileTypeChecker.Web is able to identify more than 22 different types but also you are able to register your own types. 

```c#
public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // More registration here ...

            services.AddFileTypesValidation(typeof(InputModel).Assembly);
        }
    }
```

For more information please visit our [wiki page](https://github.com/AJMitev/FileTypeChecker/wiki/What-types-of-file-are-supported%3F)

## Support the project

- If you like this library, ⭐️ the repository and show it to your friends!
- If you find this library usefull and it helps you please consider to support the project, you can do by buying me a cup of coffee.

<a href="https://www.buymeacoffee.com/ajmitev" target="_blank"><img src="https://www.buymeacoffee.com/assets/img/custom_images/orange_img.png" alt="Buy Me A Coffee" style="height: 41px !important;width: 174px !important;box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;-webkit-box-shadow: 0px 3px 2px 0px rgba(190, 190, 190, 0.5) !important;" ></a>
