//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ERP_GMEDINA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tbRol
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tbRol()
        {
            this.tbAccesoRol = new HashSet<tbAccesoRol>();
            this.tbRolesUsuario = new HashSet<tbRolesUsuario>();
        }
    
        public int rol_Id { get; set; }
        public string rol_Descripcion { get; set; }
        public int rol_UsuarioCrea { get; set; }
        public System.DateTime rol_FechaCrea { get; set; }
        public Nullable<int> rol_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> rol_FechaModifica { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbAccesoRol> tbAccesoRol { get; set; }
        public virtual tbUsuario tbUsuario { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tbRolesUsuario> tbRolesUsuario { get; set; }
    }
}
