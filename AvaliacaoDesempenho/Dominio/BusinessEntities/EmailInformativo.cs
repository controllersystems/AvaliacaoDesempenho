using System.ComponentModel.DataAnnotations;
namespace AvaliacaoDesempenho.Dominio.BusinessEntities
{
    public class EmailInformativo
    {
        [Display(Name="Assunto")]
        public string Assunto { get; set; }

        [Display(Name = "Cabeçalho HTML")]
        public string CabecalhoHTML { get; set; }

        [Display(Name = "Mensagem a ser enviada")]
        public string Mensagem { get; set; }

        [Display(Name = "Rodapé HTML")]
        public string RodapeHTML { get; set; }
    }
}