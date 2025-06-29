# Use the official .NET 9.0 SDK image
FROM mcr.microsoft.com/devcontainers/dotnet:1-9.0-bookworm

# Install additional tools that might be useful for .NET development
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
    && apt-get -y install --no-install-recommends \
        curl \
        git \
        wget \
        unzip \
        vim \
        nano \
        jq \
        tree \
    && apt-get autoremove -y && apt-get clean -y && rm -rf /var/lib/apt/lists/*

# Set the default user
USER vscode

# Verify .NET installation
RUN dotnet --version && dotnet --list-sdks
