namespace CRUD_Veiculos.Models
{
    public class VendaModel
    {
        public VendaModel(DateTime dataVenda, int totalPreco, int totalVeiculos)
        {
            DataVenda = dataVenda;
            TotalPreco = totalPreco;
            TotalVeiculos = totalVeiculos;
        }

        public DateTime DataVenda { get; set; }
        public int TotalPreco { get; set; }
        public int TotalVeiculos { get; set; }
    }
}
