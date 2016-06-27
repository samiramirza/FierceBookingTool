using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Fierce
{
    public partial class DEOrderApprovedEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText((Server.MapPath("~/PunchInSessions/DEOrderApprovedEvent.txt")), "sss");
        }
    }
}