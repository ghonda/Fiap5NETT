using Hackathon.AuthService.Data;
using Hackathon.AuthService.Models;
using Hackathon.AuthService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Hackathon.AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AppDbContext context) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        user.PasswordHash = Hash(user.PasswordHash);
        context.Users.Add(user);
        context.SaveChanges();
        return Ok("Usuário registrado");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] User login)
    {
        var user = context.Users.FirstOrDefault(u => u.Documento == login.Documento);
        if (user == null || user.PasswordHash != Hash(login.PasswordHash))
            return Unauthorized("Credenciais inválidas");

        var token = TokenService.GenerateToken(user.Tipo, user.Documento);
        return Ok(new { token });
    }

    [HttpGet("status")]
    public IActionResult Status()
    {
        return Ok("Auth Service is running");
    }

    private string Hash(string senha)
    {
        using var sha = SHA256.Create();
        return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(senha)));
    }
}