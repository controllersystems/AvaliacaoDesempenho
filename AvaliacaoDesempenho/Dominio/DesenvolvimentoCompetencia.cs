//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AvaliacaoDesempenho.Dominio
{
    using System;
    using System.Collections.Generic;
    
    public partial class DesenvolvimentoCompetencia
    {
        public int ID { get; set; }
        public string AcaoDesenvolvimento { get; set; }
        public string RescursoSuporte { get; set; }
        public int AvaliacaoPDIColaborador_ID { get; set; }
    
        public virtual AvaliacaoPDIColaborador AvaliacaoPDIColaborador { get; set; }
    }
}