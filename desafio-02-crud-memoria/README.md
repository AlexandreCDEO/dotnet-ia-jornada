# Desafio 02 — CRUD em Memória

## 📋 Descrição
Implementação de uma API CRUD completa com armazenamento em memória usando Minimal APIs, Data Annotations para validação e Endpoint Filters para centralizar regras de negócio.

## 🚀 Como Rodar

### Pré-requisitos
- .NET 8 instalado
- Terminal/PowerShell

### Passos
1. **Acesse o diretório do projeto:**
   ```bash
   cd desafio-02-crud-memoria
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
   - Health Check: `GET https://localhost:5001/api/health`

## 📊 Endpoints

### GET /api/products
Lista todos os produtos cadastrados.

**Response:**
```json
[
  {
    "id": "550e8400-e29b-41d4-a716-446655440000",
    "name": "Notebook",
    "price": 3500.00,
    "quantityInStock": 10,
    "createdAt": "2026-05-17T10:30:45.1234567Z"
  }
]
```

### GET /api/products/{id}
Retorna um produto específico ou 404 se não encontrar.

**Response (200 OK):**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "name": "Notebook",
  "price": 3500.00,
  "quantityInStock": 10,
  "createdAt": "2026-05-17T10:30:45.1234567Z"
}
```

**Response (404 Not Found):** Quando o ID não existe

### POST /api/products
Cria um novo produto e retorna 201 com header `Location`.

**Request:**
```json
{
  "name": "Notebook",
  "price": 3500.00,
  "quantityInStock": 10
}
```

**Response (201 Created):**
```
Location: /api/products/550e8400-e29b-41d4-a716-446655440000
Body: { produto criado com ID }
```

**Response (400 Bad Request):** Quando validação falha
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "Name": ["O nome do produto é obrigatorio"],
    "Price": ["O preço deve ser maior que zero"]
  }
}
```

### PUT /api/products/{id}
Atualiza um produto existente. Retorna 204 ou 404.

**Request:**
```json
{
  "name": "Notebook Gamer",
  "price": 4500.00,
  "quantityInStock": 5
}
```

**Response (204 No Content):** Sucesso (sem body)

**Response (404 Not Found):** Quando o ID não existe

### DELETE /api/products/{id}
Remove um produto. Retorna 204 ou 404.

**Response (204 No Content):** Sucesso

**Response (404 Not Found):** Quando o ID não existe

## 🎓 O que foi Aprendido

### 1. **HTTP Verbs e REST**
- `GET` — leitura de dados (seguro e idempotente)
- `POST` — criação de novo recurso (retorna 201 Created com Location header)
- `PUT` — atualização completa de recurso (idempotente)
- `DELETE` — remoção de recurso (retorna 204 No Content)
- Importância dos status codes corretos na semântica HTTP

### 2. **Status Codes Semânticos**
- `200 OK` — requisição bem-sucedida com resposta
- `201 Created` — recurso criado com sucesso (retorna Location do novo recurso)
- `204 No Content` — sucesso sem retornar body (ideal para PUT/DELETE)
- `400 Bad Request` — validação falhou
- `404 Not Found` — recurso não encontrado

### 3. **Data Annotations e Validação**
- `[Required]` — valida campos obrigatórios
- `[Range]` — valida intervalo de valores numéricos
- `MinimumIsExclusive` — torna o mínimo exclusivo (> em vez de >=)
- Mensagens de erro customizadas via `ErrorMessage`

### 4. **Endpoint Filters (IEndpointFilter)**
- Validação centralizada sem código duplicado em cada endpoint
- Pipeline customizado antes de executar o handler
- Acesso a `context.Arguments` para inspecionar parâmetros
- `Validator.TryValidateObject()` para executar validações do .NET

### 5. **Armazenamento em Memória**
- `ConcurrentDictionary<Guid, T>` para thread-safety
- Dados persistem enquanto a aplicação está rodando
- Reinicialização limpa toda a memória (intencional neste desafio)
- Ideal para prototipagem e testes, não para produção

### 6. **Record Types (C# 9+)**
- Sintaxe declarativa com `record`
- Imutabilidade por padrão (valores)
- Otimização automática para comparação
- Ideal para Data Transfer Objects (DTOs) e Commands

### 7. **Primary Constructors (C# 12)**
- Sintaxe enxuta: `class Product(string name, decimal price, int quantity)`
- Parâmetros automaticamente criados como propriedades
- Reduz boilerplate em modelos simples
- Compatível com inicialização via `new Product(...)`

### 8. **Location Header no POST**
- Padrão REST: retornar `Location: /api/products/{id}` no 201
- Cliente sabe exatamente onde encontrar o recurso criado
- Implementado com `Results.Created($"/api/products/{newProduct.Id}", newProduct)`

### 9. **Binding de Request/Response**
- ASP.NET Core faz binding automático de JSON → objetos C#
- Classes e records com propriedades públicas são bindadas automaticamente
- Tipo `CreateProductCommand` separa dados de entrada do modelo interno

### 10. **Separação de Responsabilidades**
- `Product` — modelo de domínio
- `CreateProductCommand` — DTO para input de POST/PUT
- `ProductRepository` — padrão Repository para acesso a dados
- `ValidationFilter<T>` — validação como middleware/filter

## 🔧 Estrutura do Projeto
```
desafio-02-crud-memoria/
├── Program.cs                          # Configuração e endpoints CRUD
├── Product.cs                          # Modelo de domínio
├── ProductRepository.cs                # Padrão Repository (acesso a dados)
├── CreateProductCommand.cs             # DTO com validações
├── ValidationFilter.cs                 # Middleware de validação
├── Properties/
│   └── launchSettings.json             # Configuração de porta e ambiente
├── appsettings.json                    # Configuração geral
├── appsettings.Development.json        # Config específica dev
├── desafio-02-crud-memoria.csproj      # Definição do projeto
├── DESAFIO.md                          # Especificação do desafio
└── README.md                           # Este arquivo
```

## 💡 Dicas Úteis

- **Recompilar sem parar:** Use `dotnet watch run` para reload automático
- **Testar com Swagger:** Acesse `https://localhost:5001/swagger` e teste cada endpoint
- **Testar com curl:** 
  ```bash
  # GET todos
  curl https://localhost:5001/api/products -k
  
  # POST novo
  curl -X POST https://localhost:5001/api/products -k \
    -H "Content-Type: application/json" \
    -d '{"name":"Produto","price":100,"quantityInStock":5}'
  
  # GET por ID
  curl https://localhost:5001/api/products/{id} -k
  
  # PUT para atualizar
  curl -X PUT https://localhost:5001/api/products/{id} -k \
    -H "Content-Type: application/json" \
    -d '{"name":"Novo Nome","price":150,"quantityInStock":3}'
  
  # DELETE
  curl -X DELETE https://localhost:5001/api/products/{id} -k
  ```

- **Testar validação:**
  ```bash
  # Sem nome (deve retornar 400)
  curl -X POST https://localhost:5001/api/products -k \
    -H "Content-Type: application/json" \
    -d '{"name":"","price":100,"quantityInStock":5}'
  
  # Preço negativo (deve retornar 400)
  curl -X POST https://localhost:5001/api/products -k \
    -H "Content-Type: application/json" \
    -d '{"name":"Produto","price":-10,"quantityInStock":5}'
  ```

- **Ver logs detalhados:** Execute com `dotnet run --verbose`

## ✅ Checklist de Conclusão
- [x] Modelo Product com Id, Nome, Preço, Estoque, CriadoEm
- [x] Repositório com armazenamento em memória (ConcurrentDictionary)
- [x] GET /api/products — lista todos
- [x] GET /api/products/{id} — retorna um ou 404
- [x] POST /api/products — cria e retorna 201 com Location header
- [x] PUT /api/products/{id} — atualiza ou 404, retorna 204
- [x] DELETE /api/products/{id} — remove ou 404, retorna 204
- [x] Validação: nome obrigatório
- [x] Validação: preço > 0
- [x] Validação retorna 400 Bad Request com ProblemDetails
- [x] Todos endpoints testáveis via Swagger
- [x] README com instruções

---

**Status:** ✅ Desafio Completo