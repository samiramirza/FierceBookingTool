using Fierce.BAL.Interface;
using Fierce.BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fierce.BAL.Service
{
   
    public class FierceService : IFierceCustom
    {
        private IFierceCustom _IFierceCustom;
        public FierceService(IFierceCustom objFierceCustom)
        {
            _IFierceCustom = objFierceCustom;
        }
        public FierceBook InsertPunchOutRequest(FierceBook FierceBook)
        {
            return _IFierceCustom.InsertPunchOutRequest(FierceBook);
        }
        public FierceBook GetPunchOutRequestById(int id)
        {
            return _IFierceCustom.GetPunchOutRequestById(id);
        }

        public List<GridDisplay> GetGridItems(string ConverType)
        {
            return _IFierceCustom.GetGridItems(ConverType);
        }
        public GridDisplay GetGridItems(int id, int seq, string type)
        {
            return _IFierceCustom.GetGridItems(id, seq, type);
        }
        public void InsertOrderItems(List<OrderInsert> lstOrder, string username, int punchID, string projectName)
        {
            _IFierceCustom.InsertOrderItems(lstOrder, username, punchID, projectName);
        }
        public List<FierceOutRequest> GetOrderDetailsById(int orderId)
        {
             return _IFierceCustom.GetOrderDetailsById(orderId);
        }
        public GridDisplay EditGridItems(GridDisplay gridItems, int id)
        {
            return _IFierceCustom.EditGridItems(gridItems,id);
        }
        public GridDisplay AddGridItems(GridDisplay gridItems, int seq)
        {
            return _IFierceCustom.AddGridItems(gridItems,seq);
        }
        public int GetLastSequenceNumber(string type)
        {
            return _IFierceCustom.GetLastSequenceNumber(type);
        }
        public bool DeleteModule(int id, string template) 
        {
            return _IFierceCustom.DeleteModule(id, template);
        }
        public List<ViewOrders> ViewAllOrders(DateTime startDate, DateTime endDate, string username, int DROrderID) 
        {
            return _IFierceCustom.ViewAllOrders(startDate, endDate, username, DROrderID);
        }

        public void UpdateOrderItems(FierceOutRequest objfierce)
        {
            _IFierceCustom.UpdateOrderItems(objfierce);
        }
        public void DeleteOrder(int id)
        {
            _IFierceCustom.DeleteOrder(id);
        }
    }
}
