﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Fierce.BAL.Interface;
using Fierce.BAL.Service;
using Fierce.DAL.Data;
using Fierce.Models;
using Fierce.BAL.Models;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Presentation;
using a = DocumentFormat.OpenXml.Drawing;
using OfficeOpenXml;
using System.Data;
using OfficeOpenXml.Style;


namespace Fierce.Controllers
{
   [Authorize]

    public class AdminController : Controller
    {
       private AdminModel _adminModel;
        private IFierceCustom _objFierceCustom;
        FierceService _objFierceService;
        private Fierce.BAL.Models.GridDisplay objGrid;
        private Fierce.BAL.Models.OrderInsert objOrderInsert;
      
        public AdminController()
        {
            _objFierceCustom = new FierceData();
            _objFierceService = new FierceService(_objFierceCustom);
            objGrid = new BAL.Models.GridDisplay();
            objOrderInsert = new BAL.Models.OrderInsert();
        }
      

        public ActionResult Index()
        {
            AdminModel model = new AdminModel();
            model.lstGridDisplay = _objFierceService.GetGridItems("CT");
            List<seq> lst = new List<seq>();
            lst = (from p in model.lstGridDisplay
                   select new seq()
                   {
                       Id = p.SequenceNumber,
                       Name=p.SequenceNumber.ToString()
                   }).ToList();
            model.lstSeq = new SelectList(lst, "Id", "Name");
            model.hdnType = "CT";
            return View(model);

        }
       [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult ViewOrders()
        {
            AdminModel model = new AdminModel();
            int page;
            bool flag = false;
            double recordsperpage = 10;
            if (Request.QueryString["datefrom"] == null || Request.QueryString["datefrom"] == "" || Convert.ToDateTime(Request.QueryString["datefrom"]).Year < 2000 )
            
                model.StartRange = "";
            else
                model.StartRange = Convert.ToDateTime(Request.QueryString["datefrom"]).Date.ToString("MM/dd/yyyy");

            if (Request.QueryString["dateto"] == null || Request.QueryString["dateto"] == "" || Convert.ToDateTime(Request.QueryString["dateto"]).Year < 2000)
                model.EndRange = "";
            else
                model.EndRange = Convert.ToDateTime(Request.QueryString["dateto"]).Date.ToString("MM/dd/yyyy");

            if (Request.QueryString["dateto"] != null && Request.QueryString["dateto"] != "")
            {
                model.dateTo = Convert.ToDateTime(Request.QueryString["dateto"]);
            }

            if (Request.QueryString["datefrom"] != null && Request.QueryString["datefrom"] != "")
            {
                model.dateFrom = Convert.ToDateTime(Request.QueryString["datefrom"]);
            }

            if (Request.QueryString["username"] != null && Request.QueryString["username"] != "")
            {
                model.userName = Request.QueryString["username"];
            }

            if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != "")
            {
                model.DRorderID = Request.QueryString["orderid"];
            }
            if (Request.QueryString["flag"] != null && Request.QueryString["flag"] != "" && Request.QueryString["flag"] == "True")
            {
                flag= true;
            }
            if (flag == true)
            {
                model.lstOrders = _objFierceService.ViewAllOrders(model.dateFrom, model.dateTo, model.userName, Convert.ToInt32(model.DRorderID)).OrderByDescending(x => x.OrderDate.Date).ToList();

                ViewBag.FindFlag = true;
            }
            else
            {
                model.lstOrders = _objFierceService.ViewAllOrders(Convert.ToDateTime("01/01/0001"), Convert.ToDateTime("01/01/0001"), "", 0).OrderByDescending(x => x.OrderDate.Date).ToList();
                ViewBag.FindFlag = false;
            }
                //  model.lstOrders = _objFierceService.ViewAllOrders(model.dateFrom,model.dateTo,"",0).OrderBy(x=>x.OrderDate.Date).ToList();
           if (Request.QueryString["Page"] == null || Convert.ToInt32(Request.QueryString["Page"]) == 0)
           { page = 1; }
           else
               page = Convert.ToInt32(Request.QueryString["Page"]);
            
            

            if (model.lstOrders.Count() > 0)
            {
                double total=model.lstOrders.Count();
                model.TotalPages =Convert.ToInt32(Math.Ceiling(total  / recordsperpage));
               
                    model.Page = page;

                    model.lstOrders = model.lstOrders.Skip((model.Page * Convert.ToInt32(recordsperpage)) - Convert.ToInt32(recordsperpage)).Take(Convert.ToInt32(recordsperpage)).ToList();
                //model.Page * Convert.ToInt32(recordsperpage) - ((model.Page-1) * Convert.ToInt32(recordsperpage))
//                model.lstOrders = model.lstOrders.AsEnumerable().Where(model.lstOrders.Count() < (page * recordsperpage) && x.OrderId > (page * recordsperpage - recordsperpage)).ToList();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ViewOrders(AdminModel model)
        {
            string type = model.ClickType;
            if (type == "delete")
            {
                _objFierceService.DeleteOrder(model.hiddenId);
            }
            if (model.dateFrom.Year < 2000)
                model.StartRange = "";
            else
                model.StartRange = Convert.ToDateTime(model.dateFrom).Date.ToString("MM/dd/yyyy");

            if (model.dateTo.Year < 2000)
                model.EndRange = "";
            else
                model.EndRange = Convert.ToDateTime(model.dateTo).Date.ToString("MM/dd/yyyy");

            int page;
            double recordsperpage = 10;
            model.userName = model.userName;
            model.DRorderID = model.DRorderID;
            model.lstOrders = _objFierceService.ViewAllOrders(model.dateFrom, model.dateTo, model.userName, Convert.ToInt32(model.DRorderID)).OrderByDescending(x => x.OrderDate.Date).ToList();
            ViewBag.FindFlag = true;
            if (model.Page == null || Convert.ToInt32(model.Page) == 0)
            { page = 1; }
            else
                page = Convert.ToInt32(model.Page);



            if (model.lstOrders.Count() > 0)
            {
                double total = model.lstOrders.Count();
                model.TotalPages = Convert.ToInt32(Math.Ceiling(total / recordsperpage));

                model.Page = page;

                model.lstOrders = model.lstOrders.Skip((model.Page * Convert.ToInt32(recordsperpage)) - Convert.ToInt32(recordsperpage)).Take(Convert.ToInt32(recordsperpage)).ToList();
                //model.Page * Convert.ToInt32(recordsperpage) - ((model.Page-1) * Convert.ToInt32(recordsperpage))
                //                model.lstOrders = model.lstOrders.AsEnumerable().Where(model.lstOrders.Count() < (page * recordsperpage) && x.OrderId > (page * recordsperpage - recordsperpage)).ToList();
            }
            this._adminModel = model;
            DataTable dt_tbl = new DataTable();
           
                  
          
            if (type == "excel")
            {
                if (model.lstOrders.Count > 0)
                {
                    dt_tbl = CreateDataTable(model.lstOrders);
                    using (ExcelPackage pck = new ExcelPackage())
                    {
                        //Create the worksheet
                        ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Results");
                        ws.Cells.Worksheet.Column(1).Width = 15.00;
                        ws.Cells.Worksheet.Column(2).Width = 15.00;
                        ws.Cells.Worksheet.Column(3).Width = 30.00;
                        ws.Cells.Worksheet.Column(4).Width = 30.00;
                        ws.Cells.Worksheet.Column(5).Width = 30.00;
                        ws.Cells.Worksheet.Column(6).Width = 30.00;
                        ws.Cells.Worksheet.Column(7).Width = 30.00;
                        ws.Cells.Worksheet.Column(8).Width = 30.00;
                        ws.Cells.Worksheet.Column(9).Width = 30.00;
                        ws.Cells.Worksheet.Column(10).Width = 30.00;
                        ws.Cells.Worksheet.Column(11).Width = 30.00;
                        ws.Cells.Worksheet.Column(12).Width = 30.00;
                        ws.Cells.Worksheet.Column(13).Width = 30.00;
                        ws.Cells.Worksheet.Column(14).Width = 30.00;
                        ws.Cells.Worksheet.Column(15).Width = 30.00;
                        ws.Cells.Worksheet.Column(16).Width = 30.00;
                        ws.Cells.Worksheet.Column(17).Width = 50.00;
                        ws.Cells.Worksheet.Column(18).Width = 50.00;
                        ws.Cells.Worksheet.Column(18).Width = 50.00;
                        ws.Cells.Worksheet.Column(19).Width = 50.00;
                      
                        for (int i = 19; i <= dt_tbl.Columns.Count; i++)
                        {
                            ws.Cells.Worksheet.Column(i).Width = 30.00;
                        }
                        //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                        ws.Cells[1, 4].Value = "Orders of Fierce Booking Toolkit";
                        ws.Cells[1, 4].Style.Font.Bold = true;
                        ws.Cells[1, 4].Style.Font.Size = 15;
                        ws.Cells["A3"].LoadFromDataTable(dt_tbl, true);

                        int colIndex = 1;

                        foreach (DataColumn dc in dt_tbl.Columns) //Creating Headings
                        {
                            var cell = ws.Cells[3, colIndex];

                            //Setting the background color of header cells to Gray
                            var fill = cell.Style;
                            fill.Font.Bold = true;

                            //Setting Top/left,right/bottom borders.
                            var border = cell.Style.Border;
                            border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            //Setting Value in cell
                            cell.Value = dc.ColumnName;
                            //object col22Header = ws.Cells[3, 25].Value;
                            cell.Style.WrapText = true;
                            colIndex++;

                        }

                        for (int rowNumber = 4; rowNumber <= ws.Dimension.End.Row; rowNumber++)
                        {
                            //ws.Column(52).Hidden = true;
                            //ws.Column(53).Hidden = true;
                            var cell1 = ws.Cells[rowNumber, 51];
                            object cell2 = ws.Cells[rowNumber, 52].Value;
                            object cell3 = ws.Cells[rowNumber, 53].Value;

                        }

                        using (ExcelRange rng = ws.Cells[3, 1, 3, dt_tbl.Columns.Count])
                        {
                            rng.Style.WrapText = true;

                        }

                        if (dt_tbl.Rows.Count > 0)
                        {
                            using (ExcelRange rng = ws.Cells[4, 1, dt_tbl.Rows.Count + 3, dt_tbl.Columns.Count])
                            {

                                rng.Style.WrapText = true;

                            }
                        }


                        return File(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExportResult.xlsx");
                    }
                }
                else
                {
                    ViewBag.error = "true";
                }


            }

          
            return View(model);
        }


        [HttpPost]
        public ActionResult Index(AdminModel model)
        {
            if (model.ClickType=="Get")
            {
              //  model.lstGridDisplay = _objFierceService.GetGridItems(model.hdnType.Trim());
                model.hdnType = model.hdnType;
            }
            else if (model.ClickType == "Delete")
            {
                model.Result = _objFierceService.DeleteModule(model.hiddenId, model.hdnType.Trim());
            }
            else if (model.ClickType == "Edit")
            {
                objGrid.Id = model.hiddenId;
                objGrid.CustomToolkitModule = model.CustomToolkitModule;
                objGrid.FlipBookModule = model.FlipBookModule;
                objGrid.AdditonalItem = model.AdditonalItem;
                objGrid.IsRequired = model.IsRequired;
                objGrid.SequenceNumber = model.SeqNumberSelected;
                objGrid.PaceID = model.CustomToolkitPaceID;
                objGrid.PaceFBID = model.FlipBookModulePaceID;
                objGrid.PaceAddID = model.AdditonalItemPaceID;
                objGrid.Template = model.hdnType;
                var status = _objFierceService.EditGridItems(objGrid, objGrid.Id);
              
            }
            else if (model.ClickType == "Save")
            {
                objGrid.CustomToolkitModule = model.CustomToolkitModule;
                objGrid.FlipBookModule = model.FlipBookModule;
                objGrid.AdditonalItem = model.AdditonalItem;
                objGrid.PaceID = model.CustomToolkitPaceID;
                objGrid.PaceFBID = model.FlipBookModulePaceID;
                objGrid.PaceAddID = model.AdditonalItemPaceID;
                objGrid.IsRequired = model.IsRequired;
                objGrid.ConverstionType ="EC";
                objGrid.Template = model.hdnType;
                objGrid.IsActive = true;
                objGrid.SequenceNumber = _objFierceService.GetLastSequenceNumber(model.hdnType);
                var status = _objFierceService.AddGridItems(objGrid, objGrid.SequenceNumber);
              
            }
            model.lstGridDisplay = _objFierceService.GetGridItems(model.hdnType);
            List<seq> lst = new List<seq>();
            lst = (from p in model.lstGridDisplay
                   select new seq()
                   {
                       Id = p.SequenceNumber,
                       Name = p.SequenceNumber.ToString()
                   }).ToList();
            model.lstSeq = new SelectList(lst, "Id", "Name");
            return View(model);
        }

        [HttpPost]
        public ActionResult EditModule(int id, int seq, string type)
        {
            AdminModel model = new AdminModel();
            var result = _objFierceService.GetGridItems(id, seq, type);
            return Json(new { ToolkitValues = result });
        }

      

        public ActionResult ViewDetail()
        {
            AdminModel model = new AdminModel();
            if (Request.QueryString["data"] != null)
            {
                model.lstFierceOutRequest = _objFierceService.GetOrderDetailsById(Convert.ToInt32(Request.QueryString["data"]));
            }
       
            return View(model);
        }

       [HttpPost]
        public ActionResult ViewDetail(AdminModel model)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Results");
                ws.Cells.Worksheet.Column(1).Width = 60.00;
                ws.Cells.Worksheet.Column(2).Width = 30.00;
                ws.Cells[1, 1].Value = "Custom Toolkit Modules";
                ws.Cells[1, 1].Style.Font.Bold = true;
                ws.Cells[1, 1].Style.Font.Size = 15;
                            
              
                model.lstFierceOutRequest = _objFierceService.GetOrderDetailsById(Convert.ToInt32(model.DRorderID));
                DataTable dt_tbl = new DataTable();
                dt_tbl = OrderDataTable(model.lstFierceOutRequest);

                string pjctname="";
                if(model.lstFierceOutRequest.Count>0)
                {
                    foreach(var a in model.lstFierceOutRequest)
                    {
                        if(a.ProjectName!=null && a.ProjectName!="")
                        pjctname=a.ProjectName;
                    }
                }
                ws.Cells[1, 4].Value = pjctname;
                ws.Cells[1, 4].Style.Font.Bold = true;
                ws.Cells[1, 4].Style.Font.Size = 15;
                //for (int i = 19; i <= dt_tbl.Columns.Count; i++)
                //{
                //    ws.Cells.Worksheet.Column(i).Width = 30.00;
                //}
                ////Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                //ws.Cells[1, 4].Value = "Booking Modules";
                //ws.Cells[1, 4].Style.Font.Bold = true;
                //ws.Cells[1, 4].Style.Font.Size = 15;
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
                        if (valueCell.IndexOf("Item ")>-1 || valueCell == "Order Number" || valueCell == "Custom Toolkit Build" || valueCell == "Custom Flipbook Build" || valueCell == "Portugese Toolkit Build" || valueCell == "Portugese Flipbook Build" || valueCell == "Spanish Toolkit Build" || valueCell == "Spanish Flipbook Build" || valueCell == "Additional Items" || valueCell == "Placed By User")
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

                return File(pck.GetAsByteArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", model.DRorderID+"_ExportOrderDetails.xlsx");
                
            }
            //return null;
        }
        private DataTable CreateDataTable(List<ViewOrders> lstOrders)
        {
            DataTable dtExcelTable = new DataTable();
            dtExcelTable.Columns.Add("User Name");
            dtExcelTable.Columns.Add("DR OrderId");
            dtExcelTable.Columns.Add("Order Date");
             dtExcelTable.Columns.Add("Total Money");
             dtExcelTable.Columns.Add("Delivery Type");            
            dtExcelTable.Columns.Add("Ship Address");
            dtExcelTable.Columns.Add("Ship Name");
            dtExcelTable.Columns.Add("Ship DeliverTo");
            dtExcelTable.Columns.Add("Ship Street");
            dtExcelTable.Columns.Add("Ship City");
            dtExcelTable.Columns.Add("Ship State");
            dtExcelTable.Columns.Add("Ship Country");
            dtExcelTable.Columns.Add("Ship PostalCode");
            dtExcelTable.Columns.Add("Bill Address");
            dtExcelTable.Columns.Add("Bill Name");
            dtExcelTable.Columns.Add("Bill Street");
            dtExcelTable.Columns.Add("Bill City");
            dtExcelTable.Columns.Add("Bill State");
            dtExcelTable.Columns.Add("Bill Country");
            dtExcelTable.Columns.Add("Bill PostalCode");
           
          

                      
            foreach (var q in lstOrders)
            {
                DataRow dr = dtExcelTable.NewRow();

                dr["User Name"] = q.UserName;
                dr["DR OrderId"] = q.OrderId;
                dr["Order Date"] = q.OrderDate.ToShortDateString();
                dr["Total Money"] = q.TotalMoney;
                dr["Delivery Type"] = q.ExtrinsicDeliveryType;
                dr["Ship Address"] = q.ShipAddress;
                dr["Ship Name"] = q.ShipName;
                dr["Ship DeliverTo"] = q.ShipDeliverTo;
                dr["Ship Street"] = q.ShipStreet;
                dr["Ship City"] = q.ShipCity;
                dr["Ship State"] = q.ShipState;
                dr["Ship Country"] = q.ShipCountry;
                dr["Ship PostalCode"] = q.ShipPostalCode;
                dr["Bill Address"] = q.BillAddress;
                dr["Bill Name"] = q.BillName;
                dr["Bill Street"] = q.BillStreet;
                dr["Bill City"] = q.BillCity;
                dr["Bill State"] = q.BillState;
                dr["Bill Country"] = q.BillCountry;
                dr["Bill PostalCode"] = q.BillPostalCode;
               
                
                dtExcelTable.Rows.Add(dr);
            }
  
            
            return dtExcelTable;
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
                    completequantity = completequantity+ii.quanity;                   
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
    }
}
