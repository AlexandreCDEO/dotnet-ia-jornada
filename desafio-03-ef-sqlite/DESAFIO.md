## Desafio 03 — EF Core + SQLite

`⏱ 2-3h` `📦 EF Core 8, SQLite, Fluent API`

**Objetivo:** Persistência real. Mesmo CRUD do desafio 2, agora com banco de dados.

**Especificação:**
- Criar `AppDbContext : DbContext`
- Registrar no DI: `builder.Services.AddDbContext<AppDbContext>(...)`
- Migration "Initial" criada e aplicada
- Configurações via Fluent API (não Data Annotations no modelo):
    - Nome: obrigatório, max 100 chars
    - Preço: precisão (18,2)
    - Nome: índice único
- Validações do desafio 2 mantidas

**✅ Definition of Done:**
- [ ] `dotnet ef migrations add Initial` cria migration sem erros
- [ ] `dotnet ef database update` cria o banco
- [ ] Dados persistem após reiniciar a aplicação
- [ ] Índice único de nome rejeita duplicatas com mensagem clara

**💡 Dica Prática:**
Crie um `DatabaseSeeder` para popular dados de teste ao iniciar:
```csharp
// Program.cs
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
db.Database.Migrate(); // aplica migrations pendentes automaticamente
```

**⚠️ Armadilha Comum:**
Não misture Fluent API com Data Annotations no mesmo modelo. Escolha um e seja consistente. Fluent API é preferida em projetos profissionais porque mantém o modelo limpo.

**🔧 Comandos essenciais EF Core:**
```bash
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet ef migrations add NomeDaMigration
dotnet ef database update
dotnet ef migrations remove   # desfaz última migration
dotnet ef database drop       # zera o banco (dev)
```