 https://medium.com/@zanattamateus/ef-core-6-migrations-and-docker-database-evolution-and-containerization-5bffd8d1e505
 Recomendado para produção
Mantém o sql-init como está, mas documenta o ciclo:

dotnet ef migrations add [nome]

docker-compose build sql-init

docker-compose run --rm sql-init