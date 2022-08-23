using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CakeShop.Integration.PIM.Enterspeed.Models.Pim;

namespace CakeShop.Integration.PIM.Enterspeed.Services.MyPim;

public class MyPimService : IPimService
{
    private readonly List<PimIngredientModel> _ingredients = new List<PimIngredientModel>()
    {
        new PimIngredientModel()
        {
            Id = "1234",
            Name = "Sugar",
            Description = "Making my cakes sweet."
        },
        new PimIngredientModel()
        {
            Id = "5678",
            Name = "Flour",
            Description = "Making my cakes stick."
        },
        new PimIngredientModel()
        {
            Id = "9123",
            Name = "Cacao",
            Description = "Making my cakes taste good."
        }
    };

    public Task<PimIngredientModel?> FetchIngredientById(string ingredientId)
    {
        return Task.FromResult(_ingredients.FirstOrDefault(x => x.Id == ingredientId));
    }
}