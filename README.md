# .NET Multi-platform App UI (.NET MAUI)

[![Build Status](https://dev.azure.com/xamarin/public/_apis/build/status/MAUI-public?repoName=dotnet%2Fmaui&branchName=main&label=Public)](https://dev.azure.com/xamarin/public/_build/latest?definitionId=57&repoName=dotnet%2Fmaui&branchName=main) [![Build Status](https://devdiv.visualstudio.com/DevDiv/_apis/build/status/MAUI?repoName=dotnet%2Fmaui&branchName=main&label=Private)](https://devdiv.visualstudio.com/DevDiv/_build/latest?definitionId=13330&repoName=dotnet%2Fmaui&branchName=main)

[.NET Multi-platform App UI (.NET MAUI)](https://dotnet.microsoft.com/en-us/apps/maui) is a cross-platform framework for creating mobile and desktop apps with C# and XAML. Using .NET MAUI, you can develop apps that can run on Android, iOS, iPadOS, macOS, and Windows from a single shared codebase.

This is a Fork of GitHub maui created by Derby Russell.

This Fork is used for building a GTK 3 app.

* To use this place Derby Russell maui and Derby Russell GtkSharp in the same directory.
* Switch both to the derby-gtk3-integration-prism branch.
* See the README of GtkSharp and build GtkSharp as described.
* Then Open Microsoft.Maui-GTK.sln
* The controls samples-gtk show how to use this. Specifically Maui.Controls.Sample.OnePage.GTK
* You must put your project inside the branch, or create your own branch from that.  But in that case, please do not Push to GitHub!

Code is still under development.
If you improve the code here, then please Push to GitHub.

## Getting Started ##

* [Install .NET MAUI](https://dot.net/maui)
* [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui)
* [.NET MAUI Samples](https://github.com/dotnet/maui-samples)
* [Development Guide](./.github/DEVELOPMENT.md)

## Overview

.NET Multi-platform App UI (.NET MAUI) is the evolution of Xamarin.Forms that expands capabilities beyond mobile Android and iOS into desktop apps for Windows and macOS. With .NET MAUI, you can build apps that perform great for any device that runs Windows, macOS, Android, & iOS from a single codebase. Coupled with Visual Studio productivity tools and emulators, .NET and Visual Studio significantly speed up the development process for building apps that target the widest possible set of devices. Use a single development stack that supports the best of breed solutions for all modern workloads with a unified SDK, base class libraries, and toolchain. [Read More](https://docs.microsoft.com/dotnet/maui/what-is-maui)

![.NET MAUI Weather App on all platforms](Assets/maui-weather-hero-sm.png)

## Current News

* February 21, 2023 - [Announcing .NET 8 Preview 1](https://devblogs.microsoft.com/dotnet/announcing-dotnet-8-preview-1/)
* December 2, 2022 - Derby Russell published GTK 3.0 version for further development - sample builds and runs
* November 8, 2022 - [Announcing .NET MAUI for .NET 7 General Availability](https://devblogs.microsoft.com/dotnet/dotnet-maui-dotnet-7/)

Follow the [.NET MAUI Blog](https://devblogs.microsoft.com/dotnet/category/net-maui/) and visit the [News](https://github.com/dotnet/maui/wiki/News) wiki page for more news and updates.

## FAQs

Do you have questions? Do not worry, we have prepared a complete [FAQ](https://github.com/dotnet/maui/wiki/FAQs) answering the most common questions.

## How to Engage, Contribute, and Give Feedback

Some of the best ways to [contribute](./.github/CONTRIBUTING.md) are to try things out, file issues, join in design conversations,
and make pull-requests. Proposals for changes specific to MAUI can be found [here for discussion](https://github.com/dotnet/maui/issues).

See [CONTRIBUTING](./.github/CONTRIBUTING.md), [CODE-OF-CONDUCT](./.github/CODE_OF_CONDUCT.md) and the [Development Guide](./.github/DEVELOPMENT.md).
