using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CursoWebApi.Domain;
using CursoWebApi.Persistence.Contextos;
using CursoWebApi.Persistence.Contratos;
using Microsoft.EntityFrameworkCore;

namespace CursoWebApi.Persistence
{
    public class LotePersist : ILotePersist
    {
        private readonly DataContext _context;

        public LotePersist(DataContext context)
        {
            _context = context;
            
        }

        public async Task<Lote> GetLoteByIdsAsync(int eventoId, int id)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                         .Where(lote => lote.EventoId == eventoId
                                     && lote.Id == id);

            return await query.FirstOrDefaultAsync(); 
        }

        public async Task<Lote[]> GetLotesByEventoIdAsync(int eventoId)
        {
            IQueryable<Lote> query = _context.Lotes;

            query = query.AsNoTracking()
                         .Where(lote => lote.EventoId == eventoId);

            return await query.ToArrayAsync();
        }
    }
}