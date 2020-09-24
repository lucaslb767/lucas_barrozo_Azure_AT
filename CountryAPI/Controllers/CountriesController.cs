using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Repository.Mapping.Context;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CountryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ProjectContext _context;
        public CountriesController (ProjectContext context)
        {
            _context = context;
        }

        // GET: api/<CountriesController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountries()
        {
            return await _context.Countries.Include(d => d.States).ToListAsync();
        }

        // GET api/<CountriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> GetCountry(int id)
        {
            var country = await _context.Countries.Include(z => z.States).FirstOrDefaultAsync(z => z.Id == id);
            if (country == null)
            {
                return NotFound();
            }
            return country;
        }

        // PUT api/<CountriesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCountry(int id, Country country)
        {
            country.Id = id;
            if (id != country.Id)
            {
                return BadRequest();
            }
            var countryMod = _context.Countries.Find(id);
            countryMod.Name = country.Name;
            countryMod.Flag = countryMod.Flag;

            _context.Countries.Update(countryMod);

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!CountryExist(id))
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

        private bool CountryExist(int id)
        {
            return _context.Countries.Any(z => z.Id == id);
        }

        // POST api/<CountriesController>
        [HttpPost]
        public async Task<ActionResult<Country>> PostCountry(Country country)
        {
            _context.Countries.Add(country);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCountry", new { id = country.Id }, country);
        }


        // DELETE api/<CountriesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Country>> DeleteCountry(int id)
        {
            var country = await _context.Countries.Include(z => z.States).FirstOrDefaultAsync(z => z.Id == id);
            var friends = await _context.Friends.Include(l => l.Country).Include(q => q.Friends).ToListAsync();

            if (country == null)
            {
                return NotFound();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach(var item in friends)
                    {
                        if((item.Country.Id) == country.Id)
                        {
                            foreach(var itemF in item.Friends)
                            {
                                _context.Brothers.Remove(itemF);
                            }
                            _context.Friends.Remove(item);
                        }
                    }
                    foreach (var item in country.States)
                    {
                        _context.States.Remove(item);
                    }

                    _context.Countries.Remove(country);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                } catch
                {
                    transaction.Rollback();
                }
            }
            return NoContent();
        }
    }
}
