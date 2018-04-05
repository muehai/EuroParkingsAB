using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parks.WebApp.Models.Entities;
using Parks.WebApp.Repositories;
using Parks.WebApp.Abstact;
using Parks.WebApp.Data;
using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Parks.WebApp.Infrastructure;
using System.Diagnostics;
using Parks.WebApp.Models;

namespace Parks.WebApp.Controllers
{
    public class VehiclesController : Controller
    {
        //DI variable
        private IVehicleRepository _vehicleRepository;
        private IOwnerRepository _ownerRepository;
        private IVehicleTypeRepository _vehicleTypeRepository;
        private DisposeExtension _disposeExtension = new DisposeExtension();
        
        //Parking capacity
        private const int parkingCapacity = 15;
        private int[] parkingSpace = new int[parkingCapacity];
     

        //DI
        public VehiclesController(IVehicleRepository vehicleRepository, IOwnerRepository ownerRepository, 
                                    IVehicleTypeRepository vehicleTypeRepository)
        {
            _vehicleRepository = vehicleRepository;
            _ownerRepository = ownerRepository;
            _vehicleTypeRepository = vehicleTypeRepository;
        }

        public IActionResult Index(string sortOrder, string searchTerm, Vehicle ve)
        {
            string reg = _vehicleRepository.FindVehiecleById(e => e.RegisterNr == ve.RegisterNr).ToString();

            if (!String.IsNullOrEmpty(searchTerm) || searchTerm == reg )
            {
                var regnr = _vehicleRepository.FindVehiecleById(v =>
                                               v.RegisterNr.Contains(searchTerm)
                                               || v.VehicleTypeName.Contains(searchTerm)
                                               || v.Model.Contains(searchTerm));
                                                                                        

                return View(regnr.ToList());
            }

            if (searchTerm  != ve.RegisterNr  )
            {

                return RedirectToAction("ErrorMessage", "Vehicles");
                //ViewBag.FoundMessage = "The register number is not found";
            }
            

            //Get all Vehicles
            var model = from v in _vehicleRepository.GetAllEntities()
                        select v;

            ViewBag.SortVehicleTypeName = sortOrder == "VehicleTypeName" ?  "VehicleTypeName_Desc": "VehicleTypeName";
            ViewBag.SortRegisterNr = sortOrder == "RegisterNr" ? "RegisterNrOrderby" : "RegisterNr";
            ViewBag.SortMembersFirstName = sortOrder == "FullName" ? "FullNameOrderBy" : "FullName";
            ViewBag.VehicleModel = sortOrder == "Model" ? "ModelOrderBy" : "Model";
            ViewData["VehicleFilteData"] = searchTerm;

            if (model != null)
            {
                switch (sortOrder)
                {
                    case "RegisterNr":
                        model = model.OrderByDescending(m => m.RegisterNr);
                        break;
                    case "RegisterNrOrderby":
                        model = model.OrderBy(m => m.RegisterNr);
                        break;

                    case "VehicleTypeName":
                        model = model.OrderByDescending(m => m.VehicleTypeName);
                        break;
                    case "VehicleTypeNameOrderby":
                        model = model.OrderBy(m => m.VehicleTypeName);
                        break;
                    case "FullNameOrderBy":
                        model = model.OrderByDescending(m => m.VehicleOwners.Name);
                        break;
                    case "FullName":
                        model = model.OrderBy(m => m.VehicleOwners.Name);
                        break;
                    case "Model":
                        model = model.OrderByDescending(m => m.Model);
                        break;
                    case "ModelOrderBy":
                        model = model.OrderBy(m => m.Model);
                        break;

                    default:
                        model = model.OrderBy(m => m.VehicleTypeName);
                        break;
                }

                return View(model.ToList());
            }
            //Covert the Vehicle time
            Vehicle vehicle = new Vehicle();
            ViewBag.DateTime = vehicle.ParkingTime.Day;
            ViewBag.Hours = (DateTime.Now - vehicle.ParkingTime).Hours;
            ViewBag.Minutes = (DateTime.Now - vehicle.ParkingTime).Minutes;
            ViewBag.TotalMinutes = (DateTime.Now - vehicle.ParkingTime).TotalMinutes;

            return  View(model.ToList());
        }

        public IActionResult HomePage(Vehicle vehicle)
        {
            int parkedVehicleSize = parkingCapacity - _vehicleRepository.Count();
            int parkedvehiclefigur = _vehicleRepository.Count();
            ViewBag.parkedvehiclefigur = parkedvehiclefigur;

            if (_vehicleRepository.Count() < parkingCapacity)
            {
                ViewBag.ParingCapacity = parkedVehicleSize;
            }
            if (_vehicleRepository.Count() == parkingCapacity)
            {
                ViewBag.ParkingInformation = _vehicleRepository.Count();
            }

           var vehicleLst = _vehicleRepository.GetAllEntities();

            return View(vehicleLst.ToList());
        }
        public IActionResult ErrorMessage()
        {
            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // GET: Vehicles/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = _vehicleRepository.FindVehiecleById(e => e.Id == id).FirstOrDefault();
            if (vehicle == null)
            {
               return new HttpStatusCodeResult("The vehicle register-number is null or not found.");
            }

            return View(vehicle);
        }

        public IActionResult Create()
        {
                             
            ViewBag.OwnerName = new SelectList(_ownerRepository.GetAllEntities(), "Id", "OwnerPersonalId");
            ViewBag.VehicleTypeName = new SelectList(_vehicleTypeRepository.GetAllEntities(),"Name","Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,RegisterNr,Color,Brand,Model,ParkeringSpaceId, VehicleOwnersId,VehicleTypeName")] Vehicle vehicle)
        {
            ViewBag.FreeParkSpace =_vehicleRepository.Count();
            ViewBag.GarageCapacity = parkingCapacity;
            double TotalParkingMinutes = 0;
            //Check if register nr is char
            string IsChar = "";
            //Check if register nr is char
            string IsDigit = "";
            //Result of register number
            string reg_nr = "";

            if (_vehicleRepository.Count() == parkingCapacity)
            {
                ViewBag.NoFreeParkSpace = parkingCapacity;
                // No Vehicle instans is passade to this view just ViewBag
                return View("ViewVehiclePark", vehicle);
            }
            else if (_vehicleRepository.Count() < parkingCapacity)
            {
                //Validate swedish car register number "GHK123"

                for (int i = 0; i < vehicle.RegisterNr.Length; i++)
                {
                    if (char.IsLetter(vehicle.RegisterNr[0]) && (char.IsLetter(vehicle.RegisterNr[1])) && char.IsLetter(vehicle.RegisterNr[2]))
                    {
                        IsChar = vehicle.RegisterNr[0] + vehicle.RegisterNr[1].ToString() + vehicle.RegisterNr[2].ToString();
                     }

                    if (char.IsDigit(vehicle.RegisterNr[3]) && (char.IsDigit(vehicle.RegisterNr[4])) && char.IsDigit(vehicle.RegisterNr[5]))
                    {
                        IsDigit = vehicle.RegisterNr[3].ToString() + vehicle.RegisterNr[4].ToString() + vehicle.RegisterNr[5].ToString();
                    }
                }

                reg_nr = IsChar + IsDigit;
                vehicle.RegisterNr = reg_nr; //We have now validate the car register nummber   
                vehicle.ParkingTime = DateTime.Now;
                vehicle.ParkeringSpaceId = ParkingAvailablity();
               

                var VehicleExist = _vehicleRepository.GetAllEntities().Any(v => v.RegisterNr == vehicle.RegisterNr);

                if (VehicleExist)
                {
                    ModelState.AddModelError("Registration Number", "Registration number already exist.");
                }

                try
                {

                    if (ModelState.IsValid)
                    {
                        TotalParkingMinutes = Math.Round(TotalParkingMinutes + (DateTime.Now - vehicle.ParkingTime).TotalMinutes);
                        _vehicleRepository.AddEntity(vehicle);
                        _vehicleRepository.Commit();

                        return RedirectToAction("CheckIn", "Vehicles");
                    }
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("Creating vehicles", "Unable to save changes in the database vehicle obj " +
                                             "Try again, and if the problem persists " + ex.Message);
                }
            }
            else
            {
                ModelState.AddModelError("Faile", "Fail to park cars in garage!!!");
            }

                ViewBag.VehicleOwnersId = new SelectList(_ownerRepository.GetAllEntities(),"Id", "Name", vehicle.VehicleOwnersId);
                ViewBag.VehicleTypeName = new SelectList(_vehicleTypeRepository.GetAllEntities(),"Id,Name", vehicle.VehicleTypeName);

                ViewBag.NoFreeParkSpace =_vehicleRepository.Count();
                ViewBag.GarageCapacity = parkingCapacity;
                ViewBag.TotalParkingValues = (TotalParkingMinutes * 60) / 60;

                return View(vehicle);
        }
       
        // Check Action method
       
        public IActionResult CheckIn(Vehicle v)
        {
            ViewBag.NoFreeParkSpace =_vehicleRepository.Count();
            ViewBag.GarageCapacity = parkingCapacity;
            ViewBag.Hours = (DateTime.Now - v.ParkingTime).Hours;
            ViewBag.Minutes = (DateTime.Now - v.ParkingTime).Minutes;
            ViewBag.TotalMinutes = (DateTime.Now - v.ParkingTime).TotalMinutes;

            return View(v);
        }


        // ViewVehiclePark Action method to check if the park is full
        public IActionResult ViewVehiclePark(Vehicle v)
        {
            return View(v);
        }

        // Vehicle Overview 
        public IActionResult VehicleOverView2(string sortOrder, string searchTerm)
        {
            var model = from m in _vehicleRepository.GetAllEntities()
                        select m;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                model = model.Where(m => m.VehicleTypeName.Contains(searchTerm)
                                       || m.RegisterNr.Contains(searchTerm)
                                       || m.VehicleOwners.Name.Contains(searchTerm)
                                       || m.VehicleOwners.Name.Contains(searchTerm));
            }

            // ViewBag for Searching Vehicles Obj
            ViewBag.SortVehicleTypeName = sortOrder == "VehicleTypeName" ? "VehicleTypeNameOrderby" : "VehicleTypeName";
            ViewBag.SortRegisterNr = sortOrder == "RegisterNr" ? "RegisterNrOrderby" : "RegisterNr";
            ViewBag.SortVehicleType = sortOrder == "VehicleType" ? "VehicleTypeOrderBy" : "VehicleType";
            ViewBag.SortMembersFirstName = sortOrder == "FirstName" ? "FirstNameOrderBy" : "FirstName";
            ViewBag.SortMembersLastName = sortOrder == "LastName" ? "LastNameOrderBy" : "LastName";

            switch (sortOrder)
            {
                case "VehicleTypeName":
                    model = model.OrderByDescending(m => m.VehicleTypeName);
                    break;
                case "VehicleTypeNameOrderby":
                    model = model.OrderBy(m => m.VehicleTypeName);
                    break;
                case "RegisterNr":
                    model = model.OrderByDescending(m => m.RegisterNr);
                    break;
                case "RegisterNrOrderby":
                    model = model.OrderBy(m => m.RegisterNr);
                    break;
                case "VehicleType":
                    model = model.OrderByDescending(m => m.VehicleTypeName);
                    break;
                case "VehicleTypeOrderBy":
                    model = model.OrderBy(m => m.VehicleTypeName);
                    break;
                case "FirstName":
                    model = model.OrderByDescending(m => m.VehicleOwners.Name);
                    break;
                case "FirstNameOrderBy":
                    model = model.OrderBy(m => m.VehicleOwners.Name);
                    break;

                case "LastName":
                    model = model.OrderByDescending(m => m.VehicleOwners.Name);
                    break;
                case "LastNameOrderBy":
                    model = model.OrderBy(m => m.VehicleOwners.Name);
                    break;

                default:
                    model = model.OrderByDescending(m => m.VehicleTypeName);
                    break;
            }

            Vehicle v = new Vehicle();
            ViewBag.DateTime = v.ParkingTime.Day;
            ViewBag.Hours = (DateTime.Now - v.ParkingTime).Hours;
            ViewBag.Minutes = (DateTime.Now - v.ParkingTime).Minutes;
            ViewBag.TotalMinutes = (DateTime.Now - v.ParkingTime).TotalMinutes;

            return View(model.ToList());
        }
        
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult("The vehicle register-number is null.");
            }
            Vehicle vehicle = _vehicleRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();

          
            if (vehicle == null)
            {
                ModelState.AddModelError("Editing", "Error for editing register number.");
            }
            ViewBag.VehicleOwnersId = new SelectList(_ownerRepository.GetAllEntities(), "Id", "OwnerPersonalId", vehicle.VehicleOwnersId);
            ViewBag.VehicleTypeName = new SelectList(_vehicleTypeRepository.GetAllEntities(), "Name","Name", vehicle.VehicleTypeName);

            return View(vehicle);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([Bind("RegisterNr,Color,Brand,Model,VehicleTypeName")] Vehicle vehicle)
        {
            var ve = _vehicleRepository.FindVehiecleById(e => e.Id == vehicle.Id);
            

            try
            {
                if (ModelState.IsValid)
                {
                    _vehicleRepository.UpdateEntity(vehicle);
                    _vehicleRepository.Commit();

                    return RedirectToAction("ParkingsList"); // action method "ShowGarage"
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("Editing vehicles", "Unable to save changes in the database vehicle obj " +
                                         "Try again, and if the problem persists " + ex.Message);
            }
            ViewBag.VehicleOwnerId = new SelectList(_ownerRepository.GetAllEntities(), "Id", "OwnerPersonalId", vehicle.VehicleOwnersId);
            ViewBag.VehicleTypeName = new SelectList(_vehicleTypeRepository.GetAllEntities(), "Id", "Name", vehicle.VehicleTypeName);

            return View(vehicle);
        }


        //Check out
        public IActionResult CheckOut(string searchTerm)
        {
            var vehicles = from v in _vehicleRepository.GetAllEntities()
                           select v;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                var vehicle = vehicles.Where(e => e.RegisterNr == searchTerm);
                if (!vehicle.Any())
                {
                    
                    return View(vehicle);
                }
              
                return View(vehicle);
            }
            return View(vehicles);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vehicle vehicle = _vehicleRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();
            if (vehicle == null)
            {
                ModelState.AddModelError("Deleting", "Error for deleting vehicle.");
            }

            return View(vehicle);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Vehicle vehicle = _vehicleRepository.FindVehiecleById(v => v.Id == id).FirstOrDefault();
            _vehicleRepository.DeleteEntity(vehicle); 
            _vehicleRepository.Commit();

            return RedirectToAction("Receipt", vehicle);  
        }

        // Reciept Action method
        public IActionResult Receipt(Vehicle v)
        {
            ViewBag.Hours = (DateTime.Now - v.ParkingTime).Hours;
            ViewBag.Minutes = (DateTime.Now - v.ParkingTime).Minutes;
            ViewBag.TotalMinutes = (DateTime.Now - v.ParkingTime).TotalMinutes;

            return View(v);
        }

        private int ParkingAvailablity()
        {
            parkingSpace = new int[_vehicleRepository.Count()];
            int i = 0;
            foreach (var v in _vehicleRepository.GetAllEntities())
            {
                parkingSpace[i] = v.ParkeringSpaceId;
                i++;
            }
            parkingSpace = parkingSpace.OrderBy(e => e).ToArray();

            for (int j = 0; j < parkingSpace.Length; j++)
            {
                if (parkingSpace[j] != j + 1)
                    return j + 1;
            }
            return _vehicleRepository.Count() + 1;
        }
        //Vehicle Statics
        public ActionResult ParkingStatisticsData()
        {
            ViewBag.OccupiedParkingSpaces = _vehicleRepository.Count();

            ViewBag.GarageCapacity = parkingCapacity;

            double TotalParkingMinutes = 0;

            foreach (var vehicle in _vehicleRepository.GetAllEntities())
            {
                TotalParkingMinutes = Math.Round(TotalParkingMinutes + (DateTime.Now - vehicle.ParkingTime).TotalMinutes);
            }

            ViewBag.TotalParkingMinutes = TotalParkingMinutes;

            double TotalParkingValue = 0;

            foreach (var vehicle in _vehicleRepository.GetAllEntities())
            {
                TotalParkingValue = Math.Round(TotalParkingValue + (DateTime.Now - vehicle.ParkingTime).TotalMinutes);
            }

            ViewBag.TotalParkingValue = TotalParkingValue * 60;

            return View();
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