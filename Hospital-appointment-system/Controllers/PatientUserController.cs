﻿using Hospital_appointment_system.Data;
using Hospital_appointment_system.Interfaces;
using Hospital_appointment_system.Models;
using Hospital_appointment_system.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Hospital_appointment_system.Controllers
{
    public class PatientUserController : Controller
    {
        private readonly IPatientUserRepository _PatientUserRepository;
        private readonly ApplicationDbContext _context;

        public PatientUserController( IPatientUserRepository patientUserRepository, ApplicationDbContext context)
        {
            _PatientUserRepository = patientUserRepository;
            _context = context;
        }
        public async Task <IActionResult> Index()
        {
            //var patients = _context.PatientUsers.ToList();
            //return View(patients);
            IEnumerable<PatientUser> patients =await _PatientUserRepository.GetAll();
            return View(patients);
        }
        // GET: User/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PatientUser patientUser)
        {
            if (ModelState.IsValid)
            {

                if (await _PatientUserRepository.CheckUserbyEmail(patientUser.Email))
                {
                    // Add an error to the ModelState
                    ModelState.AddModelError(string.Empty, "User already exists with the given email.");
                    return View(patientUser);
                }
                _PatientUserRepository.Add(patientUser);
                return RedirectToAction(nameof(Index));
            }
            // If we reach here, something went wrong, re-show form
            return View(patientUser);
        }
        // GET: User/Create
        [HttpGet]
        public IActionResult Delete()
        {
            return View();
        }
        // POST: User/Delete/5
        [HttpPost]
        //[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string Email)
        {
            var patient = await _PatientUserRepository.GetByEmailAsync(Email);
            if (patient == null) 
            {
                // Add an error to the ModelState
                ModelState.AddModelError(string.Empty, "User Doesn`t exists with the given email.");
                return View(patient);
            }
            _PatientUserRepository.Delete(patient);
            return RedirectToAction("Index");
        }

        //[HttpGet]
        //public IActionResult Delete()
        //{
        //    return View();
        //}
        //[HttpPost, ActionName("Delete")]
        //public async Task<IActionResult> Delete(string Email)
        //{
        //    var patient = await _PatientUserRepository.GetByEmailAsync(Email);

        //    if (patient == null)
        //    {
        //        return View("Index");
        //    }
        //    _PatientUserRepository.Delete(patient);
        //    return RedirectToAction("Index");
        //}
    }

}