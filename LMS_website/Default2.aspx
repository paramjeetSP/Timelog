<%@ Page Language="C#" Theme="Default" AutoEventWireup="true" CodeFile="Default2.aspx.cs"  Async="true"
    Inherits="Default2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="padding: 5px 5px 5px 5px;">
    <div class="round-box">
        <asp:Label ID="lblErrorMessage" runat="server" Text="" Visible="false"></asp:Label></div>
    <div>
        <asp:TextBox ID="TxtEmp_id" runat="server"></asp:TextBox>
        <asp:Button ID="btnsend" runat="server" Text="ok" OnClick="btnsend_Click" />
    </div>
    </form>
</body>
</html>
