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
    public class TestBillingsController : ControllerBase
    {

        private readonly AppDbContext _context;

        public TestBillingsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TestBilling>>> GetAll()
        {
            return await _context.TestBillings
                .Include(tb => tb.TestBillingDetails)
                .ToListAsync();
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<TestBilling>> GetById(int id)
        {
            var billing = await _context.TestBillings
                .Include(tb => tb.TestBillingDetails)
                .FirstOrDefaultAsync(tb => tb.TestBillingId == id);

            if (billing == null) return NotFound();
            return billing;
        }



        [HttpPost]
        public async Task<ActionResult> Create(TestBillingDTO dto)

        {
            if (!ModelState.IsValid)
            {
                // Log the errors in your IDE console
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"{error.Key}: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }

                return BadRequest(ModelState);
            }

            var totalAmount = dto.TestBillingDetails.Sum(d => d.Price * d.Quantity);
            var discountAmount = totalAmount * (dto.DiscountPercentage / 100);
            var payableAmount = totalAmount - discountAmount;
            var dueAmount = payableAmount - dto.PaidAmount;

            var billing = new TestBilling
            {
                BillNo = dto.BillNo,
                PatientId = dto.PatientId,
                RefBy = dto.RefBy,
                TotalAmount = totalAmount,
                DiscountAmount = discountAmount,
                DiscountPercentage = dto.DiscountPercentage,
                PaidAmount = dto.PaidAmount,
                PayableAmount = payableAmount,
                DueAmount = dueAmount,
                DeliveryDate = dto.DeliveryDate,
                DeliveryTime = TimeOnly.Parse(dto.DeliveryTime),


                TestBillingDetails = dto.TestBillingDetails.Select(d => new TestBillingDetail
                {
                    TestId = d.TestId,
                    Price = d.Price,
                    Quantity = d.Quantity,
                    TotalPrice = d.Price * d.Quantity
                }).ToList()
            };

            _context.TestBillings.Add(billing);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = billing.TestBillingId }, billing);


        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, TestBillingDTO dto)
        {
            if (id != dto.TestBillingId) return BadRequest();

            var billing = await _context.TestBillings
                .Include(tb => tb.TestBillingDetails)
                .FirstOrDefaultAsync(tb => tb.TestBillingId == id);

            if (billing == null) return NotFound();

            var totalAmount = dto.TestBillingDetails.Sum(d => d.Price * d.Quantity);
            var discountAmount = totalAmount * (dto.DiscountPercentage / 100);
            var payableAmount = totalAmount - discountAmount;
            var dueAmount = payableAmount - dto.PaidAmount;

            billing.BillNo = dto.BillNo;
            billing.PatientId = dto.PatientId;
            billing.RefBy = dto.RefBy;
            billing.TotalAmount = totalAmount;
            billing.DiscountAmount = discountAmount;
            billing.DiscountPercentage = dto.DiscountPercentage;
            billing.PaidAmount = dto.PaidAmount;
            billing.PayableAmount = payableAmount;
            billing.DueAmount = dueAmount;
            billing.DeliveryDate = dto.DeliveryDate;
            billing.DeliveryTime = TimeOnly.Parse(dto.DeliveryTime);
            ;

            // Remove old details
            _context.TestBillingDetails.RemoveRange(billing.TestBillingDetails);

            // Add new details with TotalPrice
            billing.TestBillingDetails = dto.TestBillingDetails.Select(d => new TestBillingDetail
            {
                TestId = d.TestId,
                Price = d.Price,
                Quantity = d.Quantity,
                TotalPrice = d.Price * d.Quantity
            }).ToList();

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var billing = await _context.TestBillings
                .Include(tb => tb.TestBillingDetails)
                .FirstOrDefaultAsync(tb => tb.TestBillingId == id);

            if (billing == null) return NotFound();

            _context.TestBillingDetails.RemoveRange(billing.TestBillingDetails);
            _context.TestBillings.Remove(billing);

            await _context.SaveChangesAsync();
            return NoContent();
        }


    }
}
