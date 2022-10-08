namespace OpahQueue.Models
{
    public class User
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        // ADD TO TABLE nickname, full name, firstname and lastname
        public string? Username { get; set; }
        public bool WorkStatus { get; set; } = false; // TRUE if BUSY
        
        public int Position { get; set; } = 0;

        public bool IsItInTheQueue { get; set; } = false;
        
        public DateTime LastConn { get; set; }

        // O [JsonIgnore] atributo impede que a propriedade de Secret seja serializada e retornada nas respostas da API.
        // O [JsonIgnore] é bom pra não retornar, MAS, também não deixa enviar os dados pela API. PARA FUNCIONAR TERIA QUE ENVIAR POR OUTRA CLASSE.
        // [JsonIgnore]
        public string? Password { get; set; }

        public string? UserStatus { get; set; } = "ativo";

        public User CT(User u){
            
            DateTime agora = DateTime.Now;
            Console.WriteLine(agora);
            Console.WriteLine(u.LastConn);
            var aki = agora.Subtract(u.LastConn).TotalSeconds;                
            if(aki > 60)
            {
                Console.WriteLine("É MAIOR = " + aki);
                LastConn = agora;
                // u.LastConn = agora;
            }
            else
            {
                Console.WriteLine("é menor = " + aki);
            }
            return this;
            // return u;
        }
    }
}


// sai da fila em dois momentos: loga, desloga, fica off
// entra na fila quandol loga 

// https://docs.microsoft.com/pt-br/dotnet/api/system.timespan?view=net-6.0

// https://docs.microsoft.com/pt-br/dotnet/standard/datetime/performing-arithmetic-operations

//   DateTime d1 = DateTime.Now;
//   DateTime d2 = DateTime.Now;

//   Console.WriteLine(d1.TimeOfDay);
//   Console.WriteLine(d2.TimeOfDay);
//   Console.WriteLine("d2 - d1");
//   Console.WriteLine(d2-d1);
//   Console.WriteLine("Seconds");
//   Console.WriteLine(d2.Second - d1.Second);      

//   if(d2.Second - d1.Second > 1)
//   Console.WriteLine("PASSOU");
//   else
//   Console.WriteLine("NÃO");      
//   Console.WriteLine(d1.Second);
//   Console.WriteLine(d2.Second);

// USAR ESSA AQUI EMBAIXO NO IF. FICA MELHOR ASSIM.

// Console.WriteLine("AKI");
// var aki = d2.Subtract(d1).TotalSeconds;
// Console.WriteLine(aki);
// Console.WriteLine("AKI");

// https://www.entityframeworktutorial.net/faq/set-created-and-modified-date-in-efcore.aspx

// CONVERTER DATETIME PARA SQL SERVER DATETIME
// https://stackoverflow.com/questions/17418258/datetime-format-to-sql-format-using-c-sharp

// DateTime myDateTime = DateTime.Now;
// string sqlFormattedDate = myDateTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
// Console.WriteLine(myDateTime);
// Console.WriteLine(sqlFormattedDate);

// https://www.w3schools.com/sql/sql_dates.asp
// https://docs.microsoft.com/pt-br/dotnet/api/system.datetime.tostring?view=net-6.0
