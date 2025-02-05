using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecetasAPI.Models
{
    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool EstaActivo { get; set; } = true;
        public string Password { get; set; } = "";

        public Usuario() {}

        public Usuario(string nombre, string apellido, string email, string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                throw new ArgumentException("La contraseña debe tener al menos 6 caracteres");
            }

            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Password = password;
        }

        public void MostrarDetalles()
        {
            Console.WriteLine($"Usuario: {Nombre} {Apellido}, Email: {Email}, Registrado el {FechaRegistro}, Activo: {EstaActivo}");
        }
    }
}