using AutoMapper;
using Repositories;
using ToDoWebApi.Mapper;
using ToDoWebApi.Models;

namespace Services;

public class ToDoItemService {

    private readonly ToDoItemListRepository _listRepository;
    private readonly ToDoItemRepository _itemRepository;

    private readonly IMapper _mapper;

    public ToDoItemService(IMapper mapper, ToDoItemRepository itemRepository, ToDoItemListRepository listRepository) {
        _mapper = mapper;
        _listRepository = listRepository;
        _itemRepository = itemRepository;
    }

    public async Task<List<ToDoItemDTO>> GetAllItems(long listId) {
        ToDoItemList selectedList = await _listRepository.GetList(listId);
        if(selectedList is null) {
            return null;
        }
        List<ToDoItem> itemList =await _itemRepository.GetAllItems(listId);
        return _mapper.Map<List<ToDoItemDTO>>(itemList);
    }

    public async Task<ToDoItemDTO> GetItem(long listId, long itemId) {
        ToDoItemList selectedList = await _listRepository.GetList(listId);
        if(selectedList is null) {
            return null;
        }
        return _mapper.Map<ToDoItemDTO>(await _itemRepository.GetItem(itemId, listId));        
    }

    public async Task<ToDoItemDTO> InsertItem(long listId, ToDoItemDTO toDoItemDTO){
        ToDoItemList selectedList = await _listRepository.GetList(listId);
        if(selectedList is null) {
            return null;
        }
        ToDoItem item = _mapper.Map<ToDoItem>(toDoItemDTO);
        _itemRepository.InsertItem(listId, item);
        return _mapper.Map<ToDoItemDTO>(item);
    }

    public async Task<bool> UpdateItem(long listId, long itemId, ToDoItemDTO toDoItemDTO) {
        /*ToDoItemList selectedList = await _listRepository.GetList(listId);
        if(selectedList is null) {
            return false;
        }*/
        ToDoItem item = _mapper.Map<ToDoItem>(toDoItemDTO);
        return await _itemRepository.UpdateItem(itemId, item);        
    }

    public async Task<bool> DeleteItem(long listId, long itemId) {
        ToDoItemList selectedList = await _listRepository.GetList(listId);
        if(selectedList is null) {
            return false;
        }
        return await _itemRepository.DeleteItem(itemId);        
    }

    public async Task<bool> CompleteItem(long listId, long itemId){
        ToDoItemList selectedList = await _listRepository.GetList(listId);
        if(selectedList is null) {
            return false;
        }
        return await _itemRepository.CompleteItem(itemId);
    }
}