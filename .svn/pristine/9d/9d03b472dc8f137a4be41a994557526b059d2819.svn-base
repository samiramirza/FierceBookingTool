using Fierce.BAL.Interface;
using Fierce.BAL.Models;
using Fierce.BAL.Service;
using Fierce.DAL.Data;
using Fierce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;

namespace Fierce.Controllers
{
    public class DEIntegrationController : Controller
    {
        XmlDocument _punchOutXMLDoc;
        private string _punchOutSessionsPath;
        private IFierceCustom _objFierceCustom;
        FierceService _objFierceService;
       
        // GET: /DEIntegration/

        public DEIntegrationController() 
        {
            _objFierceCustom = new FierceData();
            _objFierceService = new FierceService(_objFierceCustom);
        }
        public ActionResult Index()
        {
            CreateXMLResponse();
            return View();
        }

        public ActionResult AcceptPunchOutRequest()
        {

            //  _punchOutSessionsPath = @"C:\inetpub\wwwroot\fierce\PunchOutXMLFiles\";
          //    string customPODTemplateURL = "http://localhost:51164/custombook/index";
            string customPODTemplateURL = "http://50.57.7.78/fierce/custombook/GridDisplay";
            int uid = 0;
            try
            {

                //string sDirPath = Server.MapPath("~/PunchOutXMLFiles/b1822d66-5e70-4c8a-8122-617f41e3c763.xml");
                //_punchOutXMLDoc = new XmlDocument();
                //_punchOutXMLDoc.Load(Request.InputStream);
                //string path = Path.Combine(sDirPath, Guid.NewGuid() + ".xml");
                //_punchOutXMLDoc.Save(path);
                object Xmlobj;

                cXML objData = new cXML();
                FierceBookModel model = new FierceBookModel();
                XmlReaderSettings xSettings = new XmlReaderSettings();
                xSettings.DtdProcessing = DtdProcessing.Parse;
                XmlReader reader = XmlReader.Create(Request.InputStream, xSettings);

                Xmlobj = new XmlSerializer(typeof(cXML)).Deserialize(reader);
                objData = (cXML)Xmlobj;


                //serialize the objet  
                XmlSerializer xsSubmit = new XmlSerializer(typeof(cXML));
             
                
                StringWriter swriter = new StringWriter();
                XmlWriter writer = XmlWriter.Create(swriter);
                xsSubmit.Serialize(writer, objData);
                var xml = swriter.ToString(); // Your XML

                FierceBook objFierceBook = new FierceBook();

                objFierceBook.Punchoutxml = xml;
                objFierceBook.extrinsic_user = objData._request._punchRequest.Extrinsic;
                objFierceBook.supplierPartID = objData._request._punchRequest._selectedItem._itemID.SupplierPartID;
                objFierceBook.extrinsic_Revision = objData._request._punchRequest._selectedItem._itemID.RevisionId;
                ViewBag.message = objData._request._punchRequest._supplierSetup.Sessionid;
                _objFierceService.InsertPunchOutRequest(objFierceBook);
                uid = objFierceBook.Id;
                customPODTemplateURL = customPODTemplateURL + "?sk=" + uid;
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                Response.End();
                customPODTemplateURL += "?e=" + ex.InnerException.ToString();
            }
            ExecuteCxmlPunchoutResponse(customPODTemplateURL, false);

            return View();
        }

        public void ExecuteCxmlPunchoutResponse(string personalizationURL, bool isError)
        {

            StringBuilder returnXML = new StringBuilder("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");

            returnXML.Append("<!DOCTYPE cXML SYSTEM \"http://xml.cXML.org/schemas/cXML/1.1.010/cXML.dtd\">");
            returnXML.Append("<cXML payloadID=\"456778-199@cxml.workchairs.com\" xml:lang=\"en-US\" timestamp=\"1999-03-12T18:39:09-08:00\">");
            returnXML.Append("<Response>");

            if (isError)
                returnXML.Append("<Status code=\"400\" text=\"FAILURE\"/>");
            else
                returnXML.Append("<Status code=\"200\" text=\"OK\"/>");

            returnXML.Append("<PunchOutSetupResponse>");
            returnXML.Append("<StartPage><URL>");
            returnXML.Append(personalizationURL);
            returnXML.Append("</URL></StartPage>");
            returnXML.Append("</PunchOutSetupResponse></Response></cXML>");
            Response.ClearHeaders();
            Response.ContentType = "text/xml";

            Response.Write(returnXML.ToString());
            Response.End();

        }

        public void CreateXMLResponse()
        {
            StringBuilder returnXML = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            returnXML.Append("<Response>");
            returnXML.Append("<Status code=\"400\" text=\"Unauticated error\" />");
            returnXML.Append("<Status code=\"200\" text=\"Success\" />");
            returnXML.Append("</Response>");
            Response.ClearHeaders();

            Response.ContentType = "text/xml";
            Response.Write(returnXML.ToString());
            Response.End();

        }

        
    }
}