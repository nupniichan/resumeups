# === 1. Build Angular ===
FROM node:22-alpine AS build-node
WORKDIR /app

COPY resumeups.client/package*.json ./resumeups.client/
RUN cd resumeups.client && npm ci

COPY resumeups.client/ ./resumeups.client/
RUN cd resumeups.client && npm run build

# === 2. Build .NET Core Backend ===
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-dotnet
WORKDIR /src

COPY resumeups.Server/resumeups.Server.csproj resumeups.Server/
RUN sed -i '/<ProjectReference.*resumeups\.client\.esproj/,/<\/ProjectReference>/d' resumeups.Server/resumeups.Server.csproj

RUN dotnet restore resumeups.Server/resumeups.Server.csproj
COPY resumeups.Server/ resumeups.Server/

RUN dotnet publish resumeups.Server/resumeups.Server.csproj -c Release -o /app/publish /p:UseAppHost=false

# === 3. Final Runtime Stage ===
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build-dotnet /app/publish .

COPY --from=build-node /app/resumeups.client/dist/resumeups.client/browser ./wwwroot

ENTRYPOINT ["dotnet", "resumeups.Server.dll"]