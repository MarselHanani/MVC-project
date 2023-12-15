using Booky.ADL.Models;
using Booky.BL.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Booky.PL.Controllers
{
[Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IunitOfWork _UnitOfWork;

        public CategoryController(IunitOfWork unitOfWork)
        {
            _UnitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var categories = _UnitOfWork.CategoryRepository.GetAll();
            if (categories is null)
                return BadRequest();
            return View(categories);
        }




        //every action should has to action from same name one of them to get the view and other to post the data
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category modle)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CategoryRepository.Add(modle);
                //=========================================================================================
                _UnitOfWork.Save();
                // so now after in HttpPost action i will add function save to save data 
                //=========================================================================================
                TempData["success"] = "Category added successfully";
                return RedirectToAction("Index");
            }

            return View(modle);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id == 0 || id == null)
            {
                return BadRequest();
            }

            Category category = _UnitOfWork.CategoryRepository.GetById(id);
            if (category is null)
            {
                return NotFound();

            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")] // ActionName => because i called my action DeletePost and when the link inside index 
        // will check about Delete will found it 
        public IActionResult DeletePost(int id)
        {
            Category category = _UnitOfWork.CategoryRepository.GetById(id);
            if (category is null)
            {
                return NotFound();
            }

            _UnitOfWork.CategoryRepository.Delete(category);
            _UnitOfWork.Save();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id == 0 || id == null)
            {
                return BadRequest();
            }

            Category category = _UnitOfWork.CategoryRepository.GetById(id);
            if (category is null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _UnitOfWork.CategoryRepository.Update(category);
                _UnitOfWork.Save();
                TempData["success"] =
                    "Category updated successfully"; // this is a temp data for flash message => name pull request mean will not 
                // show just if updated success 
                return RedirectToAction("Index");

            }

            return View(category);
        }
    }
}