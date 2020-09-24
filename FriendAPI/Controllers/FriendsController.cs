using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    public class FriendsController : ControllerBase
    {
        private readonly ProjectContext _context;

        public FriendsController(ProjectContext context)
        {
            _context = context;
        }

        // GET: api/<FriendsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friend>>> GetFriends()
        {
            return await _context.Friends.Include(x => x.Country).Include( a => a.State).Include( a => a.Friends).ToListAsync();
        }

        // GET api/<FriendsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Friend>> GetFriend(int id)
        {
            var friend = await _context.Friends.Include( a => a.State).Include(a => a.Country).Include(a => a.Friends).FirstOrDefaultAsync( a => a.Id == id);

            if(friend == null)
            {
                return NotFound();
            }

            return friend;
        }

        // PUT api/<FriendsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFriend(int id, Friend friend)
        {
            friend.Id = id;
            if (id != friend.Id)
            {
                return BadRequest();
            }

            var friendMod = _context.Friends.Find(id);
            friendMod.Name = friend.Name;
            friendMod.Surname = friend.Surname;
            friendMod.Photo = friend.Photo;
            friendMod.Email = friend.Email;
            friendMod.Telephone = friend.Telephone;
            friendMod.Bday = friend.Bday;
            friendMod.Country = friendMod.Country;
            friendMod.State = friendMod.State;

            _context.Friends.Update(friendMod);

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException)
            {
                if (!FriendExists(id))
                {
                    return NotFound();
                } else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST api/<FriendsController>
        [HttpPost]
        public async Task<ActionResult<Friend>> PostFriend(FriendResponse friendResponse)
        {
            var countryTaker = await _context.Countries.FirstOrDefaultAsync(w => w.Id == friendResponse.Country.Id);
            var stateTaker = await _context.States.FirstOrDefaultAsync(w => w.Id == friendResponse.State.Id);
            friendResponse.Country = countryTaker;
            friendResponse.State = stateTaker;

            Friend friend = new Friend { Name = friendResponse.Name, Surname = friendResponse.Surname, Photo = friendResponse.Photo, Email = friendResponse.Email, Telephone = friendResponse.Telephone,Bday = friendResponse.Bday, Country = friendResponse.Country, State = friendResponse.State };

            _context.Friends.Add(friend);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFriend", new { id = friend.Id }, friend);

        }


        // DELETE api/<FriendsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Friend>> DeleteFriend(int id)
        {
            var friend = await _context.Friends.Include(q => q.Friends).FirstOrDefaultAsync(z => z.Id == id);
            if(friend == null)
            {
                return NotFound();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in friend.Friends)
                    {
                        _context.Brothers.Remove(item);
                    }
                    _context.Friends.Remove(friend);
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }catch
                {
                    transaction.Rollback();
                }
            }
            return NoContent();
        }

        private bool FriendExists(int id)
        {
            return _context.Friends.Any(r => r.Id == id);
        }
    }
}
