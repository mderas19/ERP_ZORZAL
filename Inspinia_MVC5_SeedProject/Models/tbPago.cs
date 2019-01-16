//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbPago
    {
        public int pago_Id { get; set; }
        public long fact_Id { get; set; }
        public short tpa_Id { get; set; }
        public System.DateTime pago_FechaElaboracion { get; set; }
        public decimal pago_SaldoAnterior { get; set; }
        public decimal pago_TotalPago { get; set; }
        public Nullable<decimal> pago_TotalCambio { get; set; }
        public string pago_Emisor { get; set; }
        public Nullable<short> bcta_Id { get; set; }
        public Nullable<System.DateTime> pago_FechaVencimiento { get; set; }
        public string pago_Titular { get; set; }
        public int pago_UsuarioCrea { get; set; }
        public System.DateTime pago_FechaCrea { get; set; }
        public Nullable<int> pago_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> pago_FechaModifica { get; set; }
        public bool pago_EstaAnulado { get; set; }
        public bool pago_EstaImpreso { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        public virtual tbCuentasBanco tbCuentasBanco { get; set; }
        public virtual tbFactura tbFactura { get; set; }
        public virtual tbTipoPago tbTipoPago { get; set; }
    }
}
