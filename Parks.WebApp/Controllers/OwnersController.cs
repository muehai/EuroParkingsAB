using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parks.WebApp.Data;
using Parks.WebApp.Models.Entities;
using Parks.WebApp.Abstact;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Parks.WebApp.Infrastructure;
using System.Net;

namespace Parks.WebApp.Controllers
{
    public class OwnersController : Controller
    {
        private IOwnerRepository _ownerRepository;
        private DisposeExtension _disposeExtension = new DisposeExtension();

        public OwnersController(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public IActionResult Index(string searchTerm)
        {
            var model = from m in _ownerRepository.GetAllEntities()
                        select m;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                model = model.Where( m => m.OwnerPersonalId.Contains(searchTerm) 
                                       || m.Name.Contains(searchTerm)
                                       || m.Email.Contains(searchTerm)
                                       || m.PhoneNumber.Contains(searchTerm));
            }

            
            return View(model.ToList());
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Owner owner = _ownerRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();

            if (owner == null)
            {
                ModelState.AddModelError("Owner details","There is not owner.");
            }
            return View(owner);
        }

        public IActionResult Create()
        {
            //ViewBag.Owners = new SelectList(_ownerRepository.GetAllEntities(), "Id", "OwnersVehicleTypes", "OwnersVehicleTypes");
            return View();
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id, OwnerPersonalId, Name,Email,PhoneNumber,Address")] Owner owner)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ownerRepository.AddEntity(owner);
                    _ownerRepository.Commit();

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("Creating owner", "Failed to create a new owner. " + ex.Message);
            }
            return View(owner);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Owner owner = _ownerRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();

            if (owner == null)
            {
                ModelState.AddModelError("Editing owner", "Failed to show to owner data. ");
            }
            return View(owner);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("Id,OwnerPersonalId, Name, Email, PhoneNumber, Address")] Owner owner)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _ownerRepository.UpdateEntity(owner);
                    _ownerRepository.Commit();

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("Editing owner", "Failed editing owner data. " + ex.Message);
            }
            return View(owner);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Owner owner = _ownerRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();

            if (owner == null)
            {
                ModelState.AddModelError("Delete owner", "Failed to get owner data to be deleting. ");
            }
            return View(owner);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                Owner owner = _ownerRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();
                _ownerRepository.DeleteEntity(owner);
                _ownerRepository.Commit();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("Delete owner", "Failed deleting owner data. " + ex.Message);
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