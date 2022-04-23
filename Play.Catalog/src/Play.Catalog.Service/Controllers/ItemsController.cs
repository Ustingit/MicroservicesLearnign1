using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> _items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow),
        };

        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return _items;
        }

        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = _items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
	            return NotFound();
            }

            return item;
        }

        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto dto)
        {
            var item = new ItemDto(Guid.NewGuid(), dto.Name, dto.Description, dto.Price, DateTimeOffset.UtcNow);
            _items.Add(item);

            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto dto)
        {
	        var existingItem = _items.FirstOrDefault(x => x.Id == id);

	        if (existingItem == null)
	        {
		        return NotFound();
	        }

	        var updatedItem = existingItem with
	        {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price
            };

	        var index = _items.FindIndex(x => x.Id == id);
	        _items[index] = updatedItem;

	        return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
	        var index = _items.FindIndex(x => x.Id == id);

	        if (index < 0)
	        {
		        return NotFound();
	        }

            _items.RemoveAt(index);

            return NoContent();
        }
    }
}