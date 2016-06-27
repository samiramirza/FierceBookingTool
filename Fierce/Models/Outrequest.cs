using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Fierce.Models
{
    [XmlRoot("cXML")]
    public class cOutXML
    {
        [XmlElement("Header")]
        public Header _header { get; set; }
        [XmlElement("Request")]
        public Request1 _request { get; set; }
    }
 

    
    [XmlRoot("Request")]
    public class Request1
    {
        [XmlElement("OrderRequest")]
        public OrderRequest _orderRequest { get; set; }
    }

    [XmlRoot("OrderRequest")]
    public class OrderRequest
    {
         [XmlElement("OrderRequestHeader")]
        public OrderRequestHeader _orderRequestHeader { get; set; }


         [XmlElement("ItemOut")]
         public List<ItemOut> lstItemOut { get; set; }
    }


    [XmlRoot("OrderRequestHeader")]
    public class OrderRequestHeader
    {
         [XmlElement("Total")]
        public Total _Total { get; set; }


         [XmlAttribute("orderID")]
         public string _orderID { get; set; }

         [XmlAttribute("orderDate")]
         public string _OrderDate { get; set; }
        
         [XmlElement("ShipTo")]
        public ShipTo _ShipTo { get; set; }

         [XmlElement("BillTo")]
        public BillTo _BillTo { get; set; }
    }

    
    [XmlRoot("Total")]
    public class Total
    {
        [XmlElement("Money")]
        public Money _money { get; set; }
    }


    [XmlRoot("Money")]
    public class Money
    {
        [XmlAttribute("currency")]
        public string currency { get; set; }

        [XmlText()]
        public string Text { get; set; }

    }


     [XmlRoot("ShipTo")]
    public class ShipTo
    {
        [XmlElement("Address")]
        public Address _Address { get; set; }
    }

     [XmlRoot("Address")]
     public class Address
     {
        [XmlElement("Name")]
        public string Name { get; set; }

        [XmlElement("PostalAddress")]
        public PostalAddress _PostalAddress { get; set; }

        [XmlElement("Email")]
        public string Email { get; set; }

        [XmlAttribute("isoCountryCode")]
        public string isoCountryCode { get; set; }

        [XmlAttribute("addressID")]
        public string addressID { get; set; }
    }

     [XmlRoot("PostalAddress")]
     public class PostalAddress
     {
         [XmlElement("DeliverTo")]
         public List<DeliverTo> lstDeliverTo { get; set; }

         [XmlElement("Street")]
         public string Street { get; set; }

         [XmlElement("City")]
         public string City { get; set; }

         [XmlElement("State")]
         public string State { get; set; }

         [XmlElement("PostalCode")]
         public string PostalCode { get; set; }

         [XmlElement("Country")]
         public string Country { get; set; }
     }

     [XmlRoot("DeliverTo")]
     public class DeliverTo
     {
         [XmlText()]
         public string Text { get; set; }
     }



    [XmlRoot("BillTo")]
     public class BillTo
    {
        [XmlElement("Address")]
        public Address _Address { get; set; }
    }

    [XmlRoot("ItemOut")]
    public class ItemOut
    {

        [XmlAttribute("quantity")]
        public string Quantity { get; set; }

        [XmlElement("ItemID")]
        public ItemID1 _itemID { get; set; }

        [XmlElement("ItemDetail")]
        public ItemDetail _ItemDetail { get; set; }

        [XmlElement("Distribution")]
        public Distribution _Distribution { get; set; }

    }

    [XmlRoot("ItemID")]
    public class ItemID1
    {
        [XmlElement("SupplierPartID")]
        public string SupplieroutPartID { get; set; }
        [XmlElement("SupplierPartAuxiliaryID")]
        public string SupplieroutPartAuxiliaryID { get; set; }
    }

    [XmlRoot("ItemDetail")]
    public class ItemDetail
    {
        [XmlElement("UnitPrice")]
        public UnitPrice _UnitPrice { get; set; }

        [XmlElement("Description")]
        public string Description { get; set; }

        [XmlElement("UnitOfMeasure")]
        public string UnitOfMeasure { get; set; }

        [XmlElement("LeadTime")]
        public string LeadTime { get; set; }

       [XmlElement("Extrinsic")]
        public List<Extrinsic> lstExtrinsic { get; set; }

    }

    [XmlRoot("Extrinsic")]
    public class Extrinsic
    {
       [XmlText()]
        public string Text { get; set; }

        [XmlAttribute("name")]
        public string _name { get; set; }
    }

    [XmlRoot("UnitPrice")]
    public class UnitPrice
    {
        [XmlElement("Money")]
        public Money Money { get; set; }
    }

    [XmlRoot("Distribution")]
    public class Distribution
    {
        [XmlElement("Charge")]
        public Charge _Charge { get; set; }
    }

    [XmlRoot("Charge")]
    public class Charge
    {
        [XmlElement("Money")]
        public Money _Money { get; set; }
    }


}