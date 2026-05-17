# Desafio 01 — Setup + Health Check

## 📋 Descrição
Primeiro projeto .NET da jornada com setup do ambiente, configuração do Swagger e implementação de um endpoint de health check.

## 🚀 Como Rodar

### Pré-requisitos
- .NET 8 instalado
- Terminal/PowerShell

### Passos
1. **Clone/acesse o diretório do projeto:**
   ```bash
   cd desafio-01-health-check
   ```

2. **Restaure as dependências:**
   ```bash
   dotnet restore
   ```

3. **Execute a aplicação:**
   ```bash
   dotnet run
   ```

4. **Acesse a aplicação:**
   - Swagger UI: `https://localhost:5001/swagger`
   - Endpoint de Health: `GET https://localhost:5001/api/saude`

## 📊 Endpoints

### GET /api/saude
Retorna o status da aplicação.

**Response:**
```json
{
  "status": "OK",
  "versao": "1.0",
  "timestamp": "2026-05-17T10:30:45.1234567Z"
}
```

## 🎓 O que foi Aprendido

### 1. **Minimal APIs**
- Criação rápida de APIs com `dotnet new webapi -minimal`
- Endpoint mapeado com `app.MapGet()` sem necessidade de Controllers
- Abordagem moderna e enxuta para projetos simples

### 2. **Swagger/Swashbuckle**
- Instalação e configuração automática de documentação interativa
- Geração automática de `swagger.json` via `AddEndpointsApiExplorer()`
- Configuração de `RoutePrefix` para customizar a rota da UI

### 3. **Configuração de Porta**
- Uso de `launchSettings.json` para definir portas de desenvolvimento
- Diferença entre perfis `http` e `https`
- Importância de porta fixa para ambiente de desenvolvimento

### 4. **Middleware e Ordem de Execução**
- `app.UseSwagger()` — habilita o endpoint JSON do Swagger
- `app.UseSwaggerUI()` — habilita a interface visual
- `app.UseStaticFiles()` — serve arquivos estáticos necessários
- Ordem importa! Middlewares são executados na sequência definida

### 5. **Ambiente de Desenvolvimento**
- Verificação de ambiente com `app.Environment.IsDevelopment()`
- Boas práticas: Swagger ativo apenas em desenvolvimento

### 6. **Serialização JSON**
- C# anonymous types são automaticamente serializados em JSON
- DateTime com formato ISO 8601 (`"o"`) para padrão internacional

## 🔧 Estrutura do Projeto
```
desafio-01-health-check/
├── Program.cs              # Configuração e endpoints
├── Properties/
│   └── launchSettings.json # Configuração de porta e ambiente
├── appsettings.json        # Configuração geral
├── appsettings.Development.json # Config específica dev
├── desafio-01-health-check.csproj # Definição do projeto
└── README.md               # Este arquivo
```

## 💡 Dicas Úteis

- **Recompilar sem parar:** Use `dotnet watch run` para reload automático
- **Testar endpoint:** Use Swagger UI ou ferramentas como Postman, curl, ou VS Code REST Client
- **Ver logs:** Ative com `--verbose` em `dotnet run`

## ✅ Checklist de Conclusão
- [x] Projeto criado com Minimal APIs
- [x] Swagger configurado e funcional
- [x] Endpoint `/api/saude` retornando JSON correto
- [x] Porta fixa em 5001
- [x] README com instruções

---

**Status:** ✅ Desafio Completo