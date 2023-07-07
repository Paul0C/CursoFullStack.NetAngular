using CursoWebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using CursoWebApi.Application.Contratos;
using CursoWebApi.Application.Dtos;
using CursoWebApi.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using CursoWebApi.Persistence.Models;

namespace CursoWebApi.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class EventoController : ControllerBase
{     
    private readonly IEventoService _eventoService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IUserService _userService;

    public EventoController(IEventoService eventoService,
                            IWebHostEnvironment webHostEnvironment,
                            IUserService userService){
        _userService = userService;
        _eventoService = eventoService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery]PageParams pageParams){
        try
        {
            var eventos = await _eventoService.GetAllEventosAsync(User.GetUserId(), pageParams, true);
            if(eventos == null) return NoContent();

            Response.AddPagination(eventos.CurrentPage, eventos.PageSize, eventos.TotalCount, eventos.CurrentPage);

            return Ok(eventos);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id){
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if(evento == null) return NoContent();

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar recuperar eventos. Erro: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post(EventoDto model){
        try
        {
            var evento = await _eventoService.AddEventos(User.GetUserId(), model);
            if(evento == null) return NoContent();

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar adicionar eventos. Erro: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, EventoDto model){
        try
        {
            var evento = await _eventoService.UpdateEvento(User.GetUserId(), id, model);
            if(evento == null) return NoContent();

            return Ok(evento);
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar atualizar eventos. Erro: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id){
        try
        {
            var evento = await _eventoService.GetEventoByIdAsync(User.GetUserId(), id, true);
            if(evento == null) return NoContent();

            return await _eventoService.DeleteEvento(User.GetUserId(), id) ? 
            Ok("Deletado.") :
            throw new Exception("Ocorreu um problema não específico ao tentar deletar Evento.");
        }
        catch (Exception ex)
        {
            return this.StatusCode(StatusCodes.Status500InternalServerError, 
                $"Erro ao tentar deletar eventos. Erro: {ex.Message}");
        }
    }
}