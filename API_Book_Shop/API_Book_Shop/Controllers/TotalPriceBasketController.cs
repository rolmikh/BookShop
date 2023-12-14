using API_Book_Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API_Book_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TotalPriceBasketController : ControllerBase
    {

        private readonly Book_ShopContext _context;

        public TotalPriceBasketController(Book_ShopContext context)
        {
            _context = context;
        }

        [HttpGet("TotalPrice")]
        public async Task<ActionResult<decimal>> GetTotalPriceBasket(int userId)
        {
            SqlParameter userIDParametr = new("@User_ID", userId);

            var result = await _context.Products.FromSqlRaw("select [dbo].[GetTotalPriceBasket](@User_ID)", userIDParametr).Select(x => decimal.Parse(x.PriceBook.ToString())).FirstOrDefaultAsync(); // Получаем первый элемент результата

            if (result == default(decimal)) // Проверяем, что результат не равен значению по умолчанию
            {
                return NotFound(); // Если результат не найден, возвращаем NotFound
            }

            return Ok(result);


        }
    }
}
