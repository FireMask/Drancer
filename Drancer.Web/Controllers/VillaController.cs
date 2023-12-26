using Drancer.Domain.Entities;
using Drancer.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata.Ecma335;

namespace Drancer.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VillaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var villas = _context.Villas.ToList();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa villa)
        {
            if (villa.Name == villa.Description)
                ModelState.AddModelError("Name", "The description cannot exactly match the Name");

            if(!ModelState.IsValid)
                return View();

            _context.Villas.Add(villa);
            _context.SaveChanges();

            TempData["success"] = "The villa has been created succesfully";

            return RedirectToAction("Index", "Villa");
        }

        public IActionResult Update(int villaId)
        {
            Villa? villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
            if (villa is null)
                return RedirectToAction("Error", "Home");

            return View(villa);
        }

        [HttpPost]
        public IActionResult Update(Villa villa)
        {
            if (!ModelState.IsValid && villa.Id <= 0)
            {
                TempData["error"] = "The villa could not be updated";
                return View();
            }

            _context.Villas.Update(villa);
            _context.SaveChanges();

            TempData["success"] = "The villa has been updated succesfully";

            return RedirectToAction("Index", "Villa");
        }

        public IActionResult Delete(int villaId)
        {
            Villa? villa = _context.Villas.FirstOrDefault(x => x.Id == villaId);
            if (villa is null)
                return RedirectToAction("Error", "Home");

            return View(villa);
        }

        [HttpPost]
        public IActionResult Delete(Villa villa)
        {
            Villa? objFromDb = _context.Villas.FirstOrDefault(x => x.Id == villa.Id);

            if (objFromDb is null)
            {
                TempData["error"] = "The villa could not be deleted";
                return View();
            }

            _context.Villas.Remove(objFromDb);
            _context.SaveChanges();

            TempData["success"] = "The villa has been deleted succesfully";

            return RedirectToAction("Index", "Villa");
        }
    }
}
