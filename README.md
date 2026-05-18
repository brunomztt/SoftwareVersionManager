# Software Version Manager API(backend apenas)

Uma API REST para gerenciamento de softwares e suas versões, desenvolvida com ASP.NET Core, Entity Framework Core e MySQL.

Este projeto foi desenvolvido como desafio técnico para uma vaga de Desenvolvedor Backend Jr, consegui aproveitar apenas algumas horas para desenvolver o aplicativo.
Durante o desenvolvimento utilizei como guia a mensagem com as especificações solicitadas para o projeto, como pesquisa e apoio utilizei de ferramentas de IA como ChatGPT e Claude para aprender conceitos que ainda não faziam parte da minha rotina, principalmente Docker, configuração de containers e alguns detalhes do Entity Framework Core.

Mesmo com esse apoio, fiz questão de entender e testar cada etapa da aplicação, ajustando erros manualmente, configurando o ambiente, corrigindo problemas que tive na migrations, Docker e integração com MySQL até a API funcionar corretamente.
Visando tornar os testes mais fáceis implementei uma class DbInitializer, voltada para popular o banco de dados.

Esse projeto foi importante para consolidar conhecimentos em:

* ASP.NET Core
* APIs REST
* Entity Framework Core
* SQL
* Docker
* Arquitetura em camadas

---

# 🚀 Funcionalidades

* Cadastro de softwares
* Atualização de softwares
* Remoção de softwares
* Cadastro de versões
* Controle de versões depreciadas
* API RESTful
* Swagger para documentação e testes
* Persistência com MySQL
* Containers Docker

---

# 🛠 Tecnologias utilizadas

* ASP.NET Core 8
* Entity Framework Core 8
* MySQL 8
* Pomelo.EntityFrameworkCore.MySql
* Swagger / Swashbuckle
* Docker & Docker Compose

---

# ▶️ Como executar o projeto

## Pré-requisitos

* .NET 8 SDK
* Docker Desktop

---

## Executar com Docker

```bash
docker compose up --build
```

API/Swagger:

```txt
http://localhost:8080
```

---

# 📌 Endpoints principais

## Softwares

| Método | Endpoint |
|---------|-----------|
| GET | `/api/Softwares` |
| POST | `/api/Softwares` |
| GET | `/api/Softwares/{id}` |
| PUT | `/api/Softwares/{id}` |
| DELETE | `/api/Softwares/{id}` |

---

## Software Versions

| Método | Endpoint |
|---------|-----------|
| GET | `/api/softwares/{softwareId}/versions` |
| POST | `/api/softwares/{softwareId}/versions` |
| GET | `/api/softwares/{softwareId}/versions/{versionId}` |
| PUT | `/api/softwares/{softwareId}/versions/{versionId}` |
| DELETE | `/api/softwares/{softwareId}/versions/{versionId}` |

---

# 📦 Exemplos de requisição

## Criar Software

```bash
curl -X POST http://localhost:8080/api/Softwares \
-H "Content-Type: application/json" \
-d '{
  "name": "Nome do programa",
  "description": "Descrição do programa"
}'
```

---

# 📂 Estrutura do projeto

```txt
SoftwareVersionManager/
│
├── Controllers/
├── Data/
├── DTOs/
├── Models/
├── Services/
├── appsettings.json
├── Database.sql
├── docker-compose.yml
├── Dockerfile
├── Program.cs
└── README.md
```

---

# 📖 Aprendizados

Esse foi o meu primeiro projeto utilizando Docker.

Durante o desenvolvimento enfrentei problemas relacionados a:

* configuração do Docker
* exposição de portas
* migrations do Entity Framework
* compatibilidade de versões do EF Core
* conexão entre API e MySQL

---

# 💡 Melhorias futuras:

Algumas melhorias irei adicionar futuramente:

* logs 


---

# 👨‍💻 Autor

Bruno V M Mazetto /
Desenvolvedor Backend Jr
