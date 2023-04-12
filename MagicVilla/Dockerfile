FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MagicVilla/MagicVilla.csproj", "MagicVilla/"]
RUN dotnet restore "MagicVilla/MagicVilla.csproj"
COPY . .
WORKDIR "/src/MagicVilla"
RUN dotnet build "MagicVilla.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MagicVilla.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MagicVilla.dll"]
