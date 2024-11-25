
-- Criando o banco de dados
CREATE DATABASE BibliotecaDB;

-- Utilizando o banco de dados criado
USE BibliotecaDB;

-- Tabela de Autores
CREATE TABLE Autor (
    CodAu INTEGER PRIMARY KEY AUTO_INCREMENT,  -- AUTO_INCREMENT para auto gerar o ID
    Nome VARCHAR(40) NOT NULL
);

-- Tabela de Livros
CREATE TABLE Livro (
    Codl INTEGER PRIMARY KEY AUTO_INCREMENT,  -- AUTO_INCREMENT para auto gerar o ID
    Titulo VARCHAR(40) NOT NULL,
    Editora VARCHAR(40),
    Edicao INTEGER,
    AnoPublicacao VARCHAR(4),
    Preco DECIMAL(10, 2),  -- Campo de preço
    FormaCompra VARCHAR(20) -- Campo para indicar a forma de compra (balcão, internet, etc.)
);

-- Tabela de Assuntos
CREATE TABLE Assunto (
    CodAs INTEGER PRIMARY KEY AUTO_INCREMENT,  -- AUTO_INCREMENT para auto gerar o ID
    Descricao VARCHAR(20) NOT NULL
);

-- Tabela de relacionamento Livro-Autor
CREATE TABLE Livro_Autor (
    Livro_Codl INTEGER,
    Autor_CodAu INTEGER,
    PRIMARY KEY (Livro_Codl, Autor_CodAu),
    FOREIGN KEY (Livro_Codl) REFERENCES Livro(Codl) ON DELETE CASCADE,
    FOREIGN KEY (Autor_CodAu) REFERENCES Autor(CodAu) ON DELETE CASCADE
);

-- Tabela de relacionamento Livro-Assunto
CREATE TABLE Livro_Assunto (
    Livro_Codl INTEGER,
    Assunto_CodAs INTEGER,
    PRIMARY KEY (Livro_Codl, Assunto_CodAs),
    FOREIGN KEY (Livro_Codl) REFERENCES Livro(Codl) ON DELETE CASCADE,
    FOREIGN KEY (Assunto_CodAs) REFERENCES Assunto(CodAs) ON DELETE CASCADE
);
