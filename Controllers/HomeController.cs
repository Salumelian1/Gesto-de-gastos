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

    public IActionResult Index(DateOnly? desde, DateOnly? hasta, string filtro = "mes")
    {
        try
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            if(desde == null || hasta == null)
            {
                switch (filtro)
                {
                    case "hoy":
                        desde = hoy;
                        hasta = hoy;
                        break;
                    case "semana":
                        desde = hoy.AddDays(-7);
                        hasta = hoy;
                        break;
                    case "año":
                        desde = new DateOnly(hoy.Year,1,1);
                        hasta = hoy;
                        break;
                    case "mes":
                    default:
                        desde = new DateOnly(hoy.Year,hoy.Month,1);
                        hasta = hoy;
                        break;
                }
            }
            var resumen = _movimientoRepository.GetResumen(desde.Value,hasta.Value);
            resumen.Movimientos = _movimientoRepository.GetAll(desde.Value,hasta.Value);
            return View(resumen);
        }
        catch
        {
            return View(new ResumenDTO());
        }
    }

    public JsonResult GetGastosPorCategoria(DateOnly? desde, DateOnly? hasta)
    {
        try
        { 
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            desde ??= new DateOnly(hoy.Year,hoy.Month,1);
            hasta??= hoy;
            return Json(_movimientoRepository.GetGastosPorCategoria(desde.Value,hasta.Value)); 
        }
        catch { return Json(new List<object>()); }
    }

    public JsonResult GetTopCategorias(DateOnly? desde, DateOnly? hasta)
    {
        try
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            desde ??= new DateOnly(hoy.Year,hoy.Month,1);
            hasta??= hoy;
             return Json(_movimientoRepository.GetTopCategorias(desde.Value,hasta.Value)); 
        }
        catch { return Json(new List<object>()); }
    }

    public IActionResult Error()
    {
        return View();
    }
}