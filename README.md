# Biblioteca de Registro de Livros

Este projeto é uma aplicação completa de gerenciamento de uma biblioteca, com funcionalidades para cadastro de autores, livros, e assuntos, além de gerar relatórios em PDF utilizando **FastReport**.

---

## 🚀 Funcionalidades

- Cadastro de **Autores**, **Livros** e **Assuntos**.
- Relacionamento entre **Livros e Autores** e **Livros e Assuntos**.
- Pesquisa de livros pelo título.
- Geração de relatórios agrupados por autor em formato **PDF**.
- API RESTful com rotas organizadas.

---

## 🛠️ Tecnologias Utilizadas

- **Backend**: ASP.NET Core 3.1
- **Banco de Dados**: MySQL
- **ORM**: Entity Framework Core
- **Relatórios**: FastReport OpenSource
- **Frontend**: Angular (para interface de gerenciamento)

---

## 📦 Estrutura do Projeto

```plaintext
├── backend-registro-livros/
│   ├── Controllers/
│   ├── Models/
│   ├── DbContexts/
│   ├── Reports/       # Modelos .frx para relatórios
│   └── Startup.cs     # Configuração inicial do backend
├── biblioteca-app/     # Aplicação Angular (frontend)
│   ├── pages/
│   ├── components/
│   └── app.module.ts
```


## 🔧 Configuração do Ambiente

### 1. Backend
1. Instale os pacotes necessários:
   ```bash
   dotnet restore
   ```
2. Atualize o arquivo **appsettings.json** com a string de conexão ao MySQL:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=biblioteca;User=root;Password=sua-senha;"
   }
   ```
3. Execute as migrações para criar o banco de dados:
   ```bash
   dotnet ef database update
   ```
4. Inicie o servidor backend:
   ```bash
   dotnet run
   ```

---


## 📝 Modelo de Relatório

O relatório foi desenvolvido usando **FastReport**. O modelo pode ser encontrado em:
```plaintext
backend-registro-livros/Reports/RelatorioLivrosAutores.frx
```

---

