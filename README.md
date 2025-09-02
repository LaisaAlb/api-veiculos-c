````markdown
# 🚗 API de Gerenciamento de Veículos

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Entity Framework](https://img.shields.io/badge/Entity%20Framework-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![MySQL](https://img.shields.io/badge/MySQL-4479A1?style=for-the-badge&logo=mysql&logoColor=white)

---

## 🔹 Sobre o Projeto
API para **gerenciamento de veículos**, com **CRUD completo** e **regras de negócio específicas**.  

Objetivo: fortalecer conhecimentos em **Back-End** e explorar novas possibilidades com **C#**.

---

## 🔹 Funcionalidades

### Usuário Editor
- Cadastrar veículos (`POST /veiculos`)
- Listar todos os veículos (`GET /veiculos`)
- **Restrições**:
  - Não pode listar veículo por ID
  - Não pode editar ou excluir veículos
  - Não pode cadastrar admin
  - Não pode listar admin

### Usuário Admin
- CRUD completo de veículos:
  - `POST /veiculos` – Cadastrar veículo
  - `GET /veiculos` – Listar todos os veículos
  - `GET /veiculos/{id}` – Listar veículo por ID
  - `PUT /veiculos/{id}` – Atualizar veículo
  - `DELETE /veiculos/{id}` – Excluir veículo
- Gestão de admins:
  - `POST /admin/login` – Login
  - `POST /admin` – Criar novo admin
  - `GET /admin` – Listar todos os admins
  - `GET /admin/{id}` – Listar admin por ID

---

## 🔹 Exemplos de Endpoints

### Veículos

**Cadastrar veículo**
```http
POST /veiculos
Content-Type: application/json

{
  "marca": "Ford",
  "modelo": "Fiesta",
  "ano": 2022,
  "placa": "ABC-1234"
}
````

**Listar todos os veículos**

```http
GET /veiculos
```

**Listar veículo por ID (Admin)**

```http
GET /veiculos/1
```

**Atualizar veículo (Admin)**

```http
PUT /veiculos/1
Content-Type: application/json

{
  "marca": "Ford",
  "modelo": "Focus",
  "ano": 2023,
  "placa": "ABC-1234"
}
```

**Excluir veículo (Admin)**

```http
DELETE /veiculos/1
```

---

### Admin

**Login**

```http
POST /admin/login
Content-Type: application/json

{
  "usuario": "admin",
  "senha": "123456"
}
```

**Criar novo Admin**

```http
POST /admin
Content-Type: application/json

{
  "usuario": "novoAdmin",
  "senha": "senhaSegura"
}
```

**Listar todos os Admins**

```http
GET /admin
```

**Listar Admin por ID**

```http
GET /admin/1
```

---

## 🔹 Tecnologias

* **C#**
* **.NET (Core/Framework)**
* **Entity Framework**
* Banco de dados: MySQL, SQL Server ou outro

---

## 🔹 Estrutura do Projeto

* **Controllers** – Recebem requisições HTTP
* **Services** – Lógica de negócio
* **Repositories** – Interação com banco de dados
* **Models/Entities** – Estrutura dos dados

---

## 🔹 Como Executar

1. Clone o repositório:

```bash
git clone https://github.com/LaisaAlb/api-veiculos-c
```

2. Abra o projeto em sua IDE (Visual Studio ou VS Code)
3. Configure a conexão com o banco em `appsettings.json`
4. Execute:

```bash
dotnet run
```

5. A API estará disponível em `http://localhost:<porta>`

---

## 🔹 Autor

**Laísa Albuquerque** – Desenvolvedora Front-End com interesse em Back-End C# e aprendizado contínuo em programação.

---


