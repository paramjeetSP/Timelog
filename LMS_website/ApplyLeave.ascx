<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ApplyLeave.ascx.cs" Inherits="ApplyLeave" %>
<%@ Register Src="Calendar.ascx" TagName="ControlCalendar" TagPrefix="ctrlCalendar" %>
<link href="css/smoothness/jquery-ui-1.7.1.custom.css" rel="stylesheet" type="text/css" />
<link href="css/smoothness/callout.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css" integrity="sha384-ggOyR0iXCbMQv3Xipma34MD+dH/1fQ784/j6cY/iJTQUOhcWr7x9JvoRxT2MZw1T" crossorigin="anonymous">
<script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
<script src="Scripts/jquery-ui-1.7.1.custom.min.js" type="text/javascript"></script>
<script src="Scripts/jquery.numeric.js" type="text/javascript"></script>
<script src="Scripts/jquery.alphanumeric.pack.js" type="text/javascript"></script>
<script src="Scripts/popup.js" type="text/javascript"></script>
<script src="Scripts/jquery.callout.js" type="text/javascript"></script>
<style type="text/css">
    .test1 {
        background-color: brown;
    }

    .half-check-wrapper {
        display: inline-flex;
        align-items: center;
        margin-left: 20px;
    }

    .leave-status {
        display: inline-block;
        vertical-align: top;
        width: 50%;
    }

    .leave-round table {
        display: inline-block;
        vertical-align: text-top;
    }

    .round-box {
        padding: 20px;
    }

    .status-bar td {
        padding: 3px 2px;
    }

    table.status-bar {
        margin-right: 50px;
    }

    .status-bar-lwp td {
        padding: 3px 2px;
    }

    .status-bar td:first-child {
        padding: 3px 7px;
        display: flex;
        flex: 0 0 3%;
        max-width: 3%;
    }

    .status-bar td {
        padding: 3px 7px;
        display: flex;
        flex: 0 0 25%;
        max-width: 25%;
    }

    .status-bar tr {
        display: flex;
    }

    .status-bar thead tr td:first-child {
        display: flex;
        flex: 0 0 25%;
        max-width: 25%;
    }

    .status-bar td span {
        margin: 0 7px;
    }

    thead {
        font-weight: bold;
    }

    span#ctl02_lblAllowedPaidLeaves {
        margin-right: 0;
    }

    .leave-wrapper {
        display: inline-block;
        width: 49%;
        float: right;
    }

        .leave-wrapper p {
            display: inline-block;
            font-weight: 600;
        }

        .leave-wrapper tr td, .leave-wrapper tr th {
            padding: 5px 10px;
        }

    span.half-day {
        margin-right: 4px;
    }
     .loader-wrapper {
    width: 100%;
    height: 100vh;
    position: fixed;
    z-index: 999;
    left: 0;
    top: 0;
    background: black;
    opacity: 0.5;
}
    .loader-wrapper img {
    position: absolute;
    left: 50%;
    top: 50%;
    transform: translate(-50%,-50%);
}
</style>
<script type="text/javascript">
    $(document).ready(function () {
          $(".loader-wrapper").css("display", "none");
        $(".cal").datepicker({
            changeMonth: true,
            changeYear: true,
            yearRange: "-70:+20"
        });
        //		var id = "1";
        //		if (id == "1") {
        //			id = "2";
        //			document.getElementById('ctl02_LnkBtnViewAppliedLeaveStatus').click();
        //			
        //			alert(id);
        //			return false;
        //		}


    });

    function PreventPressingChar(e) {
        return false;
    }
</script>
<script type="text/javascript">

    <%--function UserLeaveConfirmation() {
      debugger      
        if (Page_ClientValidate()) {
            var DdlLeaveType = $("#ctl02_DdlLeaveType").val();

            var TotalLeaves = parseInt('<%= Session["TotalLeaves"] ?? "" %>');
         var SickLeavesLeft = parseInt('<%= Session["SickLeavesLeft"] ?? "" %>');
         var CasualLeavesLeft = parseInt('<%= Session["CasualLeavesLeft"] ?? "" %>');
         var PaidLeavesLeft = parseInt('<%= Session["PaidLeavesLeft"] ?? "" %>');
         if (CasualLeavesLeft == 0 && SickLeavesLeft == 0 && SickLeavesLeft == 0 && PaidLeavesLeft == 0) {
             if (confirm("Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                 return true;
             else
                 return false;
         }
         else if (CasualLeavesLeft == 0 && DdlLeaveType == 1) {
             if (confirm("Casual Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                 return true;
             else
                 return false;
         }
         else if (SickLeavesLeft == 0 && DdlLeaveType == 3) {
             if (confirm("Sick Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                 return true;
             else
                 return false;
         }
         else if (PaidLeavesLeft == 0 && DdlLeaveType == 5) {
             if (confirm("Paid Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                 return true;
             else
                 return false;
         }
         else if (TotalLeaves == 0) {
             if (confirm("Total Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                 return true;
             else
                 return false;
         }
     }
 }--%>
    function UserLeaveConfirmation() {   
          $(".loader-wrapper").css("display", "block");
        if (Page_ClientValidate()) {    
            var DdlLeaveType = $("#ctl02_DdlLeaveType").val();
            var IsProbation = parseInt('<%= Session["IsProbationLeave"] ?? "" %>');
                var TotalLeaves = parseFloat('<%= Session["TotalLeaves"] ?? "" %>');
                var SickLeavesLeft = parseFloat('<%= Session["SickLeavesLeft"] ?? "" %>');
                var CasualLeavesLeft = parseFloat('<%= Session["CasualLeavesLeft"] ?? "" %>');
            var PaidLeavesLeft = parseInt('<%= Session["PaidLeavesLeft"] ?? "" %>');
            if (IsProbation == 0) {
                //if (CasualLeavesLeft == 0 && SickLeavesLeft == 0 && PaidLeavesLeft == 0)
            
                //{
                //    if (confirm("Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                //        return true;
                //    else
                //        return false;
                //}
                if ((CasualLeavesLeft+PaidLeavesLeft) == 0 && DdlLeaveType == 1) {
                    if (confirm("Casual Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                        return true;
                    else
                    {
                        $(".loader-wrapper").css("display", "none");
                        return false;
                    }
                }
                else if ((SickLeavesLeft+PaidLeavesLeft) == 0 && DdlLeaveType == 3) {
                    if (confirm("Sick Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                        return true;
                     else
                    {
                        $(".loader-wrapper").css("display", "none");
                        return false;
                    }
                }
                else if ((PaidLeavesLeft+CasualLeavesLeft)==0 && DdlLeaveType == 5) {
                    if (confirm("Earned Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                        return true;
                    else
                    {
                        $(".loader-wrapper").css("display", "none");
                        return false;
                    }
                }
                else if (TotalLeaves == 0) {
                    if (confirm("Total Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                        return true;
                    else
                    {
                        $(".loader-wrapper").css("display", "none");
                        return false;
                    }
                }
            }
            else
            {
             if (TotalLeaves == 0) {
                    if (confirm("Total Leaves alloted to you are finished. Leaves taken in future will be declared as Leave Without Pay."))
                        return true;
                    else
                    {
                        $(".loader-wrapper").css("display", "none");
                        return false;
                    }
                }
            }
    }

 }
</script>
<script type="text/javascript">

    $(function () {

        //for comment status

        $(".memo").mouseover(function () {
            $(".memo").callout("hide");
            $(this).callout({ css: "yellow", position: "left", msg: $(this).attr("msg") });
        });
        $(".memo").mouseout(function () {
            $(".memo").callout("hide");
        });




        //for leave status

        $(".leavestatus").mouseover(function () {
            $(".leavestatus").callout("hide");
            $(this).callout({ css: "black1", position: "left", msg: $(this).attr("msg") });
        });
        $(".leavestatus").mouseout(function () {
            $(".leavestatus").callout("hide");
        });
    });


    function checkTextAreaMaxLength(textBox, e, length) {

        var mLen = textBox["MaxLength"];
        if (null == mLen)
            mLen = length;
        var maxLength = parseInt(mLen);
        if (!checkSpecialKeys(e)) {
            if (textBox.value.length > maxLength - 1) {
                if (window.event)//IE
                    e.returnValue = false;
                else//Firefox
                    e.preventDefault();
            }
        }
    }
    function checkSpecialKeys(e) {
        if (e.keyCode != 8 && e.keyCode != 46 && e.keyCode != 37 && e.keyCode != 38 && e.keyCode != 39 && e.keyCode != 40)
            return false;
        else
            return true;
    }

</script>
<div class="loader-wrapper" style="display:none;">
                <img style="max-width:70px;" src="image/loadloop.gif" />
            </div>
<div class="rt-heading-small">
    <table>
        <tr>
            <td>
                <asp:LinkButton ID="LnkBtnApplyLeave" runat="server" OnClick="LnkBtnleave_Click"
                    CssClass="round-btn-small">Apply Leave</asp:LinkButton>
            </td>
            <td>
                <%--<div style="background-repeat: no-repeat; background-image: url('App_Themes/Default/images/circle16px.png');
									font-size: 11px; color: coral; padding: 1px; margin-left:15px;float:right;">
									12</div>--%>
                <asp:LinkButton runat="server" ID="LnkBtnMyActionItems" CssClass="round-btn-small"
                    OnClick="LnkBtnleave_Click">My Action Items</asp:LinkButton>
                <%--<asp:Literal runat="server" ID="LiteralMyActionItemsCount"><div style="background-repeat: no-repeat; background-image: url('App_Themes/Default/images/circle.png'); position:absolute; top: 4px; left: 946px; color:coral;padding:2px;" >
						125</div></asp:Literal>--%>
            </td>
            <td>
                <asp:LinkButton ID="LnkBtnViewLeaveStatus" runat="server" OnClick="LnkBtnleave_Click"
                    CssClass="round-btn-small">My Leaves</asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="LnkBtnViewAppliedLeaveStatus" runat="server" OnClick="LnkBtnleave_Click"
                    CssClass="round-btn-small ">Manage Leaves</asp:LinkButton>
            </td>
            <td>
                <asp:LinkButton ID="LnkBtnCalendar" runat="server" OnClick="LnkBtnleave_Click" CssClass="round-btn-small">Calendar</asp:LinkButton>
            </td>
            <td>
                <asp:Label ID="LblEmpName" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</div>
<div>
    <br />
    <div>
        <asp:Label ID="lblErrorMessage" runat="server"></asp:Label>
        <asp:HiddenField runat="server" ID="HiddenField1" />

    </div>
    <asp:Panel ID="PnlApplyLeave" runat="server" Visible="false">
        <div class="round-box-small">
            Apply Leave
        </div>
        <div class="round-box leave-round">
            <table>
                <tr>
                    <td valign="top">Leave Type
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="DdlLeaveType" runat="server" Height="18px" Width="135px" name="abc">
                            <asp:ListItem Text="Red" Value="red"></asp:ListItem>
                        </asp:DropDownList>
                        <div class="half-check-wrapper"><span class="half-day">Half Day</span>
                            <asp:CheckBox ID="CheckBoxhalfday" class="half-check" runat="server" /></div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="DdlLeaveType"
                            ErrorMessage="Select leave type from list ." runat="server" Display="None" ValidationGroup="leave"
                            InitialValue="0">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top">From Date
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="TxtFromDate" SkinID="cal" runat="server" autocomplete="off" onkeydown="return PreventPressingChar(event);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldFromDate" ControlToValidate="TxtFromDate"
                            ErrorMessage="From date is required." runat="server" Display="None" ValidationGroup="leave">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top">To Date
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="TxtToDate" runat="server" SkinID="cal" autocomplete="off" onkeydown="return PreventPressingChar(event);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="TxtToDate"
                            ErrorMessage="To date is required." runat="server" Display="None" ValidationGroup="leave">
                        </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="CompareValidator3" runat="server" ControlToCompare="TxtToDate"
                            ControlToValidate="TxtFromDate" ErrorMessage="To Date should be greater than From Date."
                            Display="None" Type="Date" Operator="LessThanEqual" ValidationGroup="leave" Text=""
                            ForeColor="Red"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top">Leave Reason
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="TxtLeaveReason" runat="server" TextMode="MultiLine" Height="70px"
                            Width="200px" MaxLength='500' onkeyDown="checkTextAreaMaxLength(this,event,'480');"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="TxtLeaveReason"
                            ErrorMessage="Leave reason is required." runat="server" Display="None" ValidationGroup="leave">
                        </asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="BtnSave" runat="server" Text="Apply" OnClick="BtnSave_Click" CausesValidation="true"
                            ValidationGroup="leave" OnClientClick="return UserLeaveConfirmation()" />
                    </td>
                </tr>
            </table>

            <% if (Session["IsProbationLeave"].ToString() == "1")
                { %>
            <div class="leave-wrapper">
                <p>Total Allocated Leave <asp:Label ID="lblAllowedTotalLeaves" runat="server" Text="1"></asp:Label></p>
                <table class="table">
                    <thead>
                        <tr>
                            <th scope="col">Leave Type</th>
                            <th scope="col">Leave Taken</th>
                            <th scope="col">Leave Left</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Casual Leave</td>
                            <td>
                                <asp:Label ID="lblTotalLeaveTaken" runat="server" Text="1"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblTotalLeavesLeft" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Leave Without Pay</td>
                            <td>
                                <asp:Label ID="lblProbLWP" runat="server" Text="Label"></asp:Label></td>
                            <td>N/A</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <% } %>
            <%else { %>
            <div class="leave-wrapper">
                <%--<p>Total Allocated Leaves <asp:Label ID="lblTotalAllocatedLeave" runat="server" Text="1"></asp:Label> (Casual Leaves <asp:Label ID="lblAllowedCasualLaves" runat="server" Text="1"></asp:Label> ,Sick Leaves <asp:Label ID="lblAllowedSickLeaves" runat="server" Text="1"></asp:Label>,Paid Leaves <asp:Label ID="lblAllowedPaidLeaves" runat="server" Text="1"></asp:Label> </p>--%>
                 <p>Total Allocated Leaves <asp:Label ID="lblAllowedLeaves" runat="server" Text="1"></asp:Label> (Casual Leaves <asp:Label ID="lblAllowedCasualLeaves" runat="server" Text="1"></asp:Label>, Sick Leaves <asp:Label ID="lblAllowedSickLeaves" runat="server" Text="1"></asp:Label>, Earned Leaves <asp:Label ID="lblAllowedPaidLeaves" runat="server" Text="1"></asp:Label>) </p>
                <table class="table">
                    <thead>
                        <tr>
                            <th scope="col">Leave Type</th>
                            <th scope="col">Leave Taken</th>
                            <th scope="col">Leave Left</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>Casual Leave</td>
                            <td>
                                <asp:Label ID="lblCasualLeavesTaken" runat="server" Text="Label"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblCasualLeavesLeft" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Sick Leave</td>
                            <td>
                                <asp:Label ID="lblSickLeavesTaken" runat="server" Text="Label"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblSickLeavesLeft" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Earned Leaves</td>
                            <td>
                                <asp:Label ID="lblPaidLeavesTakencount" runat="server" Text="Label"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblPaidLeavesLeftcount" runat="server" Text="Label"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>Leave Without Pay</td>
                            <td>
                                <asp:Label ID="lblLWP" runat="server" Text="Label"></asp:Label></td>
                            <td>N/A</td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <% } %>
        </div>
        <%--   <div class="leave-status" style="float: right;margin-top: 4px;">
        <table class="status-bar" border="0">
            
         
            <thead>
            <tr>                        
                <td>
                 Total Leaves
               <asp:Label id="lblAllowedTotalLeaves" runat="server" Text="Label" ></asp:Label>   
                </td>
                <td>
                (Casual Leaves
               <asp:Label id="lblAllowedCasualLeaves" runat="server" Text="Label" ></asp:Label>   
                </td>
                <td>
                Sick Leaves
               <asp:Label id="lblAllowedSickLeaves" runat="server" Text="Label" ></asp:Label>   
                </td>
                <td>
                Paid Leaves
               <asp:Label id="lblAllowedPaidLeaves" runat="server" Text="Label" ></asp:Label>)   
                </td>
            </tr>
                </thead>
             <tr>
                 <td colspan="2"></td>
            </tr>
            <tr>
                 <td>
                    Leave Type
                </td>
                 <td>
                    Leave Taken
                </td>
                 <td>
                    Leave Left
                </td>
            </tr>
             <tr>
                 <td colspan="2"></td>
            </tr>
            <tr>
               
                </td>
                <td>
                    Casual Leaves
                </td>
                 <td>
                     <asp:Label id="lblCasualLeavesTaken" runat="server" Text="Label" ></asp:Label>                     
                </td>
                <td>
                     <asp:Label id="lblCasualLeavesLeft" runat="server" Text="Label" ></asp:Label>                    
                </td>              
            </tr>
            <tr>
                 <td colspan="2"></td>
            </tr>
            <tr>
              
                </td>
                <td>
                    Sick Leaves
                </td>
                 <td>
                     <asp:Label id="lblSickLeavesTaken" runat="server" Text="Label" ></asp:Label>                     
                </td>
                <td>
                     <asp:Label id="lblSickLeavesLeft" runat="server" Text="Label" ></asp:Label>                      
                </td>
               
            </tr>
             <tr>
                 <td colspan="2"></td>
            </tr>
            <tr>
            
                </td>
                <td>
                     Paid Leaves
                </td>
                <td>
                     <asp:Label id="lblPaidLeavesTakencount" runat="server" Text="Label" ></asp:Label>                   
                </td>
                <td>
                     <asp:Label id="lblPaidLeavesLeftcount" runat="server" Text="Label" ></asp:Label>                   
                </td>
                 
            </tr>
             <tr>
                 <td colspan="2"></td>
            </tr>
               <tr>
              
                </td>
         <td>Leave Without Pay</td>
                <td>
               <asp:Label id="lblLWP" runat="server" Text="Label" ></asp:Label>                  
                </td>
                   <td>
                       <span>N/A</span> 
                   </td>
            </tr>

             
        </table>
                 
    </div>--%>

        <asp:ValidationSummary runat="server" ShowMessageBox="true" EnableClientScript="true"
            ValidationGroup="leave" ShowSummary="false" />
</div>

</asp:Panel>


<asp:Panel ID="PnlViewLeaveStatus" runat="server" Visible="false">
    <div class="round-box-small">
        My Leaves
    </div>
    <div class="round-box">
        <asp:ListView ID="LstViewEmployeeLeaveStatus" runat="server" OnSorting="LstViewEmployeeLeaveStatus_Sorting">
            <LayoutTemplate>
                <table class="grid" border="0" cellspacing="0" width="100%" id="yui">
                    <%--width="890px"--%>
                    <tr>
                        <th style="text-align: center;">Leave Type
                        </th>
                        <th style="text-align: center; vert-align: middle">
                            <div style="float: left">Applied On</div>
                            <div style="float: right">
                                <asp:ImageButton ID="imAppliedOn" CommandArgument="AppliedOn" CommandName="Sort" ImageUrl="~/image/dsc.jpg" runat="server" /></div>
                        </th>
                        <th style="text-align: center; vert-align: middle">
                            <div style="float: left; margin-left: 15px">Date </div>
                            <div style="float: right; margin-right: 25px">
                                <asp:ImageButton ID="imDate" CommandArgument="Date" CommandName="Sort" ImageUrl="~/image/dsc.jpg" runat="server" /></div>
                        </th>
                        <th style="text-align: center;">Total Days
                        </th>
                        <th style="text-align: center;">Applied For
                        </th>
                        <th style="text-align: center;">Leave Reason
                        </th>
                        <th style="text-align: center;">First Line Manager<br />
                        </th>
                        <th style="text-align: center;">Second Line Manager
                        </th>
                        <th style="text-align: center;">Hr Representative
                        </th>
                        <th style="text-align: center;">Leave Status
                        </th>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center" style="width: 10%">
                        <%# GetLeaveType(Convert.ToInt32(Eval("fkLeaveType").ToString()))%>
                    </td>
                    <td align="center" style="width: 13%">
                        <%# Eval("LeaveAppliedDate", "{0:dd-MMM-yyy}")%>
                    </td>
                    <td align="center" style="width: 10%">
                        <%# Eval("FromDate", "{0:dd-MMM-yyy}")%><br />
                        To
							<br />
                        <%# Eval("ToDate", "{0:dd-MMM-yyy}")%>
                    </td>
                    <td align="center" style="width: 7%">
                       <%-- <%# GetTotalDays(Convert.ToDateTime(Eval("FromDate")),Convert.ToDateTime(Eval("ToDate")))%>--%>
                         <%# GetTotalDaysCount(Convert.ToDateTime(Eval("FromDate")),Convert.ToDateTime(Eval("ToDate")),(Eval("IsHalfDay").ToString()))%>
                    </td>
                     <td align="center" style="width: 7%">
                         <%# (Eval("IsHalfDay")==null)?"Full Day": GetAppliedFor(Eval("IsHalfDay").ToString()) %>
                    </td>
                    <td align="left" style="width: 11%;">
                        <asp:Label ID="LblLeaveReason" runat="server" Text='<%# GetShortString(Convert.ToString(Eval("LeaveReason")),Convert.ToInt32("100")) %>'
                            CssClass="leavestatus" msg=' <%# (Eval("LeaveReason")=="")?"No Reason": Eval("LeaveReason") %>'></asp:Label>
                    </td>
                    <td align="center" valign="top" style="width: 14%;">
                        <%# (Eval("FirstLineManager_id") != null)?GetFullName(Eval("FirstLineManager_id").ToString()):"NA"%>
                        <br />
                        <br />
                        <asp:Label ID="Label1" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("FirstLineManagerStatus")),false,false)  %> '
                            Text='<%# GetClass(Convert.ToInt32(Eval("FirstLineManagerStatus")),true,false) %>'
                            msg=' <%# (Eval("FirstLineMangerComment")=="")?"No Comment": Eval("FirstLineMangerComment") %>'
                            ToolTip='<%# (Convert.ToInt32(Eval("FirstLineManagerStatus"))==0)?"Action Not Taken":null %>'></asp:Label><br />
                        <br />
                    </td>
                    <td align="center" valign="top" style="width: 15%;">
                        <%# (Eval("SecondLineManager_id") != null) ? GetFullName(Eval("SecondLineManager_id").ToString()) : "NA"%>
                        <br />
                        <br />
                        <asp:Label runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("SecondLineManagerStatus")),false,false)  %> '
                            Text='<%# GetClass(Convert.ToInt32(Eval("SecondLineManagerStatus")),true,false) %>'
                            msg=' <%# (Eval("SecondLineManagerComment")=="")?"No Comment": Eval("SecondLineManagerComment") %>'
                            ToolTip='<%# (Convert.ToInt32(Eval("SecondLineManagerStatus"))==0)?"Action Not Taken":null %>'></asp:Label><br />
                        <br />
                    </td>
                    <td align="center" valign="top" style="width: 12%;">
                        <%# (Eval("Hr_id") != null) ? GetFullName(Eval("Hr_id").ToString()) : "NA"%>
                        <br />
                        <br />
                        <asp:Label runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("Hr_Status")),false,false)  %> '
                            Text='<%# GetClass(Convert.ToInt32(Eval("Hr_Status")),true,false) %>' msg=' <%# (Eval("Hr_Comment")=="")?"No Comment": Eval("Hr_Comment") %>'
                            ToolTip='<%# (Convert.ToInt32(Eval("Hr_Status"))==0)?"Action Not Taken":null %>'></asp:Label><br />
                        <br />
                    </td>
                    <td align="center" valign="top" style="width: 10%;">
                        <p>
                            <br />
                            <asp:Label runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("EmpLeaveStatus")),false,true)  %> '
                                Text='<%# GetClass(Convert.ToInt32(Eval("EmpLeaveStatus")),true,true) %>' msg=' <%# (Eval("Admin_Comment")=="")?"No Comment": Eval("Admin_Comment") %>'></asp:Label>
                            <br />
                            <br />
                        </p>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                No Record Found
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
</asp:Panel>
<asp:Panel ID="PnlLeaveCommentStatus" runat="server" Visible="false">
    <div class="round-box-small">
        Manage Leaves
    </div>
    <div class="round-box" style="padding: 15px 2px 2px 2px;">
        <div style="float: left; padding-left: 5px;">
            <table style="font-size: 12px;">
                <tr>
                    <td>Pending
                    </td>
                    <td style="background-color: #A4A4A4; color: yellow; width: 20px; text-align: center; border: 1px solid yellow;"
                        valign="top">
                        <asp:Literal runat="server" ID="LiteralPending"></asp:Literal>
                    </td>
                    <td>Approved
                    </td>
                    <td style="background-color: #E0F8EC; color: #00A100; width: 20px; text-align: center; border: 1px solid #00A100;"
                        valign="top">
                        <asp:Literal runat="server" ID="LiteralApproved"></asp:Literal>
                    </td>
                    <td>Rejected
                    </td>
                    <td style="background-color: #F8E0E0; color: red; width: 20px; text-align: center; border: 1px solid red;"
                        valign="top">
                        <asp:Literal runat="server" ID="LiteralRejected"></asp:Literal>
                    </td>
                    <td>ANT
                    </td>
                    <td style="background-color: #ffffff; color: black; width: 20px; text-align: center; border: 1px solid #696969;"
                        valign="top">
                        <asp:Literal runat="server" ID="LiteralANT"></asp:Literal>
                    </td>
                    <td>Total Leaves
                    </td>
                    <td style="background-color: #F6F9FB; color: black; width: 20px; text-align: center; border: 1px solid #238EC6;"
                        valign="top">
                        <asp:Literal runat="server" ID="LiteralTotalLeaves"></asp:Literal>
                    </td>
                </tr>
            </table>
        </div>
        <span style="float: right; padding-right: 10px;">Filter By
				<asp:DropDownList runat="server" AutoPostBack="True" ID="DdlFilters" OnSelectedIndexChanged="DdlFilters_SelectedIndexChanged">
                    <asp:ListItem Value="0">Show All</asp:ListItem>
                    <asp:ListItem Value="1" Selected="True">Pending</asp:ListItem>
                    <asp:ListItem Value="2">Approved</asp:ListItem>
                    <asp:ListItem Value="3">Rejected</asp:ListItem>
                </asp:DropDownList>
            Show Records<asp:DropDownList ID="DdlShowRecord" runat="server" OnSelectedIndexChanged="DdlShowRecord_SelectedIndexChanged"
                AutoPostBack="true">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Selected="True" Value="20">20</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
                <asp:ListItem Value="100">100</asp:ListItem>
                <asp:ListItem Value="200">200</asp:ListItem>
            </asp:DropDownList>
        </span>
        <br />
        <span style="float: right; margin-top: 5px; clear: both;">
            <asp:DataPager ID="DataPagerManageleavestatus1" runat="server" PageSize="20" PagedControlID="LstViewViewAppliedLeaveStatus">
                <Fields>
                    <asp:NextPreviousPagerField ShowNextPageButton="False" PreviousPageText="« Previous"
                        ButtonCssClass="paging" />
                    <asp:NumericPagerField CurrentPageLabelCssClass="current-page" NumericButtonCssClass="paging" />
                    <asp:NextPreviousPagerField ShowPreviousPageButton="False" NextPageText="Next »"
                        ButtonCssClass="paging" />
                </Fields>
            </asp:DataPager>
        </span>
        <br />
        <br />
        <asp:ListView ID="LstViewViewAppliedLeaveStatus" OnItemDataBound="LstViewViewAppliedLeaveStatus_ItemDataBound" OnSorting="LstViewViewAppliedLeaveStatus_Sorting"
            OnPagePropertiesChanging="LstViewViewAppliedLeaveStatus_PagePropertiesChanging"
            OnItemCommand="LstViewViewAppliedLeaveStatus_ItemCommand" runat="server">
            <LayoutTemplate>
                <table class="grid" border="0" cellspacing="0" width="100%" id="yui">
                    <tr>
                        <th style="text-align: left;">Sr No.
                        </th>
                        <th style="text-align: left;">Employee Name
                        </th>
                        <th style="text-align: center;">Leave Type
                        </th>
                        <th style="text-align: center;">
                            <div style="float: left">Applied On</div>
                            <div style="float: right">
                                <asp:ImageButton ID="imAppliedOn" CommandArgument="AppliedOn" CommandName="Sort" ImageUrl="~/image/dsc.jpg" runat="server" /></div>
                        </th>
                        <th style="text-align: center;">
                            <div style="float: left; margin-left: 15px">Date</div>
                            <div style="float: right">
                                <asp:ImageButton ID="imDate" CommandArgument="Date" CommandName="Sort" ImageUrl="~/image/dsc.jpg" runat="server" /></div>
                        </th>
                        <th style="text-align: center;">Total Days
                        </th>
                        <th style="text-align: center;">Applied For
                        </th>
                        <th style="text-align: center;">Leave Reason
                        </th>
                        <th style="text-align: center;">First Line Manager
                        </th>
                        <th style="text-align: center;">Second Line Manager
                        </th>
                        <th style="text-align: center;">Hr Representative
                        </th>
                        <th style="text-align: center;">Leave Status
                        </th>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center" style="width: 5%;">
                        <a name="<%# Eval("ID") %>" style="text-decoration: none;"></a>
                        <asp:Label ID="LblSrNo" runat="server"></asp:Label>
                    </td>
                    <td align="left" style="width: 9%;">
                        <%# GetFullName(Eval("Emp_id").ToString())%>
							(<%# Eval("Emp_id") %>)
							<asp:Label ID="LblFromDate" runat="server" Visible="false" Text='<%# Eval("FromDate") %>'></asp:Label>
                    </td>
                    <td align="center" style="width: 9%;">
                        <%# GetLeaveType(Convert.ToInt32(Eval("fkLeaveType").ToString()))%>
                    </td>
                    <td align="center" style="width: 13%;">
                        <%# Eval("LeaveAppliedDate", "{0:dd-MMM-yyy}")%>
                    </td>
                    <td align="center" style="width: 10%;">
                        <%# Eval("FromDate", "{0:dd-MMM-yyy}")%><br />
                        To<br />
                        <%# Eval("ToDate", "{0:dd-MMM-yyy}")%>
                    </td>
                    <td align="center" style="width: 7%">
                      <%--  <%# GetTotalDays(Convert.ToDateTime(Eval("FromDate")),Convert.ToDateTime(Eval("ToDate")))%>--%>
                         <%# GetTotalDaysCount(Convert.ToDateTime(Eval("FromDate")),Convert.ToDateTime(Eval("ToDate")),(Eval("IsHalfDay").ToString()))%>
                    </td>
                     <td align="center" style="width: 7%">
                         <%# (Eval("IsHalfDay")==null)?"Full Day": GetAppliedFor(Eval("IsHalfDay").ToString()) %>
                    </td>
                    <td align="left" style="width: 10%;">
                        <asp:Label ID="LblLeaveReason" runat="server" Text='<%# GetShortString(Convert.ToString(Eval("LeaveReason")),Convert.ToInt32("100")) %>'
                            CssClass="leavestatus" msg=' <%# (Eval("LeaveReason")=="")?"No Reason": Eval("LeaveReason") %>'></asp:Label>
                    </td>
                    <td align="center" valign="top" style="width: 13%;">
                        <table>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%# (Eval("FirstLineManager_id") != null)?GetFullName(Eval("FirstLineManager_id").ToString()):"NA"%>
                                    <asp:Label ID="LblFirstLineMaganerid" runat="server" Visible="false" Text='<%# Eval("FirstLineManager_id") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:DropDownList ID="DdlFirstLineManagerStatus" Width="82px" Height="20px" runat="server"
                                        DataSource='<%# GetAllStatus() %>' DataTextField="Description" DataValueField="ID"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:LinkButton ID="LnkFirstLineManagerComment" runat="server" CommandName="FirstLineManager"
                                        Visible="false" CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:Label ID="LblFirstLineManagerStatus" Visible="false" runat="server" Text='<%# Eval("FirstLineManagerStatus")%>'></asp:Label>
                                    <asp:Label ID="LblFirstLineStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("FirstLineManagerStatus")),false,false)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("FirstLineManagerStatus")),true,false) %>'
                                        msg=' <%# (Eval("FirstLineMangerComment")=="")?"No Comment": Eval("FirstLineMangerComment") %>'
                                        ToolTip='<%# (Convert.ToInt32(Eval("FirstLineManagerStatus"))==0)?"Action Not Taken":null %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:TextBox ID="TxtFirstLineManagerComment" runat="server" Text='<%# Eval("FirstLineMangerComment")%>'
                                        Visible="false" TextMode="MultiLine" MaxLength='90' onkeyDown="checkTextAreaMaxLength(this,event,'90');"
                                        Height="40px" Width="120px" Font-Size="X-Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="LblErrorMsgFirstLineManager" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" valign="top" style="width: 17%;">
                        <table>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%# (Eval("SecondLineManager_id") != null) ? GetFullName(Eval("SecondLineManager_id").ToString()) : "NA"%>
                                    <asp:Label ID="LblSecoundLineManagerid" runat="server" Visible="false" Text='<%# Eval("SecondLineManager_id") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%--class="Checkstatus"--%>
                                    <asp:DropDownList ID="DdlSecoundLineManagerStatus" Width="82px" Height="20px" runat="server"
                                        DataSource='<%# GetAllStatus() %>' DataTextField="Description" DataValueField="ID"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:LinkButton ID="LnkBtnSecondLineManagerComment" runat="server" CommandName="SecoundLineManager"
                                        Visible="false" CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:Label ID="LblSecondLineManagerStatus" Visible="false" runat="server" Text='<%# Eval("SecondLineManagerStatus")%>'></asp:Label>
                                    <asp:Label ID="LblSecondLineStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("SecondLineManagerStatus")),false,false)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("SecondLineManagerStatus")),true,false) %>'
                                        msg=' <%# (Eval("SecondLineManagerComment")=="")?"No Comment": Eval("SecondLineManagerComment") %>'
                                        ToolTip='<%# (Convert.ToInt32(Eval("SecondLineManagerStatus"))==0)?"Action Not Taken":null %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:TextBox ID="TxtSecondLineManagerComment" runat="server" Text='<%# Eval("SecondLineManagerComment")%>'
                                        Visible="false" TextMode="MultiLine" Height="40px" Width="120px" MaxLength='90'
                                        onkeyDown="checkTextAreaMaxLength(this,event,'90');" Font-Size="X-Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="LblErrorMsgSecoundLineManager" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" valign="top" style="width: 12%;">
                        <table>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%# (Eval("Hr_id") != null) ? GetFullName(Eval("Hr_id").ToString()) : "NA"%>
                                    <asp:Label ID="LblHrid" runat="server" Visible="false" Text='<%# Eval("Hr_id") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:DropDownList ID="DdlHrManagerStatus" Width="82px" Height="20px" runat="server"
                                        DataSource='<%# GetAllStatus() %>' DataTextField="Description" DataValueField="ID"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:LinkButton ID="LnkBtnHrManagerComment" runat="server" CommandName="HrManager"
                                        Visible="false" CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:Label ID="LblHr_Status" runat="server" Text='<%# Eval("Hr_Status")%>' Visible="false"></asp:Label>
                                    <asp:Label ID="LblHrStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("Hr_Status")),false,false)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("Hr_Status")),true,false) %>' msg=' <%# (Eval("Hr_Comment")=="")?"No Comment": Eval("Hr_Comment") %>'
                                        ToolTip='<%# (Convert.ToInt32(Eval("Hr_Status"))==0)?"Action Not Taken":null %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:TextBox ID="TxtHrcomment" runat="server" Text='<%# Eval("Hr_Comment")%>' TextMode="MultiLine"
                                        Visible="false" Height="40px" Width="120px" MaxLength='90' onkeyDown="checkTextAreaMaxLength(this,event,'90');"
                                        Font-Size="X-Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="LblErrorMsgHrManager" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" style="width: 17%;">
                        <table>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:Label ID="LblEmpLeaveStatus" runat="server" Visible="false" Text='<%# Eval("EmpLeaveStatus")%>'></asp:Label>
                                    <asp:Label ID="LblAdminStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("EmpLeaveStatus")),false,true)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("EmpLeaveStatus")),true,false) %>' msg=' <%# (Eval("Admin_Comment")=="")?"No Comment": Eval("Admin_Comment") %>'></asp:Label>
                                    <asp:Label ID="LblLeaveAppliedDate" Text='<%# Eval("LeaveAppliedDate") %>' runat="server"
                                        Visible="false"></asp:Label>
                                </td>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:DropDownList ID="DdlAdmin" Width="82px" Height="20px" runat="server" DataSource='<%# GetAllStatus() %>'
                                        DataTextField="Description" DataValueField="ID" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:TextBox ID="TxtAdminComment" runat="server" Text='<%# Eval("Admin_Comment")%>'
                                        Visible="false" TextMode="MultiLine" Height="40px" Width="120px" MaxLength='90'
                                        onkeyDown="checkTextAreaMaxLength(this,event,'90');" Font-Size="X-Small"></asp:TextBox><br />
                                    <asp:Label ID="LblErrorMsgAdmin" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:CheckBox runat="server" ID="ChkBoxAdminSendMail" Checked="True" Text="Send Mail"
                                        Visible="False" />
                                    <asp:LinkButton ID="LnkBtnAdmin" runat="server" CommandName="Admin" Visible="false"
                                        CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                No Record Found
            </EmptyDataTemplate>
        </asp:ListView>
        <span style="float: right; margin-top: 5px;">
            <asp:DataPager ID="DataPagerManageleavestatus" runat="server" PageSize="20" PagedControlID="LstViewViewAppliedLeaveStatus">
                <Fields>
                    <asp:NextPreviousPagerField ShowNextPageButton="False" PreviousPageText="« Previous"
                        ButtonCssClass="paging" />
                    <asp:NumericPagerField CurrentPageLabelCssClass="current-page" NumericButtonCssClass="paging" />
                    <asp:NextPreviousPagerField ShowPreviousPageButton="False" NextPageText="Next »"
                        ButtonCssClass="paging" />
                </Fields>
            </asp:DataPager>
        </span>
        <br />
        <br />
        <br />
    </div>
</asp:Panel>
<asp:Panel ID="PnlMyActionItems" runat="server" Visible="false">
    <div class="round-box-small">
        My Action Items
    </div>
    <div class="round-box" style="padding: 15px 2px 2px 2px;">
        <asp:ListView ID="LstViewMyActionItems" OnItemDataBound="LstViewMyActionItems_ItemDataBound" OnSorting="LstViewMyActionItems_Sorting"
            OnItemCommand="LstViewMyActionItems_ItemCommand" runat="server">
            <LayoutTemplate>
                <table class="grid" border="0" cellspacing="0" width="100%" id="yui">
                    <tr>
                        <th style="text-align: left;">Sr No.
                        </th>
                        <th style="text-align: left;">Employee Name
                        </th>
                        <th style="text-align: center;">Leave Type
                        </th>
                        <th style="text-align: center;">
                            <div style="float: left">Applied On</div>
                            <div style="float: right">
                                <asp:ImageButton ID="imAppliedOn" CommandArgument="AppliedOn" CommandName="Sort" ImageUrl="~/image/dsc.jpg" runat="server" /></div>
                        </th>
                        <th style="text-align: center;">
                            <div style="float: left; margin-left: 15px">Date</div>
                            <div style="float: right">
                                <asp:ImageButton ID="imDate" CommandArgument="Date" CommandName="Sort" ImageUrl="~/image/dsc.jpg" runat="server" /></div>
                        </th>
                        <th style="text-align: center;">Total Days
                        </th>  
                          <th style="text-align: center;">Applied For
                        </th>   
                        <th style="text-align: center;">Leave Reason
                        </th>
                        <th style="text-align: center;">First Line Manager
                        </th>
                        <th style="text-align: center;">Second Line Manager
                        </th>
                        <th style="text-align: center;">Hr Representative
                        </th>
                        <th style="text-align: center;">Leave Status
                        </th>
                    </tr>
                    <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td align="center" style="width: 5%;">
                        <asp:Label ID="LblSrNo" runat="server"></asp:Label>
                    </td>
                    <td align="left" style="width: 9%;">
                        <%# GetFullName(Eval("Emp_id").ToString())%>
							(<%# Eval("Emp_id") %>)
							<asp:Label ID="LblFromDate" runat="server" Visible="false" Text='<%# Eval("FromDate") %>'></asp:Label>
                    </td>
                    <td align="center" style="width: 9%;">
                        <%# GetLeaveType(Convert.ToInt32(Eval("fkLeaveType").ToString()))%>
                    </td>
                    <td align="center" style="width: 13%;">
                        <%# Eval("LeaveAppliedDate", "{0:dd-MMM-yyy}")%>
                    </td>
                    <td align="center" style="width: 10%;">
                        <%# Eval("FromDate", "{0:dd-MMM-yyy}")%><br />
                        To<br />
                        <%# Eval("ToDate", "{0:dd-MMM-yyy}")%>
                    </td>
                  <%--  <td align="center" style="width: 7%">
                        <%# GetTotalDays(Convert.ToDateTime(Eval("FromDate")),Convert.ToDateTime(Eval("ToDate")))%>
                    </td>--%>
                    <td align="center" style="width: 7%">
                        <%# GetTotalDaysCount(Convert.ToDateTime(Eval("FromDate")),Convert.ToDateTime(Eval("ToDate")),(Eval("IsHalfDay").ToString()))%>
                    </td>     
                     <td align="center" style="width: 7%">
                         <%# (Eval("IsHalfDay")==null)?"Full Day": GetAppliedFor(Eval("IsHalfDay").ToString()) %>
                    </td>
                    <td align="left" style="width: 10%;">
                        <asp:Label ID="LblLeaveReason" runat="server" Text='<%# GetShortString(Convert.ToString(Eval("LeaveReason")),Convert.ToInt32("100")) %>'
                            CssClass="leavestatus" msg=' <%# (Eval("LeaveReason")=="")?"No Reason": Eval("LeaveReason") %>'></asp:Label>
                    </td>
                    <td align="center" valign="top" style="width: 13%;">
                        <table>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%# (Eval("FirstLineManager_id") != null)?GetFullName(Eval("FirstLineManager_id").ToString()):"NA"%>
                                    <asp:Label ID="LblFirstLineMaganerid" runat="server" Visible="false" Text='<%# Eval("FirstLineManager_id") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:DropDownList ID="DdlFirstLineManagerStatus" Width="82px" Height="20px" runat="server"
                                        DataSource='<%# GetAllStatus() %>' DataTextField="Description" DataValueField="ID"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:LinkButton ID="LnkFirstLineManagerComment" runat="server" CommandName="FirstLineManager"
                                        Visible="false" CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:Label ID="LblFirstLineManagerStatus" Visible="false" runat="server" Text='<%# Eval("FirstLineManagerStatus")%>'></asp:Label>
                                    <asp:Label ID="LblFirstLineStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("FirstLineManagerStatus")),false,false)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("FirstLineManagerStatus")),true,false) %>'
                                        msg=' <%# (Eval("FirstLineMangerComment")=="")?"No Comment": Eval("FirstLineMangerComment") %>'
                                        ToolTip='<%# (Convert.ToInt32(Eval("FirstLineManagerStatus"))==0)?"Action Not Taken":null %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:TextBox ID="TxtFirstLineManagerComment" runat="server" Text='<%# Eval("FirstLineMangerComment")%>'
                                        Visible="false" TextMode="MultiLine" MaxLength='90' onkeyDown="checkTextAreaMaxLength(this,event,'90');"
                                        Height="40px" Width="120px" Font-Size="X-Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="LblErrorMsgFirstLineManager" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" valign="top" style="width: 17%;">
                        <table>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%# (Eval("SecondLineManager_id") != null) ? GetFullName(Eval("SecondLineManager_id").ToString()) : "NA"%>
                                    <asp:Label ID="LblSecoundLineManagerid" runat="server" Visible="false" Text='<%# Eval("SecondLineManager_id") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%--class="Checkstatus"--%>
                                    <asp:DropDownList ID="DdlSecoundLineManagerStatus" Width="82px" Height="20px" runat="server"
                                        DataSource='<%# GetAllStatus() %>' DataTextField="Description" DataValueField="ID"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:LinkButton ID="LnkBtnSecondLineManagerComment" runat="server" CommandName="SecoundLineManager"
                                        Visible="false" CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:Label ID="LblSecondLineManagerStatus" Visible="false" runat="server" Text='<%# Eval("SecondLineManagerStatus")%>'></asp:Label>
                                    <asp:Label ID="LblSecondLineStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("SecondLineManagerStatus")),false,false)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("SecondLineManagerStatus")),true,false) %>'
                                        msg=' <%# (Eval("SecondLineManagerComment")=="")?"No Comment": Eval("SecondLineManagerComment") %>'
                                        ToolTip='<%# (Convert.ToInt32(Eval("SecondLineManagerStatus"))==0)?"Action Not Taken":null %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:TextBox ID="TxtSecondLineManagerComment" runat="server" Text='<%# Eval("SecondLineManagerComment")%>'
                                        Visible="false" TextMode="MultiLine" Height="40px" Width="120px" MaxLength='90'
                                        onkeyDown="checkTextAreaMaxLength(this,event,'90');" Font-Size="X-Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="LblErrorMsgSecoundLineManager" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" valign="top" style="width: 12%;">
                        <table>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <%# (Eval("Hr_id") != null) ? GetFullName(Eval("Hr_id").ToString()) : "NA"%>
                                    <asp:Label ID="LblHrid" runat="server" Visible="false" Text='<%# Eval("Hr_id") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:DropDownList ID="DdlHrManagerStatus" Width="82px" Height="20px" runat="server"
                                        DataSource='<%# GetAllStatus() %>' DataTextField="Description" DataValueField="ID"
                                        Visible="false">
                                    </asp:DropDownList>
                                </td>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:LinkButton ID="LnkBtnHrManagerComment" runat="server" CommandName="HrManager"
                                        Visible="false" CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:Label ID="LblHr_Status" runat="server" Text='<%# Eval("Hr_Status")%>' Visible="false"></asp:Label>
                                    <asp:Label ID="LblHrStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("Hr_Status")),false,false)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("Hr_Status")),true,false) %>' msg=' <%# (Eval("Hr_Comment")=="")?"No Comment": Eval("Hr_Comment") %>'
                                        ToolTip='<%# (Convert.ToInt32(Eval("Hr_Status"))==0)?"Action Not Taken":null %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center">
                                    <asp:TextBox ID="TxtHrcomment" runat="server" Text='<%# Eval("Hr_Comment")%>' TextMode="MultiLine"
                                        Visible="false" Height="40px" Width="120px" MaxLength='90' onkeyDown="checkTextAreaMaxLength(this,event,'90');"
                                        Font-Size="X-Small"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="LblErrorMsgHrManager" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="center" style="width: 17%; padding-top: 23px;" valign="top">
                        <table>
                            <tr>
                                <td style="border-top-style: none; border-bottom-style: none; border-right-style: none; border-left-style: none;"
                                    align="center" valign="top">
                                    <asp:Label ID="LblEmpLeaveStatus" runat="server" Visible="false" Text='<%# Eval("EmpLeaveStatus")%>'></asp:Label>
                                    <asp:Label ID="LblAdminStatusComment" runat="server" CssClass='<%# GetClass(Convert.ToInt32(Eval("EmpLeaveStatus")),false,true)  %> '
                                        Text='<%# GetClass(Convert.ToInt32(Eval("EmpLeaveStatus")),true,false) %>' msg=' <%# (Eval("Admin_Comment")=="")?"No Comment": Eval("Admin_Comment") %>'></asp:Label>
                                    <asp:Label ID="LblLeaveAppliedDate" Text='<%# Eval("LeaveAppliedDate") %>' runat="server"
                                        Visible="false"></asp:Label>
                                </td>
                                <%--	<td style="border-top-style: none; border-bottom-style: none; border-right-style: none;
										border-left-style: none;" align="center">
										<asp:DropDownList ID="DdlAdmin" Width="82px" Height="20px" runat="server" DataSource='<%# GetAllStatus() %>'
											DataTextField="Description" DataValueField="ID" Visible="false">
										</asp:DropDownList>
									</td>--%>
                                <%--</tr>--%>
                                <%--<tr>
									<td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none;
										border-left-style: none;" align="center">
										<asp:TextBox ID="TxtAdminComment" runat="server" Text='<%# Eval("Admin_Comment")%>'
											Visible="false" TextMode="MultiLine" Height="40px" Width="120px" MaxLength='90'
											onkeyDown="checkTextAreaMaxLength(this,event,'90');" Font-Size="X-Small"></asp:TextBox><br />
										<asp:Label ID="LblErrorMsgAdmin" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td colspan="2" style="border-top-style: none; border-bottom-style: none; border-right-style: none;
										border-left-style: none;" align="center">
										<asp:CheckBox runat="server" ID="ChkBoxAdminSendMail" Checked="True" Text="Send Mail"
											Visible="False" />
										<asp:LinkButton ID="LnkBtnAdmin" runat="server" CommandName="Admin" Visible="false"
											CommandArgument='<%# Eval("ID") %>'>Save</asp:LinkButton>
									</td>--%>
                            </tr>
                        </table>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <center>
						<b>No Record Found</b></center>
            </EmptyDataTemplate>
        </asp:ListView>
    </div>
</asp:Panel>
<asp:Panel ID="PnlCalendar" runat="server" Visible="False">
    <ctrlCalendar:ControlCalendar runat="server" />
</asp:Panel>

</div>
