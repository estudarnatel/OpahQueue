namespace OpahQueue.Models
{
    public class Report
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public string? Name { get; set; }
        public int TIatendeDaily { get; set; }
        public int TIatendeMonthly { get; set; }        
        public int ExcellerDaily { get; set; }
        public int ExcellerMonthly { get; set; }
        public DateTime Datetime { get; set; }        
        public string Status { get; set; } = "ativo";
    }
}

// COMO VOU TER QUE CRIAR UMA TELA DE GERENCIAMENTO. ENTÃO CRIAR UMA TABELA COM ESSA INFORMAÇÃO PARA SER RETORNADA EM UM GET
// TODA VEZ QUE ATENDER UM CHAMADO, ADICIONAR ESSES CONTADORES DESSA CLASSE LÁ NA BASE DE DADOS
// ZERAR CONTADOR DIÁRIO TODOS OS DIAS COM BASE NO DATETIME

// https://learn.microsoft.com/pt-br/dotnet/api/system.datetime.parse?view=net-6.0#system-datetime-parse(system-string)
// https://learn.microsoft.com/pt-br/dotnet/api/system.datetime.parse?view=net-6.0#system-datetime-parse(system-string)
// https://learn.microsoft.com/pt-br/dotnet/api/system.datetime.parse?view=net-6.0#system-datetime-parse(system-string)


// retornar lista com todos usuarios com status "ativo";
// a partir dessa lista, para cada usuario (por id)
// buscar na lista de TI atende
// a quantidade de chamados daquele dia naquele id
// a quantidade de chamados chamados do mes inteiro daquele id
// depois repetir os dois passos para Exceller
// salvar cada quantidade em uma variável
// criar JSON para retornar
// e criar excel para retornar
// CRIAR CONTROLADOR SÓMENTE PARA ESSE RELATÓRIO.
// E UMA TABELA PARA ESSES DADOS COM AS MIGRATIONS
// OBS. depois da primeiroa busca com  JSON. talvez salvar esse resutado numa variável statica para na hora de exportar não precisar percorrer todo o BD mais uma vez

// OBS. PARA RESOLVER OS WARNINGS DE QUE O METODO ASYNC NÃO TEM AWAIT PODE TROCAR OS SELECTS DO EXEMPLO PELAS LINQ WHERE FEITAS POR MIM COM AWAIT NO INICIO.


