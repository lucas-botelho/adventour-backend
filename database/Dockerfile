FROM mcr.microsoft.com/mssql/server:2022-latest

USER root

# Instalar pacotes necessários
RUN apt-get update && apt-get install -y \
    curl \
    gnupg \
    apt-transport-https \
    ca-certificates \
    && curl -sSL https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl -sSL https://packages.microsoft.com/config/ubuntu/22.04/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools unixodbc-dev \
    && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> ~/.bashrc \
    && echo 'export PATH="$PATH:/opt/mssql-tools/bin"' >> /etc/profile \
    && apt-get clean

# Criar diretório para scripts SQL e copiar
RUN mkdir -p /scripts

COPY init/ /scripts/

# Ajustar permissões para garantir que o usuário 'mssql' (10001) possa acessar
RUN chown -R 10001:10001 /scripts && chmod -R 755 /scripts

USER 10001
