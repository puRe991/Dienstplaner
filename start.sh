#!/bin/bash

# ShiftPilot Startup Script for Linux/macOS
# This script sets up and starts the entire ShiftPilot application

set -e  # Exit on error

echo "================================================"
echo "   🚀 ShiftPilot Startup Script - Linux/macOS  "
echo "================================================"
echo ""

# Color codes for output
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
RED='\033[0;31m'
NC='\033[0m' # No Color

# Function to print colored output
print_status() {
    echo -e "${GREEN}[✓]${NC} $1"
}

print_info() {
    echo -e "${YELLOW}[i]${NC} $1"
}

print_error() {
    echo -e "${RED}[✗]${NC} $1"
}

# Step 1: Check prerequisites
echo ""
print_info "Checking prerequisites..."
echo ""

if ! command -v dotnet &> /dev/null; then
    print_error ".NET SDK is not installed"
    echo "Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download"
    exit 1
fi
print_status ".NET SDK found: $(dotnet --version)"

if ! command -v docker &> /dev/null; then
    print_error "Docker is not installed"
    echo "Please install Docker from https://www.docker.com/products/docker-desktop"
    exit 1
fi
print_status "Docker found: $(docker --version)"

if ! command -v docker-compose &> /dev/null; then
    print_error "Docker Compose is not installed"
    echo "Please install Docker Compose from https://docs.docker.com/compose/install"
    exit 1
fi
print_status "Docker Compose found: $(docker-compose --version)"

# Step 2: Clone or update repository (if needed)
echo ""
print_info "Checking repository..."
echo ""

if [ ! -d ".git" ]; then
    print_error "Not in a git repository directory"
    exit 1
fi
print_status "Git repository found"

# Step 3: Start database with Docker Compose
echo ""
print_info "Starting database with Docker Compose..."
echo ""

docker-compose up -d db

if [ $? -eq 0 ]; then
    print_status "Database container started"
else
    print_error "Failed to start database container"
    exit 1
fi

# Wait for database to be ready
print_info "Waiting for database to be ready (30 seconds)..."
sleep 30
print_status "Database is ready"

# Step 4: Restore NuGet packages
echo ""
print_info "Restoring NuGet packages..."
echo ""

if dotnet restore > /dev/null 2>&1; then
    print_status "Packages restored successfully"
else
    print_error "Failed to restore packages"
    exit 1
fi

# Step 5: Build the solution
echo ""
print_info "Building ShiftPilot solution..."
echo ""

if dotnet build --configuration Release > /dev/null 2>&1; then
    print_status "Solution built successfully"
else
    print_error "Failed to build solution"
    exit 1
fi

# Step 6: Run database migrations
echo ""
print_info "Running database migrations..."
echo ""

cd src/ShiftPilot.API
if dotnet ef database update > /dev/null 2>&1; then
    print_status "Database migrations completed"
else
    print_error "Failed to run migrations"
    cd ../..
    exit 1
fi
cd ../..

# Step 7: Run tests
echo ""
read -p "Do you want to run unit tests? (y/n) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    print_info "Running unit tests..."
    echo ""
    if dotnet test tests/ShiftPilot.Tests --verbosity minimal > /dev/null 2>&1; then
        print_status "All tests passed"
    else
        print_error "Some tests failed"
        echo "Run 'dotnet test tests/ShiftPilot.Tests' for details"
    fi
fi

# Step 8: Start the API
echo ""
print_info "Starting ShiftPilot API..."
echo ""
echo "The API will start on:"
echo -e "  ${GREEN}https://localhost:5001${NC}"
echo -e "  ${GREEN}http://localhost:5000${NC}"
echo ""
echo "Swagger UI will be available at:"
echo -e "  ${GREEN}https://localhost:5001/swagger${NC}"
echo ""
print_info "Press Ctrl+C to stop the API"
echo ""

cd src/ShiftPilot.API
dotnet run
