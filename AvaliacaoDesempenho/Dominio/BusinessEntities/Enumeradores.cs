namespace AvaliacaoDesempenho.Dominio.BusinessEntities
{
    public class Enumeradores
    {
        public enum AcaoPagina
        {
            Criar,
            Editar,
            Visualizar,
            Apagar
        }

        public enum StatusAvaliacaoColaborador
        {
            Iniciada = 1,
            DefinicaoObjetivosMetas = 2,
            AprovacaoDefinicaoObjetivosMetas = 3,
            ObjetivosMetasDefinidos = 4,
            AutoAvaliacao = 5,
            EmAvaliacaoPelosGestores = 6
        }

        public enum StatusPDI
        {
            Criada = 1,
            AprovacaoGestor = 2,
            Aprovada = 3
        }

        public enum NivelAvaliacao
        {
            Um = 1,
            Dois = 2,
            Tres = 3,
            Quatro = 4
        }

        public enum EtapasAutoAvaliacao
        {
            AutoAvaliacao = 1,
            Competencias = 2,
            Performance = 3,
            Recomendacao = 4
        }
        
        public enum CodigoPapeis
        {
            Administrador = 1,
            Colaborador = 2
        }

        public class Papeis
        {
            public const string ADMINISTRADOR = "Administrador";
            public const string COLABORADOR = "Colaborador";
        }

        //public class TipoCompetencia
        //{
        //    public const string CORPORATIVA = "Competências Corporativas";
        //    public const string LIDERANCA = "Competências de Liderança";
        //    public const string FUNCIONAIS = "Competências Funcionais";
        //}

        public enum TipoCompetencia
        {
            Funcionais = 1,
            Corporativa = 2,
            Lideranca = 3
        }
    }
}