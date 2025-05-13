#!/bin/bash

echo "⏳ A aguardar pelo SQL Server..."
# Espera que o SQL Server aceite conexões
until /opt/mssql-tools/bin/sqlcmd -S mssql -U ${SQL_USER} -P ${SQL_PASSWORD} -Q "SELECT 1" -d master &> /dev/null; do
    echo "🔄 Ainda a tentar conectar ao SQL Server..."
    sleep 2
done

echo "🟢 SQL Server disponível!"

# Espera até a base de dados existir
echo "⏳ A verificar se a base de dados '${DATABASE}' já foi criada..."
until /opt/mssql-tools/bin/sqlcmd -S mssql -U ${SQL_USER} -P ${SQL_PASSWORD} -Q "IF DB_ID('${DATABASE}') IS NOT NULL SELECT 1 ELSE SELECT 0" -h -1 -d master -W | grep -q 1; do
    echo "🔄 A base de dados '${DATABASE}' ainda não existe. A aguardar..."
    sleep 2
done

echo "✅ A base de dados '${DATABASE}' já existe."

echo "📦 A correr scripts SQL de seed..."
/opt/mssql-tools/bin/sqlcmd -S mssql -U ${SQL_USER} -P ${SQL_PASSWORD} -d ${DATABASE} -i /database/countries.sql

echo "🚀 A iniciar a aplicação..."
exec dotnet Adventour.Api.dll
