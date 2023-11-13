using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Productos.Models;

using System.Data.SqlClient;
using System.Data;
using System.Web.Services.Description;

namespace Productos.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=DESKTOP-026EOC4\\NOSEE;Initial Catalog=petopia;Integrated Security=true";


        // GET: Acceso
        public ActionResult Login()
        {
            return View();
        }


        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrarp(Usuario oUsuario)
        {
            bool registrado;
            string mensaje;

            if(oUsuario.password == oUsuario.cpassword)
            {
                oUsuario.password = ConvertirSha256(oUsuario.password);
            }
            else
            {
                ViewData["Mensaje"] = "Las contraseñas no coinciden";
                return RedirectToAction("Contact", "Home");
            }

            using (SqlConnection cn = new SqlConnection(cadena) )
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario1", cn);
                cmd.Parameters.AddWithValue("username", oUsuario.username);
                cmd.Parameters.AddWithValue("email", oUsuario.email);
                cmd.Parameters.AddWithValue("password", oUsuario.password);
                cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("Mensaje", SqlDbType.VarChar,100).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                mensaje = cmd.Parameters["Mensaje"].Value.ToString();
            }

            ViewData["Mensaje"] = mensaje;

            if (registrado)
            {
                return RedirectToAction("Login", "Acceso");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult Loginp(Usuario oUsuario)
        {
            oUsuario.password = ConvertirSha256(oUsuario.password);

            ViewData["Mensaje"] = oUsuario.password;

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("username", oUsuario.username);
                cmd.Parameters.AddWithValue("password", oUsuario.password);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                oUsuario.id = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if(oUsuario.id != 0)
            {
                Session["usuario"] = oUsuario;
                return RedirectToAction("Contact", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";

                return RedirectToAction("Login", "Acceso");
            }

        }


        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }
            return Sb.ToString();

        }
    }
}