#!/bin/bash

# ShiftPilot Development Setup Script for Linux/macOS
# This script sets up the development environment

set -e

echo "================================================"
echo "   🔧 ShiftPilot Dev Setup - Linux/macOS     "
echo "================================================"
echo ""

# Color codes
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m'

print_status() {
    echo -e "${GREEN}[✓]${NC} $1"
}

print_info() {
    echo -e "${YELLOW}[i]${NC} $1"
}

print_error() {
    echo -e "${RED}[✗]${NC} $1"
}

# Step 1: Install .NET 8.0 (if needed)
echo ""
print_info "Checking .NET 8.0 SDK..."
echo ""

if ! command -v dotnet &> /dev/null; then
    print_info "Installing .NET 8.0 SDK..."
    
    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Linux installation
        wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
        chmod +x dotnet-install.sh
        ./dotnet-install.sh --version 8.0
        rm dotnet-install.sh
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS installation using Homebrew
        brew install dotnet
    fi
fi

if dotnet --version > /dev/null 2>&1; then
    print_status ".NET SDK ready: $(dotnet --version)"
else
    print_error "Failed to install .NET SDK"
    exit 1
fi

# Step 2: Install Docker Desktop (if needed)
echo ""
print_info "Checking Docker..."
echo ""

if ! command -v docker &> /dev/null; then
    print_info "Installing Docker..."
    
    if [[ "$OSTYPE" == "linux-gnu"* ]]; then
        # Linux Docker installation
        curl -fsSL https://get.docker.com -o get-docker.sh
        sudo sh get-docker.sh
        rm get-docker.sh
    elif [[ "$OSTYPE" == "darwin"* ]]; then
        # macOS - guide user to install Docker Desktop
        print_error "Please download Docker Desktop from https://www.docker.com/products/docker-desktop"
        exit 1
    fi
fi

if docker --version > /dev/null 2>&1; then
    print_status "Docker ready: $(docker --version)"
else
    print_error "Failed to install Docker"
    exit 1
fi

# Step 3: Install Visual Studio Code extensions (optional)
echo ""
print_info "Setting up VS Code extensions (optional)..."
echo ""

if command -v code &> /dev/null; then
    print_info "Installing recommended VS Code extensions..."
    code --install-extension ms-dotnettools.csharp
    code --install-extension ms-dotnettools.vscode-dotnet-runtime
    code --install-extension ms-mssql.mssql
    print_status "VS Code extensions installed"
else
    print_info "VS Code not found, skipping extension installation"
fi

# Step 4: Create environment files
echo ""
print_info "Creating environment configuration files..."
echo ""

if [ ! -f ".env" ]; then
    cp .env.example .env
    print_status ".env file created (update with your values)"
else
    print_info ".env file already exists"
fi

# Step 5: Restore packages and build
echo ""
print_info "Restoring NuGet packages..."
echo ""

if dotnet restore > /dev/null 2>&1; then
    print_status "Packages restored"
else
    print_error "Failed to restore packages"
    exit 1
fi

# Step 6: Summary
echo ""
echo "================================================"
echo "   ✅ Development environment setup complete!"
echo "================================================"
echo ""
echo "Next steps:"
echo "1. Update .env file with your configuration"
echo "2. Run './start.sh' to start the application"
echo "3. Visit https://localhost:5001/swagger for API docs"
echo ""
