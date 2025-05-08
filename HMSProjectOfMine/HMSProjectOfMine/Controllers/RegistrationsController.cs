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
    public class RegistrationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public RegistrationsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        private async Task<string?> SaveImageAsync(IFormFile? image)
        {
            if (image == null || image.Length == 0)
                return null;

            string uploadsFolder = Path.Combine(_environment.WebRootPath, "images/registration");
            Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/images/registration/{fileName}";
        }

        // GET: api/Registrations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistrationDTO>>> GetRegistrations()
        {
            return await _context.Registrations.Include(r => r.Patient)
                .Select(r => new RegistrationDTO
                {
                    RegistrationId = r.RegistrationId,
                    RegistrationNo = r.RegistrationNo,
                    RegistrationDate = r.RegistrationDate,
                    PatientId = r.PatientId,
                    ImageUrl = r.ImageUrl,
                    DateOfBirth = r.DateOfBirth,
                    BloodType = r.BloodType, 
                    PhoneNo = r.PhoneNo,
                    Address = r.Address,
                    EmergencyContactName = r.EmergencyContactName,
                    EmergencyContactPhone = r.EmergencyContactPhone,
                    MaritalStatus = r.MaritalStatus,
                    Occupation = r.Occupation,
                    Nationality = r.Nationality,
                    Allergies = r.Allergies,
                    IsActive = r.IsActive,
                    Notes = r.Notes
                })
                .ToListAsync();
        }

        // GET: api/Registrations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RegistrationDTO>> GetRegistration(int id)
        {
              var r = await _context.Registrations
            .Include(r => r.Patient)
   
            .FirstOrDefaultAsync(r => r.RegistrationId == id);

            if (r == null)
                return NotFound();

            return new RegistrationDTO
            {
                RegistrationId = r.RegistrationId,
                RegistrationNo = r.RegistrationNo,
                RegistrationDate = r.RegistrationDate,
                PatientId = r.PatientId,
                ImageUrl = r.ImageUrl,
                DateOfBirth = r.DateOfBirth,
                BloodType=r.BloodType,
                PhoneNo = r.PhoneNo,
                Address = r.Address,
                EmergencyContactName = r.EmergencyContactName,
                EmergencyContactPhone = r.EmergencyContactPhone,
                MaritalStatus = r.MaritalStatus,
                Occupation = r.Occupation,
                Nationality = r.Nationality,
                Allergies = r.Allergies,
                IsActive = r.IsActive,
                Notes = r.Notes
            };
        }

        // POST: api/Registrations
        [HttpPost]
        public async Task<ActionResult<RegistrationDTO>> PostRegistration([FromForm] RegistrationDTO dto)
        {
            string? imageUrl = await SaveImageAsync(dto.Image);

            var registration = new Registration
            {
                RegistrationNo = dto.RegistrationNo,
                PatientId = dto.PatientId,
                ImageUrl = imageUrl,
                DateOfBirth = dto.DateOfBirth,
                BloodType=dto.BloodType,
                PhoneNo = dto.PhoneNo,
                Address = dto.Address,
                EmergencyContactName = dto.EmergencyContactName,
                EmergencyContactPhone = dto.EmergencyContactPhone,
                MaritalStatus = dto.MaritalStatus,
                Occupation = dto.Occupation,
                Nationality = dto.Nationality,
                Allergies = dto.Allergies,
                IsActive = dto.IsActive,
                Notes = dto.Notes
            };

            _context.Registrations.Add(registration);
            await _context.SaveChangesAsync();

            dto.RegistrationId = registration.RegistrationId;
            dto.RegistrationDate = registration.RegistrationDate;
            dto.ImageUrl = registration.ImageUrl;

            return CreatedAtAction(nameof(GetRegistration), new { id = registration.RegistrationId }, dto);
        }

        // PUT: api/Registrations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRegistration(int id, [FromForm] RegistrationDTO dto)
        {
            if (id != dto.RegistrationId)
                return BadRequest();

            var registration = await _context.Registrations
            .Include(r => r.Patient)

            .FirstOrDefaultAsync(r => r.RegistrationId == id);

            if (registration == null)
                return NotFound();

            registration.RegistrationNo = dto.RegistrationNo;
            registration.PatientId = dto.PatientId;
            registration.DateOfBirth = dto.DateOfBirth;
            registration.BloodType = dto.BloodType;
            registration.PhoneNo = dto.PhoneNo;
            registration.Address = dto.Address;
            registration.EmergencyContactName = dto.EmergencyContactName;
            registration.EmergencyContactPhone = dto.EmergencyContactPhone;
            registration.MaritalStatus = dto.MaritalStatus;
            registration.Occupation = dto.Occupation;
            registration.Nationality = dto.Nationality;
            registration.Allergies = dto.Allergies;
            registration.IsActive = dto.IsActive;
            registration.Notes = dto.Notes;

            if (dto.Image != null)
            {
                // First, try to save the new image
                string? newImageUrl = await SaveImageAsync(dto.Image);

                if (!string.IsNullOrEmpty(newImageUrl))
                {
                    // Then, delete the old image file safely
                    if (!string.IsNullOrEmpty(registration.ImageUrl))
                    {
                        string oldFilePath = Path.Combine(_environment.WebRootPath, registration.ImageUrl.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }

                    // Finally, update the entity with the new image path
                    registration.ImageUrl = newImageUrl;
                }
            }


            //if (dto.Image != null)
            //{
            //    // Delete old file
            //    if (!string.IsNullOrEmpty(registration.ImageUrl))
            //    {
            //        string oldFilePath = Path.Combine(_environment.WebRootPath, registration.ImageUrl.TrimStart('/'));
            //        if (System.IO.File.Exists(oldFilePath))
            //        {
            //            System.IO.File.Delete(oldFilePath);
            //        }
            //    }

            //    // Save new image
            //    registration.ImageUrl = await SaveImageAsync(dto.Image);
            //}

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Registrations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var registration = await _context.Registrations.FindAsync(id);
            if (registration == null)
                return NotFound();

            // Delete image from disk if it exists
            if (!string.IsNullOrEmpty(registration.ImageUrl))
            {
                string imagePath = Path.Combine(_environment.WebRootPath, registration.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            _context.Registrations.Remove(registration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }

}
