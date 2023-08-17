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
            =>_context = new AppDbContext();

        [HttpGet]
        public IActionResult ObterVeiculo()
        {
            var listaVeiculos = _context.Veiculos.Find(new BsonDocument()).ToList();
            if (listaVeiculos == null)
                return NotFound();

            return Ok(listaVeiculos);
        }

        [HttpGet("byid")]
        public IActionResult ObterVeiculoPorId(string id)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound();

            return Ok(veiculo);
        }

        [HttpPost]
        public IActionResult AdicionarVeiculo([FromBody] Veiculo model)
        {
            _context.Veiculos.InsertOne(model);

            return Ok("Registro inserido com sucesso");
        }

        [HttpPut]
        public IActionResult AlterarVeiculo([FromBody] Veiculo model)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == model.Id).FirstOrDefault();
            if (veiculo == null)
                return NotFound();

            _context.Veiculos.ReplaceOne(x => x.Id == model.Id, model);

            return Ok("Registro alterado com sucesso");
        }

        [HttpDelete]
        public IActionResult DeletarVeiculoPorId(string id)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound();

            _context.Veiculos.DeleteOne(x => x.Id == id);

            return Ok("Registro removido com sucesso");
        }
    }
}
