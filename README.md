# GRUPO28_JSON

Projeto académico com frontend Angular e backend ASP.NET Core para autenticação básica.

## Estrutura atual

- [api](api): Web API em .NET 8
- [client](client): frontend Angular + Angular Material

## Funcionalidades

- Registo de utilizador
- Login
- Logout no frontend
- Página protegida para utilizador autenticado
- Health check da API

## Persistência

- Utilizadores guardados em [api/users.txt](api/users.txt)
- Passwords guardadas com hash BCrypt
- O ficheiro [api/users.txt](api/users.txt) não é versionado

## Endpoints da API

- GET /api/auth/ping
  - Retorna 200 com "pong"
- POST /api/auth/login
  - Retorna 200 com LoginResponse
- POST /api/auth/register
  - Retorna 201 quando cria
  - Retorna 409 se username já existir
  - Retorna 400 para payload inválido

### Exemplo de login

Request:

```json
{
  "username": "john",
  "password": "123456"
}
```

Response (sucesso):

```json
{
  "success": true,
  "message": "Login successful.",
  "code": "OK"
}
```

## Executar localmente

Pré-requisitos:

- .NET SDK 8+
- Node.js 18+
- Angular CLI (opcional, pode usar npx)

Backend:

```bash
cd api
dotnet restore
dotnet run
```

Frontend:

```bash
cd client
npm install
npm start
```

URLs padrão:

- API: http://localhost:5000
- Frontend: http://localhost:4200

## Configuração por ambiente

### Backend (.NET)

Config base em [api/appsettings.json](api/appsettings.json):

- Urls
- Cors:AllowedOrigins

Também pode ser sobrescrito com variáveis de ambiente.

Exemplos:

```bash
ASPNETCORE_URLS=http://localhost:5000
Cors__AllowedOrigins__0=http://localhost:4200
Cors__AllowedOrigins__1=https://app.exemplo.com
```

### Frontend (Angular)

Config runtime em [client/src/assets/app-config.json](client/src/assets/app-config.json):

```json
{
  "apiUrl": "http://localhost:5000/api"
}
```

Vantagem: pode alterar URL da API sem rebuild do frontend.

## Principais ficheiros

- [api/Program.cs](api/Program.cs): bootstrap da API, CORS e DI
- [api/Controllers/AuthController.cs](api/Controllers/AuthController.cs): endpoints
- [api/Model/Model.cs](api/Model/Model.cs): regras de autenticação e registo
- [api/Services/FileUserRepository.cs](api/Services/FileUserRepository.cs): leitura/escrita de users.txt
- [client/src/app/services/auth.service.ts](client/src/app/services/auth.service.ts): chamadas HTTP de auth
- [client/src/app/guards/auth.guard.ts](client/src/app/guards/auth.guard.ts): proteção de rota

## Observações

- Este projeto foi feito para estudo/faculdade
- Não usa JWT ou sessão persistente no browser
- O estado de login do frontend está em memória