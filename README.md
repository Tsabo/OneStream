# OneStream Candidate Assessment

![Simple CRUD Example](https://github.com/user-attachments/assets/4f20030c-20e3-4e51-87ff-10ef2b575765)

![architecture](https://github.com/user-attachments/assets/888a58ed-99a0-40ba-a544-632ab2b000e8)

## Prerequisites 

This project uses .NET Aspire. To work with .NET Aspire, you'll need the following installed locally:

- [.NET 8.0.](https://dotnet.microsoft.com/download/dotnet/8.0)
- .NET Aspire workload (installed either Visual Studio or the .NET CLI).
- An OCI compliant container runtime, such as:
    - [Docker Desktop](https://www.docker.com/products/docker-desktop) or [Podman](https://podman.io/). For more information, see [Container runtime](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio#container-runtime).
- An Integrated Developer Environment (IDE) or code editor, such as:
    - [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) version 17.10 or higher (Optional)
    - [Visual Studio Code](https://code.visualstudio.com/) (Optional)
    - [C# Dev Kit: Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit) (Optional)

For more information, see [.NET Aspire setup and tooling](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/setup-tooling?tabs=windows&pivots=visual-studio)

## How to get started
1. Clone the repo:
```shell
git clone https://github.com/Tsabo/OneStream.git
```
2. Change to the projects directory
```shell
cd ./OneStream
```
3. Ensure you have the latest Aspire and WebAssembly build tools installed:
```shell
dotnet workload update
```
4. To install the .NET Aspire and the WebAssembly build tools workload from the .NET CLI, use the dotnet workload install command:
```shell
dotnet workload install aspire wasm-tools
```
5. Run the project
```shell
dotnet run --project ./Source/src/AppHost
```
6. Open a browser and navigate to https://localhost:7120/

## .NET Aspire Dashboard

Visual Studio launches both your app and the .NET Aspire dashboard for you automatically in your browser. If you start the app using the .NET CLI, copy and paste the dashboard URL from the output into your browser, or hold Ctrl and select the link (if your terminal supports hyperlinks).

![A screenshot showing how to launch the dashboard using the CLI.](https://github.com/user-attachments/assets/561d7add-c483-4eaa-99b5-fc42c9f22b40)
