<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Calendar.ascx.cs" Inherits="Calender" %>
<link href="css/smoothness/jquery-ui-1.7.1.custom.css" rel="stylesheet" type="text/css" />
<link href="css/smoothness/callout.css" rel="stylesheet" type="text/css" />
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/jquery-ui-1.7.1.custom.min.js" type="text/javascript"></script>
<script src="Scripts/jquery.numeric.js" type="text/javascript"></script>
<script src="Scripts/jquery.alphanumeric.pack.js" type="text/javascript"></script>
<script src="Scripts/popup.js" type="text/javascript"></script>
<script src="Scripts/jquery.callout.js" type="text/javascript"></script>
<style type="text/css">
	.othermonthdays
	{
		display: none;
	}
</style>
<script type="text/javascript">
	$(function () {
		$(".calendar").live('mouseover', function () {
			if ($(this).attr("dayofweek") == "Sunday") {
				$(".calendar").callout("hide");
				$(this).callout({ css: "colors1", position: "right", msg: $(this).attr("msg") });
			} else {
				$(".calendar").callout("hide");
				$(this).callout({ css: "colors1", position: "left", msg: $(this).attr("msg") });
			}
		
		});
		$(".calendar").live('mouseout', function () {
			$(".calendar").callout("hide");
		});
	});


</script>
<div>
	<div>
		<asp:Label ID="lblErrorMessage" runat="server" Visible="False"></asp:Label>
	</div>
	<div>
		<span style="float: left; margin-top: -13px;">
			<asp:DropDownList runat="server" ID="Dddepartment" AutoPostBack="True" Visible="False">
				<asp:ListItem Value="0">Show All</asp:ListItem>
				<asp:ListItem Value="12">ADMIN</asp:ListItem>
				<asp:ListItem Value="8">ACCOUNTS</asp:ListItem>
				<asp:ListItem Value="6">BD</asp:ListItem>
				<asp:ListItem Value="5">DESIGN</asp:ListItem>
				<asp:ListItem Value="7">HR</asp:ListItem>
				<asp:ListItem Value="4">MAD</asp:ListItem>
				<asp:ListItem Value="1">MS.NET</asp:ListItem>
				<asp:ListItem Value="11">NETWORKING</asp:ListItem>
				<asp:ListItem Value="2">PHP</asp:ListItem>
				<asp:ListItem Value="9">QA</asp:ListItem>
				<asp:ListItem Value="3">RIA</asp:ListItem>
				<asp:ListItem Value="10">SEO</asp:ListItem>
			</asp:DropDownList>
		</span>
		<br />
		<span style="margin-top: -20px;">
			<asp:Calendar ID="Calendar1" runat="server" Width="100%" SelectionMode="None" ForeColor="#238EC6"
				Height="400px" DayNameFormat="Full" CaptionAlign="NotSet" Font-Size="Small" NextPrevFormat="FullMonth"
				DayHeaderStyle-HorizontalAlign="NotSet" DayStyle-HorizontalAlign="Right" DayStyle-VerticalAlign="Top"
				DayStyle-Wrap="True" BorderColor="#D8D8D8">
				<TitleStyle Font-Bold="True" />
				<TodayDayStyle ForeColor="Black" BackColor="#FFFACD"></TodayDayStyle>
				<DayStyle HorizontalAlign="Right" VerticalAlign="Top" Wrap="True"></DayStyle>
				<NextPrevStyle ForeColor="#222222"></NextPrevStyle>
			</asp:Calendar>
		</span>
	</div>
</div>
<%--<OtherMonthDayStyle CssClass="othermonthdays"></OtherMonthDayStyle>--%>