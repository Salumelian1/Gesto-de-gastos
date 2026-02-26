using Gestor_de_gastos.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Gestor_de_gastos.Controllers;


public class DashBoardController : Controller
{
    private readonly IMovimientoRepository _movimientoRepository;
    public DashBoardController(IMovimientoRepository movimientoRepository)
    {
        _movimientoRepository = movimientoRepository;
    }
    public IActionResult Index()
    {
        try
        {
            var resumen = _movimientoRepository.GetResumen();
            return View(resumen);
        }
        catch
        {
            return RedirectToAction("Error", "Home");
        }
    }

    public JsonResult GetGastosPorCategoria()
    {
        try
        {
            var datos = _movimientoRepository.GetGastosPorCategoria();
            return Json(datos);
        }
        catch
        {
            return Json(new List<object>());
        }
    }
    // Endpoint JSON para gráfico de top categorias
    public JsonResult GetTopCategorias()
    {
        try
        {
            var datos = _movimientoRepository.GetTopCategorias();
            return Json(datos);
        }
        catch
        {
            return Json(new List<object>());
        }
    }
}