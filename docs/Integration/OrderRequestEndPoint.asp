<%@ Language="VBScript" %>
<!--#include file="OrderRequest.asp"-->
<!--#include file="PunchOut.asp"-->

<%

Dim orq
Set orq = new OrderRequest
Dim po
Set po = new PunchOut
Dim isValid
isValid = po.ValidateSharedSecret("johns","john")
orq.LoadOrderRequest "False","test"

orq.ReturnOrderResponse isValid,"12"
%>


