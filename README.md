# GRUPO28_JSON

Implementação em ASP.NET Core Web API com contratos JSON mantidos, baseada na lógica do PDF "Arquitetura preliminar API JSON".

## Requisitos funcionais cobertos

- Receção de JSON com credenciais de login
- Envio de resposta em JSON
- Mensagens previstas:
	- "Username/Password incorreto/s"
	- "Login efetuado com sucesso!"
	- "Base de dados inoperacional"

## Tecnologia JSON

- Newtonsoft.Json

## Endpoint# GRUPO28_JSON

Implementação em ASP.NET Core Web API com contratos JSON mantidos, baseada na lógica do PDF "Arquitetura preliminar API JSON".

## Requisitos funcionais cobertos

- Receção de JSON com credenciais de login
- Envio de resposta em JSON
- Mensagens previstas:
	- "Username/Password incorreto/s"
	- "Login efetuado com sucesso!"
	- "Base de dados inoperacional"

## Tecnologia JSON

- Newtonsoft.Json

## Endpoint

- `POST /api/auth/login`

### Contrato de pedido (JSON)

```json
{
	"username": "admin",
	"password": "1234"
}
```

### Contrato de resposta (JSON)

```json
{
	"success": true,
	"message": "Login efetuado com sucesso!",
	"code": "OK"
}
```

## Regras de autenticação

- Sucesso:
	- username: admin
	- password: 1234
- Credenciais inválidas:
	- qualquer combinação diferente
- Base de dados inoperacional (simulação):
	- username: db_offline
	- password: qualquer
	- retorna HTTP 503

## Como executar

```bash
dotnet restore
dotnet run
```

Por omissão, a API fica disponível em:

- `http://localhost:5000`
- `https://localhost:5001`

## Exemplos curl

### Login com sucesso

```bash
curl -X POST http://localhost:5000/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{"username":"admin","password":"1234"}'
```

### Login inválido

```bash
curl -X POST http://localhost:5000/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{"username":"admin","password":"errado"}'
```

### Base inoperacional (simulação)

```bash
curl -X POST http://localhost:5000/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{"username":"db_offline","password":"qualquer"}'
```

- `POST /api/auth/login`

### Contrato de pedido (JSON)

```json
{
	"username": "admin",
	"password": "1234"
}
```

### Contrato de resposta (JSON)

```json
{
	"success": true,
	"message": "Login efetuado com sucesso!",
	"code": "OK"
}
```

## Regras de autenticação

- Sucesso:
	- username: admin
	- password: 1234
- Credenciais inválidas:
	- qualquer combinação diferente
- Base de dados inoperacional (simulação):
	- username: db_offline
	- password: qualquer
	- retorna HTTP 503

## Como executar

```bash
dotnet restore
dotnet run
```

Por omissão, a API fica disponível em:

- `http://localhost:5000`
- `https://localhost:5001`

## Exemplos curl

### Login com sucesso

```bash
curl -X POST http://localhost:5000/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{"username":"admin","password":"1234"}'
```

### Login inválido

```bash
curl -X POST http://localhost:5000/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{"username":"admin","password":"errado"}'
```

### Base inoperacional (simulação)

```bash
curl -X POST http://localhost:5000/api/auth/login \
	-H "Content-Type: application/json" \
	-d '{"username":"db_offline","password":"qualquer"}'
```