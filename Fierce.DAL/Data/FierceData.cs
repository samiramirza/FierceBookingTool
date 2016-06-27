﻿using Fierce.BAL.Interface;
using Fierce.BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace Fierce.DAL.Data
{
    public class FierceData : IFierceCustom
    {
        public FierceBook InsertPunchOutRequest(FierceBook objFierceBook)
        {         
           
            using (var context = new FierceCustomBookToolEntities())
            {
                PunchOutRequest objPunchOut = new PunchOutRequest();
             
                objPunchOut.RequestExtrinsicUser = objFierceBook.extrinsic_user;
                objPunchOut.RequestExtrinsicRevision = objFierceBook.extrinsic_Revision;
                objPunchOut.PunchOutXml = objFierceBook.Punchoutxml;
                objPunchOut.RequestSupplierPartID = objFierceBook.supplierPartID;
                objPunchOut.CreatedDate = DateTime.Now;
                objPunchOut.ModifiedDate = DateTime.Now;
                objPunchOut.IsActive = true;
                context.PunchOutRequests.Add(objPunchOut);
                context.SaveChanges();
                objFierceBook.Id = objPunchOut.ID;
            }
            return objFierceBook;
        }

        public void InsertOrderItems(List<OrderInsert> lstOrder, string username, int punchID, string projectName)
        {
            using (var context = new FierceCustomBookToolEntities())
            {
                Order objOrder = new Order();
                objOrder.CreatedDate = DateTime.Now;
                objOrder.IsActive = true;               
                objOrder.UserName = username;
                objOrder.PunchID = punchID;
                objOrder.ProjectName = projectName;
                objOrder.ModifiedDate = DateTime.Now;
                context.Orders.Add(objOrder);
                context.SaveChanges();

                int orderId = objOrder.Id;

               
                foreach (var item in lstOrder)
                {
                    OrderSeqNumber objseqnumber = new OrderSeqNumber();
                    objseqnumber.OrderId = orderId;
                    objseqnumber.IsActive = true;
                    objseqnumber.CreatedDate = DateTime.Now;
                    objseqnumber.Quantity = item.Quantity;
                    objseqnumber.Type = item.Type;
                    context.OrderSeqNumbers.Add(objseqnumber);
                    context.SaveChanges();

                    foreach (var subitem in item.lstgrid)
                    {
                        OrderItem objOrderItem = new OrderItem();
                        objOrderItem.IsActive = true;
                        objOrderItem.OrderSeqId = objseqnumber.Id;
                        objOrderItem.SeqNumber = subitem.SequenceNumber;
                        objOrderItem.GridId = subitem.Id;
                        objOrderItem.CreatedDate = DateTime.Now;
                        context.OrderItems.Add(objOrderItem);
                        context.SaveChanges();
                    }
                }
                
            }
        }

        public void UpdateOrderItems(FierceOutRequest objfierce)
        {
            using (var context = new FierceCustomBookToolEntities())
            {
               if(objfierce.lstOutItems.Count>0)
               {
                   foreach (var item in objfierce.lstOutItems)
                   {
                       Order objorder = new Order();
                       objorder = context.Orders.FirstOrDefault(x => x.PunchID == item.SupplierPartID);
                       if (objorder != null)
                       {
                           objorder.ModifiedDate = DateTime.Now;
                           objorder.DROrderId = objfierce.DROrderId;
                           objorder.OrderDate = objfierce.OrderDate;
                           objorder.TotalMoney = objfierce.TotalMoney;
                           objorder.ShipAddress = objfierce.ShipAddress;
                           objorder.ShipName = objfierce.ShipName;
                           objorder.ShipDeliverTo = objfierce.ShipDeliverTo;
                           objorder.ShipStreet = objfierce.ShipStreet;
                           objorder.ShipCity = objfierce.ShipCity;
                           objorder.ShipState = objfierce.ShipState;
                           objorder.ShipPostalCode = objfierce.ShipPostalCode;
                           objorder.ShipCountry = objfierce.ShipCountry;
                           objorder.BillAddress = objfierce.BillAddress;
                           objorder.BillName = objfierce.BillName;
                           objorder.BillStreet = objfierce.BillStreet;
                           objorder.BillCity = objfierce.BillCity;
                           objorder.BillState = objfierce.BillState;
                           objorder.BillPostalCode = objfierce.BillPostalCode;
                           objorder.BillCountry = objfierce.BillCountry;
                           objorder.ChargeMoney = item.ChargeMoney;
                           objorder.LeadTime=item.LeadTime;
                           objorder.ExtrinsicPlacedByUser = item.ExtrinsicPlacedByUser;
                           objorder.ExtrinsicDeliveryType = item.ExtrinsicDeliveryType;
                           objorder.UnitOfMeasure = item.UnitOfMeasure;
                           objorder.UnitPriceMoney = item.UnitPriceMoney;
                           objorder.SupplierPartAuxiliaryID = item.SupplierPartAuxiliaryID;
                           objorder.ChargeMoney = item.ChargeMoney;
                           context.SaveChanges();

                           OrderSeqNumber objseq=new OrderSeqNumber();
                           objseq = context.OrderSeqNumbers.FirstOrDefault(x => x.OrderId == objorder.Id);
                           if (objseq != null)
                           {
                               if (objseq.Quantity != item.quanity)
                               {
                                   objseq.Quantity = item.quanity;
                                   context.SaveChanges();
                               }
                           }
                       }
                   }
               }

            }
        }

        public List<FierceOutRequest> GetOrderDetailsById(int orderId)
        {
            List<FierceOutRequest> lstDetails = new List<FierceOutRequest>();
            using (var context = new FierceCustomBookToolEntities())
            {
                Order objOrder = new Order();
              
                var lstSeq = context.Orders.Where(x => x.DROrderId == orderId && x.IsActive==true).Include(r => r.OrderSeqNumbers).ToList();
                if (lstSeq != null && lstSeq.Count>0)
                {
                    foreach (var p in lstSeq)
                    {
                       
                           BAL.Models.FierceOutRequest objfierce = new BAL.Models.FierceOutRequest();
                           objfierce.lstOutItems = new List<BAL.Models.OutItems>();
                           objfierce.lstgrid = new List<BAL.Models.GridDisplay>();
                           objfierce.DROrderId = (int)p.DROrderId;
                           objfierce.OrderDate = (DateTime)p.OrderDate;
                           objfierce.TotalMoney = p.TotalMoney;
                           objfierce.ShipAddress = p.ShipAddress;
                           objfierce.ShipName = p.ShipName;
                           objfierce.ShipDeliverTo = p.ShipDeliverTo;
                           objfierce.ShipStreet = p.ShipStreet;
                           objfierce.ShipCity = p.ShipCity;
                           objfierce.ShipState = p.ShipState;
                           objfierce.ShipPostalCode = p.ShipPostalCode;
                           objfierce.ShipCountry = p.ShipCountry;
                           objfierce.BillAddress = p.BillAddress;
                           objfierce.BillName = p.BillName;
                           objfierce.ProjectName = p.ProjectName;
                           objfierce.BillStreet = p.BillStreet;
                           objfierce.BillCity = p.BillCity;
                           objfierce.BillState = p.BillState;
                           objfierce.BillPostalCode = p.BillPostalCode;
                           objfierce.BillCountry = p.BillCountry;
                           objfierce.ExtrinsicPlacedByUser = p.ExtrinsicPlacedByUser;
                           int count = 0;
                           foreach (var item in p.OrderSeqNumbers)
                            {
                                if (count == 0)
                                {
                                    BAL.Models.OutItems objOutItems = new BAL.Models.OutItems();
                                    objOutItems.SupplierPartID = p.PunchID;
                                    var punch = context.PunchOutRequests.FirstOrDefault(x => x.ID == p.PunchID);
                                    objOutItems.SupplierPartAuxiliaryID = punch.RequestSupplierPartID;
                                    objOutItems.UnitPriceMoney = p.UnitPriceMoney;
                                    objOutItems.ChargeMoney = p.ChargeMoney;
                                    objOutItems.UnitOfMeasure = p.UnitOfMeasure;
                                    objOutItems.LeadTime = p.LeadTime;
                                    objOutItems.quanity = item.Quantity;
                                    objOutItems.ExtrinsicPlacedByUser = p.ExtrinsicPlacedByUser;
                                    objOutItems.ExtrinsicDeliveryType = p.ExtrinsicDeliveryType;
                                    objOutItems.OrderSequenceNumber = item.Id;
                                    objfierce.lstOutItems.Add(objOutItems);

                                    count += 1;
                                    foreach (var subitem in item.OrderItems)
                                    {
                                        BAL.Models.GridDisplay objGrid = new BAL.Models.GridDisplay();
                                        objGrid.SequenceNumber = subitem.SeqNumber;
                                        var i = context.GridItems.FirstOrDefault(x => x.Id == subitem.GridId);
                                        if (i != null)
                                        {
                                            string complete = "";
                                            if (i.CustomToolkitModule != null && i.CustomToolkitModule != "")
                                            {
                                                complete = i.CustomToolkitModule;
                                            }
                                            if (i.FlipBookModule != null && i.FlipBookModule != "")
                                            {
                                                if (complete != "")
                                                {
                                                    complete += "/" + i.FlipBookModule;
                                                }
                                                else
                                                {
                                                    complete = i.FlipBookModule;
                                                }
                                            }
                                            if (i.AdditonalItem != null && i.AdditonalItem != "")
                                            {
                                                if (complete != "")
                                                {
                                                    complete += "/" + i.AdditonalItem;
                                                }
                                                else
                                                {
                                                    complete = i.AdditonalItem;
                                                }
                                            }
                                            objGrid.Complete = complete;
                                            objGrid.CustomToolkitModule = i.CustomToolkitModule;
                                            objGrid.FlipBookModule = i.FlipBookModule;
                                            objGrid.AdditonalItem = i.AdditonalItem;
                                            objGrid.IsRequired = i.IsRequired;
                                            objGrid.PaceID = i.PaceID;
                                            objGrid.PaceFBID = i.PaceFBID;
                                            objGrid.PaceAddID = i.PaceAddID;
                                            objGrid.Id = subitem.GridId;
                                            objGrid.OrderSequenceNumber = objOutItems.OrderSequenceNumber;
                                            objfierce.lstgrid.Add(objGrid);
                                        }
                                    }
                                }
                            }
                           
                           lstDetails.Add(objfierce);
                    }
                   
                }
            }

            return lstDetails;
        }

        public FierceBook GetPunchOutRequestById(int id)
        {
            FierceBook lstSubmitted = new FierceBook();
            using (var context = new FierceCustomBookToolEntities())
            {
                lstSubmitted = (from a in context.PunchOutRequests
                                where a.ID == id
                                select new FierceBook
                                {
                                    Punchoutxml=a.PunchOutXml,
                                    extrinsic_user = a.RequestExtrinsicUser,
                                    extrinsic_Revision = a.RequestExtrinsicRevision,                                 
                                    supplierPartID=a.RequestSupplierPartID,
                                    Id=a.ID
                                }).FirstOrDefault();
            }
            return lstSubmitted;
        }

        public List<GridDisplay> GetGridItems(string TemplateType)
        {
            List<GridDisplay> lstSubmitted = new List<GridDisplay>();
            using (var context = new FierceCustomBookToolEntities())
            {
                lstSubmitted = (from a in context.GridItems
                                where a.Template == TemplateType && a.IsActive == true
                                select new GridDisplay
                                {
                                   ConverstionType=a.ConverstionType,
                                   CustomToolkitModule=a.CustomToolkitModule,
                                   FlipBookModule=a.FlipBookModule,
                                   AdditonalItem=a.AdditonalItem,
                                   SequenceNumber=a.SequenceNumber,
                                   PaceID = a.PaceID,
                                   PaceFBID = a.PaceFBID,
                                   PaceAddID = a.PaceAddID,
                                   IsActive=a.IsActive,
                                   IsRequired=a.IsRequired,
                                   Template=a.Template,
                                   Id = a.Id
                                }).OrderBy(x=>x.SequenceNumber).ToList();
            }
            return lstSubmitted;
        }

        public GridDisplay GetGridItems(int id, int seq, string type)
        {
            GridDisplay objGrid = new GridDisplay();
            using (var context = new FierceCustomBookToolEntities())
            {
                objGrid = (from a in context.GridItems
                                where a.SequenceNumber== seq && a.Id==id && a.Template==type.Trim() && a.IsActive == true
                                select new GridDisplay
                                {
                                   
                                  CustomToolkitModule=a.CustomToolkitModule,
                                    FlipBookModule = a.FlipBookModule,
                                    AdditonalItem = a.AdditonalItem,
                                    SequenceNumber = a.SequenceNumber,
                                      PaceID = a.PaceID,
                                      PaceFBID = a.PaceFBID,
                                      PaceAddID = a.PaceAddID,
                                    IsActive = a.IsActive,
                                    IsRequired = a.IsRequired,
                                    Id = a.Id,
                                  Template = a.Template,
                                    ConverstionType=a.ConverstionType
                                }).FirstOrDefault();
            }
            return objGrid;
        }

        public GridDisplay EditGridItems(GridDisplay gridItems, int id)
        {
            
            using (var context = new FierceCustomBookToolEntities())
            {
                GridItem _items = new GridItem();
                _items=context.GridItems.FirstOrDefault(x => x.Id == gridItems.Id);
                if (_items != null)
                {
                    _items.CustomToolkitModule = gridItems.CustomToolkitModule;
                    _items.FlipBookModule = gridItems.FlipBookModule;
                    _items.AdditonalItem = gridItems.AdditonalItem;
                    _items.PaceID = gridItems.PaceID;
                    _items.PaceFBID = gridItems.PaceFBID;
                    _items.PaceAddID = gridItems.PaceAddID;
                    _items.Template = gridItems.Template;
                    _items.IsRequired = gridItems.IsRequired;
                    if (gridItems.SequenceNumber != _items.SequenceNumber)
                    {
                        List<GridItem> lstitems = new List<GridItem>();
                        if (gridItems.SequenceNumber > _items.SequenceNumber)
                        {
                            lstitems = context.GridItems.Where(x => x.SequenceNumber <= gridItems.SequenceNumber && x.SequenceNumber > _items.SequenceNumber && x.Template == gridItems.Template.Trim()).ToList();
                            foreach (var item in lstitems)
                            {
                                GridItem objgrd = new GridItem();
                                item.SequenceNumber = item.SequenceNumber-1;
                            }
                            context.SaveChanges();
                        }
                        else
                        {
                            lstitems = context.GridItems.Where(x => x.SequenceNumber < _items.SequenceNumber && x.SequenceNumber >= gridItems.SequenceNumber && x.Template == gridItems.Template.Trim()).ToList();
                            foreach (var item in lstitems)
                            {
                                GridItem objgrd = new GridItem();
                                item.SequenceNumber = item.SequenceNumber + 1;
                            }
                            context.SaveChanges();
                        }
                        context.SaveChanges();

                        _items.SequenceNumber = gridItems.SequenceNumber;

                    }
                     context.SaveChanges();
                }
            }
            return gridItems;
        }

        public GridDisplay AddGridItems(GridDisplay gridItems,int seq) 
        {
            GridItem __newItems = new GridItem();
            using (var context = new FierceCustomBookToolEntities())
            {
                __newItems.CustomToolkitModule = gridItems.CustomToolkitModule;
                __newItems.FlipBookModule = gridItems.FlipBookModule;
                __newItems.AdditonalItem = gridItems.AdditonalItem;
                __newItems.ConverstionType = gridItems.ConverstionType;
                __newItems.PaceID = gridItems.PaceID;
                __newItems.PaceFBID = gridItems.PaceFBID;
                __newItems.PaceAddID = gridItems.PaceAddID;
                __newItems.IsRequired = gridItems.IsRequired;
                __newItems.Template = gridItems.Template;
                __newItems.SequenceNumber = gridItems.SequenceNumber+1;
                __newItems.IsActive = gridItems.IsActive;
                context.GridItems.Add(__newItems);
                context.SaveChanges();
                gridItems.Id = __newItems.Id;
                gridItems.SequenceNumber = __newItems.SequenceNumber;
            }
            return gridItems;
        }

        // for accessing last Id and sequence Number
        public int GetLastSequenceNumber(string type)
        {
            int max;
            using (var context = new FierceCustomBookToolEntities())
            {
                max = context.GridItems.Where(x => x.Template == type.Trim()).Max(p => p.SequenceNumber);
            }
            return max;
        }

        public bool DeleteModule(int id, string template) 
        {
            using (var context = new FierceCustomBookToolEntities())
            {
                int seqnum = 0;
                GridItem objgrid = new GridItem();
                objgrid = (from p in context.GridItems
                             where p.Id == id
                             select p).FirstOrDefault();
                seqnum = objgrid.SequenceNumber;
                objgrid.SequenceNumber = 0;
                objgrid.IsActive = false;
                context.SaveChanges();
                List<GridItem> lstitems = new List<GridItem>();

                lstitems = context.GridItems.Where(x => x.SequenceNumber > seqnum && x.Template == template.Trim()).ToList();
                if (lstitems.Count > 0)
                {
                    foreach (var item in lstitems)
                    {
                        GridItem objgrd = new GridItem();
                        item.SequenceNumber = item.SequenceNumber - 1;
                    }
                    context.SaveChanges();

                }
                
            }
            return true;
        }

        public List<ViewOrders> ViewAllOrders(DateTime startDate, DateTime endDate, string username, int DROrderID) 
        {
            List<ViewOrders> orders = new List<ViewOrders>();
            using (var context = new FierceCustomBookToolEntities())
            {             
              int[] lst= (from x in context.Orders where  x.DROrderId != null && x.DROrderId != 0 && x.OrderDate != null && x.IsActive == true
                          select (int)x.DROrderId).Distinct().ToArray();
                        
              foreach (var item in lst)
              {
                  Order objorder = new Order();
                  objorder = context.Orders.FirstOrDefault(x => x.DROrderId == item);
                  if(objorder!=null)
                  {
                      ViewOrders objview = new ViewOrders();
                      objview.OrderId = objorder.DROrderId;
                      objview.UserName = objorder.UserName; 
                      objview.OrderDate = (DateTime)objorder.OrderDate;
                      objview.TotalMoney = objorder.TotalMoney;
                      objview.ShipAddress = objorder.ShipAddress;
                      objview.ShipName = objorder.ShipName;
                      objview.ShipDeliverTo = objorder.ShipDeliverTo;
                      objview.ShipStreet = objorder.ShipStreet;
                      objview.ShipCity = objorder.ShipCity;
                      objview.ShipState = objorder.ShipState;
                      objview.ShipPostalCode = objorder.ShipPostalCode;
                      objview.ShipCountry = objorder.ShipCountry;
                      objview.BillAddress = objorder.BillAddress;
                      objview.BillName = objorder.BillName;
                      objview.ExtrinsicDeliveryType = objorder.ExtrinsicDeliveryType;
                      objview.BillStreet = objorder.BillStreet;
                      objview.BillCity = objorder.BillCity;
                      objview.BillState = objorder.BillState;
                      objview.BillPostalCode = objorder.BillPostalCode;
                      objview.BillCountry = objorder.BillCountry;
                      orders.Add(objview);
                  }
              }

              if (startDate.Year > 2000 && endDate.Year > 2000 && orders.Count > 0)
              {
                  orders = orders.Where(x => x.OrderDate >= startDate && x.OrderDate <= endDate).ToList();
              }
              if (username != null && username != "" && orders.Count>0)
              {
                  orders = orders.Where(x => x.UserName.ToLower().Trim() == username.ToLower().Trim()).ToList();
              }

              if (DROrderID != 0 && orders.Count > 0)
              {
                  orders = orders.Where(x => x.OrderId == DROrderID).ToList();
              }


            }
            return orders;
        }

        public void DeleteOrder(int id)
        {
            using (var context = new FierceCustomBookToolEntities())
            {
                var lstSeq = context.Orders.Where(x => x.DROrderId == id && x.IsActive == true).ToList();
                if (lstSeq != null && lstSeq.Count > 0)
                {
                    foreach (var p in lstSeq)
                    {
                        p.IsActive = false;
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
