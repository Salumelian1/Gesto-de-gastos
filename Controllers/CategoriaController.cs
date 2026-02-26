using Gestor_de_gastos.Interface;
using Gestor_de_gastos.Models;
using Gestor_de_gastos.Repository;
using Gestor_de_gastos.viewModel;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_de_gastos.Controllers;

public class CategoriaController : Controller
{

    private readonly ICategoriaRepository _categoriaRepository;
    public CategoriaController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }
    public IActionResult Index()
    {
        try
        {
            List<Categoria> categorias = _categoriaRepository.GetAll();
            return View(categorias);
        }catch
        {
            return RedirectToAction("Error","Home");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        try
        {
            return View();
        }catch
        {
            return RedirectToAction("Error","Home");
        }
    }

    [HttpPost]

    public IActionResult Create(CategoriaViewModel categoriaViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaViewModel);
            }
            var nuevaCategoria = new Categoria
            {
                Nombre = categoriaViewModel.Nombre,
                Color = categoriaViewModel.Color
            };
            _categoriaRepository.Create(nuevaCategoria);
            return RedirectToAction("Index","Home");
        }
        catch
        {
            return RedirectToAction("Error","Home");
        }
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        try
        {
            var categoria = _categoriaRepository.GetById(id);
            var categoriaAEditar = new CategoriaViewModel
            {
                Id = categoria.Id,
                Nombre = categoria.Nombre,
                Color = categoria.Color
            };
            return View(categoriaAEditar);
        }
        catch
        {
            return RedirectToAction("Error","Home");
        }
    }

    [HttpPost]
    public IActionResult Update(CategoriaViewModel categoriaViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(categoriaViewModel);
            }
            var categoriaAEditar = new Categoria
            {
                Id = categoriaViewModel.Id,
                Nombre = categoriaViewModel.Nombre,
                Color = categoriaViewModel.Color
            };
            _categoriaRepository.Update(categoriaAEditar.Id,categoriaAEditar);
            return RedirectToAction("Index");
        }
        catch
        {
            return RedirectToAction("Error","Home");
        }
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        try
        {
            Categoria catABorrar = _categoriaRepository.GetById(id);
            return View(catABorrar);
        }
        catch
        {
            return RedirectToAction("Error","Home");
        }
        
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _categoriaRepository.Delete(id);
            return RedirectToAction("Index");
        }
        catch
        {
            return RedirectToAction("Error","Home");
        }
        
    }
}