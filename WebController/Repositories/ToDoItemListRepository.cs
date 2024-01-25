using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Models;

namespace Repositories;

public class ToDoItemListRepository {

    private readonly ToDoContext _context;

    public ToDoItemListRepository(ToDoContext context)
    {
        _context = context;
    }

    public async Task<List<ToDoItemList>> GetAllLists() {
        return await _context.Lists.AsNoTracking().Include(l => l.ToDoItems).ToListAsync();
    }

    public async Task<ToDoItemList> GetList(long id) {
        return await _context.Lists.AsNoTracking().Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<ToDoItemList> GetListTracking(long id) {
        return await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<long> InsertList(ToDoItemList toDoItemList){
        _context.Lists.Add(toDoItemList);
        await _context.SaveChangesAsync();
        return toDoItemList.Id;
    }

    public async Task<bool> UpdateList(long id, ToDoItemList toDoItemList) {
        var listToUpdate = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == id);
        listToUpdate.Name = toDoItemList.Name;
        await _context.SaveChangesAsync();
        return true; 
    }

    public async Task<bool> DeleteList(long id) {
        var listToDelete = await _context.Lists.Include(l => l.ToDoItems).FirstOrDefaultAsync(l => l.Id == id);
        _context.Remove(listToDelete);
        await _context.SaveChangesAsync();
        return true;             
    }
}

//deve restituire tutto task<entità>