FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG BUILD_CONFIGURATION=Release
ARG POSTGRES_CONNECTION
RUN apt-get update && apt-get install -y postgresql-client

WORKDIR /src
COPY ["Core/Core.csproj", "Core/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["webapi/webapi.csproj", "webapi/"]
RUN dotnet restore "webapi/webapi.csproj"
COPY . .

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
COPY Infrastructure/Migrations ./Migrations

WORKDIR "/src/webapi"
RUN dotnet build "webapi.csproj" -c ${BUILD_CONFIGURATION} -o /app/build
RUN dotnet publish "webapi.csproj"  -c ${BUILD_CONFIGURATION} -o /app/publish

FROM build AS final
WORKDIR /app/publish
COPY wait-for-postgres.sh .
RUN chmod +x wait-for-postgres.sh
RUN sed -i 's/\r//' wait-for-postgres.sh
ENTRYPOINT ["bash", "wait-for-postgres.sh"]