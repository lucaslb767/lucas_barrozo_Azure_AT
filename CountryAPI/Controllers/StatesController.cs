using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Mapping.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly ProjectContext _context;

        public StatesController(ProjectContext context)
        {
            _context = context;
        }


        // GET: api/<StatesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return await _context.States.Include(al => al.Country).ToListAsync();
        }

        // GET api/<StatesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetStates(int id)
        {
            var state = await _context.States.Include(xp => xp.Country).FirstOrDefaultAsync(xp => xp.Id == id);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        // PUT api/<StatesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(int id, State state)
        {
            state.Id = id;
            if (id != state.Id)
            {
                return BadRequest();
            }

            var stateMod = _context.States.Find(id);
            stateMod.Name = state.Name;
            stateMod.Flag = stateMod.Flag;

            _context.States.Update(stateMod);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StateExists(id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }
            return NoContent();
        }

        private bool StateExists(int id)
        {
            return _context.States.Any(z => z.Id == id);
        }

        // POST api/<StatesController>
        [HttpPost]
        public async Task<ActionResult<State>> PostState(StateResponse stateResponse)
        {
            var countryTaker = await _context.Countries.FirstOrDefaultAsync(aspdao => aspdao.Id == stateResponse.Country.Id);
            stateResponse.Country = countryTaker;
            State state = new State { Name = stateResponse.Name, Flag = stateResponse.Flag, Country = stateResponse.Country };
            _context.States.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStates", new { id = state.Id }, state);
        }


        // DELETE api/<StatesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<State>> DeleteState(int id)
        {
            var state = await _context.States.FindAsync(id);
            var friends = await _context.Friends.Include(z => z.State).Include(x => x.Friends).ToListAsync();

            if(state == null)
            {
                return NotFound();
            }
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach( var item in friends)
                    {
                        if ((item.State) == state)
                        {
                            foreach(var itemB in item.Friends)
                            {
                                _context.Brothers.Remove(itemB);
                            }
                            _context.Friends.Remove(item);
                        }
                    }
                    _context.States.Remove(state);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                }
            }
            return state;
        }
    }
}
