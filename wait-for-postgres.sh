#!/bin/bash

until pg_isready -h "postgres_db" -p "5432" -U "postgres"; do
  echo "Waiting for PostgreSQL..."
  sleep 2
done

echo "PostgreSQL is ready!"
dotnet ef database update --project /src/Infrastructure --startup-project /src/webapi
dotnet /app/publish/webapi.dll