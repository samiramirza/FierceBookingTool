<%
Class OrderRequest
  
  Public Function LoadOrderRequest(useOrderNumberForName, fileName)
     Dim docReceived
     set docReceived = CreateObject("Microsoft.XMLDOM") 
     docReceived.ValidateOnParse= False
     docReceived.async = False 
     docReceived.load Request 
     Dim orderNumber
     orderNumber = docReceived.SelectSingleNode("//OrderRequestHeader").Attributes(1).Text
     If useOrderNumberForName="True" Then
       docReceived.Save Request.ServerVariables("APPL_PHYSICAL_PATH") & "OrderRequests\" & orderNumber & ".xml"
     Else
       docReceived.Save Request.ServerVariables("APPL_PHYSICAL_PATH") & "OrderRequests\" & fileName & ".xml"
     End if
     Set docReceived = Nothing
  End Function
  
  Public Function ReturnOrderResponse(isValid, payLoadID)
    
    Dim returnXML
    Session("key")=12
    returnXML = "<?xml version=""1.0"" encoding=""UTF-8""?>"
    returnXML = returnXML &  "<!DOCTYPE cXML SYSTEM ""http://xml.cXML.org/schemas/cXML/1.1.007/cXML.dtd"">"
    returnXML = returnXML &  "<cXML payloadID='"  & Session("key") & "'  xml:lang=""en-US"" timestamp='" &  Date() & "'>"
    returnXML = returnXML &  "<Response>"


           IF isValid="True" AND Err.number=0 Then
              returnXML  = returnXML &  "<Status code=""200"" text=""OK""/>"
           ELSE
              returnXML  = returnXML &  "<Status code=""400"" text=""FAIL""/>"
           End IF

            

            returnXML = returnXML &  "</Response></cXML>"
            
            Response.Clear
            Response.ContentType = "text/xml"
            Response.Write returnXML
            'Response.End
    
  
  End Function


End Class


 %>

