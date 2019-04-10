# Bot v4.3 Starter Template
This is a .net template project which can be used as a starter for a v4.3 .net bot with the up-to-date patterns and practices.

At this time, there is no NodeJS equivalent (but that would make a great pull request).

The life of this project may be limited until such a time that there is an official v4.3 project template which adopts these patterns. Until such a template exists, you can use this project as a great starting point for a v4.3 .net Bot Framework project.

This sample is complimentary to the [Bot Framework Samples - Work In progress Branch](https://github.com/Microsoft/BotBuilder-Samples/tree/samples-work-in-progress/samples/csharp_dotnetcore). Many of the patterns used in this sample are taken from common patterns seen in these official samples.

This sample includes the following:

* Bot project with 4.3 NuGet packages
* Up-to-date patterns around `StartUp.cs`, `Program.cs`, `BotController.cs`, the main `ActivityHandler` architecture and `dialog` constrctors
* Basic dialog system with a root dialog and multiple child dialogs
* Global state being updated and used accross `ActivityHandler` and `Dialog`s
* Passing objects to dialogs on construction
* Strings using RESX files
* A placeholder (commented out) example of using Dispatch, Luis and QNAMaker

## To use

There are multiple ways you can use this project.

### Install the Visual Studio template

There is a Visual Studio template based on the contents of the `src` folder in this repository.

You can download the VSIX [here](https://github.com/martinkearn/Bot-v4.3-Template/raw/master/vsix/StarterBot.vsix). After installation, create a project based on the template as follows (steps optimized for Visual Studio 2019):

* Launch Visual Studio
* `Create new project`
* Search for "starterbot"
* Select `StarterBot v4.3`
* Follow the usual new project steps

This is probably the simplest way to use this template, but may not always be up-to-date with the very latest version of the project because the VSIX needs to be manually built and is not automated on every commit.

You can see the VSIX project itself at [/vsix/src/Templates](https://github.com/martinkearn/Bot-v4.3-Template/tree/master/vsix/src/Templates)

### Clone and rename

You can clone this repository and adapt to suit your needs.

This approach is good if you want to make sure you have the very latest version of the template or simply want to explore the code without creating a new project.

Follow these steps:

1. Clone this repository
2. Rename the [/src/StarterBot.sln](https://github.com/martinkearn/Bot-v4.3-Template/blob/master/src/StarterBot.sln) solution in Visual Studio
3. Rename the [/src/StarterBot.csproj](https://github.com/martinkearn/Bot-v4.3-Template/blob/master/src/StarterBot.csproj) project in Visual Studio
4. Set your new namespace in the project settings
5. Find and replace all instances of `namespace StarterBot` with your new namespace

## Credits
This was a collaborative effort between these main contributors:
* [Martin Kearn, Microsoft](https://github.com/martinkearn)
* [Ibrahim Kivanc, Microsoft](https://github.com/ikivanc)
* [Martin Simecek, Microsoft](https://github.com/msimecek)
