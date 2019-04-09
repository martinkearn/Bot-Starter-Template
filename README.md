# Bot v4.3 .net Starter Template
This is a .net template project which can be used as a starter for a v4.3 bot with the up-to-date patterns and practices.

At this time, there is no Node equivalent (but that would make a great pull request).

The life of this project may be limited until such a time that there is an official v4.3 project template which adopts these patterns. Until such a template exists, you can use this project as a great starting point for a v4.3 .net Bot Framework project.

This sample is complimentary to the [Bot Framework Samples - Work In progress Branch](https://github.com/Microsoft/BotBuilder-Samples/tree/samples-work-in-progress/samples/csharp_dotnetcore). Many of the patterns used in this sample are taken from common patterns seen in these official samples.

This sample includes the following:

* Bot project with 4.3 NuGet packages
* Up-to-date patterns around `StartUp.cs`, `Program.cs`, `BotController.cs` and the main `ActivityHandler` architecture
* Basic dialog system with a root dialog and multiple child dialogs
* Welcome message with state
* Strings interface

## To use

To use this sample as a starting point for your project, follow these steps:

1. Clone the repository
2. Rename the solution in Visual Studio
3. Rename the project in Visual Studio
4. Set your new namespace in the project settings
5. Find and replace all instances of `namespace StarterBot` with your new name space