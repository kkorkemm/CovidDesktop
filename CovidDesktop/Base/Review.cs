//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CovidDesktop.Base
{
    using System;
    using System.Collections.Generic;
    
    public partial class Review
    {
        public long ID { get; set; }
        public int VaccinationPointID { get; set; }
        public int PatientID { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
    
        public virtual Patient Patient { get; set; }
        public virtual VaccinationPoint VaccinationPoint { get; set; }
    }
}