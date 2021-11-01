using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeReview.Data;
using ResumeReview.Models;

namespace ResumeReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserViewedsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserViewedsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserVieweds
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewed>>> GetUserViewed()
        {
            return await _context.UserViewed.ToListAsync();
        }

        // GET: api/UserVieweds/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewed>> GetUserViewed(int id)
        {
            var userViewed = await _context.UserViewed.FindAsync(id);

            if (userViewed == null)
            {
                return NotFound();
            }

            return userViewed;
        }

        // PUT: api/UserVieweds/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserViewed(int id, UserViewed userViewed)
        {
            if (id != userViewed.Id)
            {
                return BadRequest();
            }

            _context.Entry(userViewed).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserViewedExists(id))
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

        // POST: api/UserVieweds
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserViewed>> PostUserViewed(UserViewed userViewed)
        {
            _context.UserViewed.Add(userViewed);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserViewed", new { id = userViewed.Id }, userViewed);
        }

        // DELETE: api/UserVieweds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserViewed(int id)
        {
            var userViewed = await _context.UserViewed.FindAsync(id);
            if (userViewed == null)
            {
                return NotFound();
            }

            _context.UserViewed.Remove(userViewed);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserViewedExists(int id)
        {
            return _context.UserViewed.Any(e => e.Id == id);
        }
    }
}
