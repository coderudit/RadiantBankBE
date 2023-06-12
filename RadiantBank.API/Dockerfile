FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["RadiantBank.API/RadiantBank.API.csproj", "RadiantBank.API/"]
RUN dotnet restore "RadiantBank.API/RadiantBank.API.csproj"
COPY . .
WORKDIR "/src/RadiantBank.API"
RUN dotnet build "RadiantBank.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RadiantBank.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RadiantBank.API.dll"]
