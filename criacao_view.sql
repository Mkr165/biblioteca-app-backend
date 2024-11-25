CREATE VIEW vwRelatorioLivrosAgrupados AS
SELECT 
    a.CodAu AS AutorID,
    a.Nome AS AutorNome,
    l.Codl AS LivroID,
    l.Titulo AS LivroTitulo,
    l.Editora AS LivroEditora,
    l.Edicao AS LivroEdicao,
    l.AnoPublicacao AS LivroAnoPublicacao,
    GROUP_CONCAT(DISTINCT s.Descricao SEPARATOR ', ') AS Assuntos
FROM 
    Autor a
LEFT JOIN Livro_Autor la ON a.CodAu = la.Autor_CodAu
LEFT JOIN Livro l ON la.Livro_Codl = l.Codl
LEFT JOIN Livro_Assunto ls ON l.Codl = ls.Livro_Codl
LEFT JOIN Assunto s ON ls.Assunto_CodAs = s.CodAs
GROUP BY a.CodAu, a.Nome, l.Codl, l.Titulo, l.Editora, l.Edicao, l.AnoPublicacao;
