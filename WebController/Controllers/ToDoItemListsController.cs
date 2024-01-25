using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Models;
using System.Text.Json.Serialization;
using NuGet.Protocol;
using System.IO.Compression;
using Services;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Controllers;

/// <summary>
/// Controller delle ToDoLists      //non va
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class ToDoItemListController : ControllerBase{                

    private readonly ToDoItemListService _service;

    public ToDoItemListController(ToDoItemListService service) {
        _service = service;
    }

    /// <summary>
    /// Ritorna tutte le liste con i propri item
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ToDoItemListDTO>>> GetAllLists() {
        try{
            return Ok(await _service.GetAllLists());
        }catch(NullException ex) {
            return NotFound();
            throw;
        }
        
    }

    /// <summary>
    /// Ritorna una lista con i propri item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItemListDTO>> GetList(long id) {
        try{
        return Ok(await _service.GetList(id));
        }catch(NotFoundException){
            return NotFound();
            throw;
        }
    }

    /// <summary>
    /// Inserisce una nuova lista con eventuali item
    /// </summary>
    /// <param name="toDoItemList"></param>
    /// <returns></returns>
    [HttpPost]
        public async Task<ActionResult<long>> InsertList(ToDoItemListDTO toDoItemListDto)
        {
            return Ok(await _service.InsertList(toDoItemListDto));
        }

    /// <summary>
    /// Aggiorna una lista
    /// </summary>
    /// <param name="id"></param>
    /// <param name="toDoItemList"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
        public async Task<ActionResult<bool>> UpdateList(long id, ToDoItemListDTO toDoItemListDto) {
            return await _service.UpdateList(id, toDoItemListDto);
        }

    /// <summary>
    /// Cancella una lista mantenendo gli item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteList(long id) {
        return await _service.DeleteList(id);
    }
}