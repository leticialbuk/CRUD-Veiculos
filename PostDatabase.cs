using CRUD_Veiculos.Data;
using CRUD_Veiculos.Models;

namespace CRUD_Veiculos
{
    public class PostDatabase
    {
        public PostDatabase() 
        {
            Veiculo veiculo = new Veiculo("Honda", "Civic", 2017, 50000);

            var connectionVeiculos = new DbContext();
            connectionVeiculos.Veiculos.InsertOne(veiculo);
        }
    }
}
