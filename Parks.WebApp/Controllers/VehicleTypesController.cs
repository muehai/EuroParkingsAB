using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parks.WebApp.Models.Entities;
using Parks.WebApp.Data;
using Parks.WebApp.Abstact;
using System.Net;
using Parks.WebApp.Infrastructure;

namespace Parks.WebApp.Controllers
{
    public class VehicleTypesController : Controller
    {
        private IVehicleTypeRepository _vehicleTypeRepository;
        private DisposeExtension _disposeExtension = new DisposeExtension();

        public VehicleTypesController(IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleTypeRepository = vehicleTypeRepository;
        }
        
        public IActionResult Index()
        {
            return View(_vehicleTypeRepository.GetAllEntities());
        }

        public IActionResult Details(int? id)
        {
           if(id == null)
           {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
           }

            VehicleType vehicleType = _vehicleTypeRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();

            if (vehicleType == null)
            {
                ModelState.AddModelError("VehicleType details", "There is not vehicle type.");
            }

            return View(vehicleType);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id, Name")] VehicleType vehicleType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _vehicleTypeRepository.AddEntity(vehicleType);
                    _vehicleTypeRepository.Commit();

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("Creating vehicle Type", "Failed to create a new vehicletype. " + ex.Message);
            }

            return View(vehicleType);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleType vehicleType = _vehicleTypeRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();

            if (vehicleType == null)
            {
                ModelState.AddModelError("Editing editing", "Failed to show vehicle type data. ");
            }
            return View(vehicleType);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,Name")] VehicleType vehicleType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _vehicleTypeRepository.UpdateEntity(vehicleType);
                    _vehicleTypeRepository.Commit();

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("Editing vehicle type", "Failed editing owner data. " + ex.Message);
            }
            return View(vehicleType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            VehicleType vehicleType = _vehicleTypeRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();//db.VehicleTypes.Find(id);

            if (vehicleType == null)
            {
                ModelState.AddModelError("Delete vehicle Type", "Failed to show vehicletype to be deleting. ");
            }
            return View(vehicleType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            VehicleType vehicleType = _vehicleTypeRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();
            try
            { 
                _vehicleTypeRepository.DeleteEntity(vehicleType);
                _vehicleTypeRepository.Commit();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("Deleting vehicle type", "Failed editing owner data. " + ex.Message);
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _disposeExtension.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}