<%@ Language="VBScript" %>

<!--#include file="PunchIn.asp"-->
<!--#include file="PunchOut.asp"-->

<!--

Dim po
Set po = new PunchOut


po.GetExistingSession R
Response.Write po.userName

-->

<%



If Request.Form("pb")<>"" Then
  '**** So if we have a form post then run this code
  '**** is this basically the same thing as IsPostback method in ASP.NET
  Dim pi
  Set pi = new PunchIn
  '***** Check the function for the comments on what this does
  pi.CreatePunchIn "1123", "blah", "blah", "http://localhost/ASPIntHarness/OrderComplete.asp"
Else
 
  Dim po
  SET po = new PunchOut
  '***** here is where we take the querystring parameter and 
  '***** load our session data from it
  '**** this call is critical (not that they all aren't)
  po.GetExistingSession(Request.QueryString("sk"))
 
  

%>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Untitled Page</title>
<script language="javascript" type="text/javascript">

</script>
</head>
<body>
    Welcome to the DCG POD Area<br />
    <br />
    (This is the area we are directed to after returning the "PunchOutSetupResponse".&nbsp;
    After clicking the button below,<br />
    we will issue D2S a PunchOutOrderMessage passing the SupplierPartID and SupplierPartAuxiliaryID
    to them.&nbsp; Alternately we can cancel the request.&nbsp; After completing this,
    we return a PunchOutOrderMessage and redirect them back to their cart on D2S)<br />
    The following are some of the properties loaded from the data
    
    VendorToken: <%=po.VendorToken %><br />
    UserEmail: <%=po.userEmail %><br />
    ItemNumber: <%=po.ItemNumber %><br />
    ItemDescription: <%=po.ItemDescription   %><br />
 <%End if %>
    <br />
    <br />
    <form name="form1" method="POST" action="TestPOSUResponse.asp">
    <input id="Button1" type="submit" value="Complete Order"  />
    <input type="hidden" value="pb" name="pb" />
    </form>
</body>
</html>

