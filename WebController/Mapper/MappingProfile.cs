using AutoMapper;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using ToDoWebApi.Models;

namespace ToDoWebApi.Mapper;
public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<ToDoItemList, ToDoItemListDTO>();
        CreateMap<ToDoItemListDTO, ToDoWebApi.Models.ToDoItemList>();
        CreateMap<ToDoItem, ToDoItemDTO>();
        CreateMap<ToDoItemDTO, ToDoWebApi.Models.ToDoItem>();
    }
}