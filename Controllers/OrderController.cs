using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PIzzaDeliveryAPI.Data;
using PIzzaDeliveryAPI.Models;
using PIzzaDeliveryAPI.DTOs;
namespace PizzaDeliveryAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrderController(AppDbContext context)
        {
            _context = context;
        }

        // 📌 Get all orders
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Pizza)
                .ToListAsync();
            var response =orders.Select(o => new OrderDto
            {
                Id=o.Id,
                UserName=o.User.Name,
                PizzaName=o.Pizza.Name,
                Quantity=o.Quantity,
                Status=o.Status,
                OrderDate=o.OrderdDate
            });

            return Ok(response);
        }

        // 📌 Get one order
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Pizza)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) 
                return NotFound();
            var response = new OrderDto
            {
                Id = order.Id,
                UserName = order.User.Name,
                PizzaName = order.Pizza.Name,
                Quantity = order.Quantity,
                Status = order.Status,
                OrderDate = order.OrderdDate
            };

            return Ok(response);
        }

        // 📌 Create new order
        [HttpPost]
        public async Task<ActionResult<OrderDto>>CreateOrder(CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                PizzaId = dto.PizzaId,
                Quantity = dto.Quantity,
                Status = dto.Status
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            var user = await _context.Users.FindAsync(dto.UserId);
            var pizza = await _context.Pizzas.FindAsync(dto.PizzaId);
            var response = new OrderDto
            {
                Id = order.Id,
                UserName = user?.Name,
                PizzaName = pizza?.Name,
                Quantity = order.Quantity,
                Status = order.Status,
                OrderDate = order.OrderdDate
            };

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, response);
        }
        //delete:api/order/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
    


     