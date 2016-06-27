using Fierce.BAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fierce.Models
{
    public class AdminModel
    {
        public List<Fierce.BAL.Models.GridDisplay> lstGridDisplay { get; set; }
        public Fierce.BAL.Models.GridDisplay objGridDisplay { get; set; }
        public string CustomToolkitModule { get; set; }
        public string FlipBookModule { get; set; }
        public string AdditonalItem { get; set; }
        public string CustomToolkitPaceID { get; set; }
        public string FlipBookModulePaceID { get; set; }
        public string AdditonalItemPaceID { get; set; }    
        public string StartRange { get; set; }
        public string EndRange { get; set; }
        public string SeqNumber { get; set; }
        public int SeqNumberSelected { get; set; }
        public int id { get; set; }
        public int hiddenId { get; set; }
        public bool IsRequired { get; set; }
        public List<string> seqlst { get; set; }
        public string hdnType { get; set; }
        public string ClickType { get; set; }
        public bool Result { get; set; }     
        public DateTime dateFrom { get; set; }      
        public DateTime dateTo { get; set; }
        public string userName { get; set; }
        public SelectList lstSeq { get; set; }
        public string DRorderID { get; set; }
        public  List<FierceOutRequest> lstFierceOutRequest { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
      
      public List<ViewOrders> lstOrders { get; set; }
        
    }

    public class seq
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}