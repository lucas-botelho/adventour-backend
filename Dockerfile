# Dockerfile com suporte para sql-init + api

ARG DOTNET_RUNTIME=mcr.microsoft.com/dotnet/aspnet:8.0
ARG DOTNET_SDK=mcr.microsoft.com/dotnet/sdk:8.0

# Etapa base para runtime
FROM ${DOTNET_RUNTIME} AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Etapa base para build
FROM ${DOTNET_SDK} AS buildbase
WORKDIR /src
COPY . .
RUN dotnet restore

# Etapa sql-init: aplica migrations EF Core
FROM buildbase AS sqlinit
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

ENTRYPOINT ["dotnet-ef","database","update","--project", "api/Adventour.Api.csproj","--startup-project", "api/Adventour.Api.csproj"]

# Etapa final da aplicação publicada
FROM buildbase AS publish
RUN dotnet publish api/Adventour.Api.csproj -c Release -o /app/publish

# Etapa final com runtime para API
FROM base AS final
WORKDIR /app

# Instalar sqlcmd no container final da API
RUN apt-get update && \
    apt-get install -y curl gnupg apt-transport-https && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/10/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev && \
    echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc

ENV PATH="${PATH}:/opt/mssql-tools/bin"
COPY --from=publish /app/publish .

# Copiar scripts de seed e entrypoint
COPY ./entrypoint.sh /entrypoint.sh
COPY ./database /app/database
RUN chmod +x /entrypoint.sh

ENTRYPOINT ["/entrypoint.sh"]
