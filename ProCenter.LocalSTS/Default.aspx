﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ProCenter.LocalSTS.Default" ValidateRequest="false" %>
<%@ OutputCache Location="None" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ASP.NET Security Token Service Web Site</title>
    <style type="text/css">
        .style1
        {
            font-size: large;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <b><span class="style1">Windows Identity Foundation - ASP.NET Security Token Service Web Site</span><br 
            class="style1" />
        </b>
        <b>Note : This test STS uses Forms based authentication with validation against pre-defined names/passwords:<br />
        <ul>
            <li>user01/password01</li>
            <li>user02/password02</li>
            <li>user03/password03</li>
        </ul><br />
         Use more secure authentication mode for production scenarios.</b>
    </div>
    </form>
</body>
</html>
