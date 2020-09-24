using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Mapping.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FriendAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrothersController : ControllerBase
    {
        private readonly ProjectContext _context;

        public BrothersController( ProjectContext context)
        {
            _context = context;
        }

        // GET: api/<BrothersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Brother>>> GetBrothers()
        {
            return await _context.Brothers.Include(z => z.Friend).ToListAsync();
        }

        // GET api/<BrothersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Brother>> GetBrother(int id)
        {
            var brother = await _context.Brothers.Include(z => z.Friend).FirstOrDefaultAsync(z => z.Id == id);

            if(brother == null)
            {
                return NotFound();
            }

            return brother;
        }

        // PUT api/<BrothersController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBrother(int id, Brother brother)
        {
            brother.Id = id;
            if (id != brother.Id)
            {
                return BadRequest();
            }

            var brotherMod = _context.Brothers.Find(id);
            brotherMod.Name = brother.Name;
            brotherMod.Surname = brother.Surname;
            brotherMod.Email = brother.Email;
            brotherMod.Telephone = brother.Telephone;

            _context.Brothers.Update(brotherMod);

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!BrotherExist(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool BrotherExist(int id)
        {
            return _context.Brothers.Any(z => z.Id == id);
        }

        // POST api/<BrothersController>
        [HttpPost]
        public async Task<ActionResult<Brother>> PostBrother(BrotherResponse brotherResponse)
        {
            var friendTake = await _context.Friends.FirstOrDefaultAsync(z => z.Id == brotherResponse.Friend.Id);
            brotherResponse.Friend = friendTake;
            Brother brother = new Brother { Name = brotherResponse.Name, Surname = brotherResponse.Surname, Email = brotherResponse.Email, Telephone = brotherResponse.Telephone, Friend = brotherResponse.Friend };
            _context.Brothers.Add(brother);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBrother", new {id=brother.Id }, brother);
        }


        // DELETE api/<BrothersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Brother>> DeleteBrother(int id)
        {
            var brother = await _context.Brothers.FindAsync(id);
            if(brother == null)
            {
                return NotFound();
            }

            _context.Brothers.Remove(brother);
            await _context.SaveChangesAsync();

            return brother;
        }
    }
}
