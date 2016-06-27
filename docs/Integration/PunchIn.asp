

<%

Class PunchIn


  Public Function CreatePunchIn(payLoadID, supplierPartID, supplierPartAuxID, postToURL)
         
     Set xDoc = CreateObject("Microsoft.XMLDOM") 
     xDoc.ValidateOnParse= False
     '**change these FUCKING stupid map path paths to this vir dir
     'Response.Write Server.MapPath("/Integraions") & "\OrderRequests\" & key & ".xml"
     xDoc.Load Server.MapPath("/Integration") & "\cXmlDocuments\PunchOutOrderMessage.xml"
     Dim xDocNew
    
     Set xDocNew = xDoc
     xDocNew.SelectSingleNode("//cXML/").Attributes(1).text = payLoadID
     xDocNew.SelectSingleNode("//cXML/").Attributes(2).text = Now()
     xDocNew.SelectSingleNode("//SupplierPartID").text = supplierPartID
     xDocNew.SelectSingleNode("//SupplierPartAuxiliaryID").text = supplierPartAuxID
     'xDocNew.Save 
    
     Dim formPost
     formPost = "<form name='submitForm' action='" & postToURL & "' Method='POST'>"
     formPost = formPost  & "<input name='cxml-urlencoded' type='hidden' value='" & Server.UrlEncode(xDocNew.xml) & "'>"
     formPost = formPost  & "</form>" 
     formPost = formPost  & "<br><script>document.submitForm.submit()</script>"
     Response.Write formPost
 
  End Function

End Class

%>



