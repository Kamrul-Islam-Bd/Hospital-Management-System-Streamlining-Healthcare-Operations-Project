using HMSProjectOfMine.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
using HMSProjectOfMine.Enums;
using HMSProjectOfMine.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HMSProjectOfMine.Data
{
    public class AppDbContext: IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> op) : base(op) { }
                
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicine> PrescriptionMedicines { get; set; }
        public DbSet<PrescriptionTest> PrescriptionTests { get; set; }
        public DbSet<PrescriptionDiagnosis> PrescriptionDiagnoses { get; set; }
        public DbSet<PrescriptionAdvice> PrescriptionAdvices { get; set; }
        public DbSet<PhysicalSymptom> PhysicalSymptoms { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestReport> TestReports { get; set; }
        public DbSet<TestBilling> TestBillings { get; set; }
        public DbSet<TestBillingDetail> TestBillingDetails { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Chamber> Chambers { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicinePurchase> MedicinePurchases { get; set; }
        public DbSet<MedicineSale> MedicineSales { get; set; }
        public DbSet<MedicineProfit> MedicineProfits { get; set; }
        public DbSet<MedicineLoss> MedicineLosses { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<DoctorFee> DoctorFees { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<DoctorChamber> DoctorChambers { get; set; }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //obosshoi with many te diye dite hobe. eta amake onek vugiyeche.
            modelBuilder.Entity<Prescription>()
       .HasOne(p => p.Token)
       .WithMany(d => d.Prescriptions) // If Token has a `Prescriptions` collection, put it here
       .HasForeignKey(p => p.TokenId)
       .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Prescription>()
    .HasOne(p => p.Doctor)
    .WithMany(d => d.Prescriptions) // If Doctor has a `Prescriptions` collection, use it here
    .HasForeignKey(p => p.DoctorId)
    .OnDelete(DeleteBehavior.Cascade);

            // MedicinePurchase Configuration
            modelBuilder.Entity<MedicinePurchase>(entity =>
            {
                entity.Property(mp => mp.TotalPrice)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired()
                      .HasComputedColumnSql("[PurchasePrice] * [QuantityPurchased] + [VAT]", stored: true);
            });

            // MedicineSale Configuration
            modelBuilder.Entity<MedicineSale>(entity =>
            {
                entity.Property(ms => ms.TotalPrice)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired()
                      .HasComputedColumnSql("[SalePrice] * [QuantitySold] - [Discount]", stored: true);
            });

            // MedicineProfit Configuration - Fixed subquery
            modelBuilder.Entity<MedicineProfit>(entity =>
            {
                entity.Property(mp => mp.TotalProfit)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired()
                      .HasComputedColumnSql("[ProfitAmount] * [QuantitySold]", stored: true);

                entity.Property<int>("QuantitySold")
                      .HasColumnName("QuantitySold");
            });

            // MedicineLoss Configuration
            modelBuilder.Entity<MedicineLoss>(entity =>
            {
                entity.Property(ml => ml.TotalLoss)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired()
                      .HasComputedColumnSql("[LossAmount] * [QuantityLoss]", stored: true);
            });

            // DoctorFee Configuration
            modelBuilder.Entity<DoctorFee>(entity =>
            {
                entity.Property(e => e.Fees)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.Property(e => e.DiscountAmount)
                      .HasColumnType("decimal(18,2)")
                      ;

                entity.Property(e => e.EffectiveDate)
                      .HasColumnType("datetime2")
                      .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ChargedFee)
                      .HasColumnType("decimal(18,2)")
                      .HasComputedColumnSql("[Fees] - [DiscountAmount]", stored: true);
            });

         



            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Patient_PatientType", "[PatientType] IN ('Indoor', 'Outdoor')");
                    t.HasCheckConstraint("CK_Patient_VisitType", "[VisitType] IN ('FirstVisit', 'FollowUp', 'TestReview', 'Others')");
                });

                entity.Property(p => p.PatientType)
                      .HasConversion<string>()
                      .HasMaxLength(10);

                entity.Property(p => p.VisitType)
                      .HasConversion<string>()
                      .HasMaxLength(20);

                entity.Property(p => p.FirstVisitDate)
                      .HasDefaultValueSql("GETDATE()");
            });


            //HasConstraint solution
                modelBuilder.Entity<Registration>(entity =>
                {
                    entity.ToTable(t =>
                    {
                        t.HasCheckConstraint("CK_Registration_MaritalStatus",
                            "[MaritalStatus] IN ('Single', 'Married', 'Divorced', 'Widowed')");
                    });

                    entity.Property(r => r.RegistrationDate)
                          .HasDefaultValueSql("GETDATE()");

                    entity.Property(r => r.MaritalStatus)
                          .HasConversion<string>()
                          .HasMaxLength(10);
                });


            // TestBilling Configuration
            modelBuilder.Entity<TestBilling>(entity =>
            {
                //    entity.Property(e => e.TotalAmount)
                //      .HasColumnType("decimal(18,2)")
                //      .HasComputedColumnSql("([TotalAmount] * [DiscountPercentage]) / 100", stored: true);

                //    entity.Property(e => e.DiscountAmount)
                //        .HasColumnType("decimal(18,2)")
                //        .HasComputedColumnSql("([TotalAmount] * [DiscountPercentage]) / 100", stored: true);

                //    entity.Property(e => e.PayableAmount)
                //        .HasColumnType("decimal(18,2)")
                //        .HasComputedColumnSql("([TotalAmount] - ([TotalAmount] * [DiscountPercentage]) / 100)", stored: true);

                //    entity.Property(e => e.DueAmount)
                //        .HasColumnType("decimal(18,2)")
                //        .HasComputedColumnSql("(([TotalAmount] - ([TotalAmount] * [DiscountPercentage]) / 100) - [PaidAmount])", stored: true);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<TestBilling>(entity =>
            {
                entity.Property(e => e.TotalAmount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DiscountAmount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DiscountPercentage)
                      .HasColumnType("decimal(5,2)");

                entity.Property(e => e.PayableAmount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.DueAmount)
                      .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CreatedAt)
                      .HasDefaultValueSql("GETDATE()");
            });

            // TestBillingDetail Configuration
            //modelBuilder.Entity<TestBillingDetail>(entity =>
            //{
            //    entity.Property(e => e.TotalPrice)
            //        .HasColumnType("decimal(18,2)")
            //        .HasComputedColumnSql("([Price] * [Quantity])", stored: true);
            //});

            modelBuilder.Entity<TestBillingDetail>(entity =>
            {

                entity.Property(e => e.TotalPrice)
                      .HasColumnType("decimal(18,2)");
            });
          


            modelBuilder.Entity<Ward>()
             .ToTable(t => t.HasCheckConstraint("CK_WardType_ValidValues", "WardType IN ('General', 'ICU', 'Maternity', 'Cabin')"))
             .Property(r => r.WardType)
              .HasConversion<string>()
              .HasMaxLength(10);








            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_AppointmentStatus_ValidValues",
                        "AppointmentStatus IN ('Scheduled', 'Arrived', 'InConsultation', 'Completed', 'Cancelled', 'NoShow')");
                    t.HasCheckConstraint("CK_AppointmentType_ValidValues",
                        "AppointmentType IN ('General', 'FollowUp', 'Emergency', 'PostOperative', 'Preventive', 'Referral')");
                });

                entity.Property(r => r.AppointmentStatus)
                      .HasConversion<string>()
                      .HasMaxLength(10);

                entity.Property(r => r.AppointmentType)
                      .HasConversion<string>()
                      .HasMaxLength(20); // Adjust as per your actual enum values length
            });

            modelBuilder.Entity<Registration>(entity =>
            {
                // Enum ke string e store korar jonno
                entity.Property(e => e.BloodType)
                    .HasConversion<string>()
                    .HasMaxLength(10);  // "AB+" jemon longest value, 5 character lagbe

                // Check constraint jeno valid value store hoy
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_BloodGroups_BloodType",
                        "[BloodType] IN ('A_POS', 'A_NEG', 'B_POS', 'B_NEG', 'O_POS', 'O_NEG', 'AB_POS', 'AB_NEG')");
                });
            });

        

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.Gender)
                    .HasConversion<string>()
                    .HasMaxLength(6); // "Female" is the longest value

                // Add CHECK CONSTRAINT to ensure valid enum values only
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Patient_Gender",
                        "[Gender] IN ('Male', 'Female', 'Other')");
                });
            });













            // Department Seed Data
            modelBuilder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, DepartmentName = "Cardiology" },
                new Department { DepartmentId = 2, DepartmentName = "Neurology" },
                new Department { DepartmentId = 3, DepartmentName = "Orthopedics" },
                new Department { DepartmentId = 4, DepartmentName = "ENT" },
                new Department { DepartmentId = 5, DepartmentName = "Urology" },
                new Department { DepartmentId = 6, DepartmentName = "Pediatrics" },
                new Department { DepartmentId = 7, DepartmentName = "Pathology" },
                new Department { DepartmentId = 8, DepartmentName = "Ophthalmology" }
            );

            // Patient Seed Data
            modelBuilder.Entity<Patient>().HasData(
                new Patient { PatientId = 1, PatientNo = "1A", FirstName = "Maria", LastName = "Ahmed", Gender = Gender.Female, FirstVisitDate = DateTime.Now, PatientType = PatientType.Outdoor, VisitType = VisitType.FirstVisit }
                );

            // Specialization Seed Data
            modelBuilder.Entity<Specialization>().HasData(
                new Specialization { SpecializationId = 1, SpecializationName = "Madesin" },
                new Specialization { SpecializationId = 2, SpecializationName = "Sergary" }
            );



            // Ward Seed Data
            modelBuilder.Entity<Ward>().HasData(
                new Ward { WardId = 1, WardName = "Male", WardType = WardType.General },
                new Ward { WardId = 2, WardName = "Female", WardType = WardType.General },
                new Ward { WardId = 3, WardName = "Male", WardType = WardType.Cabin },
                new Ward { WardId = 4, WardName = "Female", WardType = WardType.Cabin }
            );

            // Bed Seed Data
            modelBuilder.Entity<Bed>().HasData(
                new Bed { BedId = 1, WardId = 1, BedNumber = "101", IsOccupied = true },
                new Bed { BedId = 2, WardId = 2, BedNumber = "102", IsOccupied = false },
                new Bed { BedId = 3, WardId = 3, BedNumber = "103", IsOccupied = true },
                new Bed { BedId = 4, WardId = 4, BedNumber = "104", IsOccupied = false }
            );
            


            // Doctor Seed Data
            modelBuilder.Entity<Doctor>().HasData(
                new Doctor { DoctorId = 1, FirstName = "Remon", LastName = "Hossan", DepartmentId = 1, SpecializationId = 1, Phone = "01236454487", Email = "Remon@gmail.com", ImageUrl = "sjfgbasfg" }
            );

            // Token Seed Data
            modelBuilder.Entity<Token>().HasData(
                new Token { TokenId = 1, TokenNumber = "101", TokenFee = 20, PatientId = 1, DoctorId = 1, ChamberId = 1, IssueTime = new DateTime(2025, 5, 1) }
            );


            // Chamber Seed Data
            modelBuilder.Entity<Chamber>().HasData(
                new Chamber { ChamberId = 1, ChamberNo = "201A", Location = "2nd Floor" },
                new Chamber { ChamberId = 2, ChamberNo = "301A", Location = "3rd Floor" },
                new Chamber { ChamberId = 3, ChamberNo = "401A", Location = "4th Floor" }
            );


            // Prescription Seed Data
            modelBuilder.Entity<Prescription>().HasData(
                new Prescription { PrescriptionId = 1, TokenId = 1, DoctorId = 1, PrescriptionDate = new DateTime(2025, 5, 1), NextVisitDate = new DateTime(2025, 5, 29) },
                 new Prescription
                 {
                     PrescriptionId = 2,
                     TokenId = 1,
                    
                     DoctorId = 1,
                     PrescriptionDate = new DateTime(2025, 5, 2),
                     NextVisitDate = new DateTime(2025, 6, 1)
                 }
            ); ;
            // PrescriptionTest Seed Data
            modelBuilder.Entity<PrescriptionTest>().HasData(
                new PrescriptionTest { PrescriptionTestId = 1, PrescriptionId = 1, TestId = 1 }
            );

            // Test Seed Data
            modelBuilder.Entity<Test>().HasData(
                new Test { TestId = 1, TestName = "CBC", Price = 2000 },
                new Test { TestId = 2, TestName = "dfg", Price = 2000 }
            );

            // TestReport Seed Data
            modelBuilder.Entity<TestReport>().HasData(
                new TestReport { TestReportId = 1, PrescriptionTestId = 1, TestResult = "No Issues Found",ReportDate= new DateTime(2025, 5, 29) ,IsFinalized=false}
            );


            // Admission Seed Data
            modelBuilder.Entity<Admission>().HasData(
                new Admission
                {
                    AdmissionId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    BedId = 1,
                    AdmissionDate = new DateTime(2025, 5, 1),
                    DischargeDate = new DateTime(2025, 5, 10),
                    NurseName = "Ayesha Khatun",
                    ReferredBy = "Dr. Rafiq",
                    Floor = "2nd Floor",
                    ChargePerDay = 1500.00m,
                    AdmissionFee = 5000.00m
                }
               
            );

            modelBuilder.Entity<Appointment>().HasData(
                new Appointment
                {
                    AppointmentId = 1,
                    PatientId = 1,
                    DoctorId = 1,
                    AdmissionId = 1,
                    TokenId = 1,
                    AppointmentDate = new DateTime(2025, 5, 2, 10, 30, 0), // 2nd May 2025 at 10:30 AM
                    PatientPhone = "01712345678",
                    ReferralCode = "REF123",
                    AppointmentStatus = AppointmentStatus.Scheduled,
                    AppointmentType = AppointmentType.General
                }
            );
            
            modelBuilder.Entity<Medicine>().HasData(
                new Medicine { MedicineId=1, MedicineType="Tablet", MedicineName= "Monus", GenericName= "Montelukast Sodium", Company= "Incepta " },
                new Medicine { MedicineId = 2, MedicineType = "Syrup", MedicineName = "Napa", GenericName = "Paracetamol ", Company = "Beximco " }
            );









        }


    }
}
