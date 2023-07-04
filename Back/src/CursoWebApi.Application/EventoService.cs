using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CursoWebApi.Application.Contratos;
using CursoWebApi.Application.Dtos;
using CursoWebApi.Domain;
using CursoWebApi.Persistence.Contratos;

namespace CursoWebApi.Application
{
    public class EventoService : IEventoService
    {
        private readonly IEventoPersist _eventoPersist;
        private readonly IGeralPersist _geralPersist;
        private readonly IMapper _mapper;

        public EventoService(IEventoPersist eventoPersist, 
                              IGeralPersist geralPersist,
                              IMapper mapper)
        {
            _geralPersist = geralPersist;
            _mapper = mapper;
            _eventoPersist = eventoPersist;
            
        }

        public async Task<EventoDto> AddEventos(int userId, EventoDto model)
        {
            try
            {
                var evento = _mapper.Map<Evento>(model);
                evento.UserId = userId;

                _geralPersist.Add<Evento>(evento);

                if(await _geralPersist.SaveChangesAsync()){
                    var eventoRetorno = _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);
                    
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> UpdateEvento(int userId, int eventoId, EventoDto model)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
                if(evento == null) return null;

                model.Id = evento.Id;
                model.UserId = userId;

                _mapper.Map(model, evento);

                _geralPersist.Update<Evento>(evento);
                
                if(await _geralPersist.SaveChangesAsync()){
                    var eventoRetorno = _eventoPersist.GetEventoByIdAsync(userId, evento.Id, false);
                    
                    return _mapper.Map<EventoDto>(eventoRetorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteEvento(int userId, int eventoId)
        {
             try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, false);
                if(evento == null) throw new Exception("Evento para delete não foi encontrado");

                _geralPersist.Delete<Evento>(evento);
                return await _geralPersist.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetAllEventosAsync(int userId, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetAllEventosAsync(userId, includePalestrantes);
                if(eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto> GetEventoByIdAsync(int userId, int eventoId, bool includePalestrantes = false)
        {
            try
            {
                var evento = await _eventoPersist.GetEventoByIdAsync(userId, eventoId, includePalestrantes);
                if(evento == null) return null;

                var resultado = _mapper.Map<EventoDto>(evento);

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }

        public async Task<EventoDto[]> GetEventosByTemaAsync(int userId, string tema, bool includePalestrantes = false)
        {
            try
            {
                var eventos = await _eventoPersist.GetEventosByTemaAsync(userId, tema, includePalestrantes);
                if(eventos == null) return null;

                var resultado = _mapper.Map<EventoDto[]>(eventos);

                return resultado;
            }
            catch (Exception ex)
            {
                
                throw new Exception(ex.Message);
            }
        }
    }
}