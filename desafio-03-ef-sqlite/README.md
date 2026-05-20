# Desafio 03 — EF Core + SQLite

## 📋 Descrição
Evolução do CRUD do desafio 2 com persistência real usando Entity Framework Core e SQLite. Implementação de migrations, configuração via Fluent API e armazenamento durável de dados em banco de dados.

## 🚀 Como Rodar

### Pré-requisitos
- .NET 8+ instalado
- Terminal/PowerShell

### Passos
1. **Acesse o diretório do projeto:**
   ```bash
   cd desafio-03-ef-sqlite
   ```

2. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

3. **Execute a aplicação:**
   ```bash
   dotnet run
   ```
   > A migration é aplicada automaticamente ao iniciar via `SeedDatabase()` no Program.cs

4. **Acesse a aplicação:**
   - Swagger UI: `https://localhost:7240/swagger`
   - API de Products: `https://localhost:7240/products`

## 📊 Endpoints

### GET /products
Lista todos os produtos cadastrados (persiste em SQLite).

**Response (200 OK):**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Laptop",
    "price": 2500.00,
    "quantityInStock": 5,
    "createdAt": "2026-05-20T01:09:16.1234567Z"
  }
]
```

### GET /products/{id}
Retorna um produto específico do banco ou 404.

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Laptop",
  "price": 2500.00,
  "quantityInStock": 5,
  "createdAt": "2026-05-20T01:09:16.1234567Z"
}
```

**Response (404 Not Found):** Quando o ID não existe

### POST /products
Cria um novo produto e persiste no banco (retorna 201).

**Request:**
```json
{
  "name": "Monitor",
  "price": 1200.00,
  "quantityInStock": 5
}
```

**Response (201 Created):**
```
Location: /products/550e8400-e29b-41d4-a716-446655440001
Body: { produto criado com ID }
```

**Response (400 Bad Request):** Validação falha
- Name é obrigatório e máx 100 caracteres
- Price deve ser positivo
- Quantity não pode ser negativo
- Índice único: nome duplicado também retorna 400

### PUT /products/{id}
Atualiza um produto no banco.

**Request:**
```json
{
  "name": "Monitor 4K",
  "price": 1500.00,
  "quantityInStock": 3
}
```

**Response (204 No Content):** Sucesso

**Response (404 Not Found):** Quando o ID não existe

### DELETE /products/{id}
Remove um produto do banco.

**Response (204 No Content):** Sucesso

**Response (404 Not Found):** Quando o ID não existe

## 🎓 O que foi Aprendido

### 1. **Entity Framework Core (EF Core)**
- ORM (Object-Relational Mapping) que mapeia objetos C# para tabelas do banco
- `DbContext` como abstração da conexão e repositório de entidades
- Lazy loading e eager loading de relacionamentos
- Change tracking automático de entidades modificadas

### 2. **DbContext e Configuração**
- Herança de `DbContext` para criar contexto de aplicação
- `DbSet<T>` para representar tabelas do banco
- Configuração via `OnModelCreating()` para customizar o schema
- Registrar no DI com `builder.Services.AddDbContext<AppDbContext>()`

### 3. **Fluent API vs Data Annotations**
- **Fluent API** — configuração em código C#, mantém modelo limpo
- **Data Annotations** — atributos no modelo, menos flexível
- Escolha: Fluent API é preferida em projetos profissionais pela separação de responsabilidades
- Este desafio usa **apenas Fluent API**, sem `[Required]`, `[StringLength]`, etc. no modelo

### 4. **Configuração com Fluent API**
```csharp
// Exemplo de configurações via Fluent API
modelBuilder.Entity<Product>()
    .Property(p => p.Name)
    .IsRequired()
    .HasMaxLength(100);

modelBuilder.Entity<Product>()
    .Property(p => p.Price)
    .HasPrecision(18, 2);

modelBuilder.Entity<Product>()
    .HasIndex(p => p.Name)
    .IsUnique();
```

### 5. **Migrations — Versionamento do Schema**
- Migration = mudança incremental no schema do banco
- `dotnet ef migrations add NomeDaMigration` — cria arquivo de migration
- `dotnet ef database update` — aplica migrations pendentes
- `dotnet ef migrations remove` — desfaz última migration (antes de publicar)
- Migrations são arquivos C# que descrevem Up/Down (aplicar/desfazer)

### 6. **SQLite como Banco de Dados**
- Banco de arquivo (arquivo `app.db` no projeto)
- Leve, sem servidor, ideal para desenvolvimento e apps pequenas
- Usa `Microsoft.EntityFrameworkCore.Sqlite` package
- Connection string aponta para arquivo local

### 7. **Índices e Restrições Únicas**
- `.HasIndex()` cria índice para melhorar performance de queries
- `.IsUnique()` garante unicidade — dois registros não podem ter mesmo valor
- Validação em banco: INSERT/UPDATE com duplicata retorna erro do DB
- Validação em API: capturar erro e retornar 400 Bad Request com mensagem clara

### 8. **Precisão Decimal para Preços**
- `HasPrecision(18, 2)` — 18 dígitos totais, 2 casas decimais
- Armazena valores monetários com exatidão (não usa `float` ou `double`)
- `decimal` em C# corresponde a `DECIMAL(18, 2)` no SQL

### 9. **DatabaseSeeder para Dados de Teste**
- Popular banco com dados iniciais ao iniciar a aplicação
- Usar `Database.Migrate()` para aplicar migrations antes de seeder
- Padrão: criar scope, chamar seeder, descartar
- Ideal para desenvolvimento e testes

### 10. **Persistência de Dados**
- Dados salvos no arquivo `app.db` no diretório do projeto
- Dados sobrevivem ao reiniciar a aplicação
- Importante validar: parar a app, rodar novamente, dados estão lá

### 11. **Tratamento de Erros de Banco**
- `DbUpdateException` — erro genérico de persistência
- Indexar `InnerException` para detalhes reais (violação de constraint, etc.)
- Mensagens de erro do DB podem ser técnicas — traduzir para usuário

## 🔧 Estrutura do Projeto
```
desafio-03-ef-sqlite/
├── Program.cs                          # Configuração DI, endpoints CRUD inline
├── Domain/
│   ├── Product.cs                      # Modelo de domínio (sem validações)
│   └── ProductRequest.cs               # DTO para input
├── Data/
│   ├── AppDbContext.cs                 # DbContext com DbSet<Product>
│   └── ProductConfiguration.cs         # Configuração Fluent API para Product
├── Extensions/
│   ├── DataBaseSeederExtension.cs      # Popula dados iniciais + Migrate()
│   └── DatabaseExtension.cs            # Registra DbContext no DI
├── Migrations/
│   ├── 20260520010916_Initial.cs       # Primeira migration (auto-gerada)
│   ├── 20260520010916_Initial.Designer.cs
│   └── AppDbContextModelSnapshot.cs
├── Properties/
│   └── launchSettings.json             # Portas: 5192 (http), 7240 (https)
├── appsettings.json                    # Connection string
├── appsettings.Development.json        # Config dev
├── app.db                              # Arquivo do banco SQLite (gerado)
├── desafio-03-ef-sqlite.csproj         # Definição do projeto
├── DESAFIO.md                          # Especificação do desafio
└── README.md                           # Este arquivo
```

## 💡 Dicas Úteis

### Workflow Desenvolvimento
- **Recompilar sem parar:** Use `dotnet watch run` para reload automático
- **Alterar schema:** Edite o modelo → `dotnet ef migrations add NomeDaMudanca` → `dotnet ef database update`
- **Resetar banco:** Delete o arquivo `app.db` e reinicie a app (seeder recria tudo)
- **Ver migrations:** `dotnet ef migrations list` mostra todas as migrations criadas

### Testar com Swagger
- Acesse `https://localhost:7240/swagger`
- Teste cada endpoint diretamente na UI
- Swagger mostra modelos esperados automaticamente

### Testar com curl
```bash
# GET todos
curl https://localhost:7240/products -k

# POST novo produto
curl -X POST https://localhost:7240/products -k \
  -H "Content-Type: application/json" \
  -d '{"name":"Teclado","price":150,"quantityInStock":20}'

# GET por ID
curl https://localhost:7240/products/{id} -k

# PUT para atualizar
curl -X PUT https://localhost:7240/products/{id} -k \
  -H "Content-Type: application/json" \
  -d '{"name":"Teclado Gamer","price":200,"quantityInStock":15}'

# DELETE
curl -X DELETE https://localhost:7240/products/{id} -k

# Tentar criar duplicado (deve retornar 400 - índice único)
curl -X POST https://localhost:7240/products -k \
  -H "Content-Type: application/json" \
  -d '{"name":"Teclado","price":150,"quantityInStock":20}'
```

### Inspecionar Banco
- Arquivo `app.db` é um SQLite válido (fica na raiz do projeto)
- Use ferramentas como DB Browser for SQLite, DBeaver, ou Visual Studio Extension
- Query manual: `SELECT * FROM Products` para verificar dados persistidos
- O banco é criado automaticamente na primeira execução

### Adicionar Novas Migrations
Se precisar alterar o schema do banco:
1. Edite a classe `Product` ou a configuração em `ProductConfiguration.cs`
2. Execute: `dotnet ef migrations add NomeDaMudanca`
3. Execute: `dotnet ef database update`
4. O `Migrate()` no seeder aplica automaticamente ao iniciar

## ✅ Checklist de Conclusão
- [x] Projeto criado com Minimal APIs
- [x] `AppDbContext : DbContext` implementado
- [x] `DbSet<Product>` configurado
- [x] DI: `builder.Services.AddDbContext<AppDbContext>()` via `AddDatabase()`
- [x] Migration "Initial" criada automaticamente
- [x] `database.Migrate()` aplicado via `SeedDatabase()` no startup
- [x] Configurações Fluent API para Product:
  - [x] Nome obrigatório, max 100 chars
  - [x] Preço com precisão (18,2)
  - [x] Índice único no nome
- [x] Validações mantidas (obrigatório, max length, price > 0)
- [x] GET /products — retorna lista do banco
- [x] GET /products/{id} — retorna do banco ou 404
- [x] POST /products — persiste no banco, retorna 201
- [x] PUT /products/{id} — atualiza no banco, retorna 204
- [x] DELETE /products/{id} — remove do banco, retorna 204
- [x] Índice único rejeita duplicatas com mensagem clara
- [x] Dados persistem após reiniciar a aplicação
- [x] DatabaseSeeder popula dados iniciais
- [x] Todos endpoints testáveis via Swagger
- [x] README com instruções

---

**Status:** ✅ Desafio Completo