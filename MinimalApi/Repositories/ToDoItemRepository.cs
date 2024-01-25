using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Models;

namespace Repositories;

public class ToDoItemRepository {

    private readonly ToDoContext _context;

    public ToDoItemRepository(ToDoContext context) {
        _context = context;
    }

    public async Task<List<ToDoItem>> GetAllItems(long listId) {
        ToDoItemList selectedList = await _context.Lists.Include(l => l.ToDoItems).AsNoTracking().FirstOrDefaultAsync(l => l.Id == listId);
        return selectedList.ToDoItems;
    }

    public async Task<ToDoItem> GetItem(long itemId, long listId) {
        ToDoItemList selectedList = await _context.Lists.Include(l => l.ToDoItems).AsNoTracking().FirstOrDefaultAsync(l => l.Id == listId);
        return selectedList.ToDoItems.FirstOrDefault(i => i.Id == itemId);
    }

    public async Task<ToDoItem> InsertItem(long listId, ToDoItem item) {
        ToDoItemList selectedList = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == listId);
        selectedList.ToDoItems.Add(item);
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> UpdateItem(long itemId, ToDoItem item) {
        ToDoItem selectedItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
        selectedItem.Name = item.Name;
        selectedItem.Completed = item.Completed;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteItem(long itemId) {
        ToDoItem selectedItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId);
        _context.Remove(selectedItem);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CompleteItem(long itemId) {
        ToDoItem selectedItem = await _context.Items.FirstOrDefaultAsync(i =>i.Id == itemId);
        selectedItem.Completed = true;
        return selectedItem.Completed;
    }
}

//chiedere se posso accedere solo dall'itemId o necessito di passare anche per la lista