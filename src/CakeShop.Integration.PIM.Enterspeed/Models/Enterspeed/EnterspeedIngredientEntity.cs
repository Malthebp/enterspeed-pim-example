using System;
using System.Collections.Generic;
using Enterspeed.Source.Sdk.Api.Models;
using Enterspeed.Source.Sdk.Api.Models.Properties;

namespace CakeShop.Integration.PIM.Enterspeed.Models.Enterspeed;

public class EnterspeedIngredientEntity : IEnterspeedEntity
{
    public EnterspeedIngredientEntity(
        string id,
        string type,
        IDictionary<string, IEnterspeedProperty> properties)
    {
        Id = id;
        Type = type;
        Properties = properties;
    }

    public string Id { get; }

    // Used to trigger schemas
    public string Type { get; }

    // If you have the need for a URL, you can add it here, otherwise you can create a schema with a handle with the ID.
    // See docs here: https://docs.enterspeed.com/reference/fields#routing-by-handles
    public string Url { get; } = string.Empty;
    public string[] Redirects { get; } = Array.Empty<string>();

    // If you data has a hierarchy, you can set the immediate parent ID.
    public string ParentId { get; } = string.Empty;

    // All properties from your system that needs to go into Enterspeed.
    public IDictionary<string, IEnterspeedProperty> Properties { get; }
}