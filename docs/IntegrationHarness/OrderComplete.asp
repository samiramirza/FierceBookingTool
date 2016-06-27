<%@ Language="VBScript" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

<%
Dim xmlText
If Request.Form("s")<>"" Then
    Dim objxml
    
	Set objxml = Server.CreateObject("Microsoft.XMLDOM")
	
	objxml.async = False
	objxml.ResolveExternals = False
	objxml.load Request.ServerVariables("APPL_PHYSICAL_PATH") & "cXmlDocuments\OrderRequest.xml"

    dim xmlhttp 
	set xmlhttp = server.Createobject("MSXML2.ServerXMLHTTP")
	'**** we want to make sure we are submitting a pure xml request here 
	'**** and we can get back the xml and then do the redirect
	xmlhttp.Open "POST","http://localhost/ASPIntegration/OrderRequestEndPoint.asp",false
	xmlhttp.setRequestHeader "Content-Type", "text/xml"
	'Response.Write objxml.xml
	'Response.End
	xmlhttp.send objxml.xml
	xmlText = xmlhttp.responseText
End if

Function URLDecode (s)
  URLDecode = Unescape(s)
End Function

%>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head>
    <title>Untitled Page</title>
<script language="javascript" type="text/javascript">
<!--

function Button1_onclick() {

}

// -->
</script>
</head>
<body>
    This is the Decoded URL Response from our &nbsp;integration code.&nbsp; We are back
    in the online<br />
    store at this point.&nbsp; Note the item numbers and all in the XML.&nbsp; This
    is the response DE will recieve from us<br />
    <textarea id="TextArea1"  style="width: 504px; height: 169px">
    <%=URLDecode(Request.Form("cxml-urlencoded")) %>
    </textarea>
    <br />
    <br />
    <?xml namespace="" prefix="asp" ?><asp:label id="lblItem" runat="server"></asp:label><br />
    ____________________________<br />
    <br />
    (now we have the order returned and finally we submit the order.&nbsp; Upon submitting
    the order, a final stream is sent to DCG confirming the order was successful or
    unsuccessful)<br />
    <br />
    <br />
    <form name="form1" method="Post" action="OrderComplete.asp">
    &nbsp;
    <input type="hidden" name="s" value="1" />
    <input id="Button1" type="submit" value="Submit Order Request" language="javascript" onclick="return Button1_onclick()" /><br />
    <br />
    <textarea id="Textarea2" style="width: 511px; height: 103px">
     <%=xmlText %>
    </textarea>
    </form>

</body>
</html>

