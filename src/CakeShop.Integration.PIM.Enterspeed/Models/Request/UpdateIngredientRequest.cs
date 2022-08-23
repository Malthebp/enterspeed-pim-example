namespace CakeShop.Integration.PIM.Enterspeed.Models.Request;

public class UpdateIngredientRequest
{
    public UpdateIngredientRequest(string ingredientId)
    {
        IngredientId = ingredientId;
    }

    public string IngredientId { get; set; }
}