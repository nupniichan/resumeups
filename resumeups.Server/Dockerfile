FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY resumeups.Server.csproj .
RUN sed -i '/<ProjectReference.*resumeups\.client\.esproj/,/<\/ProjectReference>/d' resumeups.Server.csproj
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "resumeups.Server.dll"]