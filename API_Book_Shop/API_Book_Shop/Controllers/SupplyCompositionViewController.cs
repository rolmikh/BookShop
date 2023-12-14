using API_Book_Shop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Book_Shop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplyCompositionViewController : ControllerBase
    {

        private readonly Book_ShopContext _context;

        public SupplyCompositionViewController(Book_ShopContext context)
        {
            _context = context;
        }

        [HttpGet("ViewSupplyComposition")]
        public async Task<ActionResult<SupplyCompositionView>> GetView()
        {
            var result = await _context.SupplyCompositionViews.FromSqlRaw("select * from [Supply_Composition_View]").ToListAsync();

            return Ok(result[0]);


        }
    }
}