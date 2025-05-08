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
    public class DoctorsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public DoctorsController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: api/Doctors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoctorDTO>>> GetDoctors()
        {
            var doctors = await _context.Doctors
                .Select(d => new DoctorDTO
                {
                    DoctorId = d.DoctorId,
                    FirstName = d.FirstName,
                    LastName = d.LastName,
                    //DepartmentId = d.DepartmentId,
                    //SpecializationId = d.SpecializationId,
                    DepartmentName = d.Department.DepartmentName,
                    SpecializationName = d.Specialization.SpecializationName,
                    Phone = d.Phone,
                    Email = d.Email,
                    ImageUrl = d.ImageUrl,
                    //Departments = new List<Department>().ToList(),
                    //Specializations = new List<Specialization>().ToList(),
                })
                .ToListAsync();

            return Ok(doctors);
        }
        // GET: api/Doctors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DoctorDTO>> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            var dto = new DoctorDTO
            {
                DoctorId = doctor.DoctorId,
                FirstName = doctor.FirstName,
                LastName = doctor.LastName,
                Phone = doctor.Phone,
                Email = doctor.Email,
                ImageUrl = doctor.ImageUrl
            };

            return Ok(dto);
        }




        //POST: api/Doctors
        [HttpPost]
        public async Task<ActionResult> CreateDoctor([FromForm] DoctorDTO dto)
        {

            // Check or create Department
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName == dto.DepartmentName);

            if (department == null)
            {
                department = new Department { DepartmentName = dto.DepartmentName };
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();
            }

            // Check or create Specialization (if provided)
            Specialization? specialization = null;
            if (!string.IsNullOrWhiteSpace(dto.SpecializationName))
            {
                specialization = await _context.Specializations
                    .FirstOrDefaultAsync(s => s.SpecializationName == dto.SpecializationName);

                if (specialization == null)
                {
                    specialization = new Specialization { SpecializationName = dto.SpecializationName };
                    _context.Specializations.Add(specialization);
                    await _context.SaveChangesAsync();
                }
            }

            // Create Doctor
            var doctor = new Doctor
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DepartmentId = department.DepartmentId,
                SpecializationId = specialization?.SpecializationId,
                Phone = dto.Phone,
                Email = dto.Email
            };

            // Save image if provided
            if (dto.ImageFile != null)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImageFile.FileName)}";
                var filePath = Path.Combine(_env.WebRootPath, "images/doctors", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);
                }

                doctor.ImageUrl = $"/images/doctors/{fileName}";
            }

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok();
        }

        //// PUT: api/Doctors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, [FromForm] DoctorDTO dto)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            // Check or create Department
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.DepartmentName == dto.DepartmentName);

            if (department == null)
            {
                department = new Department { DepartmentName = dto.DepartmentName };
                _context.Departments.Add(department);
                await _context.SaveChangesAsync();  // Ensure department is saved
            }

            // Check or create Specialization (if provided)
            Specialization? specialization = null;
            if (!string.IsNullOrWhiteSpace(dto.SpecializationName))
            {
                specialization = await _context.Specializations
                    .FirstOrDefaultAsync(s => s.SpecializationName == dto.SpecializationName);

                if (specialization == null)
                {
                    specialization = new Specialization { SpecializationName = dto.SpecializationName };
                    _context.Specializations.Add(specialization);
                    await _context.SaveChangesAsync();  // Ensure specialization is saved
                }
            }

            // Update Doctor details
            doctor.FirstName = dto.FirstName;
            doctor.LastName = dto.LastName;
            doctor.DepartmentId = department.DepartmentId;  // Updated to use created or existing department
            doctor.SpecializationId = specialization?.SpecializationId;  // Updated to use created or existing specialization
            doctor.Phone = dto.Phone;
            doctor.Email = dto.Email;

            // If new image is uploaded, delete old one and save new
            if (dto.ImageFile != null)
            {
                if (!string.IsNullOrEmpty(doctor.ImageUrl))
                {
                    var oldPath = Path.Combine(_env.WebRootPath, doctor.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);  // Delete the old file
                }

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ImageFile.FileName)}";
                var filePath = Path.Combine(_env.WebRootPath, "images/doctors", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);  // Ensure the directory exists

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ImageFile.CopyToAsync(stream);  // Save new image
                }

                doctor.ImageUrl = $"/images/doctors/{fileName}";  // Update image URL
            }

            // Save changes to the doctor record
            await _context.SaveChangesAsync();

            return NoContent();  // Return a success response
        }
        // DELETE: api/Doctors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null) return NotFound();

            // Delete image file
            if (!string.IsNullOrEmpty(doctor.ImageUrl))
            {
                var imagePath = Path.Combine(_env.WebRootPath, doctor.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
