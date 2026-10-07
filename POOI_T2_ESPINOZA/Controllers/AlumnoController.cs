using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using POOI_T2_ESPINOZA.Models;

namespace POOI_T2_ESPINOZA.Controllers
{
    public class AlumnoController : Controller
    {
        private List<Alumno> Cargar()
        {
            string ruta = Server.MapPath("~/App_Data/Alumnos.json");

            if (!System.IO.File.Exists(ruta))
            {
                System.IO.File.WriteAllText(ruta, "[]");
            }

            string json = System.IO.File.ReadAllText(ruta);

            return JsonConvert.DeserializeObject<List<Alumno>>(json);
        }
        private List<Alumno> Deserializar()
        {
            return Cargar();
        }
        public ActionResult Serializar()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Cargar(), Formatting.Indented);

                string ruta = Server.MapPath("~/App_Data/Alumnos.json");

                System.IO.File.WriteAllText(ruta, json);

                TempData["Mensaje"] = "Colección guardada en alumnos.json";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Alumno
        public ActionResult Index()
        {
            List<Alumno> temporal = Deserializar();

            ViewBag.Mensaje = TempData["Mensaje"];

            return View(temporal);
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(Alumno alumno)
        {
            List<Alumno> temporal = Deserializar();

            if (temporal.Any(a => a.dni == alumno.dni))
            {
                ViewBag.Mensaje = "El DNI ya existe";
                return View(alumno);
            }

            temporal.Add(alumno);

           
            string json = JsonConvert.SerializeObject(temporal, Formatting.Indented);
            string ruta = Server.MapPath("~/App_Data/Alumnos.json");
            System.IO.File.WriteAllText(ruta, json);

            TempData["Mensaje"] = "Alumno registrado correctamente";

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(string dni)
        {
            List<Alumno> temporal = Deserializar();

            int index = temporal.FindIndex(a => a.dni == dni);

            if (index >= 0)
            {
                temporal.RemoveAt(index);

                string json = JsonConvert.SerializeObject(temporal, Formatting.Indented);
                string ruta = Server.MapPath("~/App_Data/Alumnos.json");
                System.IO.File.WriteAllText(ruta, json);

                TempData["Mensaje"] = "Alumno eliminado correctamente";
            }
            else
            {
                TempData["Mensaje"] = "No se encontró el alumno";
            }

            return RedirectToAction("Index");
        }

        public ActionResult Detalles(string dni)
        {
            List<Alumno> temporal = Deserializar();

            Alumno alumno = temporal.Find(a => a.dni == dni);

            return View(alumno);
        }

        public ActionResult Actualizar(string dni)
        {
            List<Alumno> temporal = Deserializar();

            Alumno alumno = temporal.Find(a => a.dni == dni);

            return View(alumno);
        }

        [HttpPost]
        public ActionResult Actualizar(Alumno alumno)
        {
            List<Alumno> temporal = Deserializar();

            int index = temporal.FindIndex(a => a.dni == alumno.dni);

            if (index >= 0)
            {
                temporal[index] = alumno;

                string json = JsonConvert.SerializeObject(temporal, Formatting.Indented);
                string ruta = Server.MapPath("~/App_Data/Alumnos.json");
                System.IO.File.WriteAllText(ruta, json);

                TempData["Mensaje"] = "Alumno actualizado correctamente";

                return RedirectToAction("Index");
            }

            ViewBag.Mensaje = "El DNI no existe";

            return View(alumno);
        }
    }
}