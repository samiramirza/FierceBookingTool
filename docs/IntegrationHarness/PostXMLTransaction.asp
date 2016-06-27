<%@ Language="VBScript" %>



<%

if Request.Form("s")<>"" Then
    '**** This is the first page that mimics what will be sent from D2S
    '**** First we create an instance of our XML DOM
    Dim objxml
	Set objxml = CreateObject("MSXML2.DOMDocument.5.0")
	'**** Try to ignore the references in the Xml to a schema
	objxml.async=false
	objxml.validateOnParse=false
	objxml.resolveExternals = false
	objxml.preserveWhitespace = false
	
	
	
	
	
	'**** Pick up the local Xml file that is the same type of file 
	'**** that will come from D2S
	if objxml.load("C:\Inetpub\wwwroot\ASPIntHarness\cXMLDocuments\PunchOut.xml") Then
	  'Response.Write objxml.xml
	  'Response.End
	Else
	  'Response.Write Server.MapPath(Request.ServerVariables("PATH_INFO"))  & "<br>"
	  Response.Write "error!!<br><br>"
	End If
    '**** If we hit an error end it
    If objxml.parseError.errorCode <>0 Then
      Response.Write objxml.parseError.reason 
      Response.End
    End IF
    
    '****************
    
    s="http://localhost/ASPIntegration/AcceptPunchOutRequest.asp" 
    set xd=createobject("MSXML2.DOMDocument.5.0") 
    xd.async = false
    xd.validateOnParse = false
    xd.resolveExternals = false
    xd.preserveWhiteSpace = false

    xd.loadXML objxml.xml 
    set xh=createobject("msxml2.xmlhttp.5.0") 
    xh.open "post",s,false 
    xh.send xd.xml
    Response.ContentType="text/xml" 
    Dim stringXml 
    stringXml = xh.responseText 
   

    '*****************
    
    
	SET NewDoc = CreateObject("MSXML2.DOMDocument.5.0")
	
    NewDoc.async = false
    NewDoc.validateOnParse = false
    NewDoc.resolveExternals = false
    NewDoc.preserveWhiteSpace = false
       
    newdoc.loadXML stringXml
    
    Set XMLSend = NewDoc 
    Set poster = Nothing 
    '****** The URL node will contain the text that D2S should redirect to
    '****** This now posts to our actual integration code
	Dim redir 
    redir = newDoc.selectSingleNode("//URL").text
  
    Set xmlhttp = nothing
    Response.Redirect redir
    'Response.Write xmlhttp.responseXML
 End if

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
    <strong>Welcome to Direct 2 Shop<br />
        <br />
    </strong>(This page is a test harness that sends the "PunchOutSetupRequest" stream
    to DCG. Upon recieving the stream, DCG<br />
    then responds with a "PunchOutSetupResponse" that contains the URL to redirect the
    user to.&nbsp; The stream going out with this request contains the item number,
    username, user email, and other information)<br />
    <br />
    <br />
    <br />
    <form name="form1" method="Post" action="PostXmlTransaction.asp">
    <input type="hidden" value="1" name="s" />
    <input id="Button1" type="submit" value="Create Custom Document" language="javascript" onclick="return Button1_onclick()" />
    </form>
</body>
</html>

