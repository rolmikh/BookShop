using API_Book_Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace API_Book_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewController : ControllerBase
    {
        private readonly Book_ShopContext _context;

        public ViewController(Book_ShopContext context)
        {
            _context = context;
        }

        [HttpGet("ViewSupply")]
        public async Task<ActionResult<SupplyView>> GetView()
        {
            var result = await _context.SupplyViews.FromSqlRaw("select * from [Supply_View]").ToListAsync();

            return Ok(result[0]);


        }
    }
}
