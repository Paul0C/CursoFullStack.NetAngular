using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursoWebApi.Domain;
using CursoWebApi.Persistence.Contextos;
using CursoWebApi.Persistence.Contratos;
using CursoWebApi.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace CursoWebApi.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly DataContext _context;

        public EventoPersist(DataContext context)
        {
            _context = context;
            
        }
         
        public async Task<Evento> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(e => e.Id)
                        .Where(e => e.Id == eventoId && e.UserId == userId);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<PageList<Evento>> GetAllEventosAsync(int userId, PageParams pageParams, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(e => e.Lotes)
                .Include(e => e.RedesSociais);

            if(includePalestrantes){
                query = query
                    .Include(e => e.PalestrantesEventos)
                    .ThenInclude(pe => pe.Palestrante);
            }

            query = query.AsNoTracking()
                         .Where(e => e.Tema.ToLower().Contains(pageParams.Term.ToLower()) &&
                                     e.UserId == userId)
                         .OrderBy(e => e.Id);

            return await PageList<Evento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }
    }
}