using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClientesAPI.Data;
using ClientesAPI.Models;
using ClientesAPI.DTOs;

namespace ClientesAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(ApplicationDbContext context, ILogger<ClientesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cliente>>> GetClientes()
        {
            try
            {
                var clientes = await _context.Clientes.ToListAsync();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Cliente>> GetCliente(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return NotFound(new { error = "Cliente no encontrado" });
                }

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cliente con ID {Id}", id);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<Cliente>> PostCliente(ClienteCreateDto clienteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var clienteExistente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.CorreoElectronico == clienteDto.CorreoElectronico);

                if (clienteExistente != null)
                {
                    return Conflict(new { error = "Ya existe un cliente con ese correo electronico" });
                }

                var cliente = new Cliente
                {
                    Nombre = clienteDto.Nombre,
                    CorreoElectronico = clienteDto.CorreoElectronico,
                    Telefono = clienteDto.Telefono
                };

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCliente), new { id = cliente.Id }, cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, ClienteUpdateDto clienteDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return NotFound(new { error = "Cliente no encontrado" });
                }

                var clienteConMismoCorreo = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.CorreoElectronico == clienteDto.CorreoElectronico && c.Id != id);

                if (clienteConMismoCorreo != null)
                {
                    return Conflict(new { error = "Ya existe otro cliente con ese correo electronico" });
                }

                cliente.Nombre = clienteDto.Nombre;
                cliente.CorreoElectronico = clienteDto.CorreoElectronico;
                cliente.Telefono = clienteDto.Telefono;

                await _context.SaveChangesAsync();

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar cliente con ID {Id}", id);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchCliente(int id, ClienteUpdateDto clienteDto)
        {
            return await PutCliente(id, clienteDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);

                if (cliente == null)
                {
                    return NotFound(new { error = "Cliente no encontrado" });
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cliente con ID {Id}", id);
                return StatusCode(500, new { error = "Error interno del servidor" });
            }
        }
    }
}