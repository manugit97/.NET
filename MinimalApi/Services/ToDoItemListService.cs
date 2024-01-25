using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repositories;
using ToDoWebApi.Models;

namespace Services;

public class ToDoItemListService {
    private readonly IMapper _mapper;

    private readonly ToDoItemListRepository _repository;

    public ToDoItemListService(IMapper mapper, ToDoItemListRepository repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<ToDoItemListDTO>> GetAllLists() { 
        List<ToDoItemList> lists = await _repository.GetAllLists();
        var mappedLists = _mapper.Map<List<ToDoItemListDTO>>(lists);
        return mappedLists;
    }

    public async Task<ToDoItemListDTO> GetList(long id) {
        var selectedList = await _repository.GetList(id);

        if (selectedList is null) {
            return null;
        }

        var listDTO = _mapper.Map<ToDoItemListDTO>(selectedList);
        return listDTO;
    }

    public async Task<long> InsertList(ToDoItemListDTO toDoItemListDto)
    {
        var listToInsert = _mapper.Map<ToDoItemList>(toDoItemListDto);
        long insertId = await _repository.InsertList(listToInsert);

        return insertId;
    }

    public async Task<bool> UpdateList(long id, ToDoItemListDTO toDoItemListDto) {     
        var listToUpdate = await _repository.GetList(id);       //controlla se esiste
            
        if (listToUpdate is null) {  
            return false; //NotFound
        }

        await _repository.UpdateList(id, _mapper.Map<ToDoItemList>(toDoItemListDto));

        return true;
    }                                                                                       
                                                                                            
    public async Task<bool> DeleteList(long id) {                                        
        var selectedList = await _repository.GetList(id); //controlla se esiste

        if (selectedList is null) {
            return false;
        }

        await _repository.DeleteList(id);

        return true;
    }    
}

//restituisce dto o entità