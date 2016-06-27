


<%

Class PunchOut


Public Property Get itemNumber()
      Set xDoc = CreateObject("MSXML2.DOMDocument.5.0") 
      xDoc.async = false
      xDoc.validateOnParse = false
      xDoc.ResolveExternals = False
      xDoc.LoadXml Session("keyXML")
      itemNumber = xDoc.SelectSingleNode("//SupplierPartID").Text
      Set xDoc = Nothing
End Property


Public Property Get itemDescription()
      Set xDoc = CreateObject("MSXML2.DOMDocument.5.0")
      xDoc.async = false
      xDoc.validateOnParse = false
      xDoc.ResolveExternals = False 
      xDoc.LoadXml Session("keyXML")
      itemDescription = xDoc.SelectSingleNode("//SupplierPartAuxiliaryID").Text
      Set xDoc = Nothing
End Property

Public Property Get dcgEndPoint()
      Set xDoc = CreateObject("MSXML2.DOMDocument.5.0") 
      xDoc.async = false
      xDoc.validateOnParse = false
      xDoc.ResolveExternals = False
      xDoc.LoadXml Session("keyXML")
      dcgEndPoint = xDoc.SelectSingleNode("//SupplierSetup/URL").Text
      Set xDoc = Nothing
End Property

Public Property Get userEmail()
      Dim xDoc
      Set xDoc = CreateObject("MSXML2.DOMDocument.5.0") 
      xDoc.async = false
      xDoc.validateOnParse = false
      xDoc.ResolveExternals = False
      xDoc.LoadXml Session("keyXML")
      userEmail = xDoc.SelectSingleNode("//Email").Text
      Set xDoc = Nothing
End Property

Public Property Get vendorToken()
      Set xDoc = CreateObject("MSXML2.DOMDocument.5.0") 
      xDoc.async = false
      xDoc.validateOnParse = false
      xDoc.ResolveExternals = False
      xDoc.LoadXml Session("keyXML")
      vendorToken = xDoc.SelectSingleNode("//SharedSecret").Text
      Set xDoc = Nothing
End Property

Public Property Get browserFormPostURL()
      Set xDoc = CreateObject("MSXML2.DOMDocument.3.0") 
      xDoc.async = false
      xDoc.validateOnParse = false
      xDoc.ResolveExternals = False
      xDoc.LoadXml Session("keyXML")
      browserFormPostURL = xDoc.SelectSingleNode("//BrowserFormPost/URL").Text
End Property

Public Property Get userName()
      Set xDoc = CreateObject("MSXML2.DOMDocument.5.0") 
      xDoc.async = false
      xDoc.validateOnParse = false
      xDoc.ResolveExternals = False
      xDoc.LoadXml Session("keyXML")
      userName = xDoc.SelectSingleNode("//Extrinsic").Text
      Set xDoc = Nothing
End Property



Function CreateGUID(tmpLength)
  Randomize Timer
  Dim tmpCounter,tmpGUID
  Const strValid = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ"
  For tmpCounter = 1 To tmpLength
    tmpGUID = tmpGUID & Mid(strValid, Int(Rnd(1) * Len(strValid)) + 1, 1)
  Next
  CreateGUID = tmpGUID
End Function



Public Function LoadIncomingPayload
  set xmldoc=Server.createobject("msxml2.domdocument.5.0")
  xmldoc.async = false   
  xmldoc.validateOnParse=false
  Xmldoc.Resolveexternals=false
  xmldoc.preservewhitespace = false
  
  xmldoc.load request 
   

  '**** What this method does is create a physical artifact for each
  '**** Session.  We have to do this becuase as the user is directed
  '**** Back and forth between our app and D2S, we lose state
  '**** we append this guid on the querystring and when the user
  '**** directed back from D2S, we look at the querystring id and pick
  '**** Up the file and then reset all of our session data based on that
 
  
  
   Dim guid
   guid = CreateGUID(16)
   '*** here we want to store that session key
   Session("key") = guid
   xmldoc.save Request.ServerVariables("APPL_PHYSICAL_PATH") & "PunchOutSessions\" &  guid & ".xml"
   'Response.End
End Function

Public Function ValidateSharedSecret(vendorToken, localToken)
  Dim isValid 
  isValid = "True"
  If vendorToken <> localToken Then
    isValid = "False"
  End if
  
  ValidateSharedSecret = isValid

End Function

Public Function GetExistingSession(key)
     Set xDoc = CreateObject("MSXML2.DOMDocument.5.0") 
     xDoc.async = false
     xDoc.ValidateOnParse = false
     xDoc.ResolveExternals = false
     xDoc.Load Request.ServerVariables("APPL_PHYSICAL_PATH") & "PunchOutSessions\" & key & ".xml"
     
     Session("key") = key
     Session("keyXML") = xDoc.xml
     'now that we have that xml we can load our properties or session values
     'with the data
     Set xDoc = Nothing
End Function

Public Function CreatePunchOutResponse(isError, personalizationURL)
  '**** If there is a reason to pass in an error, you can do that here
  '**** to send back a failure response
  Dim returnXML 
  returnXML = returnXML & "<?xml version=""1.0"" encoding=""UTF-8""?>"
  '***** Note we have to uncomment the below, it will be fine with D2S but I can't
  '***** Get ASP classic to ignore the DTD declaration
  returnXML = returnXML & "<!DOCTYPE cXML SYSTEM ""http://xml.cXML.org/schemas/cXML/1.1.010/cXML.dtd"">"
  returnXML = returnXML & "<cXML payloadID=""456778-199@cxml.workchairs.com"" xml:lang=""en-US"" timestamp=""1999-03-12T18:39:09-08:00"">"
  returnXML = returnXML & "<Response>"
            if isError="true" THEN
                returnXML = returnXML & "<Status code=""400"" text=""FAILURE""/>"
                returnXML = returnXML & "<PunchOutSetupResponse>"
                returnXML = returnXML & "<StartPage><URL>" + personalizationURL + "</URL></StartPage>"
                returnXML = returnXML & "</PunchOutSetupResponse></Response></cXML>"

            ELSE
                returnXML = returnXML & "<Status code=""200"" text=""OK""/>"
                returnXML = returnXML & "<PunchOutSetupResponse>"
                returnXML = returnXML & "<StartPage><URL>" + personalizationURL + "</URL></StartPage>"
                returnXML = returnXML & "</PunchOutSetupResponse></Response></cXML>"
            End if
  
  CreatePunchOutResponse = returnXML
End Function


End Class
%>


