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
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class tbCuentasBanco
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbCuentasBanco()
        {
            this.tbPago = new HashSet<tbPago>();
        }
    
        public short bcta_Id { get; set; }
        public short ban_Id { get; set; }
        public short mnda_Id { get; set; }
        public byte bcta_TipoCuenta { get; set; }
        public decimal bcta_TotalCredito { get; set; }
        public decimal bcta_TotalDebito { get; set; }
        public System.DateTime bcta_FechaApertura { get; set; }
        public string bcta_Numero { get; set; }
        public int bcta_UsuarioCrea { get; set; }
        public System.DateTime bcta_FechaCrea { get; set; }
        public Nullable<int> bcta_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> bcta_FechaModifica { get; set; }
    
        public virtual tbUsuario tbUsuario { get; set; }
        public virtual tbUsuario tbUsuario1 { get; set; }
        public virtual tbBanco tbBanco { get; set; }
        public virtual tbMoneda tbMoneda { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbPago> tbPago { get; set; }

        [NotMapped]
        public List<cTipoCuenta> TipoCuentaList { get; set; }
    }
}
