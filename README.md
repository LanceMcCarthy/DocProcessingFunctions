# DocProcessingFunctions
A set of Azure C# Functions that leverage Telerik Document Processing Libraries (aka DPL) in a fast and efficient manner.

The relevant Telerik assemblies this project uses are the .NET Standard 2.0 compliant versions. Currently these are only distributed with [Telerik UI for Xamarin](https://www.telerik.com/xamarin-ui).

This project is referencing the trial version of the DPL assemblies, located in the adjacent `TelerikLibs` folder.

> There is a current issue in which the Telerik assemblies are not being packaged/ published with the finction, so it fails when you attempt to publish. The workaround is to use the `Telerik.UI.for.Xamarin` (or `Telerik.UI.for.Xamarin.Trial`) NuGet package instead. [See this documentation for full instructions](https://docs.telerik.com/devtools/xamarin/installation-and-deployment/telerik-nuget-server).
