using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services;
using ToDoWebApi.Models;

namespace Controllers;

/// <summary>
/// Controller dei ToDoItems            //non va
/// </summary>
[ApiController]
[Route("api/todoitemlists/{listId}/[controller]")]
public class ToDoItemController : ControllerBase {

    private readonly ToDoItemService _service;

    public ToDoItemController(ToDoItemService service) {
        _service = service;
    }

    /// <summary>
    /// Ritorna tutti gli item di una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<List<ToDoItem>>> GetItemsByList([FromRoute]long listId) {
        return Ok(await _service.GetAllItems(listId));
    }

    /// <summary>
    /// Ritorna un singolo item di una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="itemId"></param>
    /// <returns></returns>
    [HttpGet("{itemId}")]
    public async Task<ActionResult<ToDoItem>> GetItemById([FromRoute] long listId, [FromRoute] long itemId) {
        return Ok(await _service.GetItem(listId, itemId));
    }

    /// <summary>
    /// Aggiunge un item ad una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="toDoItem"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ToDoItem>> InsertItem([FromRoute]long listId, ToDoItemDTO toDoItemDto) {
        return Ok(await _service.InsertItem(listId, toDoItemDto));
    }

    /// <summary>
    /// Aggiorna un item di una lista
    /// </summary>
    /// <param name="listId"></param>
    /// <param name="id"></param>
    /// <param name="toDoItem"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ToDoItem>> UpdateItem([FromRoute] long listId,[FromRoute] long id, ToDoItemDTO toDoItemDto) {
        _service.UpdateItem(listId, id, toDoItemDto);
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
        _service.DeleteItem(listId, id);
        return NoContent();
    }
}