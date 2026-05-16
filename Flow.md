## Fluxo UML (PlantUML)

Copiar os blocos abaixo para o site https://plantuml.com/ para visualizar os diagramas.

### 1) Login Flow

```plantuml
@startuml
title Login Flow (Client + API)

actor User
participant "LoginComponent" as LoginUI
participant "AuthService (client)" as ClientAuth
participant "AuthController" as Controller
participant "Model" as Model
participant "FileUserRepository" as Repo
database "users.txt" as UsersFile

User -> LoginUI : Submit username/password
LoginUI -> ClientAuth : login(request)
ClientAuth -> Controller : POST /api/auth/login
Controller -> Model : AuthenticateAsync(request)
Model -> Repo : FindUser(username)
Repo -> UsersFile : Read lines (JSON)
UsersFile --> Repo : UserRecord or null
Repo --> Model : UserRecord or null

alt User found and password hash matches
  Model --> Controller : LoginResponse{success=true, code=OK}
  Controller --> ClientAuth : HTTP 200 + JSON
  ClientAuth -> ClientAuth : set loggedInUsername
  ClientAuth --> LoginUI : success=true
  LoginUI -> User : Navigate to /home
else Invalid credentials
  Model --> Controller : LoginResponse{success=false, code=INVALID_CREDENTIALS}
  Controller --> ClientAuth : HTTP 200 + JSON
  ClientAuth --> LoginUI : success=false
  LoginUI -> User : Show error message
end

@enduml
```

### 2) Register Flow

```plantuml
@startuml
title Register Flow (Client + API)

actor User
participant "RegisterComponent" as RegisterUI
participant "AuthService (client)" as ClientAuth
participant "AuthController" as Controller
participant "Model" as Model
participant "FileUserRepository" as Repo
database "users.txt" as UsersFile

User -> RegisterUI : Submit username/email/password
RegisterUI -> ClientAuth : register(request)
ClientAuth -> Controller : POST /api/auth/register
Controller -> Model : RegisterAsync(request)

alt Missing fields or invalid payload
  Model --> Controller : RegisterResponse{code=MISSING_FIELDS/INVALID_REQUEST}
  Controller --> ClientAuth : HTTP 400 + JSON
  ClientAuth --> RegisterUI : error
else Username already exists
  Model -> Repo : UserExists(username)
  Repo -> UsersFile : Read lines
  UsersFile --> Repo : Existing user found
  Repo --> Model : true
  Model --> Controller : RegisterResponse{code=USERNAME_TAKEN}
  Controller --> ClientAuth : HTTP 409 + JSON
  ClientAuth --> RegisterUI : conflict
else Valid registration
  Model -> Repo : AddUser(UserRecord with BCrypt hash)
  Repo -> UsersFile : Append JSON line
  UsersFile --> Repo : persisted
  Repo --> Model : done
  Model --> Controller : RegisterResponse{code=CREATED}
  Controller --> ClientAuth : HTTP 201 + JSON
  ClientAuth --> RegisterUI : success
  RegisterUI -> User : Navigate to /login
end

@enduml
```

### 3) Protected Route + Logout Flow

```plantuml
@startuml
title Protected Route and Logout (Client)

actor User
participant "Router" as Router
participant "AuthGuard" as Guard
participant "AuthService" as ClientAuth
participant "HomeComponent" as Home

User -> Router : Navigate to /home
Router -> Guard : canActivate()
Guard -> ClientAuth : isLoggedIn

alt Logged in
  Guard --> Router : true
  Router --> Home : render
else Not logged in
  Guard -> Router : navigate('/login')
  Guard --> Router : false
end

User -> Home : Click "Log Out"
Home -> ClientAuth : logout()
ClientAuth -> ClientAuth : clear loggedInUsername
Home -> Router : navigate('/login')

@enduml
```

### 4) Health Check

```plantuml
@startuml
title API Health Check

actor Client
participant "AuthController" as Controller

Client -> Controller : GET /api/auth/ping
Controller --> Client : HTTP 200 + "pong"

@enduml
```