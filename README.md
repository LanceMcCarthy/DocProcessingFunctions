# DocProcessingFunctions
A set of Azure C# Functions that leverage Telerik Document Processing Libraries in a fast and efficient manner.

The relevant Telerik assemblies this project uses are the .NET Standard 2.0 compliant versions. Currently these are only distributed with [Telerik UI for Xamarin](https://www.telerik.com/xamarin-ui).


Two options to add the Telerik references to the project:

1. Use the `Telerik.UI.for.Xamarin` (or `Telerik.UI.for.Xamarin.Trial`) NuGet package. [See this documentation for full instructions](https://docs.telerik.com/devtools/xamarin/installation-and-deployment/telerik-nuget-server).
2. Directly reference the assemblies from the UI for Xamarin installation folder - `C:\Program Files (x86)\Progress\Telerik UI for Xamarin [release number]\Binaries\Portable`

> There's a current problem being investigated that might prevent option 2 from working. It seems that when using direct assembly references, the assemblies are not copied and published to the wwwroot/Bin folder. This is being investigated.