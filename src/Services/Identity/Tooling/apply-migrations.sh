#!/bin/bash

# Exit script on error
set -e

# Define the project path (update this if necessary)
PROJECT_PATH="../Identity.API/Identity.API.csproj"

# Apply EF Core migrations
echo "Applying EF Core migrations..."
dotnet ef database update --project "$PROJECT_PATH" -v

echo "Migrations applied successfully!"