using Microsoft.AspNetCore.Mvc;
using RTCodingExercise.Microservices.Services;
using RTCodingExercise.Microservices.Models;

namespace RTCodingExercise.Microservices.Controllers
{
    public class PlatesController : Controller
    {
        private readonly IPlateService _plateService;

        public PlatesController(IPlateService plateService)
        {
            _plateService = plateService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 20)
        {
            var plates = await _plateService.GetPagedPlatesAsync(page, pageSize);
            return View(plates);
        }

        [HttpPost]
        public async Task<IActionResult> AddPlate(Plate plate)
        {
            if (ModelState.IsValid)
            {
                await _plateService.AddPlateAsync(plate);
                return RedirectToAction("Index");
            }
            return View(plate);
        }
    }
}
