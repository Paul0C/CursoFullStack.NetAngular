using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CursoWebApi.Application.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public double Preco { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public int quantidade { get; set; }
        public int EventoId { get; set; }
        public EventoDto Evento { get; set; }
    }
}