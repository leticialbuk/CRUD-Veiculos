using MongoDB.Bson.Serialization.Attributes;

namespace CRUD_Veiculos.Models
{
    public class Veiculo
    {  
        public Veiculo( string marca, string modelo, int ano, int preco)
        {
            //Id = id;
            Marca = marca;
            Modelo = modelo;
            Ano = ano;
            Preco = preco;
        }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        //public string Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public int Preco { get; set; }

    }
}
