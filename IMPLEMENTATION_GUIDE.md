# 📋 GUIA DE IMPLEMENTAÇÃO E DEPLOY

## 🎯 Resumo do Projeto

Este projeto é uma **API REST completa** para gerenciamento de versões de software, desenvolvida com as melhores práticas de backend. É pronta para produção e totalmente dockerizada.

### ✨ Destaques para a Entrevista

1. **Arquitetura Profissional**
   - Padrão Repository com Dependency Injection
   - Separação clara de responsabilidades (Controllers → Services → Data)
   - DTOs para segurança e performance

2. **Padrões REST Puros**
   - CRUD completo (GET, POST, PUT, DELETE)
   - HTTP Status codes apropriados
   - Relacionamentos adequados entre recursos

3. **Documentação Automática (Swagger)**
   - API totalmente documentada
   - Testes diretos pelo navegador
   - Especificação OpenAPI gerada automaticamente

4. **Banco de Dados Bem Estruturado**
   - Índices para performance
   - Constraints de integridade
   - Migrations com versionamento

5. **Docker & DevOps**
   - Dockerfile multi-stage otimizado
   - Docker Compose com MySQL integrado
   - Health checks configurados

## 🚀 Começar Rapidamente

### Opção 1: Com Docker (Recomendado para produção)

```bash
# Clone o repositório
git clone https://github.com/brunomztt/SoftwareVersionManager.git
cd SoftwareVersionManager

# Build e inicie com um comando
docker-compose up --build

# A API estará em http://localhost:8080
# Swagger em http://localhost:8080/swagger/index.html
```

### Opção 2: Desenvolvimento Local

```bash
# Instale o MySQL localmente
# Ou execute apenas o MySQL com Docker:
docker run -d -p 3306:3306 -e MYSQL_ROOT_PASSWORD=root mysql:8.0

# Execute o script para criar o banco
# No MySQL Workbench ou outro cliente, execute: Database.sql

# Restore dependências
dotnet restore

# Execute as migrations
dotnet ef database update

# Inicie a aplicação
dotnet run

# Swagger em https://localhost:5001/swagger/index.html
```

## 📝 Exemplos de Uso da API

### 1️⃣ Criar um Software

```bash
curl -X POST http://localhost:8080/api/softwares \
  -H "Content-Type: application/json" \
  -d '{
	"name": "Visual Studio Code",
	"description": "Editor de código moderno",
	"developer": "Microsoft"
  }'
```

**Resposta (201 Created):**
```json
{
  "id": 1,
  "name": "Visual Studio Code",
  "description": "Editor de código moderno",
  "developer": "Microsoft",
  "createdAt": "2024-01-15T10:30:00",
  "updatedAt": "2024-01-15T10:30:00",
  "versions": []
}
```

### 2️⃣ Criar uma Versão

```bash
curl -X POST http://localhost:8080/api/softwares/1/versions \
  -H "Content-Type: application/json" \
  -d '{
	"versionNumber": "1.85.0",
	"releaseNotes": "Melhorias de performance e novos temas",
	"releaseDate": "2024-01-15",
	"isDeprecated": false
  }'
```

### 3️⃣ Listar Todos os Softwares

```bash
curl -X GET http://localhost:8080/api/softwares
```

### 4️⃣ Obter Software com Suas Versões

```bash
curl -X GET http://localhost:8080/api/softwares/1
```

**Resposta:**
```json
{
  "id": 1,
  "name": "Visual Studio Code",
  "description": "Editor de código moderno",
  "developer": "Microsoft",
  "createdAt": "2024-01-15T10:30:00",
  "updatedAt": "2024-01-15T10:30:00",
  "versions": [
	{
	  "id": 1,
	  "softwareId": 1,
	  "versionNumber": "1.85.0",
	  "releaseNotes": "Melhorias de performance",
	  "releaseDate": "2024-01-15T00:00:00",
	  "isDeprecated": false,
	  "createdAt": "2024-01-15T10:31:00",
	  "updatedAt": "2024-01-15T10:31:00"
	}
  ]
}
```

### 5️⃣ Marcar Versão como Depreciada

```bash
curl -X PUT http://localhost:8080/api/softwares/1/versions/1 \
  -H "Content-Type: application/json" \
  -d '{
	"versionNumber": "1.85.0",
	"releaseNotes": "Melhorias de performance",
	"releaseDate": "2024-01-15",
	"isDeprecated": true
  }'
```

### 6️⃣ Atualizar Software

```bash
curl -X PUT http://localhost:8080/api/softwares/1 \
  -H "Content-Type: application/json" \
  -d '{
	"name": "Visual Studio Code",
	"description": "Editor de código poderoso e gratuito",
	"developer": "Microsoft"
  }'
```

### 7️⃣ Deletar Software

```bash
curl -X DELETE http://localhost:8080/api/softwares/1
```

## 📚 Estrutura do Projeto

```
SoftwareVersionManager/
│
├── Controllers/                    # Endpoints da API
│   ├── SoftwaresController.cs
│   └── SoftwareVersionsController.cs
│
├── Services/                       # Lógica de Negócio
│   ├── ISoftwareService.cs
│   ├── SoftwareService.cs
│   ├── ISoftwareVersionService.cs
│   └── SoftwareVersionService.cs
│
├── Models/                         # Entidades de Domínio
│   ├── Software.cs
│   └── SoftwareVersion.cs
│
├── DTOs/                           # Data Transfer Objects
│   └── SoftwareDto.cs
│
├── Data/                           # Acesso a Dados
│   ├── ApplicationDbContext.cs
│   └── ApplicationDbContextFactory.cs
│
├── Migrations/                     # Versionamento do BD
│   ├── 20240101000000_InitialCreate.cs
│   └── ApplicationDbContextModelSnapshot.cs
│
├── Program.cs                      # Configuração da Aplicação
├── appsettings.json               # Configurações
├── Dockerfile                      # Containerização
├── docker-compose.yml             # Orquestração
├── Database.sql                   # Script SQL
└── README.md                       # Documentação
```

## 🏗️ Por Que Essa Arquitetura?

### 1. **Separation of Concerns**
- Controllers: Recebem requisições HTTP
- Services: Lógica de negócio
- Data: Acesso ao banco de dados
- Models: Representação dos dados
- DTOs: Contrato de API

### 2. **Dependency Injection**
```csharp
builder.Services.AddScoped<ISoftwareService, SoftwareService>();
builder.Services.AddScoped<ISoftwareVersionService, SoftwareVersionService>();
```
Fácil para testes e manutenção.

### 3. **Async/Await em Todo Lugar**
Melhor performance e escalabilidade:
```csharp
public async Task<List<Software>> GetAllSoftwaresAsync()
{
	return await _context.Softwares
		.Include(s => s.Versions)
		.ToListAsync();
}
```

### 4. **Entity Framework Core**
- ORM poderoso
- Migrations automáticas
- LINQ type-safe

### 5. **Swagger/OpenAPI**
Documentação automática e interativa. Ótimo para impressionar!

## 🐳 Docker em Produção

### Build da Imagem

```bash
docker build -t software-version-manager:latest .
```

### Run com Docker Compose

```bash
docker-compose up -d
```

### Verificar Logs

```bash
docker-compose logs -f api
```

### Parar Containers

```bash
docker-compose down
```

## 🔐 Segurança e Boas Práticas

✅ **Implementado:**
- CORS configurado
- Validação de entrada
- Tratamento de erros robusto
- Logging estruturado
- Status codes HTTP corretos
- Índices de banco de dados

🔒 **Para Produção (Adicionar):**
- JWT Authentication
- HTTPS obrigatório
- Rate limiting
- SQL Injection prevention (já feito com EF)
- CORS restrito
- Secrets management

## 📊 Performance

- **Índices:** Primary keys + Index em Name
- **Queries:** Include() para evitar N+1
- **Async:** Requisições não bloqueantes
- **Migrations:** Versionadas para rollback

## 🧪 Testando a API

### Via Swagger (Recomendado)
1. Acesse http://localhost:8080 (Docker) ou https://localhost:5001 (Local)
2. Veja todos os endpoints
3. Clique em "Try it out"
4. Envie requisições

### Via Postman/Insomnia
1. Importe o Swagger JSON: `/swagger/v1/swagger.json`
2. Teste os endpoints

### Via cURL (Visto acima)

## 🚀 Push para GitHub

```bash
cd C:\Users\bruno mazetto\source\repos\brunomztt\SoftwareVersionManager

git add .
git commit -m "feat: Sistema de Gestão de Versões de Software - API REST completa com Docker"
git push origin main
```

## 📈 Próximos Passos (Bônus)

Para impressionar ainda mais:

1. **Testes Unitários**
```bash
dotnet add package xunit
dotnet add package Moq
```

2. **CI/CD com GitHub Actions**
```yaml
name: Build and Test
on: [push]
jobs:
  build:
	runs-on: ubuntu-latest
	steps:
	  - uses: actions/checkout@v2
	  - uses: actions/setup-dotnet@v1
	  - run: dotnet build
	  - run: dotnet test
```

3. **Health Checks**
```csharp
app.MapHealthChecks("/health");
```

4. **Paginação**
```csharp
public async Task<List<Software>> GetAllSoftwaresAsync(int page = 1, int pageSize = 10)
```

## 🎓 O Que Você Aprendeu

✅ ASP.NET Core 8.0
✅ Entity Framework Core com MySQL
✅ RESTful API Design
✅ Docker & Docker Compose
✅ Swagger/OpenAPI
✅ Dependency Injection
✅ Async/Await
✅ Migrations
✅ Logging

## ❓ FAQ

**P: Por que usar DTOs?**
R: Segurança (não expor models), validação, versioning de API.

**P: Por que Async?**
R: Melhor performance sob carga, escalabilidade.

**P: Por que Migrations?**
R: Controle de versão do banco, reproducibilidade, rollback.

**P: Por que Docker?**
R: Mesma aplicação em dev e prod, CI/CD, scaling.

## 💪 Você Está Pronto!

Este projeto demonstra:
- Conhecimento de padrões de design
- Boas práticas de clean code
- Experiência com ferramentas modernas
- Atenção aos detalhes

**Boa sorte na entrevista! 🚀**
