FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/SesiCatalogo.Web/SesiCatalogo.Web.csproj src/SesiCatalogo.Web/
RUN dotnet restore src/SesiCatalogo.Web/SesiCatalogo.Web.csproj
COPY src/ src/
RUN dotnet publish src/SesiCatalogo.Web/SesiCatalogo.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# The non-root app user only needs write access to the folder where the SQLite file is created.
RUN mkdir -p /app/data && chown $APP_UID /app/data
USER $APP_UID
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ["dotnet", "SesiCatalogo.Web.dll"]
