using HMSProjectOfMine.Data;
using HMSProjectOfMine.DTOs;
using HMSProjectOfMine.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HMSProjectOfMine.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TestsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Test
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestDTO>>> GetTests()
        {
            var tests = await _context.Tests
                .Select(t => new TestDTO
                {
                    TestId = t.TestId,
                    TestName = t.TestName,
                    Price = t.Price
                })
                .ToListAsync();

            return Ok(tests);
        }

        // GET: api/Test/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TestDTO>> GetTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);

            if (test == null)
                return NotFound();

            var dto = new TestDTO
            {
                TestId = test.TestId,
                TestName = test.TestName,
                Price = test.Price
            };

            return Ok(dto);
        }

        // POST: api/Test
        [HttpPost]
        public async Task<ActionResult<TestDTO>> CreateTest(TestDTO dto)
        {
            var test = new Test
            {
                TestName = dto.TestName,
                Price = dto.Price
            };

            _context.Tests.Add(test);
            await _context.SaveChangesAsync();

            dto.TestId = test.TestId; // Return generated ID
            return CreatedAtAction(nameof(GetTest), new { id = test.TestId }, dto);
        }

        // PUT: api/Test/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTest(int id, TestDTO dto)
        {
            if (id != dto.TestId)
                return BadRequest("ID mismatch");

            var test = await _context.Tests.FindAsync(id);
            if (test == null)
                return NotFound();

            test.TestName = dto.TestName;
            test.Price = dto.Price;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Test/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            var test = await _context.Tests.FindAsync(id);
            if (test == null)
                return NotFound();

            _context.Tests.Remove(test);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
