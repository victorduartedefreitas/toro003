# TORO-003
WebApi para realizar depósitos em uma conta Toro

## Arquitetura

Este projeto utiliza o padrão de arquitetura hexagonal. A escolha deste modelo de arquitetura é pela facilidade de manutenção e facilidade de modularização.
A solução contém 5 projetos, divididos em 3 diretórios:
- 0 Core
  - Toro.Domain
  - Toro.Application
  - Toro.Application.Tests (Projeto xUnit para testes unitários do projeto Toro.Application)
- 1 Infra
  - Toro.Infra.Repository
- 2 Presentation
  - Toro.WebApi

### Design Patterns
- DDD
- IoC
- Dependency Injection

### Dependências
- Dapper
- Moq
- Microsoft.Extensions.DependencyInjection
- Microsoft.Extensions.DependencyInjection.Abstractions

## Instruções

Para testar a Api, antes de mais nada é necessário criar o banco de dados. Dentro da pasta "sql" existem 2 arquivos para a criação do banco de dados.
- Primeiramente crie uma base de dados;
- Abra o arquivo "Create Tables.sql" e altere o nome da base de dados que foi criada
  - Em seguida, execute este script;
- Abra e execute o arquivo "base_insert.sql"

Após criar o banco de dados, é necessário apenas alterar a string de conexão no projeto Toro.WebApi
- Para isto, abra o arquivo "appsettings.json" e altere o valor da propriedade "SqlConnectionString" de acordo com sua conexão.

Feitos os passos acima, execute o projeto "Toro.WebApi". Irá abrir um browser na página do Swagger.
Para realizar os testes, basta inserir o Json desejado no swagger, no método /spb/events.

Existem algumas contas criadas previamente para efeito de testes. São elas:
  - Fulano
    - CPF: "45358996060"
    - Account: "300123"
    - Bank: "352"
    - Branch: "0001"

  - Ciclano
    - CPF: "50562883061"
    - Account: "412055"
    - Bank: "352"
    - Branch: "0001"

  - Beltrano
    - CPF: "56641234002"
    - Account: "123456"
    - Bank: "352"
    - Branch: "0001"
