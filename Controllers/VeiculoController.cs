using CRUD_Veiculos.Data;
using CRUD_Veiculos.Entities;
using CRUD_Veiculos.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CRUD_Veiculos.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VeiculoController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VeiculoController() => _context = new AppDbContext();

        [HttpGet]
        public IActionResult ObterVeiculos()
        {
            var listaVeiculos = _context.Veiculos.Find(x => x.Vendido == false).ToList();
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
        public IActionResult AdicionarVeiculo([FromBody] VeiculoModel model)
        {
            var veiculo = new Veiculo(model.Marca, model.Modelo, model.Ano, model.Preco);
            _context.Veiculos.InsertOne(veiculo);

            return Ok("Registro inserido com sucesso");
        }

        [HttpPut]
        public IActionResult AlterarVeiculo(string id, [FromBody] VeiculoModel model)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound();

            veiculo.Marca = model.Marca;
            veiculo.Modelo = model.Modelo;
            veiculo.Ano = model.Ano;
            veiculo.Preco = model.Preco;

            _context.Veiculos.ReplaceOne(x => x.Id == id, veiculo);

            return Ok("Registro alterado com sucesso");
        }

        [HttpPatch]
        public IActionResult VenderVeiculo(string id)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound();

            if (veiculo.Vendido == true)
                return BadRequest("Este veiculo já está vendido");
            
            veiculo.Vendido = true;
            _context.Veiculos.ReplaceOne(x => x.Id == id, veiculo);

            return Ok("Veiculo vendido com sucesso");
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
