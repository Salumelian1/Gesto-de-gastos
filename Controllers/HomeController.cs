using Gestor_de_gastos.Interface;
using Microsoft.AspNetCore.Mvc;
using Gestor_de_gastos.DTOs;

public class HomeController : Controller
{
    private readonly IMovimientoRepository _movimientoRepository;

    public HomeController(IMovimientoRepository movimientoRepositorio)
    {
        _movimientoRepository = movimientoRepositorio;
    }

    public IActionResult Index()
    {
        try
        {
            var resumen = _movimientoRepository.GetResumen();
            resumen.Movimientos = _movimientoRepository.GetAll();
            return View(resumen);
        }
        catch
        {
            return View(new ResumenDTO());
        }
    }

    public JsonResult GetGastosPorCategoria()
    {
        try { return Json(_movimientoRepository.GetGastosPorCategoria()); }
        catch { return Json(new List<object>()); }
    }

    public JsonResult GetTopCategorias()
    {
        try { return Json(_movimientoRepository.GetTopCategorias()); }
        catch { return Json(new List<object>()); }
    }

    public IActionResult Error()
    {
        return View();
    }
}