﻿using Hospital_appointment_system.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_appointment_system.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Doctor>Doctors { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<PatientUser> PatientUsers { get; set; }
        public DbSet<AdminUser> AdminUser { get; set; }
        public DbSet<Clinic> Clinic { get; set; }
        public DbSet<WorkingHour>WorkingHours { get; set; }

        public DbSet <Departments> Departments { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\ProjectModels;Database=Hospital;Trusted_Connection=True;");


        }
    }
}