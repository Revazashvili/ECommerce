#!/bin/bash

# Exit script on error
set -e

PROJECT_PATH="../Payment.API/Payment.API.csproj"

# Apply EF Core migrations
echo "removing EF Core migrations..."
dotnet ef migrations remove --project "$PROJECT_PATH" -v

echo "Migrations removed successfully!"