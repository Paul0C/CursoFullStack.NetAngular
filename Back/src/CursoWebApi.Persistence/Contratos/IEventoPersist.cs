using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursoWebApi.Domain;
using CursoWebApi.Persistence.Models;

namespace CursoWebApi.Persistence.Contratos
{
    public interface IEventoPersist
    {
        Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false);
        Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false);

    }
}