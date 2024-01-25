using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using ToDoWebApi.Models;

public class ToDoContext : DbContext {

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

    }

    public ToDoContext(DbContextOptions<ToDoContext> options) : base(options) {}    //costruttore che serve per lo using in Program.cs
    
    public DbSet<ToDoItemList> Lists {get; set;}
    public DbSet<ToDoItem> Items {get; set;}
}
