using AvaliacaoDesempenho.Dominio.BusinessEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AvaliacaoDesempenho.Models.Relatorios
{
    public class EnvioDeEmailRatingFinal
    {
        public int? CicloAvaliacaoID { get; set; }

        public EmailInformativo Email { get; set; }

        public bool VerEmail { get; set; }

        [Display(Name = "Área")]
        public string AreaPesquisada { get; set; }
        [Display(Name = "Diretoria")]
        public string DiretoriaPesquisada { get; set; }
        [Display(Name = "Gestor")]
        public string GestorPesquisado { get; set; }
        public EnvioDeEmailRatingFinal()
        {
            Email = new EmailInformativo();
        }
    }
}