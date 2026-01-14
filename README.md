# Venice Orders API

API de processamento de pedidos construída com .NET 10, seguindo os princípios da Clean Architecture, DDD e Arquitetura Orientada a Eventos.

## Decisões Técnicas e Arquitetura

A solução foi desenhada para suportar alta escalabilidade, garantindo desacoplamento.

### 1. Padrões de Design e Arquitetura
* **Clean Architecture:** Separação rigorosa entre Domínio, Aplicação e Infraestrutura, permitindo testes isolados e facilidade na troca de provedores de dados.
* **Domain-Driven Design (DDD):** O coração da aplicação, onde Agregados implementam suas regras e protegem a integridade do negócio através de `DomainExceptions`.
* **Repository:** Separa a lógica de acesso a dados da lógica de negócios, permitindo que a aplicação manipule dados de forma mais organizada e orientada a objetos, sem se preocupar com os detalhes de como os dados são realmente armazenados ou recuperados
* **Event-Driven:** Uso de mensageria assíncrona para promover o desacoplamento entre serviços.
* **Cache-Aside:** Estratégia de cache para reduzir a latência de leitura e poupar recursos do banco de dados principal.

### 2. Infraestrutura e Performance
* **Cache com Redis:** Utilizado na camada de aplicação para reduzir a latência em consultas de pedidos frequentes.
* **Mensageria com RabbitMQ:** Utiliza o padrão **Fanout** para distribuir eventos de `OrderCreatedEvent`.
* **Segurança (JWT):** Autenticação Bearer configurada globalmente no Swagger UI para facilitar o teste dos endpoints protegidos.

### 3. Estrutura da Solução
* **Venice.Orders.Domain:** Entidades, Agregados, Domain Exceptions e Interfaces de Repositório.
* **Venice.Orders.Application:** Casos de uso, DTOs, Validadores.
* **Venice.Orders.Infra.Data:** Implementação do pattern Repository, configura o Redis.
* **Venice.Orders.Infra.Data.Sql:** Implementação de DB SQL Server, EF Core.
* **Venice.Orders.Infra.Data.NoSql:** Implementação de DB Mongo, documents.
* **Venice.Orders.Infra.Messaging:** Implementação de RabbitMq.
* **Venice.Orders.API:** Endpoints, Configurações de Middlewares e Swagger.
* **Venice.Orders.Test:** Suíte de testes unitários com xUnit e Moq. Para este projeto criei testes de dominio e aplicacao, para garantir que as regras de negócio sejam respeitadas e que a infraestrutura esteja bem orquestrada.

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
4. Utilize o token JWT gerado nos endpoints de pedido
