<%@ Language="VBScript" %>
<!--#include file="PunchOut.asp"-->

<%
    '**** Create an instane of our Punch Out class then call the 
    '**** LoadIncomingPayload method
    
    Dim po
    SET po = new PunchOut
    po.LoadIncomingPayload 
    
    '**** Now craft our response that contains the URL to redirect to 
    '**** and send the output back to the calling app via response.write
    '**** We have to do the clear and end to ensure the extra tags of HTML are
    '**** not sent
    Response.Clear
    'Response.Flush
    Response.ContentType = "text/xml"
	Response.Write po.CreatePunchOutResponse("false","http://localhost/ASPIntegration/TestPOSUResponse.asp?sk=" & Session("key"))
	Response.End
%>
</body>
</html>


