using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AvaliacaoDesempenho.Dominio.BusinessEntities
{
    public class EmailInformativo
    {
        [Display(Name="Assunto")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(200, ErrorMessage = "O {0} deve ter o tamanho máximo de 200 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string Assunto { get; set; }

        [Display(Name = "Cabeçalho HTML")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(1000, ErrorMessage = "O {0} deve ter o tamanho máximo de 1000 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string CabecalhoHTML { get; set; }

        [Display(Name = "Mensagem a ser enviada")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(1000, ErrorMessage = "A {0} deve ter o tamanho máximo de 1000 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string Mensagem { get; set; }

        [Display(Name = "Rodapé HTML")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(200, ErrorMessage = "O {0} deve ter o tamanho máximo de 200 caracteres.")]
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        public string RodapeHTML { get; set; }

        public List<string> ListaDeEmails { get; set; }
    }
}