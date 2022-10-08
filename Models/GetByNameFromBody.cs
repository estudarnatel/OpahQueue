namespace OpahQueue.Models
{
    public class GetByNameFromBody
    {
        public String? Name { get; set; } //https://docs.microsoft.com/pt-br/dotnet/csharp/nullable-references (para saber a razão da interrogação na String? Name)
        // warning CS8618: O propriedade não anulável 'Name' precisa conter um valor não nulo ao sair do construtor. Considere declarar o propriedade como anulável
    }
}