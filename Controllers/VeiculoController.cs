﻿using CRUD_Veiculos.Data;
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
        public IActionResult ObterVeiculos(int? ano, string? modelo, int? precoMaiorQue, int? precoMenorQue, string? dataDeCadastroMaiorQue, string? dataDeCadastroMenorQue, int? skip, int? take)
        {
            var listaVeiculos = _context.Veiculos.Find(x => x.Vendido == false).ToEnumerable();

            if (ano != null)
                listaVeiculos = listaVeiculos.Where(x => x.Ano == ano);

            if (modelo != null)
                listaVeiculos = listaVeiculos.Where(x => x.Modelo == modelo);

            if (precoMaiorQue != null)
                listaVeiculos = listaVeiculos.Where(x => x.Preco > precoMaiorQue);
            
            if(precoMenorQue != null)
                listaVeiculos = listaVeiculos.Where(x => x.Preco > precoMenorQue);

            if (dataDeCadastroMaiorQue != null)
                listaVeiculos = listaVeiculos.Where(x => x.DataCriacao < DateTime.Parse(dataDeCadastroMaiorQue));

            if (dataDeCadastroMenorQue != null)
                listaVeiculos = listaVeiculos.Where(x => x.DataCriacao < DateTime.Parse(dataDeCadastroMenorQue));

            skip = skip == null ? 0 : skip;
            take = take == null ? 5 : take;

            listaVeiculos = listaVeiculos.Skip((int)skip).Take((int)take).ToList();
            if (listaVeiculos.Count() == 0)
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
        public IActionResult VenderVeiculo(string id, int desconto)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound();

            if (veiculo.Vendido == true)
                return BadRequest("Este veiculo já está vendido");

            veiculo.Preco = veiculo.Preco - (veiculo.Preco * desconto / 100);
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
