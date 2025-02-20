#!/bin/bash

# Exit script on error
set -e

PROJECT_PATH="../Ordering.Infrastructure/Ordering.Infrastructure.csproj"

# Apply EF Core migrations
echo "removing EF Core migrations..."
dotnet ef migrations remove --project "$PROJECT_PATH" -v

echo "Migrations removed successfully!"