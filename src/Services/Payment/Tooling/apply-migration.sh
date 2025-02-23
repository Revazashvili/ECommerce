#!/bin/bash

# Exit script on error
set -e

# Define the project path (update this if necessary)
PROJECT_PATH="../Payment.API/Payment.API.csproj"
STARTUP_PROJECT_PATH="../Payment.API/Payment.API.csproj"

# Apply EF Core migrations
echo "Applying EF Core migrations..."
dotnet ef database update --project "$PROJECT_PATH" -v

echo "Migrations applied successfully!"