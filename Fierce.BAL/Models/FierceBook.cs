﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fierce.BAL.Models
{
   public  class FierceBook
    {
        public int Id { get; set; }    
        public string extrinsic_user { get; set; } 
        public string supplierPartID { get; set; }
        public string Punchoutxml { get; set; }
        public string extrinsic_Revision { get; set; }
    }

   public class FierceOutRequest
   {
       public int DROrderId { get; set; }
       public DateTime OrderDate { get; set; }
       public string TotalMoney { get; set; }
       public string ShipAddress { get; set; }
       public string ShipName { get; set; }
       public string ShipDeliverTo { get; set; }
       public string ShipStreet { get; set; }
       public string ShipCity { get; set; }
       public string ShipState { get; set; }
       public string ShipPostalCode { get; set; }
       public string ShipCountry { get; set; }
       public string ProjectName { get; set; }
       public string BillAddress { get; set; }
       public string BillName { get; set; }
       public string BillStreet { get; set; }
       public string BillCity { get; set; }
       public string BillState { get; set; }
       public string BillPostalCode { get; set; }
       public string BillCountry { get; set; }
       public List<OutItems> lstOutItems { get; set; }
       public List<GridDisplay> lstgrid { get; set; }
       public string ExtrinsicPlacedByUser { get; set; }
   }

   public class OutItems
   {
       public int SupplierPartID { get; set; }
       public string SupplierPartAuxiliaryID { get; set; }
       public string UnitPriceMoney { get; set; }
       public string Description { get; set; }
       public string ChargeMoney { get; set; }
       public string UnitOfMeasure { get; set; }
       public string LeadTime { get; set; }
       public int quanity { get; set; }
       public string ExtrinsicPlacedByUser { get; set; }
       public string ExtrinsicDeliveryType { get; set; }
       public int OrderSequenceNumber { get; set; }
   }

   public class GridDisplay
   {
       public int Id { get; set; }
       public string CustomToolkitModule { get; set; }
       public string Template { get; set; }
       public string FlipBookModule { get; set; }
       public string AdditonalItem { get; set; }
       public string ConverstionType { get; set; }
       public string Complete { get; set; }
       public string PaceID { get; set; }
       public string PaceFBID { get; set; }
       public string PaceAddID { get; set; }
       public bool IsRequired { get; set; }
       public bool IsActive { get; set; }
       public int SequenceNumber { get; set; }
       public int OrderSequenceNumber { get; set; }
      

   }

   public class OrderInsert
   {
       public int Quantity { get; set; }
       public int SeqItem { get; set; }
       public string Type { get; set; }
       public List<GridDisplay> lstgrid{ get; set; }
   }
  
   public class OrderItems
   {
       public int Id { get; set; }
       public string CustomToolkitModule { get; set; }
       public string FlipBookModule { get; set; }
       public string AdditonalItem { get; set; }
       public string ConverstionType { get; set; }
       public bool IsRequired { get; set; }
       public int SequenceNumber { get; set; }
   }
   public class ViewOrders
   {
       public string UserName { get; set; }
       public int? OrderId { get; set; }
       public DateTime OrderDate { get; set; }
       public string TotalMoney { get; set; }
       public string ShipAddress { get; set; }
       public string ShipName { get; set; }
       public string ShipDeliverTo { get; set; }
       public string ShipStreet { get; set; }
       public string ShipCity { get; set; }
       public string ShipState { get; set; }
       public string ShipPostalCode { get; set; }
       public string ShipCountry { get; set; }
       public string BillAddress { get; set; }
       public string BillName { get; set; }
       public string BillStreet { get; set; }
       public string BillCity { get; set; }
       public string BillState { get; set; }
       public string BillPostalCode { get; set; }
       public string BillCountry { get; set; }
       public string ExtrinsicDeliveryType { get; set; }
       
   } 
   
}
