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
        public IActionResult ObterVeiculos(int? ano, string? modelo, int? precoMenorQue, int? precoMaiorQue, DateTime? dataDeCadastroMaiorQue, DateTime? dataDeCadastroMenorQue, int? skip, int? take)
        {
            var builder = Builders<Veiculo>.Filter;
            var filter = builder.Eq(x => x.Vendido, false);

            if (ano != null)
                filter &= builder.Eq(x => x.Ano, ano);

            if (modelo != null)
                filter &= builder.Eq(x => x.Modelo, modelo);

            if (precoMenorQue != null)
                filter &= builder.Where(x => x.Preco < precoMenorQue);

            if (precoMaiorQue != null)
                filter &= builder.Where(x => x.Preco > precoMaiorQue);

            if (dataDeCadastroMaiorQue != null)
                filter &= builder.Where(x => x.DataCriacao > dataDeCadastroMaiorQue);

            if (dataDeCadastroMenorQue != null)
                filter &= builder.Where(x => x.DataCriacao < dataDeCadastroMenorQue);

            skip = skip == null ? 0 : skip;
            take = take == null ? 5 : take;

            var listaVeiculos = _context.Veiculos.Find(filter).Skip(skip).Limit(take).ToEnumerable();

            return Ok(listaVeiculos);
        }

        [HttpGet("byid")]
        public IActionResult ObterVeiculoPorId(string id)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound("Ops! Nenhuma informação encontrada");

            return Ok(veiculo);
        }

        [HttpGet("vendas")]
        public IActionResult Vendas(DateTime dataInicio, DateTime dataFim)
        {
            var builder = Builders<Veiculo>.Filter;
            var filter = builder.Eq(x => x.Vendido, true);
            filter &= builder.Where(x => x.DataVenda >= dataInicio && x.DataVenda <= dataFim);

            var veiculos = _context.Veiculos.Find(filter).ToList();
            if (veiculos == null)
                return NotFound("Ops! Nenhuma informação encontrada");

            var totalVeiculos = veiculos.Count();

            var totalPreco = 0;
            foreach (var veiculo in veiculos)
            {
                totalPreco += veiculo.Preco;
            }

            var vendaModel = new VendaModel(totalPreco, totalVeiculos);

            return Ok(vendaModel);
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
                return NotFound("Ops! Nenhuma informação encontrada");

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
                return NotFound("Ops! Nenhuma informação encontrada");

            if (veiculo.Vendido == true)
                return BadRequest("Este veiculo já está vendido");

            veiculo.Preco = veiculo.Preco - (veiculo.Preco * desconto / 100);
            veiculo.Vendido = true;
            veiculo.DataVenda = DateTime.Now.Date;

            _context.Veiculos.ReplaceOne(x => x.Id == id, veiculo);

            return Ok("Veiculo vendido com sucesso");
        }

        [HttpDelete]
        public IActionResult DeletarVeiculoPorId(string id)
        {
            var veiculo = _context.Veiculos.Find(x => x.Id == id).FirstOrDefault();
            if (veiculo == null)
                return NotFound("Ops! Nenhuma informação encontrada");

            _context.Veiculos.DeleteOne(x => x.Id == id);

            return Ok("Registro removido com sucesso");
        }
    }
}
