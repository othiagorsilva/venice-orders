# Venice Orders API

API de processamento de pedidos construída com .NET 10, seguindo os princípios da Clean Architecture, DDD e Arquitetura Orientada a Eventos.

## Decisões Técnicas e Arquitetura

A solução foi desenhada para suportar alta escalabilidade, garantindo desacoplamento entre escrita e leitura.

### 1. Padrões de Design
* **Clean Architecture:** Separação rigorosa entre Domínio, Aplicação e Infraestrutura, permitindo testes isolados e facilidade na troca de provedores de dados.
* **Domain-Driven Design (DDD):** O coração da aplicação, onde Agregados protegem a integridade do negócio através de `DomainExceptions`.

### 2. Infraestrutura e Performance
* **Cache com Redis:** Implementado na camada de aplicação para reduzir a latência em consultas de pedidos frequentes.
* **Mensageria com RabbitMQ:** Utiliza o padrão **Fanout** para distribuir eventos de `OrderCreatedEvent`.
* **Segurança (JWT):** Autenticação Bearer configurada globalmente no Swagger UI para facilitar o teste dos endpoints protegidos.

### 3. Estrutura da Solução
* Venice.Orders.Domain: Entidades, Agregados, Domain Exceptions e Interfaces de Repositório.
* Venice.Orders.Application: Casos de uso, DTOs, Validadores.
* Venice.Orders.Infra.Data: Implementação do pattern Repository, como abstração entre o domínio, aplicação e a persistência de dados, permitindo que a lógica de negócio manipule objetos como se estivessem em uma coleção na memória, sem conhecer os detalhes de acesso ao banco de dados.
* Venice.Orders.Infra.Data.Sql: Implementação de DB SQL Server, EF Core.
* Venice.Orders.Infra.Data.NoSql: Implementação de DB Mongo, documents.
* Venice.Orders.Infra.Messaging: Implementação de RabbitMq.
* Venice.Orders.API: Endpoints, Configurações de Middlewares e Swagger.
* Venice.Orders.Test: Suíte de testes unitários com xUnit e Moq.

## Como Rodar o Projeto

Toda a infraestrutura necessária (SQL Server, MongoDB, Redis, RabbitMQ) está configurada via **Docker Compose**.

### Pré-requisitos
* Docker e Docker Compose instalados.
* SDK do .NET 10 (caso queira rodar os testes localmente).

### Passo a Passo
1. **Subir os containers:** Este comando irá baixar as imagens, configurar as redes e iniciar todos os serviços.
   ```bash
   docker-compose up -d --build
2. **Acessar a Documentação (Swagger):** Acesse: http://localhost:5010/index.html
3. **Autenticação:** Realize o login no endpoint de auth/login para obter o Token JWT (usuario admin e senha password123).