//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_ZORZAL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbFactura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbFactura()
        {
            this.tbSalida = new HashSet<tbSalida>();
            this.tbDevolucion = new HashSet<tbDevolucion>();
            this.tbFacturaHistorica = new HashSet<tbFacturaHistorica>();
            this.tbPago = new HashSet<tbPago>();
            this.tbPedido = new HashSet<tbPedido>();
            this.tbFacturaDetalle = new HashSet<tbFacturaDetalle>();
        }
    
        public long fact_Id { get; set; }
        public string fact_Codigo { get; set; }
        public System.DateTime fact_Fecha { get; set; }
        public byte esfac_Id { get; set; }
        public short cja_Id { get; set; }
        public short suc_Id { get; set; }
        public int clte_Id { get; set; }
        public string pemi_NumeroCAI { get; set; }
        public bool fact_AlCredito { get; set; }
        public int fact_DiasCredito { get; set; }
        public decimal fact_PorcentajeDescuento { get; set; }
        public bool fact_AutorizarDescuento { get; set; }
        public string fact_Vendedor { get; set; }
        public string clte_RTN_Identidad_Pasaporte { get; set; }
        public string clte_Nombres { get; set; }
        public int fact_UsuarioCrea { get; set; }
        public System.DateTime fact__FechaCrea { get; set; }
        public Nullable<int> fact__UsuarioModifica { get; set; }
        public Nullable<System.DateTime> fact_FechaModifica { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbSalida> tbSalida { get; set; }
        public virtual tbCaja tbCaja { get; set; }
        public virtual tbCliente tbCliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbDevolucion> tbDevolucion { get; set; }
        public virtual tbEstadoFactura tbEstadoFactura { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbFacturaHistorica> tbFacturaHistorica { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbPago> tbPago { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbPedido> tbPedido { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbFacturaDetalle> tbFacturaDetalle { get; set; }
        public virtual tbSucursal tbSucursal { get; set; }
    }
}
