using CRUD_Veiculos.Data;
using CRUD_Veiculos.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CRUD_Veiculos.Controllers
{
    [ApiController]
    [Route("controller")]
    public class VeiculoController : ControllerBase
    {
        
        [HttpPost]
        public IActionResult AddVeiculo([FromBody] Veiculo veiculo) 
        {
            var connectionVeiculos = new AppDbContext();
            connectionVeiculos.Veiculos.InsertOne(veiculo);

            return Ok("Registro inserido com sucesso");
        }

        [HttpGet]
        public IActionResult GetVeiculo() 
        {
            var connectionVeiculos = new AppDbContext();
            var listVeiculos = connectionVeiculos.Veiculos.Find(new BsonDocument()).ToList();

            return Ok(listVeiculos);
        }

    }
}
