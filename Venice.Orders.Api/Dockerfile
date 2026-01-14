# ===== STAGE 1 — Build =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copia a solution
COPY ../venice-orders.sln .

# Copia TODOS os .csproj com estrutura correta
COPY ../Venice.Orders.Api/Venice.Orders.Api.csproj Venice.Orders.Api/
COPY ../Venice.Orders.Application/Venice.Orders.Application.csproj Venice.Orders.Application/
COPY ../Venice.Orders.Domain/Venice.Orders.Domain.csproj Venice.Orders.Domain/
COPY ../Venice.Orders.Infra.Data/Venice.Orders.Infra.Data.csproj Venice.Orders.Infra.Data/
COPY ../Venice.Orders.Infra.Data.Sql/Venice.Orders.Infra.Data.Sql.csproj Venice.Orders.Infra.Data.Sql/
COPY ../Venice.Orders.Infra.Data.NoSql/Venice.Orders.Infra.Data.NoSql.csproj Venice.Orders.Infra.Data.NoSql/
COPY ../Venice.Orders.Infra.Messaging/Venice.Orders.Infra.Messaging.csproj Venice.Orders.Infra.Messaging/

# Restore
RUN dotnet restore

# Copia tudo (backend inteiro)
COPY .. .

# Publica
RUN dotnet publish Venice.Orders.Api/Venice.Orders.Api.csproj -c Release -o /app

# ===== STAGE 2 — Runtime =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Instala a lib necessária para SMTP (System.Net.Mail)
RUN apt-get update && apt-get install -y libkrb5-3

COPY --from=build /app .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Venice.Orders.Api.dll"]
