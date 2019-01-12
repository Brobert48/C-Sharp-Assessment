using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIGapi.Models;
using System;

namespace FIGapi.Controllers
{
    [Route("Teams")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ApiContext _context;

        public TeamController(ApiContext context)
        {
            _context = context;
        }

        // GET: /Teams
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeam()
        {
            //GET: /Teams?Key=Value
            //Will only run the first query.
            if (Request.QueryString.HasValue)
            {
                char[] Char = { '?', ' ' };
                char[] splitChar = { '&' };
                var query = Request.QueryString.Value.Trim(Char);
                string[] queries = query.Split(splitChar);
                var key = queries[0].Split('=')[0];
                var value = queries[0].Split('=')[1];

                //    /Teams?orderBy=Value
                if (key == "orderBy" || key== "OrderBy")
                {
                    //OrderBy Name
                    if (value == "Name" || value == "name")
                    {
                        //Code to show Teams in Aplphabetical order by Name
                        return await _context.Teams.OrderBy(t => t.Name).Include(t => t.Players).ToListAsync();
                    }
                    // OrderBy Location
                    if (value == "Location" || value == "location")
                    {
                        //Code to show Teams in Aplphabetical order by Location
                        return await _context.Teams.OrderBy(t => t.Location).Include(t => t.Players).ToListAsync();
                    }
                }

                //Find Team by ID
                if (key =="id"|| key== "Id" || key == "ID")
                {
                    int valueInt = Int32.Parse(value);
                    return await _context.Teams.Where(p => p.Id == valueInt).Include(t => t.Players).ToListAsync();
                }

                return BadRequest("Invalid Query String");
            }
            //Full GET
            return await _context.Teams.Include(t => t.Players).ToArrayAsync();
        }

        // POST: /Teams
        [HttpPost]
        public async Task<ActionResult<Team>> PostTeam(Team team)
        {
            if (_context.Teams.Any(u => u.Name == team.Name) && _context.Teams.Any(u => u.Location == team.Location)){
                return BadRequest("A team by that name already exists in this location");
            }

            _context.Teams.Add(team);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTeam", new { id = team.Id }, team);
        }

        // PUT: /Teams/:id
        //For Updating Team info. Must Include full team object in request.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam(long id, Team team)
        {
            if (id != team.Id)
            {
                return BadRequest();
            }

            _context.Entry(team).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /Teams/:id
        [HttpDelete("{id}")]
        public async Task<ActionResult<Team>> DeleteTeam(long id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return team;
        }
    }

    [Route("Players")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ApiContext _context;

        public PlayerController(ApiContext context)
        {
            _context = context;

        }
        // GET: /Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayer()
        {
            //if Route = /Players?key=value 
            //Will only run the first query.
            if (Request.QueryString.HasValue)
            {
                char[] Char = { '?', ' ' };
                char[] splitChar = { '&' };
                var query = Request.QueryString.Value.Trim(Char);
                string[] queries = query.Split(splitChar);
                string key = queries[0].Split('=')[0];
                string value = queries[0].Split('=')[1];

                //Find Players on one Team 
                //Players?Team=value
                if (key == "Team" || key == "team")
                {
                    int valueInt = Int32.Parse(value);
                    return await _context.Players.Where(p => p.TeamId == valueInt).Include(t => t.Team).ThenInclude(p => p.Players).ToListAsync();
                }
                //Find Player by Id 
                //Players?Id=value
                if (key == "Id" || key == "id" || key == "ID")
                {
                    int valueInt = Int32.Parse(value);
                    return await _context.Players.Where(p => p.Id == valueInt).Include(t => t.Team).ThenInclude(p => p.Players).ToListAsync();
                }
                //Find Player by LastName
                //Players?LastName=value
                if (key == "LastName" || key == "lastname" || key == "Lastname" || key == "lastName")
                {
                    return await _context.Players.Where(p => p.LastName == value).Include(t => t.Team).ThenInclude(p => p.Players).ToListAsync();
                }
            }
            return await _context.Players.Include(t => t.Team).ThenInclude(p => p.Players).ToListAsync();
        }

        // POST: /Players
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            //Checks if Player Already Exists
            if (_context.Players.Any(u => u.FirstName == player.FirstName) && _context.Players.Any(u => u.LastName == player.LastName))
            {
                return BadRequest("Player Already Exists");
            }
            //Checks if team is full
           if(_context.Players.Count(p=> p.TeamId == player.TeamId) >= 25)
            {
                return BadRequest("Team is Full");
            }
            
            _context.Players.Add(player);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // PUT: Players/5
        //For Updating Player info. Must Include full Player object in request.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(long id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }
            _context.Entry(player).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: Players/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Player>> DeletePlayer(long id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            _context.Players.Remove(player);
            await _context.SaveChangesAsync();

            return player;
        }
    }

}

