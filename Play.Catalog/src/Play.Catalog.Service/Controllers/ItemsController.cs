using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
	    private readonly IItemsRepository repository;

	    public ItemsController(IItemsRepository repo)
	    {
		    repository = repo;
	    }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await repository.GetAllAsync()).Select(x => x.AsDto());

            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
            var item = await repository.GetAsync(id);

            if (item == null)
            {
	            return NotFound();
            }

            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> Post(CreateItemDto dto)
        {
            var item = new Item
            {
	            Id = Guid.NewGuid(), 
	            Name = dto.Name,
	            Description = dto.Description, 
	            Price = dto.Price,
	            CreatedDate = DateTimeOffset.UtcNow
            };
            await repository.CreateAsync(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, UpdateItemDto dto)
        {
	        var existingItem = await repository.GetAsync(id);

	        if (existingItem == null)
	        {
		        return NotFound();
	        }

	        existingItem.Name = dto.Name;
	        existingItem.Description = dto.Description;
	        existingItem.Price = dto.Price;

	        await repository.UpdateAsync(existingItem);

	        return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
	        var item = await repository.GetAsync(id);

	        if (item == null)
	        {
		        return NotFound();
	        }

	        await repository.DeleteAsync(item.Id);

            return NoContent();
        }
    }
}