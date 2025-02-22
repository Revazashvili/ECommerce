#!/bin/bash

# Exit script on error
set -e

# Define the project path (update this if necessary)
PROJECT_PATH="../Products.Infrastructure/Products.Infrastructure.csproj"
STARTUP_PROJECT_PATH="../Products.API/Products.API.csproj"

# Apply EF Core migrations
echo "Applying EF Core migrations..."
dotnet ef database update --project "$PROJECT_PATH" -v

echo "Migrations applied successfully!"