FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080
#ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["BCP/BCP.csproj", "BCP/"]
RUN dotnet restore "BCP/BCP.csproj"
COPY . .
WORKDIR "/src/BCP"
RUN dotnet build "BCP.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BCP.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BCP.dll"]

