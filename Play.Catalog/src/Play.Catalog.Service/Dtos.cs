using System;

namespace Play.Catalog.Service
{
    public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreationDate);

    public record CreateItemDto(string Name, string Description, decimal Price);

    public record UpdateItemDto(string Name, string Description, decimal Price);
}