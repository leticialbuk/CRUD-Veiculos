using CRUD_Veiculos.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CRUD_Veiculos
{
    public class GetDatabase
    {
        public GetDatabase() 
        {
            var connectionVeiculos = new DbContext();
            Console.WriteLine("Listando veíclos");

            var listaVeiculos = connectionVeiculos.Veiculos.Find(new BsonDocument()).ToList();
            foreach (var item in listaVeiculos)
                Console.WriteLine(item.ToJson<Models.Veiculo>());
        }
    }
}
