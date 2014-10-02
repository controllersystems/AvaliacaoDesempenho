using AvaliacaoDesempenho.Dominio.BusinessEntities;
using AvaliacaoDesempenho.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using AvaliacaoDesempenho.Dominio.Validators;

namespace AvaliacaoDesempenho.Models.CiclosAvaliacao
{
    public class ManterCicloAvaliacaoViewModel
    {
        [Key]
        public int? ID { get; set; }

        /*****************************************************************************************
        * Dados Básicos
        ******************************************************************************************/

        [Display(Name = "Descrição do Ciclo de Avaliação")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(200, ErrorMessage = "A {0} deve ter o tamanho máximo de 200 caracteres.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        public string Descricao { get; set; }

        [Display(Name = "Data de Início da Vigência")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        //[Remote("ValidarDataVigenciaInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataFimVigencia",
        //        ErrorMessage = "A {0} deve ser menor que a Data de Fim da Vigência.")]
        public string DataInicioVigencia { get; set; }

        [Display(Name = "Data de Fim da Vigência")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        [Required(ErrorMessage = "A {0} é obrigatória.")]
        //[Remote("ValidarDataVigenciaInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataInicioVigencia",
        //        ErrorMessage = "A {0} deve ser maior que a Data de Início da Vigência.")]
        public string DataFimVigencia { get; set; }

        /*****************************************************************************************
        * Grupo de Competências
        ******************************************************************************************/

        [Display(Name = "URL do Guia de Competências")]
        [DataType(DataType.Url, ErrorMessage = "A {0} é inválida.")]
        [StringLength(200, ErrorMessage = "A {0} deve ter o tamanho máximo de 200 caracteres.")]
        public string URLCompetencia { get; set; }

        [Display(Name = "Orientação para o Colaborador")]
        [DataType(DataType.Text, ErrorMessage = "A {0} é inválida.")]
        [StringLength(200, ErrorMessage = "A {0} deve ter o tamanho máximo de 200 caracteres.")]
        public string OrientacaoCompetencia { get; set; }

        /*****************************************************************************************
        * Objetivos e Metas
        ******************************************************************************************/

        [Display(Name = "Data de Início")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataObjetivosMetasInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataTerminoObjetivosMetas",
        //        ErrorMessage = "A {0} deve ser menor que a Data de Término.")]
        public string DataInicioObjetivosMetas { get; set; }

        [Display(Name = "Data de Término")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataObjetivosMetasInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataInicioObjetivosMetas",
        //        ErrorMessage = "A {0} deve ser maior que a Data de Início.")]
        public string DataTerminoObjetivosMetas { get; set; }

        [Display(Name = "Título da Orientação")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(200, ErrorMessage = "O {0} deve ter o tamanho máximo de 200 caracteres.")]
        public string TituloOrientacaoObjetivosMetas { get; set; }

        [Display(Name = "Texto da Orientação")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(1000, ErrorMessage = "O {0} deve ter o tamanho máximo de 1000 caracteres.")]
        public string TextoOrientacaoObjetivosMetas { get; set; }

        /*****************************************************************************************
        * Auto-avaliação
        ******************************************************************************************/

        [Display(Name = "Data de Início")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataAutoAvaliacaoInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataTerminoAutoAvaliacao",
        //        ErrorMessage = "A {0} deve ser menor que a Data de Término.")]
        public string DataInicioAutoAvaliacao { get; set; }

        [Display(Name = "Data de Término")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataAutoAvaliacaoInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataInicioAutoAvaliacao",
        //        ErrorMessage = "A {0} deve ser maior que a Data de Início.")]
        public string DataTerminoAutoAvaliacao { get; set; }

        [Display(Name = "Título da Orientação")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(200, ErrorMessage = "O {0} deve ter o tamanho máximo de 200 caracteres.")]
        public string TituloOrientacaoAutoAvaliacao { get; set; }

        [Display(Name = "Texto da Orientação")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(1000, ErrorMessage = "O {0} deve ter o tamanho máximo de 1000 caracteres.")]
        public string TextoOrientacaoAutoAvaliacao { get; set; }

        /*****************************************************************************************
        * Avaliação do Gestor
        ******************************************************************************************/

        [Display(Name = "Data de Início")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataAvaliacaoGestorInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataTerminoAvaliacaoGestor",
        //        ErrorMessage = "A {0} deve ser menor que a Data de Término.")]
        public string DataInicioAvaliacaoGestor { get; set; }

        [Display(Name = "Data de Término")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataAvaliacaoGestorInicialMenorFinal", "CiclosAvaliacao",
        //        AdditionalFields = "DataInicioAvaliacaoGestor",
        //        ErrorMessage = "A {0} deve ser maior que a Data de Início.")]
        public string DataTerminoAvaliacaoGestor { get; set; }

        [Display(Name = "Título da Orientação")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(200, ErrorMessage = "O {0} deve ter o tamanho máximo de 200 caracteres.")]
        public string TituloOrientacaoAvaliacaoGestor { get; set; }

        [Display(Name = "Texto da Orientação")]
        [DataType(DataType.Text, ErrorMessage = "O {0} é inválido.")]
        [StringLength(1000, ErrorMessage = "O {0} deve ter o tamanho máximo de 1000 caracteres.")]
        public string TextoOrientacaoAvaliacaoGestor { get; set; }

        /*****************************************************************************************
        * Gerenciamento do Ciclo
        ******************************************************************************************/

        /*****************************************************************************************
        * Gerenciamento de PDI
        ******************************************************************************************/

        [Display(Name = "Data de Início")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataAvaliacaoGerenciamentoPDI", "CiclosAvaliacao",
        //        AdditionalFields = "DataTerminoGerenciamentoPDI",
        //        ErrorMessage = "A {0} deve ser menor que a Data de Término.")]
        public string DataInicioGerenciamentoPDI { get; set; }

        [Display(Name = "Data de Término")]
        [RegularExpression(ExpressoesRegulares.DATA, ErrorMessage = "A {0} é inválida.")]
        //[Remote("ValidarDataAvaliacaoGerenciamentoPDI", "CiclosAvaliacao",
        //        AdditionalFields = "DataInicioGerenciamentoPDI",
        //        ErrorMessage = "A {0} deve ser maior que a Data de Início.")]
        public string DataTerminoGerenciamentoPDI { get; set; }



        public Enumeradores.AcaoPagina AcaoPagina { get; set; }

        public int SituacaoCicloAvaliacaoSelecionadoID { get; set; }

        public List<SelectListItem> SituacoesCicloAvaliacao { get; set; }
    }
}