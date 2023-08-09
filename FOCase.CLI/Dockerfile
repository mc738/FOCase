FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["FOCase.CLI/FOCase.CLI.fsproj", "FOCase.CLI/"]
RUN dotnet restore "FOCase.CLI/FOCase.CLI.fsproj"
COPY . .
WORKDIR "/src/FOCase.CLI"
RUN dotnet build "FOCase.CLI.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FOCase.CLI.fsproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FOCase.CLI.dll"]
