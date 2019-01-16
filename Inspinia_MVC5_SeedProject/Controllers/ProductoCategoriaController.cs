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
using Newtonsoft.Json;

namespace ERP_ZORZAL.Controllers
{
    public class ProductoCategoriaController : Controller
    {
        private ERP_ZORZALEntities db = new ERP_ZORZALEntities();

        // GET: /ProductoCategoria/
        public ActionResult Index()
        {
            try { ViewBag.smserror = TempData["smserror"].ToString(); } catch { }
            try { ViewBag.smsexito = TempData["smsexito"].ToString(); } catch { }

            var tbproductocategoria = db.tbProductoCategoria.Include(t => t.tbProductoSubcategoria);
            //ViewBag.smserror = "";
            return View(db.tbProductoCategoria.ToList());
        }

        // GET: /ProductoCategoria/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbProductoCategoria tbProductoCategoria = db.tbProductoCategoria.Find(id);
            if (tbProductoCategoria == null)
            {
                return HttpNotFound();
            }
            return View(tbProductoCategoria);
        }


        // GET: /ProductoCategoria/Create
        public ActionResult Create()
        {
            //ViewBag.smserror = "";
            Session["tbProductoSubcategoria"] = null;
            return View();
        }


        // POST: /ProductoCategoria/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pcat_Id,pcat_Nombre,pcat_UsuarioCrea,pcat_FechaCrea,pcat_UsuarioModifica,pcat_FechaModifica, pcat_EsActivo")] tbProductoCategoria tbProductoCategoria)
        {
            IEnumerable<object> cate = null;
            IEnumerable<object> sub = null;
            var idMaster = 0;
            var MsjError = "";
            var listaSubCategoria = (List<tbProductoSubcategoria>)Session["tbProductoSubcategoria"];
            if (ModelState.IsValid)
            {

                using (TransactionScope _Tran = new TransactionScope())
                {
                    try
                    {

                        cate = db.UDP_Inv_tbProductoCategoria_Insert(tbProductoCategoria.pcat_Nombre);
                        foreach (UDP_Inv_tbProductoCategoria_Insert_Result categoria in cate)
                            idMaster = Convert.ToInt32(categoria.MensajeError);
                        if (MsjError == "-")
                        {
                            ModelState.AddModelError("", "No se Guardo el Registro");
                            return View(tbProductoCategoria);
                        }
                        else
                        {
                            if (listaSubCategoria != null)
                            {
                                if (listaSubCategoria.Count > 0)
                                {
                                    foreach (tbProductoSubcategoria subcategoria in listaSubCategoria)
                                    {
                                        sub = db.UDP_Inv_tbProductoSubcategoria_Insert(subcategoria.pscat_Descripcion
                                                                                    , idMaster,
                                                                                    subcategoria.pscat_ISV
                                                                                    );
                                        foreach (UDP_Inv_tbProductoSubcategoria_Insert_Result ProdSubCate in sub)

                                        //if (MensajeError == "-1")
                                        {
                                            ModelState.AddModelError("", "No se Guardo el Registro");
                                            //return View(tbProductoCategoria);
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
                                //return RedirectToAction("Index");
                            }

                        }

                    }
                    catch (Exception Ex)
                    {
                        Ex.Message.ToString();
                        //ModelState.AddModelError("", "No se Guardo el Registro");
                        //return View(tbProductoCategoria);
                        MsjError = "-1";
                    }
                }
                return RedirectToAction("Index");
            }
        
          
            return View(tbProductoCategoria);
        }

        // GET: /ProductoCategoria/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tbProductoCategoria tbProductoCategoria = db.tbProductoCategoria.Find(id);
            if (tbProductoCategoria == null)
            {
                return HttpNotFound();
            }
            Session["tbProductoSubcategoria"] = null;
            return View(tbProductoCategoria);
        }


        [HttpPost]
        public JsonResult GuardarSubCategoria(tbProductoSubcategoria tbsubcategoria)
        {
            Session["tbProductoSubcategoria"] = null;
            List<tbProductoSubcategoria> sessionsubCate = new List<tbProductoSubcategoria>();
            var list = (List<tbProductoSubcategoria>)Session["tbProductoSubCategoria"];
            if (list == null)
            {
                sessionsubCate.Add(tbsubcategoria);
                Session["tbProductoSubCategoria"] = sessionsubCate;
            }
            else
            {
                list.Add(tbsubcategoria);
                Session["tbProductoSubCategoria"] = list;
            }
            return Json("Exito", JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubCate(int pscat_Id)
        {
            
            var list = db.tbProductoSubcategoria.Where(x => x.pscat_Id == pscat_Id).SingleOrDefault();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateSubCategoria(tbProductoSubcategoria EditarSubCategoria)
        {
            string Msj = "";
            try
            {
                IEnumerable<object> list = null;
                list = db.UDP_Inv_tbProductoSubcategoria_Update(
                                                        EditarSubCategoria.pscat_Id,
                                                        EditarSubCategoria.pscat_Descripcion,
                                                       EditarSubCategoria.pcat_Id,
                                                       EditarSubCategoria.pscat_UsuarioCrea,
                                                      EditarSubCategoria.pscat_FechaCrea,
                                                      EditarSubCategoria.pscat_ISV
                    );
                foreach (UDP_Inv_tbProductoSubcategoria_Update_Result subcate in list)
                    Msj = subcate.MensajeError;

                if (Msj == "-1")
                {

                    ModelState.AddModelError("", "No se Guardo el Registro");
                   
                }
                else
                {
                    //return Json("Index");
                    
                }

            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se Guardo el registro");
            }
            return Json("", JsonRequestBehavior.AllowGet); ;
        }

        // POST: /ProductoCategoria/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind(Include= "pcat_Id,pcat_Nombre,pcat_UsuarioCrea,pcat_FechaCrea,pcat_UsuarioModifica,pcat_FechaModifica,pcat_EsActivo")] tbProductoCategoria tbProductoCategoria)
        {
            IEnumerable<object> cate = null;
            IEnumerable<object> subcate = null;
            var idMaster = 0;
            var MsjError = "";
            var List = (List<tbProductoSubcategoria>)Session["tbProductoSubCategoria"];
            if (ModelState.IsValid)
            {

                using (TransactionScope _Tran = new TransactionScope())
                {
                    try
                    {

                        cate = db.UDP_Inv_tbProductoCategoria_Update(tbProductoCategoria.pcat_Id,
                                              tbProductoCategoria.pcat_Nombre,
                                              tbProductoCategoria.pcat_UsuarioCrea,
                                              tbProductoCategoria.pcat_FechaCrea);
                        foreach (UDP_Inv_tbProductoCategoria_Update_Result ProductoCategoria in cate)


                            idMaster = Convert.ToInt32(ProductoCategoria.MensajeError);

                        if (MsjError == "-")
                        {
                            ModelState.AddModelError("", "No se Actualizó el Registro");
                            return View(tbProductoCategoria);
                        }
                        else
                        {
                            if (List != null)
                            {
                                if (List.Count > 0)
                                {

                                    foreach (tbProductoSubcategoria subcategoria in List)
                                    {
                                        subcategoria.pscat_UsuarioCrea = 1;
                                        subcategoria.pscat_FechaCrea = DateTime.Now;

                                        subcate = db.UDP_Inv_tbProductoSubcategoria_Insert(subcategoria.pscat_Descripcion
                                                                                    , idMaster,
                                                                                    subcategoria.pscat_ISV
                                                                                    );
                                       
                                        foreach (UDP_Inv_tbProductoSubcategoria_Insert_Result ProdSubCate in subcate)

                                        //if (MensajeError == "-1")
                                        {
                                            ModelState.AddModelError("", "No se Actualizó el Registro");
                                           
                                            //}
                                            //else
                                            //{
                                            //    _Tran.Complete();
                                            //    return RedirectToAction("Index");
                                        }
                                    }
                                }
                            }

                            //else
                            {
                                _Tran.Complete();
                                //return RedirectToAction("Index");
                            }

                        }

                    }
                    catch (Exception Ex)
                    {
                        Ex.Message.ToString();

                        MsjError = "-1";
                    }
                }
                return RedirectToAction("Index");
            }

            return View(tbProductoCategoria);

        }

        [HttpPost]
        public JsonResult removeSubCategoria(tbProductoSubcategoria subcate)
        {
            var list = (List<tbProductoSubcategoria>)Session["tbProductoSubCategoria"];

            if (list != null)
            {
                var itemToRemove = list.Single(r => r.pscat_Id == subcate.pscat_Id);
                list.Remove(itemToRemove);
                Session["tbProductoSubCategoria"] = list;
            }
            
            return Json("", JsonRequestBehavior.AllowGet);

        }
        
        public ActionResult EliminarProductoCategoria(int? id)
         {
            
            try
            {
                tbProductoCategoria obj = db.tbProductoCategoria.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbProductoCategoria_Delete(id);
                foreach (UDP_Inv_tbProductoCategoria_Delete_Result obje in list)
                    MsjError = obje.MensajeError;

                if (MsjError.StartsWith("-1 The DELETE statement conflicted with the REFERENCE constraint"))
                {
                    TempData["smserror"] = " No se puede eliminar el dato porque tiene dependencia.";
                    ViewBag.smserror = TempData["smserror"];

                    ModelState.AddModelError("", "No se puede borrar el registro");
                    return RedirectToAction("Index");
                }
                
                else
                {
                    ViewBag.smserror = "";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se puede borrar el registro");
                return RedirectToAction("Index");
            }


            //return RedirectToAction("Index");
          
        }


        public ActionResult EliminarProductoSubCategoria(int? id)
        {
            try
            {
                tbProductoCategoria obj = db.tbProductoCategoria.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbProductoSubCategoria_Delete(id);
                foreach (UDP_Inv_tbProductoSubCategoria_Delete_Result obje in list)
                    MsjError = obje.MensajeError;

                if (MsjError == "-1")
                {
                    ModelState.AddModelError("", "No se puede borrar el registro");
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se puede borrar el registro");
                return RedirectToAction("Index");
            }


            //return RedirectToAction("Index");

        }



        public ActionResult EliminarProductoCategoriaSubCategoria(int? id)
        {
            try
            {
                tbProductoCategoria obj = db.tbProductoCategoria.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbProductoCategoriaSubCate_Delete(id);
                foreach (UDP_Inv_tbProductoCategoriaSubCate_Delete_Result obje in list)
                    MsjError = obje.MensajeError;

                if (MsjError == "-1")
                {
                    
                    ModelState.AddModelError("", "No se puede borrar el registro");
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
                ModelState.AddModelError("", "No se puede borrar el registro");
                  
                TempData["smserror"] = " no se puede eliminar el dato porque tiene dependencia";
                ViewBag.smserror = TempData["smserror"];
                return RedirectToAction("Index");
          
            }


            //return RedirectToAction("Index");

        }





        public ActionResult ActivarCate(int? id)
        {

            try
            {
                tbProductoCategoria obj = db.tbProductoCategoria.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbProductoCategoria_Update_Estado(id, EstadoCategoria.Activo);
                foreach (UDP_Inv_tbProductoCategoria_Update_Estado_Result obje in list)
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



        public ActionResult InactivarCate(int? id)
        {

            try
            {
                tbProductoCategoria obj = db.tbProductoCategoria.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbProductoCategoria_Update_Estado(id, EstadoCategoria.Inactivo);
                foreach (UDP_Inv_tbProductoCategoria_Update_Estado_Result obje in list)
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
        }


        public ActionResult ActivarSub(int? id)
        {

            try
            {
                tbProductoSubcategoria obj = db.tbProductoSubcategoria.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbProductoSubCategoria_Update_Estado(id, EstadoSubCategoria.Activo);
                foreach (UDP_Inv_tbProductoSubCategoria_Update_Estado_Result obje in list)
                    MsjError = obje.MensajeError;

                if (MsjError == "-1")
                {
                    ModelState.AddModelError("", "No se Actualizo el registro");
                    return RedirectToAction("Edit/" + id);
                }
                else
                {
                    return RedirectToAction("Edit/" + id); ;
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
       

        public ActionResult InactivarSub(int? id)
        {

            try
            {
                tbProductoSubcategoria obj = db.tbProductoSubcategoria.Find(id);
                IEnumerable<object> list = null;
                var MsjError = "";
                list = db.UDP_Inv_tbProductoSubCategoria_Update_Estado(id, EstadoSubCategoria.Inactivo);
                foreach (UDP_Inv_tbProductoSubCategoria_Update_Estado_Result obje in list)
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
          }

    }

    }

