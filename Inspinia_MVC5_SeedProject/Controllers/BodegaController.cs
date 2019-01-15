﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using ERP_GMEDINA.Models;

namespace ERP_GMEDINA.Controllers
{
    public class BodegaController : Controller
    {
        private ERP_ZORZALEntities db = new ERP_ZORZALEntities();

        // GET: /Bodega/
        public ActionResult Index()
        {
            var tbbodega = db.tbBodega.Include(t => t.tbUsuario).Include(t => t.tbUsuario1).Include(t => t.tbMunicipio);
            this.AllLists();
            return View(tbbodega.ToList());
        }

        // GET: /Bodega/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbBodega tbBodega = db.tbBodega.Find(id);
            
            if (tbBodega == null)
            {
                return HttpNotFound();
            }

            this.AllLists();
            return View(tbBodega);
        }
        
        public ActionResult _DetallesProductos(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbProducto tbProducto = db.tbProducto.Find(id);
            if (tbProducto == null)
            {
                return HttpNotFound();
            }
            return View(tbProducto);
        }
        
        private void AllLists()
        {
            ViewBag.Producto = db.tbProducto.ToList();
            ViewBag.Depto = db.tbDepartamento.ToList();
            ViewBag.Muni = db.tbMunicipio.ToList();
            var _departamentos = db.tbDepartamento.Select(s => new
            {
                dep_Codigo = s.dep_Codigo,
                dep_Nombre = string.Concat(s.dep_Codigo + " - " + s.dep_Nombre)
            }).ToList();

            var _Municipios = db.tbMunicipio.Select(s => new
            {
                mun_Codigo = s.mun_Codigo,
                mun_Nombre = string.Concat(s.mun_Codigo + " - " + s.mun_Nombre)
            }).ToList();

            var _EncargadoBodega = db.tbEmpleado.Select(s => new
            {
                emp_Nombres = s.emp_Nombres,
                emp_Apellidos = string.Concat(s.emp_Nombres + " " + s.emp_Apellidos)
            }).ToList();

            ViewBag.DepartamentoList = new SelectList(_departamentos, "dep_Codigo", "dep_Nombre", "Seleccione");
            ViewBag.MunicipioList = new SelectList(_Municipios, "mun_Codigo", "mun_Nombre" , "Seleccione");
            ViewBag.ResponsableBodegaList = new SelectList(db.tbEmpleado, "emp_Id", "emp_Nombres", "emp_Apellidos"/*, "Seleccione"*/);
            //ViewBag.ResponsableBodegaList = new SelectList(_EncargadoBodega, "emp_Nombres", "emp_Apellidos"/*, "emp_Apellidos"*//*, "Seleccione"*/);
            ////
            ///
        }

        [HttpPost]
        public JsonResult GetMunicipios(string CodDepartamento)
        {
            var list = db.spGetMunicipios(CodDepartamento).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDepartamento(string CodMunicipio)
        {
            var list = db.spGetDepartamento(CodMunicipio).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult removeBodegaDetalle(tbBodegaDetalle bodedaDetalle)
        {
            var list = (List<tbBodegaDetalle>)Session["tbBodegaDetalles"];

            if (list != null)
            {
                var itemToRemove = list.Single(r => r.bodd_Id == bodedaDetalle.bodd_Id);
                list.Remove(itemToRemove);
                Session["tbBodegaDetalless"] = list;
            }
            return Json("", JsonRequestBehavior.AllowGet);

        }
        
        // GET: /Bodega/Create
        public ActionResult Create()
        {
            this.AllLists();
            Session["tbBodegaDetalle"] = null;
            return View();
        }

        // POST: /Bodega/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="bod_Id,bod_Nombre,bod_ResponsableBodega,bod_Direccion,bod_Correo,bod_Telefono,usu_Id,mun_Codigo,bod_EsActiva")] tbBodega tbBodega)
        {
            IEnumerable<object> BODEGA = null;
            IEnumerable<object> DETALLE = null;
            var idMaster = 0;
            var MsjError = "";
            var listaDetalle = (List<tbBodegaDetalle>)Session["tbBodegaDetalle"];
            if (ModelState.IsValid)
            {

                using (TransactionScope _Tran = new TransactionScope())
                {
                    try
                    {

                        BODEGA = db.UDP_Inv_tbBodega_Insert(tbBodega.bod_Nombre,
                                                         tbBodega.bod_ResponsableBodega
                                                        , tbBodega.bod_Direccion
                                                        , tbBodega.bod_Correo
                                                        , tbBodega.bod_Telefono
                                                        , tbBodega.mun_Codigo);
                        foreach (UDP_Inv_tbBodega_Insert_Result bodega in BODEGA)
                            idMaster = Convert.ToInt32(bodega.MensajeError);
                        if (MsjError == "-")
                        {
                            ModelState.AddModelError("", "No se Guardo el Registro");
                            return View(tbBodega);
                        }
                        else
                        {
                            if (listaDetalle != null)
                            {
                                if (listaDetalle.Count > 0)
                                {
                                    foreach (tbBodegaDetalle bodd in listaDetalle)
                                    {
                                        DETALLE = db.UDP_Inv_tbBodegaDetalle_Insert(bodd.prod_Codigo
                                                                                    , idMaster
                                                                                    , bodd.bodd_CantidadMinima
                                                                                    , bodd.bodd_CantidadMaxima
                                                                                    , bodd.bodd_PuntoReorden
                                                                                    , bodd.bodd_Costo
                                                                                    , bodd.bodd_CostoPromedio);
                                        foreach (UDP_Inv_tbBodegaDetalle_Insert_Result B_detalle in DETALLE)
                                            MsjError = B_detalle.MensajeError;
                                        if (MsjError == "-1")
                                            {
                                            ModelState.AddModelError("", "No se Guardo el Registro");
                                                return View(tbBodega);
                                            }
                                        else
                                        {
                                            _Tran.Complete();
                                            return RedirectToAction("Index");
                                        }
                                    }
                                }
                            }

                            else
                            {
                                _Tran.Complete();
                                //return RedirectToAction("Index");
                            }

                        }

                    }
                    catch (Exception Ex)
                    {
                        Ex.Message.ToString();
                        //ModelState.AddModelError("", "No se Guardo el Registro");
                        //return View(tbBodega);
                        MsjError = "-1"; 
                    }
                }
                return RedirectToAction("Index");
            }
            this.AllLists();
            return View(tbBodega);
        }
        
        // GET: /Bodega/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbBodega tbBodega = db.tbBodega.Find(id);
            ViewBag.IdBodegaDetalleEdit = id;
            if (tbBodega == null)
            {
                return HttpNotFound();
            }
            this.AllLists();
            ViewBag.dep_Codigo = new SelectList(db.tbDepartamento, "dep_Codigo", "dep_Nombre", tbBodega.dep_Codigo);
            ViewBag.mun_Codigo = new SelectList(db.tbMunicipio, "mun_Codigo", "mun_Nombre", tbBodega.mun_Codigo);
            Session["tbBodegaDetalle"] = null;
            return View(tbBodega);
        }
       
        //[HttpPost]
        //public JsonResult Getbodegadetalle()
        //{
        //    var list = (List<tbBodegaDetalle>)Session["tbBodegaDetalle"];
        //    return Json(list, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult SaveBodegaDetalle(tbBodegaDetalle BODEGADETALLE)
        {
            List<tbBodegaDetalle> sessionbodegadetalle = new List<tbBodegaDetalle>();
            var list = (List<tbBodegaDetalle>)Session["tbBodegaDetalle"];
            if (list == null)
            {
                sessionbodegadetalle.Add(BODEGADETALLE);
                Session["tbBodegaDetalle"] = sessionbodegadetalle;
            }
            else
            {
                list.Add(BODEGADETALLE);
                Session["tbBodegaDetalle"] = list;
            }
            return Json("Exito", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveNuevoDetalle(tbBodegaDetalle GUARDAR_NUEVO_DETALLE)
        {
            return Json("Index");
        }

        [HttpPost]
        public ActionResult UpdateBodegaDetalle(tbBodegaDetalle ACTUALIZAR_tbBodegaDetalle)
        {
            string Msj = "";
            try
            {
                IEnumerable<object> list = null;
                list = db.UDP_Inv_tbBodegaDetalle_Update(ACTUALIZAR_tbBodegaDetalle.bodd_Id
                                                        , ACTUALIZAR_tbBodegaDetalle.prod_Codigo
                                                        , ACTUALIZAR_tbBodegaDetalle.bod_Id
                                                        , ACTUALIZAR_tbBodegaDetalle.bodd_CantidadMinima
                                                        , ACTUALIZAR_tbBodegaDetalle.bodd_CantidadMaxima
                                                        , ACTUALIZAR_tbBodegaDetalle.bodd_PuntoReorden
                                                        , ACTUALIZAR_tbBodegaDetalle.bodd_UsuarioCrea
                                                        , ACTUALIZAR_tbBodegaDetalle.bodd_FechaCrea
                                                        , ACTUALIZAR_tbBodegaDetalle.bodd_Costo
                                                        , ACTUALIZAR_tbBodegaDetalle.bodd_CostoPromedio
                                                                            );
                foreach (UDP_Inv_tbBodegaDetalle_Update_Result bodega in list)
                    Msj = bodega.MensajeError;

                if (Msj == "-1")
                {
                    ModelState.AddModelError("", "No se pudo actualizar el registro, favor contacte al administrador.");
                    return PartialView("_EditBodegaDetalleModal");

                }
                else
                {
                    //return View("Edit/" + bod_Id);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se Actualizo el registro");
                return PartialView("_EditBodegaDetalleModal", ACTUALIZAR_tbBodegaDetalle);
            }
        }

        public JsonResult BuscarProductos(string term)
        {
            using (ERP_ZORZALEntities db = new ERP_ZORZALEntities())
            {
                var resultado = db.tbProducto.Where(x => x.prod_Codigo.Contains(term))
                    .Select(x => x.prod_Codigo).Take(5).ToList();
                return Json(resultado, JsonRequestBehavior.AllowGet);
            }
        }

        // POST: /Bodega/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind(Include="bod_Id,bod_Nombre,bod_ResponsableBodega,bod_Direccion,bod_Correo,bod_Telefono,usu_Id,mun_Codigo,bod_EsActiva,bod_UsuarioCrea,bod_FechaCrea,bod_UsuarioModifica,bod_FechaModifica,dep_Codigo")] tbBodega tbBodega)
        {   
            IEnumerable<object> BODEGA = null;
            IEnumerable<object> DETALLE = null;
            var idMaster = 0;
            var MsjError = "";
            var listaDetalle = (List<tbBodegaDetalle>)Session["tbBodegaDetalle"];
            if (ModelState.IsValid)
            {

                using (TransactionScope _Tran = new TransactionScope())
                {
                    try
                    {

                        BODEGA = db.UDP_Inv_tbBodega_Update(tbBodega.bod_Id
                                                            , tbBodega.bod_Nombre
                                                            , tbBodega.bod_ResponsableBodega
                                                            , tbBodega.bod_Direccion
                                                            , tbBodega.bod_Correo
                                                            , tbBodega.bod_Telefono
                                                            , tbBodega.mun_Codigo
                                                            , tbBodega.bod_UsuarioCrea
                                                            , tbBodega.bod_FechaCrea);
                        foreach (UDP_Inv_tbBodega_Update_Result bodega in BODEGA)
                            idMaster = Convert.ToInt32(bodega.MensajeError);
                        if (MsjError == "-")
                        {
                            ModelState.AddModelError("", "No se Actualizó el Registro");
                            return View(tbBodega);
                        }
                        else
                        {
                            if (listaDetalle != null)
                            {
                                if (listaDetalle.Count > 0)
                                {
                                    foreach (tbBodegaDetalle bodd in listaDetalle)
                                    {
                                        DETALLE = db.UDP_Inv_tbBodegaDetalle_Insert( 
                                                                                      bodd.prod_Codigo
                                                                                    , idMaster
                                                                                    , bodd.bodd_CantidadMinima
                                                                                    , bodd.bodd_CantidadMaxima
                                                                                    , bodd.bodd_PuntoReorden
                                                                                    , bodd.bodd_Costo
                                                                                    , bodd.bodd_CostoPromedio);
                                        foreach (UDP_Inv_tbBodegaDetalle_Insert_Result B_detalle in DETALLE)
                                            //MsjError = B_detalle.MensajeError;

                                        //if (MsjError == "-1")
                                        {
                                            //ViewBag.deparatamento_Edit = new SelectList(db.tbDepartamento, "dep_Codigo", "dep_Nombre", tbBodega.dep_Codigo);
                                            //ViewBag.municipio_Edit = new SelectList(db.tbMunicipio, "mun_Codigo", "mun_Nombre", tbBodega.mun_Codigo);
                                            ModelState.AddModelError("", "No se Actualizó el Registro");
                                        //        return View(tbBodega);
                                        //}
                                        //else
                                        //{
                                        //    _Tran.Complete();
                                        //    return RedirectToAction("Index");
                                        }
                                    }
                                }
                            }
                            //else
                            {
                                _Tran.Complete();
                                return RedirectToAction("Index");
                            }

                        }

                    }
                    catch (Exception Ex)
                    {
                        Ex.Message.ToString();
                        ModelState.AddModelError("", "No se Actualizó el Registro");
                        //ViewBag.deparatamento_Edit = new SelectList(db.tbDepartamento, "dep_Codigo", "dep_Nombre", tbBodega.dep_Codigo);
                        //ViewBag.municipio_Edit = new SelectList(db.tbMunicipio, "mun_Codigo", "mun_Nombre", tbBodega.mun_Codigo);
                        //return View(tbBodega);
                        //MsjError = "-1";
                    }
                }
                return RedirectToAction("Index");
            }
            this.AllLists();
            ViewBag.deparatamento_Edit= new SelectList(db.tbDepartamento, "dep_Codigo", "dep_Nombre", tbBodega.dep_Codigo);
            ViewBag.municipio_Edit = new SelectList(db.tbMunicipio, "mun_Codigo", "mun_Nombre", tbBodega.mun_Codigo);
            return View(tbBodega);
        }

        // GET: /Bodega/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbBodega tbBodega = db.tbBodega.Find(id);
            if (tbBodega == null)
            {
                return HttpNotFound();
            }
            return View(tbBodega);
        }

        // POST: /Bodega/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            tbBodega tbBodega = db.tbBodega.Find(id);
            db.tbBodega.Remove(tbBodega);
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

        //para Mostrar la cantidad en Existencia de la bodega
      

        //para que cambie estado a activar
        public ActionResult EstadoInactivar(int? id)
        {

            try
            {
                tbBodega obj = db.tbBodega.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbBodega_Update_Estado(id, EstadoBodega.Inactivo);
                foreach (UDP_Inv_tbBodega_Update_Estado_Result obje in list)
                    MsjError = obje.MensajeError;

                if (MsjError == "-1")
                {
                    ModelState.AddModelError("", "No se Actualizo el registro");
                    return RedirectToAction("Edit/" + id);
                }
                else
                {
                    return RedirectToAction("Edit/" + id);
                }
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se Actualizo el registro");
                return RedirectToAction("Edit/" + id);
            }


            //return RedirectToAction("Index");
        }
        //para que cambie estado a inactivar
        public ActionResult Estadoactivar(int? id)
        {

            try
            {
                tbBodega obj = db.tbBodega.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbBodega_Update_Estado(id, EstadoBodega.Activo);
                foreach (UDP_Inv_tbBodega_Update_Estado_Result obje in list)
                    MsjError = obje.MensajeError;

                if (MsjError == "-1")
                {
                    ModelState.AddModelError("", "No se Actualizo el registro");
                    return RedirectToAction("Edit/" + id);
                }
                else
                {
                    return RedirectToAction("Edit/" + id);
                }
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se Actualizo el registro");
                return RedirectToAction("Edit/" + id);
            }


            //return RedirectToAction("Index");
        }

    }
}
