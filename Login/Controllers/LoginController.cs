using Microsoft.AspNetCore.Mvc;
using Login.Models;
using System.Text;
using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;

namespace Login.Controllers
{
    public class LoginController : Controller
    {
        static string cadena = "Data Source=DESKTOP-0N795D9\\SQLEXPRESS;Initial Catalog=InicioDB;Integrated Security=true";

        //GET
        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IniciarSesion(Usuario oUsuario)
        {
            if (oUsuario.passUsuario != null && oUsuario.userUsuario != null) { 
            oUsuario.passUsuario = CodificarSha256(oUsuario.passUsuario);
            
                using (SqlConnection cn = new SqlConnection(cadena))
                {
                    SqlCommand cmd = new SqlCommand("validarUsuario", cn);
                    cmd.Parameters.AddWithValue("Usuario", oUsuario.userUsuario);
                    cmd.Parameters.AddWithValue("Pass",oUsuario.passUsuario);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    oUsuario.idUsuario = Convert.ToInt32(cmd.ExecuteScalar().ToString());
                }
            }
            if (oUsuario.idUsuario != 0)
            {
                HttpContext.Session.SetString("Usuario",oUsuario.ToString());
                
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario o Contraseña Erróneo";
                

                return View();
            }
           
            
        }

        public static string CodificarSha256(string texto)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {             
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach(byte b in result)
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }
    
    }
}
