using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Models;
using System.Text.Json.Serialization;
using NuGet.Protocol;
using System.IO.Compression;

namespace Controllers;

/// <summary>
/// Controller delle ToDoLists      //non va
/// </summary>
[ApiController]
[Route("/api/[controller]")]
public class ToDoItemListController : ControllerBase{

    private readonly ToDoContext _context;                  //motivo dietro il _ come nomenclatura ? tutti readonly private

    public ToDoItemListController(ToDoContext context) {
        _context = context;
    }

    /// <summary>
    /// Ritorna tutte le liste con i propri item
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ToDoItemList>>> GetAllLists() {
        return Ok(await _context.Lists.AsNoTracking().Include(l => l.ToDoItems).ToListAsync());
    }

    /// <summary>
    /// Ritorna una lista con i propri item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItemList>> GetList(long id) {
        var selectedList = await _context.Lists.AsNoTracking().Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == id);

        if (selectedList is null) {
            return NotFound();
        }
        return Ok(selectedList);
    }

    /// <summary>
    /// Inserisce una nuova lista con eventuali item
    /// </summary>
    /// <param name="toDoItemList"></param>
    /// <returns></returns>
    [HttpPost]
        public async Task<ActionResult<ToDoItemList>> InsertList(ToDoItemList toDoItemList)
        {
            _context.Lists.Add(toDoItemList);
            await _context.SaveChangesAsync();

            return Ok(toDoItemList);
        }

    /// <summary>
    /// Aggiorna una lista
    /// </summary>
    /// <param name="id"></param>
    /// <param name="toDoItemList"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
        public async Task<ActionResult<ToDoItemList>> UpdateList(long id, ToDoItemList toDoItemList) {
            if (id != toDoItemList.Id) {
                return BadRequest();
            }
            _context.Update(toDoItemList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    /// <summary>
    /// Cancella una lista mantenendo gli item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ToDoItemList>> DeleteList(long id) {
        var selectedList = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == id); //mantiene item elimina lista

        if (selectedList is null) {
            return NotFound();
        }

        _context.Lists.Remove(selectedList);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}