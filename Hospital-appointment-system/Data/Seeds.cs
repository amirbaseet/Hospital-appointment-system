using Hospital_appointment_system.Models;

namespace Hospital_appointment_system.Data
{
    //deffining intial data for the data base  
    public class Seeds
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope=applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService <ApplicationDbContext >();
                context.Database.EnsureCreated();


                // Seed Departments
                if (!context.Departments.Any())
                {
                  context.Departments.AddRange(new List<Departments>
                  {
                     new Departments { Name = "Cardiology", Description = "Heart related diseases and treatments" },
                     new Departments { Name = "Neurology", Description = "Brain and nervous system related diseases and treatments" }
                  });
                    context.SaveChanges();
                }

                // Seed Clinics
                if (!context.Clinic.Any())
                {
                  context.Clinic.AddRange(new List<Clinic>
                  {
                    new Clinic { DepartmentID = 1, Name = "Heart Clinic", Location = "Building A, Floor 3" },
                    new Clinic { DepartmentID = 2, Name = "Brain Health Clinic", Location = "Building B, Floor 2" }
                  });
                     context.SaveChanges();
                }
                // Seed Doctors Hours
                if (!context.Doctors.Any())
                {
                    context.Doctors.AddRange(new List<Doctor>
                    {
                      new Doctor {  Name = "Dr. Eren Yılmaz", Specialization = "Cardiologist", ClinicID = 1 },
                      new Doctor {  Name = "Dr. Ayşe Güneş", Specialization = "Neurologist", ClinicID = 2 }
                    });
                    context.SaveChanges();
                }  
                // Seed Working Hours
                //if (!context.WorkingHours.Any())
                //{
                //    context.WorkingHours.AddRange(new List<WorkingHour>
                //    {
                //    new WorkingHour { DoctorID = 3, DayOfWeek = "Monday", StartTime = TimeSpan.Parse("08:00"), EndTime = TimeSpan.Parse("16:00") },
                //    new WorkingHour { DoctorID = 4, DayOfWeek = "Monday", StartTime = TimeSpan.Parse("09:00"), EndTime = TimeSpan.Parse("17:00") }
                //    });
                //    context.SaveChanges();
                //}
                // Seed Users
                if (!context.PatientUsers.Any())
                {
                   context.PatientUsers.AddRange(new List<PatientUser>
                {
                     new PatientUser {  Username = "Ali", Password = "123", Email = "Ali@example.com"},
                     new PatientUser { Username = "Ola", Password = "123", Email = "Ola@example.com"}
                     });
                    context.SaveChanges();
                }
                //// Seed Appointments
                //if (!context.Appointments.Any())
                //{
                //    context.Appointments.AddRange(new List<Appointment>
                //    {
                //    new Appointment { PatientUserID = 1, DoctorID = 1, Date = DateTime.Parse("2023-12-01"), Time = TimeSpan.Parse("10:00"), Status = "Scheduled" },
                //    new Appointment { PatientUserID = 123457, DoctorID = 2, Date = DateTime.Parse("2023-12-02"), Time = TimeSpan.Parse("11:00"), Status = "Scheduled" }
                //     });
                //    context.SaveChanges();
                //}

                // Seed Admins
                if (!context.AdminUser.Any())
                {
                    context.AdminUser.AddRange(new List<AdminUser>
                    {
                    new AdminUser {  Password = "sau", Email = "B201210560@sakarya.edu.tr" },
                     });
                    context.SaveChanges();
                }

            }

        }
    }
}
