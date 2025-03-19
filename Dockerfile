FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG BUILD_CONFIGURATION=Release

WORKDIR /src
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["webapi/webapi.csproj", "webapi/"]
RUN dotnet restore "webapi/webapi.csproj"
COPY . .

WORKDIR "/src/webapi"
RUN dotnet build "webapi.csproj" -c ${BUILD_CONFIGURATION} -o /app/build
RUN dotnet publish "webapi.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["sh", "-c", "dotnet webapi.dll"]