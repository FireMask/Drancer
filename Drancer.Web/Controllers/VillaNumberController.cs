using Drancer.Domain.Entities;
using Drancer.Infrastructure.Data;
using Drancer.Infrastructure.Migrations;
using Drancer.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata.Ecma335;

namespace Drancer.Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaNumberController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var villaNumbers = _context.VillaNumbers
                .Include(x => x.Villa)
                .ToList();
            return View(villaNumbers);
        }

        public IActionResult Create()
        {
            VillaNumberVM villaNumberVM = new VillaNumberVM
            {
                VillaList = _context.Villas.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Create(VillaNumberVM vm)
        {
            if(vm.VillaNumber is null)
                return View(vm);

            bool villaNumberExists = _context.VillaNumbers.Any(x => x.Villa_Number == vm.VillaNumber.Villa_Number);

            if (ModelState.IsValid && !villaNumberExists)
            {
                _context.VillaNumbers.Add(vm.VillaNumber);
                _context.SaveChanges();

                TempData["success"] = "The villa Number has been created succesfully";
                return RedirectToAction(nameof(Index));
            }

            if (villaNumberExists)
            {
                TempData["error"] = "The villa number already exists, try another one";
            }

            vm.VillaList = _context.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(vm);

        }

        public IActionResult Update(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _context.Villas.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
                return RedirectToAction("Error", "Home");

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Update(VillaNumberVM villaNumberVM)
        {
            if (ModelState.IsValid && villaNumberVM.VillaNumber is not null)
            {
                _context.VillaNumbers.Update(villaNumberVM.VillaNumber);
                _context.SaveChanges();

                TempData["success"] = "The villa Number has been updated succesfully";
                return RedirectToAction(nameof(Index));
            }

            villaNumberVM.VillaList = _context.Villas.ToList().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

            return View(villaNumberVM);
        }

        public IActionResult Delete(int villaNumberId)
        {
            VillaNumberVM villaNumberVM = new()
            {
                VillaList = _context.Villas.ToList().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }),
                VillaNumber = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == villaNumberId)
            };

            if (villaNumberVM.VillaNumber is null)
                return RedirectToAction("Error", "Home");

            return View(villaNumberVM);
        }

        [HttpPost]
        public IActionResult Delete(VillaNumberVM vm)
        {
            if(vm.VillaNumber is null)
            {
                TempData["error"] = "There was an error trying to delete";
                return View(vm);
            }

            VillaNumber? objFromDb = _context.VillaNumbers.FirstOrDefault(x => x.Villa_Number == vm.VillaNumber.Villa_Number);

            if (objFromDb is not null)
            {
                _context.VillaNumbers.Remove(objFromDb);
                _context.SaveChanges();
                TempData["success"] = "The villa has been deleted succesfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["error"] = "The villa number could not be deleted";
            return View();
        }
    }
}
