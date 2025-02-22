#!/bin/bash

# Exit script on error
set -e

PROJECT_PATH="../Products.Infrastructure/Products.Infrastructure.csproj"

# Apply EF Core migrations
echo "adding EF Core migrations..."
dotnet ef migrations add "$1" --project "$PROJECT_PATH" -v

echo "Migrations added successfully!"