using System;

namespace Play.Inventory.Service.Dtos
{
    public record GrantItemsDto(Guid UserId, Guid CatalogItemId, int Quatity);

    public record InventoryItemDto(Guid CatalogItemId, int Quatity, DateTimeOffset AcquiredDate);
}