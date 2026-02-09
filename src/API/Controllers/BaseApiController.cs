using Microsoft.AspNetCore.Mvc;

namespace JuliGastos.API.Controllers;

/// <summary>
/// Controlador base para todos los controllers de la API
/// Equivalente a una clase base con @RestController en Spring
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
}
