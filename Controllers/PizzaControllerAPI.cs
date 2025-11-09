using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIzzaDeliveryAPI.Models;
using PIzzaDeliveryAPI.Data;
using PIzzaDeliveryAPI.DTOs;

namespace PIzzaDeliveryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PizzaController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/pizza
        [HttpGet("get_pizza")]
        public async Task<IActionResult> GetPizzas()
        {
            var pizzas = await _context.Pizzas.ToListAsync();
            return Ok(pizzas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPizza(int id)
        {
            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza == null)
                return NotFound("Invalid ID");
            return Ok(pizza);

        }
        [HttpPost("create_pizza")]
        public async Task<ActionResult<PizzaDto>>CreatePizza(CreatePizzaDto pizzaDto)
        {
            var pizza = new Pizza
            {
                Name = pizzaDto.Name,
                Price = pizzaDto.Price
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Pizzas.Add(pizza);
            await _context.SaveChangesAsync();
            
            var response = new PizzaDto
            {
                Id = pizza.Id,
                Name = pizza.Name,
                Price = pizza.Price
            };
            return CreatedAtAction(nameof(GetPizza), new { id = pizza.Id }, response);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdatePizza(int id,UpdatePizzaDto updateDto)
        {
            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza == null)
                return NotFound();
            pizza.Name = updateDto.Name;
            pizza.Price = updateDto.Price;
            await _context.SaveChangesAsync();
            return Ok("successfully updated");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizza(int id)
        {
            var pizza = await _context.Pizzas.FindAsync(id);
            if (pizza == null)
                return NotFound();
            _context.Pizzas.Remove(pizza);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
