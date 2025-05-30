﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Travel.API.Data;
using Travel.API.Models;

namespace Travel.API.Controllers
{
    // secure authorization, use JWT
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class LogController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LogController(ApplicationDbContext context)
        {
            _context = context;
        }

        /*[HttpGet]
        public async Task<ActionResult<IEnumerable<Log>>> GetLogs()
        {
            return await _context.Logs.Include(l => l.User).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Log>> GetLog(int id)
        {
            var log = await _context.Logs.Include(l => l.User).FirstOrDefaultAsync(l => l.Id == id);

            if (log == null)
                return NotFound();

            return log;
        }

        [HttpPost]
        public async Task<ActionResult<Log>> CreateLog(Log log)
        {
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLog), new { id = log.Id }, log);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLog(int id, Log updatedLog)
        {
            if (id != updatedLog.Id)
                return BadRequest();

            _context.Entry(updatedLog).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLog(int id)
        {
            var log = await _context.Logs.FindAsync(id);

            if (log == null)
                return NotFound();

            _context.Logs.Remove(log);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/


        // Returns the last N logs (ordered by Timestamp DESC)
        [HttpGet("get/{n}")]
        public async Task<ActionResult<IEnumerable<Log>>> GetLastNLogs(int n)
        {
            var logs = await _context.Logs
                .Include(l => l.User)
                .OrderByDescending(l => l.Timestamp)
                .Take(n)
                .ToListAsync();

            return logs;
        }

        // Returns total number of logs
        [HttpGet("count")]
        public async Task<ActionResult<int>> GetLogCount()
        {
            return await _context.Logs.CountAsync();
        }

    }
}