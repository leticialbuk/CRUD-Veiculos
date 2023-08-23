namespace CRUD_Veiculos.Models
{
    public class VendaModel
    {
        public VendaModel(int totalPreco, int totalVeiculos)
        {
            TotalPreco = totalPreco;
            TotalVeiculos = totalVeiculos;
        }

        public int TotalPreco { get; set; }
        public int TotalVeiculos { get; set; }
    }
}
