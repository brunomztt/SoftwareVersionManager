FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["SoftwareVersionManager.csproj", "./"]
RUN dotnet restore "SoftwareVersionManager.csproj"

COPY . .
RUN dotnet build "SoftwareVersionManager.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SoftwareVersionManager.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

EXPOSE 8080
ENTRYPOINT ["dotnet", "SoftwareVersionManager.dll"]
