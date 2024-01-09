using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Models;

namespace Controllers;

/// <summary>
/// Controller dei ToDoItems            //non va
/// </summary>
[ApiController]
[Route("api/todoitemlists/{listId}/[controller]")]
public class ToDoItemController : ControllerBase {

    private readonly ToDoContext _context;

    public ToDoItemController(ToDoContext context) {
        _context = context;
    }

    /// <summary>
    /// Ritorna tutti gli item di una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ToDoItem>>> GetItemsByList([FromRoute]long listId) {
        var selectedList = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == listId);

        if (selectedList is null) {
            return NotFound();
        }

        return Ok(selectedList.ToDoItems);
    }

    /// <summary>
    /// Ritorna un singolo item di una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ToDoItem>> GetItemById([FromRoute] long listId, long id) {
        var selectedList = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == listId);

        if (selectedList is null) {
            return NotFound();
        }

        var selectedItem = selectedList.ToDoItems.FirstOrDefault(i => i.Id == id);
        return Ok(selectedItem);
    }

    /// <summary>
    /// Aggiunge un item ad una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="toDoItem"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ToDoItem>> InsertItem([FromRoute]long listId, ToDoItem toDoItem) {
        var selectedList = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == listId);

        if (selectedList is null) {
            return NotFound();
        }

        selectedList.ToDoItems.Add(toDoItem);
        await _context.SaveChangesAsync();
        return Ok(toDoItem);
    }

    /// <summary>
    /// Aggiorna un item di una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="id"></param>
    /// <param name="toDoItem"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ToDoItem>> UpdateItem([FromRoute] long listId,[FromRoute] long id, ToDoItem toDoItem) {
       var selectedList = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == listId);

        if (selectedList is null) {
            return NotFound();
        }

        var selectedItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        
        if (selectedItem is null) {
            return NotFound();
        }

        selectedItem.Name = toDoItem.Name;
        selectedItem.Completed = toDoItem.Completed;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>
    /// Cancella un item da una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="404">If the list or the item is not found</response>
    /// <response code="204">If the item is successfully deleted</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ToDoItem>> DeleteItem([FromRoute] long listId,[FromRoute] long id) {
        var selectedList = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == listId);

        if (selectedList is null) {
            return NotFound();
        }

        var selectedItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        
        if (selectedItem is null) {
            return NotFound();
        }

        _context.Remove(selectedItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}