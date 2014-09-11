using AutoMapper;
using AvaliacaoDesempenho.Dominio;
using AvaliacaoDesempenho.Models.Usuarios;
using AvaliacaoDesempenho.Util.Mapeamentos.AutoMap;

namespace AvaliacaoDesempenho.Util.Mapeamentos
{
    public class DeUsuarioParaItemListaAdministradorViewModel : IMapeamento
    {
        public void Configurar()
        {
            Mapper.CreateMap<Usuario, ItemListaAdministradorViewModel>();
        }
    }
}