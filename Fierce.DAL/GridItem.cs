//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Fierce.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class GridItem
    {
        public GridItem()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }
    
        public int Id { get; set; }
        public string CustomToolkitModule { get; set; }
        public string FlipBookModule { get; set; }
        public string AdditonalItem { get; set; }
        public string ConverstionType { get; set; }
        public bool IsRequired { get; set; }
        public int SequenceNumber { get; set; }
        public bool IsActive { get; set; }
        public string PaceID { get; set; }
        public string PaceFBID { get; set; }
        public string PaceAddID { get; set; }
        public string Template { get; set; }
    
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
