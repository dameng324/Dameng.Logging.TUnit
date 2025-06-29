# Development Container for Dameng.Logging.TUnit

This folder contains the configuration for a development container that provides a complete .NET development environment for the Dameng.Logging.TUnit project.

## What's Included

- **.NET 9.0 SDK** - The latest .NET SDK with all required tools
- **VS Code Extensions** - Pre-configured extensions for .NET development:
  - C# Dev Kit
  - C# extension
  - .NET Runtime extension
  - PowerShell
  - REST Client
  - .NET Test Explorer
  - NuGet Package Manager
- **Global .NET Tools**:
  - `dotnet-ef` - Entity Framework Core tools
  - `dotnet-format` - Code formatting tool
  - `dotnet-outdated-tool` - Check for outdated packages
  - `dotnet-reportgenerator-globaltool` - Generate test coverage reports
- **Additional Tools**:
  - Git
  - GitHub CLI
  - Node.js LTS (for any front-end tooling)

## How to Use

1. **Prerequisites**: 
   - Install [Docker](https://www.docker.com/products/docker-desktop)
   - Install [VS Code](https://code.visualstudio.com/)
   - Install the [Dev Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers)

2. **Open in Container**:
   - Open this repository in VS Code
   - Press `F1` and select "Dev Containers: Reopen in Container"
   - Or click the notification to reopen in container when prompted

3. **What Happens Automatically**:
   - The container builds with .NET 9.0 SDK
   - All dependencies are restored (`dotnet restore`)
   - The project is built (`dotnet build`)
   - VS Code extensions are installed
   - Git configuration is mounted from your host machine

## Development Workflow

Once the container is running, you can:

```bash
# Build the solution
dotnet build

# Run tests
dotnet test

# Pack NuGet package
dotnet pack

# Format code
dotnet format
```

## Customization

You can customize the development environment by:

- Modifying `.devcontainer/devcontainer.json` for VS Code settings and extensions
- Updating `.devcontainer/Dockerfile` to add additional tools or packages
- Adding additional VS Code settings in the `customizations.vscode.settings` section

## Troubleshooting

- **Container won't start**: Make sure Docker is running and you have sufficient disk space
- **Extensions not loading**: Try rebuilding the container with "Dev Containers: Rebuild Container"
- **Git authentication issues**: The container mounts your host Git configuration, ensure your Git credentials are properly configured on your host machine
