using System.Threading.Tasks;
using CakeShop.Integration.PIM.Enterspeed.Models.Pim;

namespace CakeShop.Integration.PIM.Enterspeed.Services;

public interface IPimService
{
    Task<PimIngredientModel?> FetchIngredientById(string ingredientId);
}