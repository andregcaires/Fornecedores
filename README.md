# Exemplo de sistema CRUD de Fornecedores usando C# com Windows Forms e ADO .NET

Para utilizar, criar um banco de dados e uma tabela utilizando a seguinte query:
CREATE TABLE [DBO].[DadosCadastrais]
{
[codigo] INT NOT NULL PRIMARY KEY,
[nome] VARCHAR(35) NOT NULL,
[email] VARCHAR(35) NULL,
[telefone] VARCHAR(35) NULL,
[pessoa] VARCHAR(15) NULL,
[documento] VARCHAR(20) NULL
}

Em seguida, inserir a string de conexão na classe Dados.cs
