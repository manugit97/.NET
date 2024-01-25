using System.Text.Json.Serialization;

namespace ToDoWebApi.Models;

    public class ToDoItemList {
        public long Id {get; set;}
        public string Name {get; set;}
        public List<ToDoItem> ToDoItems {get; set;}
    }