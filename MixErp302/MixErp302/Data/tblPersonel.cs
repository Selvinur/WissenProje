//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MixErp302.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblPersonel
    {
        public int Id { get; set; }
        public string PersonelNo { get; set; }
        public string Adi { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public Nullable<int> DepartmanId { get; set; }
        public Nullable<System.DateTime> IsBasTarih { get; set; }
        public Nullable<System.DateTime> IsSonTarih { get; set; }
        public Nullable<System.DateTime> DogumTarih { get; set; }
        public string Tcno { get; set; }
        public string Email { get; set; }
    
        public virtual blgDepartman blgDepartman { get; set; }
    }
}
