﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Models;
using System.Transactions;

namespace ERP_ZORZAL.Controllers
{
    public class DepartamentoController : Controller
    {
        private ERP_ZORZALEntities db = new ERP_ZORZALEntities();

        // GET: /Departamento/
        public ActionResult Index()
        {
            return View(db.tbDepartamento.ToList());
        }
        public ActionResult _IndexMunicipio()
        {
            return View();
        }
        public ActionResult _IndexMunicipio_Botones()
        {
            return View();
        }
        // GET: /Departamento/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDepartamento tbDepartamento = db.tbDepartamento.Find(id);
            if (tbDepartamento == null)
            {
                return HttpNotFound();
            }
            return View(tbDepartamento);
        }

        // GET: /Departamento/Create
        public ActionResult Create()
        {
            ViewBag.dep_Codigo = new SelectList(db.tbDepartamento, "dep_Codigo", "dep_Nombre", "Seleccione");
            ViewBag.dep_Codigo = new SelectList(db.tbMunicipio, "dep_Codigo", "dep_Nombre", "Seleccione");
            return View();
        }

        // POST: /Departamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="dep_Codigo,dep_Nombre,dep_UsuarioCrea,dep_FechaCrea,dep_UsuarioModifica,dep_FechaModifica")] tbDepartamento tbDepartamento)
        {
            IEnumerable<object> list = null;
            IEnumerable<object> lista = null;
            string MensajeError = "0";
            string MsjError = "0";
            var listMunicipios = (List<tbMunicipio>)Session["tbMunicipio"];
            if (ModelState.IsValid)
            {

                using (TransactionScope _Tran = new TransactionScope())
                {
                    try
                    {

                        list = db.UDP_Gral_tbDepartamento_Insert(tbDepartamento.dep_Codigo, tbDepartamento.dep_Nombre);
                        foreach (UDP_Gral_tbDepartamento_Insert_Result departamento in list)
                            MsjError = (departamento.MensajeError);
                        if (MsjError.Substring(0, 1) == "-")
                        {
                            ModelState.AddModelError("", "No se Guardo el Registro");
                            return View(tbDepartamento);
                        }
                        else
                        {
                            if (listMunicipios != null )
                            {
                                if (listMunicipios.Count > 0)
                                {
                                    foreach (tbMunicipio mun in listMunicipios)
                                    {
                                        lista = db.UDP_Gral_tbMunicipio_Insert(mun.mun_Codigo, tbDepartamento.dep_Codigo, mun.mun_Nombre);
                                        foreach (UDP_Gral_tbMunicipio_Insert_Result municipios in lista)
                                           
                                        {
                                            MensajeError = (municipios.MensajeError);
                                        }
                                    }
                                }
                            }

                            {
                                _Tran.Complete();
                               
                            }
                            
                        }

                    }
                    catch (Exception )
                    {
                        MsjError = "-1";
                    }
                }

                return RedirectToAction("Index");
            }
            
            return View(tbDepartamento);
        }

        // GET: /Departamento/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDepartamento tbDepartamento = db.tbDepartamento.Find(id);
            ViewBag.UsuarioCrea = db.tbUsuario.Find(tbDepartamento.dep_UsuarioCrea).usu_NombreUsuario;
            var UsuarioModfica = tbDepartamento.dep_UsuarioModifica;
            if (UsuarioModfica == null)
            {
                ViewBag.UsuarioModifica = "";
            }
            else
            {
                ViewBag.UsuarioModifica = db.tbUsuario.Find(UsuarioModfica).usu_NombreUsuario;
            };
            if (tbDepartamento == null)
            {
                return HttpNotFound();
            }
            return View(tbDepartamento);
        }

        // POST: /Departamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id,[Bind(Include="dep_Codigo,dep_Nombre,dep_UsuarioCrea,dep_FechaCrea,dep_UsuarioModifica,dep_FechaModifica")] tbDepartamento tbDepartamento)
        {
           
            if (ModelState.IsValid)
            {
                IEnumerable<object> list = null;
                string MsjError = "0";
                {
                    try
                    {

                        list = db.UDP_Gral_tbDepartamento_Update(tbDepartamento.dep_Codigo, tbDepartamento.dep_Nombre, tbDepartamento.dep_UsuarioCrea, tbDepartamento.dep_FechaCrea);
                        foreach (UDP_Gral_tbDepartamento_Update_Result departamento in list)
                            MsjError = (departamento.MensajeError);
                        if (MsjError.Substring(0, 1) == "-")
                        {
                            ModelState.AddModelError("", "No se Guardo el Registro");
                            return View(tbDepartamento);
                        }
                        else
                        {
                            return RedirectToAction("Edit");
                        }

                    }
                    catch (Exception)
                    {
                        MsjError = "-1";
                    }



                    return RedirectToAction("Index");
                }

            }

            return View(tbDepartamento);
        }

        // GET: /Departamento/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbDepartamento tbDepartamento = db.tbDepartamento.Find(id);
            if (tbDepartamento == null)
            {
                return HttpNotFound();
            }
            return View(tbDepartamento);
        }

        // POST: /Departamento/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            tbDepartamento tbDepartamento = db.tbDepartamento.Find(id);
            db.tbDepartamento.Remove(tbDepartamento);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        [HttpPost]
        public JsonResult AnadirMunicipio(tbMunicipio Municipio)
        {
            List<tbMunicipio> sessionMunicipio = new List<tbMunicipio>();
            var list = (List<tbMunicipio>)Session["tbMunicipio"];
            if (list == null)
            {
                sessionMunicipio.Add(Municipio);
                Session["tbMunicipio"] = sessionMunicipio;
            }
            else
            {
                list.Add(Municipio);
                Session["tbMunicipio"] = list;
            }
            return Json("Exito", JsonRequestBehavior.AllowGet);
        }
        

        [HttpPost]
        public JsonResult RemoveMunicipios(tbMunicipio Municipios)
        {
            var list = (List<tbMunicipio>)Session["tbMunicipio"];

            if (list != null)
            {
                var itemToRemove = list.Single(r => r.mun_UsuarioCrea == Municipios.mun_UsuarioCrea);
                list.Remove(itemToRemove);
                Session["tbMunicipio"] = list;
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ActualizarMunicipio(tbMunicipio ActualizarMun)

         { string MsjError = "";

           try
            {
                 IEnumerable<object> list = null;
                list = db.UDP_Gral_tbMunicipio_Update(ActualizarMun.mun_Codigo, ActualizarMun.dep_Codigo, ActualizarMun.mun_Nombre);
                foreach (UDP_Gral_tbMunicipio_Update_Result mun in list)
                    MsjError = (mun.MensajeError);
                if (MsjError.Substring(0, 1) == "-1")
                {
                            
                    ModelState.AddModelError("", "No se Guardo el Registro");
                    return Json(MsjError, JsonRequestBehavior.AllowGet);
                }
              else
                {
                    db.Entry(ActualizarMun).State = EntityState.Modified;
                    db.SaveChanges();
                    MsjError = "Exito";
                    return Json(MsjError, JsonRequestBehavior.AllowGet);
                    }

                }
              catch (Exception Ex)
               {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se Guardo el registro");
                }
            return Json(MsjError, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarMun(tbMunicipio GuardarMun)
        {
            { string MsjError = "";

                try
                {
                    IEnumerable<object> list = null;
                    list = db.UDP_Gral_tbMunicipio_Insert(GuardarMun.mun_Codigo, GuardarMun.dep_Codigo, GuardarMun.mun_Nombre);
                    foreach (UDP_Gral_tbMunicipio_Insert_Result mun in list)
                        MsjError = (mun.MensajeError);
                    if (MsjError.Substring(0, 1) == "-1")
                    {

                        ModelState.AddModelError("", "No se Guardo el Registro");
                        return Json(MsjError, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        db.Entry(GuardarMun).State = EntityState.Modified;
                        db.SaveChanges();
                        MsjError = "Exito";
                        return Json(MsjError, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception Ex)
                {
                    Ex.Message.ToString();
                    ModelState.AddModelError("", "No se Guardo el registro");
                }
                return Json(MsjError, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult EliminarMunicipio(string mun_Codigo)
        {
            var Msj = "";
            try
            {
                if (mun_Codigo != null)
                {
                    db.UDP_Gral_tbMunicipio_Delete(mun_Codigo);
                    Msj = "1";
                }
            }
            catch (Exception)
            {
                Msj = "-1";
            }
            return Json(Msj, JsonRequestBehavior.AllowGet);
        }
    }
}
