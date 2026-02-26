using Gestor_de_gastos.Interface;
using Gestor_de_gastos.Models;
using Gestor_de_gastos.Repository;
using Gestor_de_gastos.viewModel;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_de_gastos.Controllers;

public class MovimientoController : Controller
{
    private readonly IMovimientoRepository _movimientoRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    public MovimientoController(IMovimientoRepository movimientoRepository, ICategoriaRepository categoriaRepository)
    {
        _movimientoRepository = movimientoRepository;
        _categoriaRepository  = categoriaRepository;
    }

    public IActionResult Index()
    {
        try
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            List<Movimiento> movimientos = _movimientoRepository.GetAll(new DateOnly(2000, 1, 1), hoy);
            return View(movimientos);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public IActionResult Create()
    {
        try
        {
            var viewModel = new MovimientoViewModel
            {
                Categorias = _categoriaRepository.GetAll()
            };
            return View(viewModel);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public IActionResult Create(MovimientoViewModel movimientoViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return View(movimientoViewModel);
            }

            var nuevoMovimiento = new Movimiento
            {
                Descripcion = movimientoViewModel.Descripcion,
                Monto       = movimientoViewModel.Monto,
                Fecha       = movimientoViewModel.Fecha,
                Tipo        = movimientoViewModel.Tipo,
                Comentario  = movimientoViewModel.Comentario,
                CategoriaId = movimientoViewModel.CategoriaId
            };

            _movimientoRepository.Create(nuevoMovimiento);
            return RedirectToAction("Index","Home");
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        try
        {
            var movimiento = _movimientoRepository.GetById(id);

            var movimientoAEditar = new MovimientoViewModel
            {
                Id          = movimiento.Id,
                Descripcion = movimiento.Descripcion,
                Monto       = movimiento.Monto,
                Fecha       = movimiento.Fecha,
                Tipo        = movimiento.Tipo,
                Comentario  = movimiento.Comentario,
                CategoriaId = movimiento.CategoriaId,
                Categorias  = _categoriaRepository.GetAll()
            };

            return View(movimientoAEditar);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public IActionResult Update(MovimientoViewModel movimientoViewModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                movimientoViewModel.Categorias = _categoriaRepository.GetAll();
                return View(movimientoViewModel);
            }

            var movimientoAEditar = new Movimiento
            {
                Id          = movimientoViewModel.Id,
                Descripcion = movimientoViewModel.Descripcion,
                Monto       = movimientoViewModel.Monto,
                Fecha       = movimientoViewModel.Fecha,
                Tipo        = movimientoViewModel.Tipo,
                Comentario  = movimientoViewModel.Comentario,
                CategoriaId = movimientoViewModel.CategoriaId
            };

            _movimientoRepository.Update(movimientoAEditar.Id, movimientoAEditar);
            return RedirectToAction("Index","Home");
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        try
        {
            Movimiento movABorrar = _movimientoRepository.GetById(id);
            return View(movABorrar);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    [HttpPost]
    public IActionResult DeleteConfirmed(int id)
    {
        try
        {
            _movimientoRepository.Delete(id);
            return RedirectToAction("Index","Home");
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }
}