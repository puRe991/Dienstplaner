#!/bin/bash

# ShiftPilot Stop Script for Linux/macOS
# This script stops all running ShiftPilot services

echo "================================================"
echo "   ⏹️  ShiftPilot Stop Script - Linux/macOS   "
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

print_info "Stopping Docker containers..."
echo ""

if docker-compose down; then
    print_status "All containers stopped successfully"
else
    print_error "Failed to stop containers"
    exit 1
fi

echo ""
print_info "Removing volumes (optional)..."
read -p "Do you want to remove database volumes? (y/n) " -n 1 -r
echo
if [[ $REPLY =~ ^[Yy]$ ]]; then
    if docker-compose down -v; then
        print_status "Volumes removed"
    fi
fi

echo ""
print_status "ShiftPilot has been stopped"
