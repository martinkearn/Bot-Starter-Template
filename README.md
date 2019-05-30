# Bot Starter Template for v4.4.4
This can be used as a starter template for a .net bot with the latest patterns and practices. 

At this time, there is no NodeJS equivalent (but that would make a great pull request).

The latest Bot Framework version is v4.4 but there is no official template. The life of this project may be limited until such a time that there is an official project template which adopts these patterns. Until such a template exists, you can use this project as a great starting point for a v4.4 .net Bot Framework project.

This sample is complimentary to the [Bot Framework Samples - Work In progress Branch](https://github.com/Microsoft/BotBuilder-Samples/tree/samples-work-in-progress/samples/csharp_dotnetcore). Many of the patterns used in this sample are taken from common patterns seen in these official samples.

This sample includes the following:

* Bot project with v4.4.4 `Microsoft.Bot.Builder.*` NuGet packages
* Up-to-date patterns around `StartUp.cs`, `Program.cs`, `BotController.cs`, the main `ActivityHandler` architecture and `dialog` constrctors
* Basic dialog system with a root dialog and multiple child dialogs
* Global state being updated and used accross `ActivityHandler` and `Dialog`s
* Passing objects to dialogs on construction
* Strings using RESX files
* A placeholder (commented out) example of using Dispatch, Luis and QNAMaker

## To use

There are multiple ways you can use this project.

### Install the Visual Studio 2019 template

There is a Visual Studio 2019 template based on the contents of the `src` folder in this repository.

To use the VSIX, follow these steps (steps optimized for Visual Studio 2019)

1. Download `StarterBot.vsix` from [here](https://github.com/martinkearn/Bot-v4.3-Template/raw/master/vsix/StarterBot.vsix) 
2. Install `StarterBot.vsix`
3. Launch Visual Studio 2019
4. `Create new project`
5. Search for "starterbot"
6. Select the `StarterBot` template
7. Set a `Project name`, `Location` and `Solution name` as required and `Create`

This is only compatible with Visual Studio 2019. If you have Visual Studio 2017, see the 'Clone and rename' section.

This is the simplest way to use this template, but may not always be up-to-date because the VSIX needs to be manually built and is not automated on every commit. If you want to be sure to have the latest version, please see the 'Clone and rename' section.

You can see the VSIX project itself at [/vsix/src](https://github.com/martinkearn/Bot-v4.3-Template/tree/master/vsix/src).

### Clone and rename

You can clone this repository and adapt to suit your needs.

This approach is appropriate if you want the very latest version of the template or simply want to explore the code without creating a new project.

Follow these steps:

1. Clone this repository
2. Rename the [/src/StarterBot.sln](https://github.com/martinkearn/Bot-v4.3-Template/blob/master/src/StarterBot.sln) solution in Visual Studio
3. Rename the [/src/StarterBot.csproj](https://github.com/martinkearn/Bot-v4.3-Template/blob/master/src/StarterBot.csproj) project in Visual Studio
4. Set your new namespace in the project settings
5. Find and replace all instances of `namespace StarterBot` with your new namespace
6. Replace all instances of `StarterBot` in the resource files (`SharedStrings.resx`, `CountryStrings.resx`, `NameAgeStrings.resx` and `RootStrings.resx` ) with your new namespace by simple re-saving the resource files

## Credits
This was a collaborative effort between these main contributors:
* [Martin Kearn, Microsoft](https://github.com/martinkearn)
* [Ibrahim Kivanc, Microsoft](https://github.com/ikivanc)
* [Martin Simecek, Microsoft](https://github.com/msimecek)
