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
        private readonly AppDbContext _context;

        public VeiculoController()
        {
            _context = new AppDbContext();
        }

        [HttpPost]
        public IActionResult AdicionarVeiculo([FromBody] Veiculo veiculo)
        {
            _context.Veiculos.InsertOne(veiculo);

            return Ok("Registro inserido com sucesso");
        }

        [HttpGet]
        public IActionResult ObterVeiculo()
        {
            var listVeiculos = _context.Veiculos.Find(new BsonDocument()).ToList();

            return Ok(listVeiculos);
        }

        [HttpGet("byid")]
        public IActionResult ObterVeiculoPorId(string id)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound();

            return Ok(veiculo);
        }

    }
}
