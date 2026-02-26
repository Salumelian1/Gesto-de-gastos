💰 Gestor de Gastos
Aplicación web para gestionar ingresos y gastos personales, desarrollada con ASP.NET Core MVC y SQLite.
📸 Vista previa

Podés agregar capturas de pantalla aquí

🚀 Funcionalidades

✅ Registrar ingresos y gastos con descripción, monto, fecha y categoría
✅ Crear categorías personalizadas con color propio
✅ Dashboard con resumen de ingresos, gastos y saldo
✅ Gráfico de torta con gastos por categoría
✅ Gráfico de barras con top categorías con más gasto
✅ Filtros por día, semana, mes o rango personalizado de fechas
✅ Listado de movimientos con opción de editar y eliminar

🛠️ Tecnologías utilizadas
Tecnología                   Uso
ASP.NET Core MVC      Framework principal
C#                    Lenguaje de programación
SQLite                Base de datos
ADO.NET               Acceso a datos
Bootstrap 5           Estilos y diseño responsive
Chart.js              Gráficos interactivos


Gestor_de_gastos/
├── Controllers/
│   ├── HomeController.cs
│   ├── MovimientoController.cs
│   └── CategoriaController.cs
├── Models/
│   ├── Movimiento.cs
│   └── Categoria.cs
├── DTOs/
│   ├── ResumenDTO.cs
│   ├── GastosPorCategoriaDTO.cs
│   └── IngresoVsGastoDTO.cs
├── ViewModels/
│   ├── MovimientoViewModel.cs
│   └── CategoriaViewModel.cs
├── Repository/
│   ├── MovimientoRepositorio.cs
│   └── CategoriaRepositorio.cs
├── Interface/
│   ├── IMovimientoRepository.cs
│   └── ICategoriaRepository.cs
└── Views/
    ├── Home/
    ├── Movimiento/
    └── Categoria/
