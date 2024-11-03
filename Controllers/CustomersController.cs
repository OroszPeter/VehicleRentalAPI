using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleRentalAPI.Context;
using VehicleRentalAPI.Entities;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(VehicleRentalContext context) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await context.Customers.ToListAsync();
            return Ok(customers);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await context.Customers.FindAsync(id);

            if (customer == null)
            {
                return NotFound("A bérlő nem található.");
            }

            return Ok(customer);
        }


        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCustomer), new { id = customer.Id }, customer);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, Customer updatedCustomer)
        {
            var customerToUpdate = await context.Customers.FirstOrDefaultAsync(c => c.Id == id);

            if (customerToUpdate is null)
            {
                return NotFound("A bérlő nem található.");
            }

            customerToUpdate.Name = updatedCustomer.Name;
            customerToUpdate.Email = updatedCustomer.Email;
            customerToUpdate.PhoneNumber = updatedCustomer.PhoneNumber;
            await context.SaveChangesAsync();

            return Ok("Sikeres módosítás!");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound("A bérlő nem található.");
            }

            context.Customers.Remove(customer);
            await context.SaveChangesAsync();

            return Ok("Sikeres törlés.");
        }
    }
}
