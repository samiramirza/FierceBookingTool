using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Fierce.Models
{
    [XmlRoot("cXML")]
    public class cXML
    {
        [XmlElement("Header")]
        public Header _header { get; set; }
        [XmlElement("Request")]
        public Request _request { get; set; }
    }
    [XmlRoot("Header")]
    public class Header
    {
        [XmlElement("From")]
        public From _from { get; set; }
        [XmlElement("To")]
        public To _to { get; set; }
        [XmlElement("Sender")]
        public Sender _sender { get; set; }
    }

    [XmlRoot("From")]
    public class From
    {
        [XmlElement("Credential")]
        public FromAttr _fromCredential { get; set; }
    }

    [XmlRoot("FromAttr")]
    public class FromAttr
    {
        [XmlElement("Identity")]
        public string FromIdentity { get; set; }
    }

    [XmlRoot("To")]
    public class To
    {
        [XmlElement("Credential")]
        public ToAttr _toCredential { get; set; }
    }

    [XmlRoot("ToAttr")]
    public class ToAttr
    {
        [XmlElement("Identity")]
        public string ToIdentity { get; set; }
    }

    [XmlRoot("Sender")]
    public class Sender
    {
        [XmlElement("Credential")]
        public SenderAttr _senderCredential { get; set; }
        [XmlElement("UserAgent")]
        public string UserAgent { get; set; }
    }

    [XmlRoot("SenderAttr")]
    public class SenderAttr
    {
        [XmlElement("Identity")]
        public string SenderIdentity { get; set; }
        [XmlElement("SharedSecret")]
        public string SharedSecret { get; set; }
    }

    [XmlRoot("Request")]
    public class Request
    {
        [XmlElement("PunchOutSetupRequest")]
        public PunchOutSetupRequest _punchRequest { get; set; }
    }

    [XmlRoot("PunchOutSetupRequest")]
    public class PunchOutSetupRequest
    {
        [XmlElement("BuyerCookie")]
        public string BuyerCookie { get; set; }

        [XmlElement("Extrinsic")]
        public string Extrinsic { get; set; }

        [XmlElement("BrowserFormPost")]
        public BrowserFormPost _browserSupport { get; set; }
        [XmlElement("Contact")]
        public Contact _contact { get; set; }
        [XmlElement("SupplierSetup")]
        public SupplierSetup _supplierSetup { get; set; }
        [XmlElement("SelectedItem")]
        public SelectedItem _selectedItem { get; set; }
    }

    [XmlRoot("BrowserFormPost")]
    public class BrowserFormPost
    {
        [XmlElement("URL")]
        public string BrowserFormPostUrl { get; set; }
    }

    [XmlRoot("Contact")]
    public class Contact
    {
        [XmlElement("Email")]
        public string Email { get; set; }
    }

    [XmlRoot("SupplierSetup")]
    public class SupplierSetup
    {
        [XmlElement("URL")]
        public string SupplierSetupUrl { get; set; }
        [XmlElement("Extrinsic")]
        public string Sessionid { get; set; }
    }

    [XmlRoot("SelectedItem")]
    public class SelectedItem
    {
        [XmlElement("ItemID")]
        public ItemID _itemID { get; set; }
    }

    [XmlRoot("ItemID")]
    public class ItemID
    {
        [XmlElement("SupplierPartID")]
        public string SupplierPartID { get; set; }
        [XmlElement("SupplierPartAuxiliaryID")]
        public string SupplierPartAuxiliaryID { get; set; }
        [XmlElement("Extrinsic")]
        public string RevisionId { get; set; }
    }
}