using Fierce.BAL.Interface;
using Fierce.BAL.Service;
using Fierce.BAL.Models;
using Fierce.DAL.Data;
using Fierce.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Serialization;
using OfficeOpenXml;
using System.Data;
using System.Configuration;
using System.Net.Mail;

namespace Fierce.Controllers
{
    public class CustomBookController : Controller
    {
        XmlDocument _orderRequestXMLDoc;
        //
        // GET: /CustomBook/
        private IFierceCustom _objFierceCustom;
        FierceService _objFierceService;
        private Fierce.BAL.Models.GridDisplay objGrid;
        private Fierce.BAL.Models.OrderInsert objOrderInsert;
        private Fierce.BAL.Models.FierceBook objFierceBook;

        public CustomBookController()
        {
            _objFierceCustom = new FierceData();
            _objFierceService = new FierceService(_objFierceCustom);
            objGrid = new BAL.Models.GridDisplay();
            objOrderInsert = new BAL.Models.OrderInsert();
            objFierceBook = new BAL.Models.FierceBook();
          
        }

        public ActionResult Index()
        {

            if (Request.QueryString["sk"] != null)
            {
                int punchID = 0;
                bool val = Int32.TryParse(Request.QueryString["punchid"].ToString(), out punchID);
                if (punchID > 0)
                {
                    FierceBookModel fiercemodel = new FierceBookModel();
                    fiercemodel.PunchOutRequestData = _objFierceService.GetPunchOutRequestById(punchID);
                    ViewBag.QString = "Success";
                    return View(fiercemodel);
                }
                else
                {

                    ViewBag.QString = "Error processing your request.";
                    return View();
                }
            }
            else
            {
                string qparams = Request.QueryString.ToString();
                ViewBag.QString = qparams;
                return View();
            }
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ActionResult GridDisplay()
        {

            GridDisplayModel model = new GridDisplayModel();
          
            if (Request.QueryString["sk"]!= null)
            {
                int punchID = 0;
                bool val = Int32.TryParse(Request.QueryString["sk"].ToString(), out punchID);
                if (punchID > 0)
                {
                    // FierceBookModel fiercemodel = new FierceBookModel();
                    objFierceBook = _objFierceService.GetPunchOutRequestById(punchID);
                    if (objFierceBook != null)
                    {
                        string partId = "";
                        if (objFierceBook.supplierPartID.ToLower().Trim() == "porttoolkit1")
                            partId = "PT";
                        else if (objFierceBook.supplierPartID.ToLower().Trim() == "esptoolkit1")
                            partId = "ST";
                        else
                            partId = "CT";

                        model.lstGridDisplay = _objFierceService.GetGridItems(partId);
                        model.hdnType = partId;
                        model.username = objFierceBook.extrinsic_user;
                        model.punchID = Request.QueryString["sk"].ToString();
                    }
                    else
                        return RedirectToAction("ErrorMessage");
                }
                else
                {
                    return RedirectToAction("ErrorMessage");
                }

            }
            else
            {
                return RedirectToAction("ErrorMessage");

            }
            return View(model);
        }


        [HttpPost]
        public ActionResult GetGridbyType(string type)
        {
            GridDisplayModel model = new GridDisplayModel();
            model.lstGridDisplay = _objFierceService.GetGridItems(type);
            return Json(new { CarList = model.lstGridDisplay });
        }

        [HttpPost]
        public EmptyResult GridDisplay(GridDisplayModel model)
        {

            //if (model.hdnType == "EC" && model.hdnSubmit != "submit")
            //{
            //    model.lstGridDisplay = _objFierceService.GetGridItems("EC");
            //    model.hdnType = "EC";
            //}
            //else if (model.hdnType == "ED" && model.hdnSubmit != "submit")
            //{
            //    model.lstGridDisplay = _objFierceService.GetGridItems("ED");
            //    model.hdnType = "ED";
            //}
            if (model.hdnSubmit == "submit")
            {
                string[] ia = null;
                ia = model.hdnString.Split(';').Select(x => x).ToArray();
                List<BAL.Models.OrderInsert> lstOrder = new List<BAL.Models.OrderInsert>();
                string type = model.hdnType;
                string username = model.username;
                int punchID = Convert.ToInt32(model.punchID);
                List<Result> lstresult = new List<Result>();
                if (ia.Length > 0)
                {
                    for (int i = 0; i < ia.Length; i++)
                    {
                        //BAL.Models.OrderInsert objorder = new BAL.Models.OrderInsert();
                        string[] subArray = null;
                        subArray = ia[i].Split(',').Select(x => x).ToArray();
                        Result objresult = new Result();
                        objresult.GridId = Convert.ToInt32(subArray[0]);
                        objresult.count = Convert.ToInt32(subArray[1]);
                        objresult.quantity = Convert.ToInt32(subArray[2]);
                        objresult.type = subArray[3];
                        lstresult.Add(objresult);
                    }

                    if (lstresult.Count > 0)
                    {
                        var lstcount = lstresult.Select(cc => new { cc.count, cc.quantity, cc.type }).Distinct().ToList();

                        foreach (var item in lstcount)
                        {
                            objOrderInsert = new BAL.Models.OrderInsert();
                            var p = lstresult.Where(x => x.count == Convert.ToInt32(item.count)).ToList();
                            objOrderInsert.Quantity = Convert.ToInt32(item.quantity);
                            objOrderInsert.SeqItem = Convert.ToInt32(item.count);
                            objOrderInsert.Type = item.type;
                            int count = 1;
                            List<BAL.Models.GridDisplay> lstGrid = new List<BAL.Models.GridDisplay>();
                            foreach (var subitem in p)
                            {
                                objGrid = new BAL.Models.GridDisplay();
                                objGrid.SequenceNumber = count;
                                objGrid.Id = subitem.GridId;
                                count += 1;
                                lstGrid.Add(objGrid);
                            }
                            objOrderInsert.lstgrid = lstGrid;
                            lstOrder.Add(objOrderInsert);
                        }

                        if (lstOrder.Count > 0)
                        {
                            _objFierceService.InsertOrderItems(lstOrder, username, punchID,model.ProjectName);

                            // return RedirectToAction("Message");
                        }
                    }
                }
                var punchOutData = _objFierceService.GetPunchOutRequestById(punchID);
                //uncomment
                PostOrderConfirmationToDE(punchID.ToString(), punchOutData.supplierPartID, objOrderInsert.Quantity, punchOutData.extrinsic_Revision, "http://shops3.directedje.com/fierce/pod-punch-in.asp", punchOutData.Punchoutxml);

              
            }

            return null;
        }

        public bool PostOrderConfirmationToDE(string punchId, string partId, int quantity, string revision, string postToURL, string punchOutXml)
        {
           
            bool isSuccess = true;
            StringBuilder returnXML = new StringBuilder();
            if (punchOutXml == null)
                Response.Redirect("http://50.57.7.78/fierce/custombook/message");

            int startTag = punchOutXml.IndexOf("<?");
            int headerEnd = punchOutXml.IndexOf("</Header>");

            returnXML.Append(punchOutXml.Substring(startTag, headerEnd + 9));
            returnXML.Append("<Message><PunchOutOrderMessage><BuyerCookie>NOTUSED</BuyerCookie><PunchOutOrderMessageHeader operationAllowed=\"inspect\"><Total><Money currency=\"USD\">0.0</Money></Total></PunchOutOrderMessageHeader>");
            returnXML.Append("<ItemIn quantity=\"" + quantity + "\"><ItemID><SupplierPartID>" + punchId + "</SupplierPartID><SupplierPartAuxiliaryID>" + partId + "</SupplierPartAuxiliaryID><Extrinsic name=\"revision\">" + revision + "</Extrinsic></ItemID><ItemDetail><UnitPrice><Money currency=\"USD\"></Money></UnitPrice><Description xml:lang=\"en\">Description of Item</Description><UnitOfMeasure>EA</UnitOfMeasure><Classification domain=\"UNSPSC\">NOTUSED</Classification></ItemDetail></ItemIn></PunchOutOrderMessage></Message></cXML>");

            //StringBuilder returnXML = new StringBuilder("<?xml version=\"1.0\"?>");
            //returnXML.Append("<!DOCTYPE cXML SYSTEM \"http://xml.cxml.org/schemas/cXML/1.1.007/cXML.dtd\"[]>");
            //returnXML.Append("<cXML version=\"1.1.007\" xml:lang=\"en-US\" payloadID=\"20150326T19:55:15-04:00@aig.directedje.com\" timestamp=\"20150520T12:55:15-04:00\">");
            //returnXML.Append("<Header><From><Credential domain=\"DUNS\"><Identity>012345678</Identity></Credential></From>");
            //returnXML.Append("<To><Credential domain=\"DUNS\"><Identity>02-528-7058</Identity></Credential></To>");
            //returnXML.Append("<Sender><Credential domain=\"DUNS\"><Identity>012345678</Identity><SharedSecret>iIkMEfuKIXFHgOw6O1QC5YxuxzTrXd25dWfB882uR61gaQw9jAOFHKUY9NXg0fQT</SharedSecret></Credential><UserAgent>Direct Response</UserAgent></Sender></Header>");
            //returnXML.Append("<Message><PunchOutOrderMessage><BuyerCookie>NOTUSED</BuyerCookie><PunchOutOrderMessageHeader operationAllowed=\"inspect\"><Total><Money currency=\"USD\">0.0</Money></Total></PunchOutOrderMessageHeader>");
            //returnXML.Append("<ItemIn quantity=\"" + quantity + "\"><ItemID><SupplierPartID>" + punchId + "</SupplierPartID><SupplierPartAuxiliaryID>" + partId + "</SupplierPartAuxiliaryID><Extrinsic name=\"revision\">" + revision + "</Extrinsic></ItemID><ItemDetail><UnitPrice><Money currency=\"USD\"></Money></UnitPrice><Description xml:lang=\"en\">Description of Item</Description><UnitOfMeasure>EA</UnitOfMeasure><Classification domain=\"UNSPSC\">NOTUSED</Classification></ItemDetail></ItemIn></PunchOutOrderMessage></Message></cXML>");
         
           
            StringBuilder formPost = new StringBuilder();
            formPost.Append("<form name='submitForm' action='");
            formPost.Append("http://shops3.directedje.com/fierce/pod-punch-in.asp");
            formPost.Append("' Method=\"POST\">");
            formPost.Append("<input name='cxml-urlencoded' type='hidden' value='");
            formPost.Append(HttpContext.Server.UrlEncode(returnXML.ToString()));
            formPost.Append("'></input>");
            formPost.Append("</form>");
            formPost.Append("<script>document.submitForm.submit();</script>");

            try
            {
                HttpContext.Response.Write(formPost);
                // Response.End();
            }
            catch (Exception ex)
            {
                isSuccess = false;
                throw;
            }

            return isSuccess;
        }

        private XmlDocument PersistOrderCXmlFile(string payLoadID, string ImprintID, string productID, string revision)
        {
            XmlDocument orderConfirmXxmlDoc = new XmlDocument();

            orderConfirmXxmlDoc.Load(HttpContext.Server.MapPath("~/PunchInSessions/PunchIn.xml"));

            // orderConfirmXxmlDoc.SelectSingleNode("//cXML").Attributes["payloadID"].InnerText = payLoadID;
            //    orderConfirmXxmlDoc.SelectSingleNode("//SupplierPartID").InnerText = ImprintID;
            //  orderConfirmXxmlDoc.SelectSingleNode("//SupplierPartAuxiliaryID").InnerText = productID;
            //orderConfirmXxmlDoc.SelectSingleNode("//Extrinsic[@name='revision']").InnerText = revision;

            //     orderConfirmXxmlDoc.Save(HttpContext.Server.MapPath("~") + @"\PunchInSessions\" + payLoadID + ".xml");
            return orderConfirmXxmlDoc;
        }

        public ActionResult Message()
        {
            return View();
        }

        public ActionResult ErrorMessage()
        {
            return View();
        }

        private class Result
        {
            public int quantity { get; set; }
            public int count { get; set; }
            public int GridId { get; set; }
            public string type { get; set; }
        }

        public ActionResult OrderComplete()
        {
            
            bool isValid = true;
            try
            {
                object Xmlobj;
                cOutXML objData = new cOutXML();
                FierceBookModel model = new FierceBookModel();
                XmlReaderSettings xSettings = new XmlReaderSettings();
                xSettings.DtdProcessing = DtdProcessing.Parse;
                XmlReader reader = XmlReader.Create(Request.InputStream, xSettings);
                Xmlobj = new XmlSerializer(typeof(cOutXML)).Deserialize(reader);

              
                objData = (cOutXML)Xmlobj;

                FierceOutRequest objFierceBook = new FierceOutRequest();


                objFierceBook.DROrderId = Convert.ToInt32(objData._request._orderRequest._orderRequestHeader._orderID);
                objFierceBook.OrderDate = Convert.ToDateTime(objData._request._orderRequest._orderRequestHeader._OrderDate);
                objFierceBook.TotalMoney = objData._request._orderRequest._orderRequestHeader._Total._money.currency + " " + objData._request._orderRequest._orderRequestHeader._Total._money.Text;
                objFierceBook.ShipAddress = objData._request._orderRequest._orderRequestHeader._ShipTo._Address.isoCountryCode + "-" + objData._request._orderRequest._orderRequestHeader._ShipTo._Address.addressID;
                objFierceBook.ShipName = HttpContext.Server.UrlDecode(objData._request._orderRequest._orderRequestHeader._ShipTo._Address.Name);
                if (objData._request._orderRequest._orderRequestHeader._ShipTo._Address._PostalAddress.lstDeliverTo.Count > 0)
                {
                    foreach (var item in objData._request._orderRequest._orderRequestHeader._ShipTo._Address._PostalAddress.lstDeliverTo)
                    {
                        objFierceBook.ShipDeliverTo = objFierceBook.ShipDeliverTo + HttpContext.Server.UrlDecode(item.Text) + ",";
                    }
                    if (objFierceBook.ShipDeliverTo != "")
                    {
                        objFierceBook.ShipDeliverTo = objFierceBook.ShipDeliverTo.Substring(0, objFierceBook.ShipDeliverTo.Length - 1);
                    }
                }

                objFierceBook.ShipStreet = HttpContext.Server.UrlDecode(objData._request._orderRequest._orderRequestHeader._ShipTo._Address._PostalAddress.Street);
                objFierceBook.ShipCity = objData._request._orderRequest._orderRequestHeader._ShipTo._Address._PostalAddress.City;
                objFierceBook.ShipState = objData._request._orderRequest._orderRequestHeader._ShipTo._Address._PostalAddress.State;
                objFierceBook.ShipPostalCode = objData._request._orderRequest._orderRequestHeader._ShipTo._Address._PostalAddress.PostalCode;
                objFierceBook.ShipCountry = objData._request._orderRequest._orderRequestHeader._ShipTo._Address._PostalAddress.Country;
                objFierceBook.BillAddress = objData._request._orderRequest._orderRequestHeader._BillTo._Address.isoCountryCode + "-" + objData._request._orderRequest._orderRequestHeader._BillTo._Address.addressID;
                objFierceBook.BillName = HttpContext.Server.UrlDecode(objData._request._orderRequest._orderRequestHeader._BillTo._Address.Name);
                objFierceBook.BillStreet = HttpContext.Server.UrlDecode(objData._request._orderRequest._orderRequestHeader._BillTo._Address._PostalAddress.Street);
                objFierceBook.BillCity = objData._request._orderRequest._orderRequestHeader._BillTo._Address._PostalAddress.City;
                objFierceBook.BillState = objData._request._orderRequest._orderRequestHeader._BillTo._Address._PostalAddress.State;
                objFierceBook.BillPostalCode = objData._request._orderRequest._orderRequestHeader._BillTo._Address._PostalAddress.PostalCode;
                objFierceBook.BillCountry = objData._request._orderRequest._orderRequestHeader._BillTo._Address._PostalAddress.Country;
                objFierceBook.lstOutItems = new List<OutItems>();
                if (objData._request._orderRequest.lstItemOut.Count > 0)
                {
                    foreach (var mainitem in objData._request._orderRequest.lstItemOut)
                    {
                        OutItems objItem = new OutItems();
                        objItem.quanity = Convert.ToInt32(mainitem.Quantity);
                        objItem.SupplierPartID = Convert.ToInt32(mainitem._itemID.SupplieroutPartID);
                        objItem.SupplierPartAuxiliaryID = mainitem._itemID.SupplieroutPartAuxiliaryID;
                        objItem.UnitPriceMoney = mainitem._ItemDetail._UnitPrice.Money.currency + " " + mainitem._ItemDetail._UnitPrice.Money.Text;
                        objItem.Description = HttpContext.Server.UrlDecode(mainitem._ItemDetail.Description);
                        objItem.LeadTime = mainitem._ItemDetail.LeadTime;
                        objItem.ChargeMoney = mainitem._Distribution._Charge._Money.currency + " " + mainitem._Distribution._Charge._Money.Text;
                        if (mainitem._ItemDetail.lstExtrinsic.Count > 0)
                        {
                            foreach (var item in mainitem._ItemDetail.lstExtrinsic)
                            {
                                if (item._name.ToLower() == "deliverytype")
                                    objItem.ExtrinsicDeliveryType = item.Text;
                                else if(item._name.ToLower() == "placedbyuser")
                                    objItem.ExtrinsicPlacedByUser = item.Text;
                                else
                                    objItem.ExtrinsicPlacedByUser = "";
                            }
                        }

                        objFierceBook.lstOutItems.Add(objItem);
                    }
                }


                _objFierceService.UpdateOrderItems(objFierceBook);
                EmailExcel(objFierceBook.DROrderId);
            }
            catch (Exception ex)
            {
                isValid = false;
            }

               string returnXML = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

               returnXML += "<!DOCTYPE cXML SYSTEM \"http://xml.cXML.org/schemas/cXML/1.1.007/cXML.dtd\">";
               returnXML += "<cXML xml:lang=\"en-US\" timestamp=\"";
               returnXML += System.DateTime.Now.ToFileTimeUtc();
               returnXML += "\">";
               returnXML += "<Response>";

               if (isValid)
                   returnXML += "<Status code=\"200\" text=\"OK\"/>";
               else
                   returnXML += "<Status code=\"400\" text=\"FAIL\"/>";

               returnXML += "</Response></cXML>";

               HttpContext.Response.ClearHeaders();
               HttpContext.Response.ContentType = "text/xml";
               HttpContext.Response.Write(returnXML);
               HttpContext.Response.End();

               return null;
        }

        private DataTable OrderDataTable(List<FierceOutRequest> lstOrders)
        {
            DataTable dtExcelTable = new DataTable();
            dtExcelTable.Columns.Add("Main");
            dtExcelTable.Columns.Add("PaceID");
            dtExcelTable.Columns.Add("Seq");
            dtExcelTable.Columns.Add("Quantity");
            int completequantity = 0;
            foreach (var item in lstOrders)
            {
                foreach (var ii in item.lstOutItems)
                {
                    completequantity = completequantity + ii.quanity;
                }
            }
            int count = 1;
            foreach (var item in lstOrders)
            {
                int quantity = 0;
                string type = "";
                foreach (var ii in item.lstOutItems)
                {
                    quantity = ii.quanity;
                    type = ii.SupplierPartAuxiliaryID;

                }
                if (count == 1)
                {

                    DataRow dr1 = dtExcelTable.NewRow();
                    dr1["Main"] = "Order Number";
                    dr1["PaceID"] = item.DROrderId;
                    dr1["Seq"] = "Quantity";
                    dr1["Quantity"] = completequantity.ToString();
                    dtExcelTable.Rows.Add(dr1);

                    DataRow dr2 = dtExcelTable.NewRow();
                    dr2["Main"] = "Placed By User";
                    dr2["PaceID"] = "" + item.ExtrinsicPlacedByUser;
                    dr2["Seq"] = "";
                    dr2["Quantity"] = "";
                    dtExcelTable.Rows.Add(dr2);

                    DataRow dremptyy = dtExcelTable.NewRow();
                    dremptyy["Main"] = "";
                    dremptyy["PaceID"] = "";
                    dremptyy["Seq"] = "";
                    dremptyy["Quantity"] = "";
                    dtExcelTable.Rows.Add(dremptyy);
                }

                if (lstOrders.Count > 1)
                {
                    DataRow drItem = dtExcelTable.NewRow();
                    drItem["Main"] = "Item " + count;
                    drItem["PaceID"] = "";
                    drItem["Seq"] = "";
                    drItem["Quantity"] = "";
                    dtExcelTable.Rows.Add(drItem);
                }

                count += 1;
                int custom = 1;
                int flip = 1;
                int add = 1;

                foreach (var subitem in item.lstgrid)
                {
                    if (subitem.CustomToolkitModule != null && subitem.CustomToolkitModule != "")
                    {
                        DataRow dr = dtExcelTable.NewRow();
                        if (custom == 1)
                        {
                            DataRow dr2 = dtExcelTable.NewRow();
                            if (type == "ESPToolkit1")
                                dr2["Main"] = "Spanish Toolkit Build";
                            else if (type == "PortToolkit1")
                                dr2["Main"] = "Portugese Toolkit Build";
                            else
                                dr2["Main"] = "Custom Toolkit Build";
                            dr2["PaceID"] = "Pace ID Number";
                            dr2["Seq"] = "Sequence";
                            dr2["Quantity"] = "Quantity";
                            dtExcelTable.Rows.Add(dr2);
                        }

                        dr["Main"] = subitem.CustomToolkitModule;
                        dr["PaceID"] = subitem.PaceID;
                        dr["Seq"] = custom.ToString();
                        dr["Quantity"] = quantity;
                        custom += 1;
                        dtExcelTable.Rows.Add(dr);
                    }

                }

                DataRow drempty = dtExcelTable.NewRow();
                drempty["Main"] = "";
                drempty["PaceID"] = "";
                drempty["Seq"] = "";
                drempty["Quantity"] = "";
                dtExcelTable.Rows.Add(drempty);

                foreach (var subitem in item.lstgrid)
                {

                    if (subitem.FlipBookModule != null && subitem.FlipBookModule != "")
                    {
                        DataRow dr = dtExcelTable.NewRow();
                        if (flip == 1)
                        {
                            DataRow dr2 = dtExcelTable.NewRow();
                            if (type == "ESPToolkit1")
                                dr2["Main"] = "Spanish Flipbook Build";
                            else if (type == "PortToolkit1")
                                dr2["Main"] = "Portugese Flipbook Build";
                            else
                                dr2["Main"] = "Custom Flipbook Build";

                            dr2["PaceID"] = "Pace ID Number";
                            dr2["Seq"] = "Sequence";
                            dr2["Quantity"] = "Quantity";
                            dtExcelTable.Rows.Add(dr2);
                        }

                        dr["Main"] = subitem.FlipBookModule;
                        dr["PaceID"] = subitem.PaceFBID;
                        dr["Seq"] = flip.ToString();
                        dr["Quantity"] = quantity;

                        flip += 1;
                        dtExcelTable.Rows.Add(dr);
                    }

                }

                DataRow drempty1 = dtExcelTable.NewRow();
                drempty1["Main"] = "";
                drempty1["PaceID"] = "";
                drempty1["Seq"] = "";
                drempty1["Quantity"] = "";
                dtExcelTable.Rows.Add(drempty1);

                foreach (var subitem in item.lstgrid)
                {

                    if (subitem.AdditonalItem != null && subitem.AdditonalItem != "")
                    {
                        DataRow dr = dtExcelTable.NewRow();
                        if (add == 1)
                        {
                            DataRow dr2 = dtExcelTable.NewRow();
                            dr2["Main"] = "Additional Items";
                            dr2["PaceID"] = "Pace ID Number";
                            dr2["Seq"] = "Sequence";
                            dr2["Quantity"] = "Quantity";
                            dtExcelTable.Rows.Add(dr2);
                        }

                        dr["Main"] = subitem.AdditonalItem;
                        dr["PaceID"] = subitem.PaceAddID;
                        dr["Seq"] = add.ToString();
                        dr["Quantity"] = quantity;

                        add += 1;
                        dtExcelTable.Rows.Add(dr);
                    }

                }
                DataRow drempty2 = dtExcelTable.NewRow();
                drempty2["Main"] = "";
                drempty2["PaceID"] = "";
                drempty2["Seq"] = "";
                drempty2["Quantity"] = "";
                dtExcelTable.Rows.Add(drempty2);
                // }
            }
            return dtExcelTable;
        }

        public  void EmailExcel(int orderid)
        {
            AdminModel model = new AdminModel();
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Results");
                ws.Cells.Worksheet.Column(1).Width = 60.00;
                ws.Cells.Worksheet.Column(2).Width = 30.00;
                ws.Cells[1, 1].Value = "Custom Toolkit Modules";
                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells[1, 1].Style.Font.Size = 15;


                model.lstFierceOutRequest = _objFierceService.GetOrderDetailsById(Convert.ToInt32(orderid));
                DataTable dt_tbl = new DataTable();
                dt_tbl = OrderDataTable(model.lstFierceOutRequest);

                string pjctname="";
                string user = "";
                if(model.lstFierceOutRequest.Count>0)
                {
                    foreach(var a in model.lstFierceOutRequest)
                    {
                        user = a.ExtrinsicPlacedByUser;
                        if(a.ProjectName!=null && a.ProjectName!="")
                        pjctname=a.ProjectName;
                        
                    }
                }
                ws.Cells[1, 4].Value = pjctname;
                ws.Cells[1, 4].Style.Font.Bold = true;
                ws.Cells[1, 4].Style.Font.Size = 15;
             
                ws.Cells["A3"].LoadFromDataTable(dt_tbl, false);

                if (dt_tbl.Rows.Count > 0)
                {
                 
                   for (int rowNumber = 3; rowNumber <= ws.Dimension.End.Row; rowNumber++)
                    {
                        var cell = ws.Cells[rowNumber, 1];
                        var cell2 = ws.Cells[rowNumber, 2];
                        var cell3 = ws.Cells[rowNumber, 3];
                        var cell4 = ws.Cells[rowNumber, 4];
                        string valueCell = cell.Value.ToString();
                        if (valueCell.IndexOf("Item ") > -1 || valueCell == "Order Number" || valueCell == "Custom Toolkit Build" || valueCell == "Custom Flipbook Build" || valueCell == "Portugese Toolkit Build" || valueCell == "Portugese Flipbook Build" || valueCell == "Spanish Toolkit Build" || valueCell == "Spanish Flipbook Build" || valueCell == "Additional Items" || valueCell == "Placed By User")
                        {
                            //Setting the background color of header cells to Gray
                            var fill = cell.Style;
                            fill.Font.Bold = true;

                            var fill2 = cell2.Style;
                            fill2.Font.Bold = true;

                            var fill3 = cell3.Style;
                            fill3.Font.Bold = true;

                            var fill4 = cell4.Style;
                            fill4.Font.Bold = true;
                        }
                      

                    }
                }
                Byte[] bin = pck.GetAsByteArray();
               
              
                 string ExcelFilePath = System.Web.HttpContext.Current.Server.MapPath(@"~/" + orderid + "_ExportOrderDetails.xlsx");
                  System.IO.File.WriteAllBytes(ExcelFilePath.Trim(), bin);
                  OrderNotifyEmail(ExcelFilePath.Trim(), orderid, user);
            }
        }

        public static void OrderNotifyEmail(string filepath, int OrderId, string user)
        {
            string toNames = "";
             MailMessage mMailMessage = new MailMessage();
            mMailMessage.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"], ConfigurationManager.AppSettings["FromName"]);

            if (ConfigurationManager.AppSettings["IsEmail"].ToString() == "0")
            {
                mMailMessage.To.Add("bhartip@grazitti.com");
            }
            else
            {
                mMailMessage.To.Add(new MailAddress(ConfigurationManager.AppSettings["ToEmail"]));
                //  mMailMessage.To.Add("nitishj@grazitti.com");
                if (user != "")
                {
                    mMailMessage.CC.Add(user);
                }
                mMailMessage.CC.Add("Janice.wisman@dcgone.com");
                mMailMessage.CC.Add("Linda.israeli@dcgone.com");
                mMailMessage.CC.Add("Kate.cook@dcgone.com");
                mMailMessage.CC.Add("Karen.Christiansen@dcgone.com");
                mMailMessage.Bcc.Add("Mary.Brennan@dcgone.com");
                mMailMessage.Bcc.Add("samiram@grazitti.com");
                mMailMessage.Bcc.Add("bhartip@grazitti.com");
            }
        
                           
                  
                  mMailMessage.Subject = "Custom Toolkit Order details for order no:-" + OrderId;
            string email = "";
            email = " Please find the attached order details for order no:-" + OrderId + ". " + "<br/><br/>Thank you,<br/>";
             mMailMessage.Body = email;
            mMailMessage.IsBodyHtml = true;
            if (filepath != "")
            {
               
                mMailMessage.Attachments.Add(new Attachment(filepath));
            }

            var client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"], Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPORT"]))
            {
                Credentials = new NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]),
                EnableSsl = true
            };
            try
            {
                client.Send(mMailMessage);
                mMailMessage.Dispose();
                client.Dispose();
                if (System.IO.File.Exists(filepath))
                {
                    System.IO.File.Delete(filepath);
                }
            }
            catch
            {


            }


        }
           
    }
}