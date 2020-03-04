<%@ Page Language="C#" Theme="Default" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="applyleave" TagName="footer" Src="~/ApplyLeave.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server" style="padding: 1px 1px 1px 1px;"><%--style="padding: 10px 10px 10px 10px;"--%>
    <div class="round-box" >
        <applyleave:footer runat="server" />
    </div>
    </form>
</body>
</html>
