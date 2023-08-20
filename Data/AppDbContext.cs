using CRUD_Veiculos.Entities;
using MongoDB.Driver;

namespace CRUD_Veiculos.Data
{
    public class AppDbContext 
    {
        public const string STRING_DE_CONEXAO = "mongodb://localhost:27017";
        public const string NOME_DA_BASE = "Concessionaria";
        public const string NOME_DA_COLECAO = "Veiculos";

        private static readonly IMongoClient _client;
        private static readonly IMongoDatabase _database;

        static AppDbContext() 
        {
            _client = new MongoClient(STRING_DE_CONEXAO);
            _database = _client.GetDatabase(NOME_DA_BASE);
        }

        public IMongoClient Client { get { return _client; } }
        public IMongoCollection<Veiculo> Veiculos { get { return _database.GetCollection<Veiculo>(NOME_DA_COLECAO); } }
    }
}
