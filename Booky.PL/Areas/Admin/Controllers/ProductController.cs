using AutoMapper;
using Booky.ADL.Models;
using Booky.BL.Interface;
using Booky.PL.Helper;
using Booky.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Booky.PL.Controllers; 

[Area("Admin")]
public class ProductController : Controller
{ 
  private readonly IunitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public ProductController( IunitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public IActionResult Index()
    {
        var products = _unitOfWork.ProductRepository.GetAll();
        // this line under it will convert IEnumerable<Product> to IEnumerable<ProductViewModel> with mapper to can deal with 
        // ProductViewModel instead of Product and then pass it to view 
        var mapperProducts = _mapper.Map<IEnumerable<Product>,IEnumerable<ProductViewModel>>(products);
        if (mapperProducts is null)
        {
            return BadRequest();
        }

        return View(mapperProducts);

    }

    [HttpGet]
    public IActionResult Create()
    {
        // instead of pass category id and you dont save id of category we will select category name by make 
        IEnumerable<SelectListItem> catSelectListItems =
            _unitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
        ViewBag.Category = catSelectListItems;return View();
    }

    [HttpPost]
    public IActionResult Create(ProductViewModel ProductVM)
    {
        if (ModelState.IsValid)
        {
          ProductVM.ImageUrl = FileManagement.Upload(ProductVM.Image,"images");
          var mapperProduct = _mapper.Map<ProductViewModel,Product>(ProductVM);
            _unitOfWork.ProductRepository.Add(mapperProduct);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        return View(ProductVM);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        if (id == 0 || id == null)
        {
            return BadRequest();
        }

        var product = _unitOfWork.ProductRepository.GetById(id);
        var mapperProduct = _mapper.Map<Product,ProductViewModel>(product);
        if (mapperProduct is null)
        {
            return NotFound();

        }

        return View(mapperProduct);
    }


    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteProduct(int id)
    {
        Product product = _unitOfWork.ProductRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.Save();
        return RedirectToAction("Index");


    }

    [HttpGet]

    public IActionResult Edit(int id)
    {
        if (id == 0 || id == null)
        {
            return BadRequest();
        }
        var product = _unitOfWork.ProductRepository.GetById(id);
        var mapperProduct = _mapper.Map<Product,ProductViewModel>(product);
        if (mapperProduct is null)
        {
            return NotFound();
        }
        IEnumerable<SelectListItem> catSelectListItems =
            _unitOfWork.CategoryRepository.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });

        ViewBag.Category = catSelectListItems;
        return View(mapperProduct);
        
    }

    [HttpPost]
    public IActionResult Edit(ProductViewModel productVM)
    {
        if (ModelState.IsValid)
        {
            var mapperProduct = _mapper.Map<ProductViewModel,Product>(productVM);
            _unitOfWork.ProductRepository.Update(mapperProduct);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        return View(productVM);
        
    }

}