FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY src/Web/Web.csproj src/Web/
COPY src/Application/Application.csproj src/Application/
COPY src/Infrastructure/Infrastructure.csproj src/Infrastructure/
COPY src/Domain/Domain.csproj src/Domain/
RUN dotnet restore src/Web/Web.csproj

COPY . .
WORKDIR /src/src/Web
# NOTE: no --no-restore here. The restore above runs on csproj files only
# (no .razor sources), so the Microsoft.AspNetCore.App.Internal.Assets pack
# (blazor.web.js etc.) is missing from its graph. Publish must re-restore
# with full sources present, otherwise _framework/* is silently omitted.
RUN dotnet publish Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV DOTNET_RUNNING_IN_CONTAINER=true \
    ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000

COPY --from=build /app/publish .

CMD ASPNETCORE_URLS=http://*:${PORT:-10000} dotnet Web.dll
