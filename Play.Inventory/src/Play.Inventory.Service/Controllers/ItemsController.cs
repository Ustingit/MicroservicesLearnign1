using Microsoft.AspNetCore.Mvc;
using Play.Inventory.Service.Entities;
using Play.Common;
using System.Threading.Tasks;
using System.Collections.Generic;
using Play.Inventory.Service.Dtos;
using System;
using System.Linq;
using Play.Inventory.Service.Clients;

namespace Play.Inventory.Service.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> repository;
        private readonly CatalogClient catalogClient;

        public ItemsController(IRepository<InventoryItem> repo, CatalogClient catalogClient)
        {
            repository = repo;
            this.catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var catalogItems = await catalogClient.GetCatalogItemsAsync();
            var inventoryItemEntities = await repository.GetAllAsync(i => i.UserId == userId);

            var inventoryItemDtos = inventoryItemEntities.Select(ii =>
            {
                var catalogItem = catalogItems.Single(ci => ci.Id == ii.CatalogItemId);
                return ii.AsDto(catalogItem.Name, catalogItem.Description);
            });

            return Ok(inventoryItemDtos);
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemsDto)
        {
            var inventoryItem = await repository.GetAsync(item => item.UserId == grantItemsDto.UserId && item.CatalogItemId == grantItemsDto.CatalogItemId);

            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem()
                {
                    CatalogItemId = grantItemsDto.CatalogItemId,
                    UserId = grantItemsDto.UserId,
                    Quantity = grantItemsDto.Quatity,
                    AcquiredDate = DateTimeOffset.UtcNow
                };

                await repository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemsDto.Quatity;

                await repository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}