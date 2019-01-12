FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY *.sln ./
COPY webapi/webapi.csproj webapi/
COPY webapi.infrastructure/webapi.infrastructure.csproj webapi.infrastructure/
COPY webapi.core/webapi.core.csproj webapi.core/
COPY webapi.tests/webapi.tests.csproj webapi.tests/
RUN dotnet restore

COPY . ./
RUN dotnet publish webapi/webapi.csproj -c Release -o /app/out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "webapi.dll"]