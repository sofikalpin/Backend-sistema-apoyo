using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaApoyo.BLL.Servicios.Contrato;
using SistemaApoyo.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SistemaApoyo.API.Utilidad;

[Route("API/[controller]")]
[ApiController]
public class RolController : ControllerBase
{
    private readonly IRolService _rolService;
    private readonly ILogger<RolController> _logger;

    public RolController(IRolService rolService, ILogger<RolController> logger)
    {
        _rolService = rolService;
        _logger = logger;
    }

    [HttpGet]
    [Route("Lista Roles")]

    public async Task<IActionResult> Lista()
    {
        var rsp = new Response<List<RolDTO>>();
        try
        {
            var roles = await _rolService.Lista();
            rsp.status = true;
            rsp.msg = "Lista de roles obtenida.";
            rsp.value = roles;
        }
        catch (Exception ex)
        {
            rsp.status = false;
            _logger.LogError(ex, "Error al obtener la lista de roles.");

        }
        return Ok(rsp);
    }
}