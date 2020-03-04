using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using BAL;
using System.Web.UI;
using BAL.ServiceReference1;
using MS.Internal.Xml.XPath;
using System.Data.SqlClient;
using System.Web.Security;
using System.Web;

public partial class ApplyLeave : System.Web.UI.UserControl
{

    LeaveOperation obj = new LeaveOperation();
    HRMSEntities1 enititiesobj;

    public object ClientScript { get; private set; }

    public ApplyLeave()
    {
        enititiesobj = new HRMSEntities1();
    }

    #region pageload

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Page.User.Identity.IsAuthenticated)
        {
            if (!IsPostBack)
            {
                try
                {
                    if (Session["IsProbationLeave"].ToString() == "0")
                    {
                        var iid = Request.QueryString["empid"];
                        var SickLeavesLeft = Session["SickLeavesLeft"].ToString();
                        var CasualLeavesLeft = Session["CasualLeavesLeft"].ToString();
                        var PaidLeavesLeft = Session["PaidLeavesLeft"].ToString();
                        var SickLeaveTaken = Session["SickLeaveTaken"].ToString();
                        var CasualLeaveTaken = Session["CasualLeaveTaken"].ToString();
                        var PaidLeaveTaken = Session["PaidLeaveTaken"].ToString();
                        var TotalLeaves = Session["TotalLeaves"].ToString();
                        var AllowedTotalLeaves = Session["AllowedTotalLeaves"].ToString();
                        var LWP = Session["LWP"].ToString();
                        var TotalLeaveTaken = Session["TotalLeaveTaken"].ToString();
                        var AllowedCasualLeaves = Session["AllowedCasualLeaves"].ToString();
                        var AllowedSickLeaves = Session["AllowedSickLeaves"].ToString();
                        var AllowedPaidLeaves = Session["AllowedPaidLeaves"].ToString();
                        //lblTotalLeavescount.Text = TotalLeaves;
                        lblPaidLeavesLeftcount.Text = PaidLeavesLeft;
                        lblPaidLeavesTakencount.Text = PaidLeaveTaken;
                        lblCasualLeavesLeft.Text = CasualLeavesLeft;
                        lblCasualLeavesTaken.Text = CasualLeaveTaken;
                        lblSickLeavesLeft.Text = SickLeavesLeft;
                        lblSickLeavesTaken.Text = SickLeaveTaken;
                        //  lblAllowedTotalLeaves.Text = AllowedTotalLeaves;
                        lblLWP.Text = LWP;
                        lblAllowedLeaves.Text = AllowedTotalLeaves;
                        lblAllowedCasualLeaves.Text = AllowedCasualLeaves;
                        lblAllowedSickLeaves.Text = AllowedSickLeaves;
                        lblAllowedPaidLeaves.Text = AllowedPaidLeaves;
                        lblTotalLeaveTaken.Text = TotalLeaveTaken;
                        if (obj.DiplayLeaveType() != null)
                        {
                            DdlLeaveType.DataSource = obj.DiplayLeaveType();

                            DdlLeaveType.DataTextField = "Description";
                            DdlLeaveType.DataValueField = "ID";
                            DdlLeaveType.DataBind();
                            DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
                            DdlLeaveType.Items.Insert(3, new ListItem("Earned Leave", "5"));
                        }
                        else
                            DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
                        clear();
                    }
                    else
                    {
                        var AllowedTotalLeaves = Session["AllowedTotalLeaves"].ToString();
                        var TotalLeavesLeft = Session["TotalLeaves"].ToString();
                        var TotalLeaveTaken = Session["TotalLeaveTaken"].ToString();
                        var LWP = Session["LWP"].ToString();
                        lblAllowedTotalLeaves.Text = AllowedTotalLeaves;
                        lblTotalLeaveTaken.Text = TotalLeaveTaken;
                        lblTotalLeavesLeft.Text = TotalLeavesLeft;
                        lblProbLWP.Text = LWP;
                        if (obj.DiplayProbLeaveType() != null)
                        {
                            DdlLeaveType.DataSource = obj.DiplayProbLeaveType();

                            DdlLeaveType.DataTextField = "Description";
                            DdlLeaveType.DataValueField = "ID";
                            DdlLeaveType.DataBind();
                            DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
                        }
                        else
                            DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
                        clear();
                    }
                    PnlApplyLeave.Visible = true;
                    LblEmpName.Text = "[ " + obj.GetFullName(Page.User.Identity.Name) + " ]";
                    lblErrorMessage.Visible = false;
                    BindListView("checklnkbtn");
                    if (Request.QueryString["id"] != null)
                        LnkBtnleave_Click(LnkBtnViewAppliedLeaveStatus, null);
                }
                catch (Exception ex)
                {
                    lblErrorMessage.Text = ex.Message;
                    lblErrorMessage.CssClass = "error";
                    lblErrorMessage.Visible = true;
                }
            }
        }
        else
            Response.Redirect("Default2.aspx");
    }

    #endregion pageload

    #region Custom Function

    public void clear()
    {
        TxtFromDate.Text = TxtLeaveReason.Text = TxtToDate.Text = string.Empty;
        DdlLeaveType.SelectedIndex = 0;
    }


    public string GetFullName(string empid)
    {
        return obj.GetFullName(empid);
    }

    public string GetLeaveType(int leavetypeid)
    {
        return obj.GetLeaveType(leavetypeid);
    }

    public List<tbl_E_Status> GetAllStatus()
    {
        return obj.GetAllStatus();
    }

    // Bind all listview by employee id
    public void BindListView(String lnkbtnid)
    {
        lblErrorMessage.Visible = false;
        List<LeaveStatu> leavestatusobj = obj.GetAllLeaveStatus();
        var resultcount = CheckActionCount(leavestatusobj).Count;

        switch (lnkbtnid)
        {
            case "ViewLeaveStatus":
                if (resultcount == 0)
                    LnkBtnMyActionItems.Visible = false;
                LstViewEmployeeLeaveStatus.DataSource = leavestatusobj.Where(c => c.Emp_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).ToList();
                LstViewEmployeeLeaveStatus.DataBind();
                break;

            case "ViewAppliedLeaveStatus":
                if (resultcount > 0)
                    LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + resultcount + "</span>";
                else
                {
                    LnkBtnMyActionItems.Visible = false;
                }

                ViewAppliedLeaveStatusFilter(1);
                break;

            case "checklnkbtn":
                if (leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name || c.Hr_id == Page.User.Identity.Name).Count() > 0 || Page.User.IsInRole("Admin"))
                {
                    LnkBtnViewAppliedLeaveStatus.Visible = true;
                    LnkBtnMyActionItems.Visible = true;
                    if (resultcount > 0)
                        LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + resultcount + "</span>";
                    else
                        LnkBtnMyActionItems.Visible = false;
                }
                else
                {
                    LnkBtnViewAppliedLeaveStatus.Visible = false;
                    LnkBtnMyActionItems.Visible = false;
                }
                break;

            case "LnkBtnMyActionItems":
                var actionleaves = CheckActionCount(leavestatusobj);
                LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + actionleaves.Count + "</span>";
                LstViewMyActionItems.DataSource = actionleaves;
                LstViewMyActionItems.DataBind();
                break;

            case "LnkBtnApplyLeave":
                if (resultcount == 0)
                    LnkBtnMyActionItems.Visible = false;
                break;

            case "LnkBtnCalendar":
                if (resultcount == 0)
                    LnkBtnMyActionItems.Visible = false;
                break;
        }
    }

    public void BindList(String lnkbtnid, string order, string field)
    {
        lblErrorMessage.Visible = false;
        List<LeaveStatu> leavestatusobj = obj.GetAllLeaveStatus();
        var resultcount = CheckActionCount(leavestatusobj).Count;

        switch (lnkbtnid)
        {
            case "ViewLeaveStatus":
                if (resultcount == 0)
                    LnkBtnMyActionItems.Visible = false;
                if (order == "DESC")
                {
                    if (field == "AppliedOn")
                    {
                        LstViewEmployeeLeaveStatus.DataSource = leavestatusobj.Where(c => c.Emp_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).ToList();
                        LstViewEmployeeLeaveStatus.DataBind();
                    }
                    else
                    {
                        LstViewEmployeeLeaveStatus.DataSource = leavestatusobj.Where(c => c.Emp_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).ToList();
                        LstViewEmployeeLeaveStatus.DataBind();
                    }
                }
                else
                {
                    if (field == "AppliedOn")
                    {
                        LstViewEmployeeLeaveStatus.DataSource = leavestatusobj.Where(c => c.Emp_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).ToList();
                        LstViewEmployeeLeaveStatus.DataBind();
                    }
                    else
                    {
                        LstViewEmployeeLeaveStatus.DataSource = leavestatusobj.Where(c => c.Emp_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).ToList();
                        LstViewEmployeeLeaveStatus.DataBind();
                    }
                }

                break;

            case "ViewAppliedLeaveStatus":
                if (resultcount > 0)
                    LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + resultcount + "</span>";
                else
                {
                    LnkBtnMyActionItems.Visible = false;
                }
                if (order == "DESC")
                {
                    if (field == "AppliedOn")
                    {
                        ViewAppliedLeaveStatusFilter(1);
                    }
                    else
                    {
                        ViewAppliedLeaveStatusFilter1(1);
                    }
                }
                else
                {
                    if (field == "AppliedOn")
                    {
                        ViewAppliedLeaveStatusFilteras(1);
                    }
                    else
                    {
                        ViewAppliedLeaveStatusFilter1as(1);
                    }
                }

                break;

            case "checklnkbtn":
                if (leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name || c.Hr_id == Page.User.Identity.Name).Count() > 0 || Page.User.IsInRole("Admin"))
                {
                    LnkBtnViewAppliedLeaveStatus.Visible = true;
                    LnkBtnMyActionItems.Visible = true;
                    if (resultcount > 0)
                        LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + resultcount + "</span>";
                    else
                        LnkBtnMyActionItems.Visible = false;
                }
                else
                {
                    LnkBtnViewAppliedLeaveStatus.Visible = false;
                    LnkBtnMyActionItems.Visible = false;
                }
                break;

            case "LnkBtnMyActionItems":
                if (order == "DESC")
                {
                    if (field == "AppliedOn")
                    {
                        var actionleaves = CheckActionCount(leavestatusobj);
                        LstViewMyActionItems.DataSource = actionleaves;
                        LstViewMyActionItems.DataBind();
                        LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + actionleaves.Count + "</span>";
                    }
                    else
                    {
                        var actionleaves = CheckActionCount1(leavestatusobj);
                        LstViewMyActionItems.DataSource = actionleaves;
                        LstViewMyActionItems.DataBind();
                        LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + actionleaves.Count + "</span>";
                    }
                }
                else
                {
                    if (field == "AppliedOn")
                    {
                        var actionleaves = CheckActionCountas(leavestatusobj);
                        LstViewMyActionItems.DataSource = actionleaves;
                        LstViewMyActionItems.DataBind();
                        LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + actionleaves.Count + "</span>";
                    }
                    else
                    {
                        var actionleaves = CheckActionCount1as(leavestatusobj);
                        LstViewMyActionItems.DataSource = actionleaves;
                        LstViewMyActionItems.DataBind();
                        LnkBtnMyActionItems.Text = "My Action Items" + " <span style='color:red;font-weight: bold;'>" + actionleaves.Count + "</span>";
                    }
                }

                break;

            case "LnkBtnApplyLeave":
                if (resultcount == 0)
                    LnkBtnMyActionItems.Visible = false;
                break;

            case "LnkBtnCalendar":
                if (resultcount == 0)
                    LnkBtnMyActionItems.Visible = false;
                break;
        }
    }

    public List<LeaveStatu> CheckActionCount(List<LeaveStatu> obj)
    {
        var leaveStatus = obj;
        var mainleavestatus = new List<LeaveStatu>();

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                //firstline manager
                case 0:
                    var flmanager = leaveStatus.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate);
                    foreach (var items in flmanager)
                    {
                        //if (items.FirstLineManagerStatus == 1 && (items.fkLeaveType == 1 || items.fkLeaveType == 5) && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date))
                        if (items.FirstLineManagerStatus == 1 && (items.fkLeaveType == 1 || items.fkLeaveType == 5) && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date) && (DateTime.Now.Date <= Convert.ToDateTime(items.FromDate).Date))
                        {
                            mainleavestatus.Add(items);
                        }
                    }
                    break;

                //secondline manager
                case 1:
                    var slmanager = leaveStatus.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate);
                    foreach (var items in slmanager)
                    {
                        if (items.SecondLineManagerStatus == 1 && (items.fkLeaveType == 1 || items.fkLeaveType == 5) && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date) && (DateTime.Now.Date <= Convert.ToDateTime(items.FromDate).Date))
                        {
                            if (items.FirstLineManagerStatus == 1 && (items.fkLeaveType == 1 || items.fkLeaveType == 5) && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date) && (DateTime.Now.Date <= Convert.ToDateTime(items.FromDate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }
                    }
                    break;

                //hrrepresentative
                case 2:
                    var hrrepresentative = leaveStatus.Where(c => c.Hr_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate);
                    foreach (var items in hrrepresentative)
                    {
                        if (items.Hr_Status == 1 && (items.fkLeaveType == 1 || items.fkLeaveType == 5) && (DateTime.Now.Date <= Convert.ToDateTime(items.HRRDecisiondate).Date) && (items.FirstLineManagerStatus != 1 || items.SecondLineManagerStatus != 1) && (DateTime.Now.Date <= Convert.ToDateTime(items.FromDate).Date))
                        {
                            if (items.SecondLineManagerStatus == 1 && (items.fkLeaveType == 1 || items.fkLeaveType == 5) && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date) && (DateTime.Now.Date <= Convert.ToDateTime(items.FromDate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }
                    }
                    break;
            }
        }

        return mainleavestatus;
    }

    public List<LeaveStatu> CheckActionCountas(List<LeaveStatu> obj)
    {
        var leaveStatus = obj;
        var mainleavestatus = new List<LeaveStatu>();

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                //firstline manager
                case 0:
                    var flmanager = leaveStatus.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate);
                    foreach (var items in flmanager)
                    {
                        if (items.FirstLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date))
                        {
                            mainleavestatus.Add(items);
                        }
                    }
                    break;

                //secondline manager
                case 1:
                    var slmanager = leaveStatus.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate);
                    foreach (var items in slmanager)
                    {
                        if (items.SecondLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date))
                        {
                            if (items.FirstLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }
                    }
                    break;

                //hrrepresentative
                case 2:
                    var hrrepresentative = leaveStatus.Where(c => c.Hr_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate);
                    foreach (var items in hrrepresentative)
                    {
                        if (items.Hr_Status == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.HRRDecisiondate).Date) && (items.FirstLineManagerStatus != 1 || items.SecondLineManagerStatus != 1))
                        {
                            if (items.SecondLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }
                    }
                    break;
            }
        }

        return mainleavestatus;
    }

    public List<LeaveStatu> CheckActionCount1(List<LeaveStatu> obj)
    {
        var leaveStatus = obj;
        var mainleavestatus = new List<LeaveStatu>();

        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                //firstline manager
                case 0:
                    var flmanager = leaveStatus.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate);
                    foreach (var items in flmanager)
                    {
                        if (items.FirstLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date))
                        {
                            mainleavestatus.Add(items);
                        }
                    }
                    break;

                //secondline manager
                case 1:
                    var slmanager = leaveStatus.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate);
                    foreach (var items in slmanager)
                    {
                        if (items.SecondLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date))
                        {
                            if (items.FirstLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }

                    }
                    break;

                //hrrepresentative
                case 2:
                    var hrrepresentative = leaveStatus.Where(c => c.Hr_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate);
                    foreach (var items in hrrepresentative)
                    {
                        if (items.Hr_Status == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.HRRDecisiondate).Date) && (items.FirstLineManagerStatus != 1 || items.SecondLineManagerStatus != 1))
                        {
                            if (items.SecondLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }

                    }
                    break;
            }
        }

        return mainleavestatus;
    }

    public List<LeaveStatu> CheckActionCount1as(List<LeaveStatu> obj)
    {
        var leaveStatus = obj;
        var mainleavestatus = new List<LeaveStatu>();
        for (int i = 0; i < 3; i++)
        {
            switch (i)
            {
                //firstline manager
                case 0:
                    var flmanager = leaveStatus.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate);
                    foreach (var items in flmanager)
                    {
                        if (items.FirstLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date))
                        {
                            mainleavestatus.Add(items);
                        }
                    }
                    break;

                //secondline manager
                case 1:
                    var slmanager = leaveStatus.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate);
                    foreach (var items in slmanager)
                    {
                        if (items.SecondLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date))
                        {
                            if (items.FirstLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.FLDecisiondate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }
                    }
                    break;

                //hrrepresentative
                case 2:
                    var hrrepresentative = leaveStatus.Where(c => c.Hr_id == Page.User.Identity.Name).OrderBy(c => c.FromDate);
                    foreach (var items in hrrepresentative)
                    {
                        if (items.Hr_Status == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.HRRDecisiondate).Date) && (items.FirstLineManagerStatus != 1 || items.SecondLineManagerStatus != 1))
                        {
                            if (items.SecondLineManagerStatus == 1 && items.fkLeaveType == 1 && (DateTime.Now.Date <= Convert.ToDateTime(items.SLDecisiondate).Date))
                            {

                            }
                            else
                            {
                                mainleavestatus.Add(items);
                            }
                        }
                    }
                    break;
            }
        }

        return mainleavestatus;
    }

    // common event hanlder for all linkbutton
    public void checksender(object sender)
    {
        switch (sender.ToString())
        {
            case "LnkBtnApplyLeave":
                PnlApplyLeave.Visible = true;
                PnlLeaveCommentStatus.Visible = PnlViewLeaveStatus.Visible = PnlCalendar.Visible = PnlMyActionItems.Visible = false;
                BindListView("LnkBtnApplyLeave");
                break;

            case "LnkBtnViewLeaveStatus":
                PnlApplyLeave.Visible = PnlLeaveCommentStatus.Visible = PnlCalendar.Visible = PnlMyActionItems.Visible = false;
                PnlViewLeaveStatus.Visible = true;
                BindListView("ViewLeaveStatus");
                break;

            case "LnkBtnViewAppliedLeaveStatus":
                PnlApplyLeave.Visible = PnlViewLeaveStatus.Visible = PnlCalendar.Visible = PnlMyActionItems.Visible = false;
                PnlLeaveCommentStatus.Visible = true;
                BindListView("ViewAppliedLeaveStatus");
                break;

            case "LnkBtnCalendar":
                PnlApplyLeave.Visible = PnlViewLeaveStatus.Visible = PnlLeaveCommentStatus.Visible = PnlMyActionItems.Visible = false;
                PnlCalendar.Visible = true;
                BindListView("LnkBtnCalendar");
                break;

            case "LnkBtnMyActionItems":
                PnlApplyLeave.Visible = PnlViewLeaveStatus.Visible = PnlCalendar.Visible = PnlLeaveCommentStatus.Visible = false;
                PnlMyActionItems.Visible = true;
                BindListView("LnkBtnMyActionItems");
                break;
        }
    }

    public string GetClass(int LeaveStatus, bool isText, bool isAdmin)
    {
        string cssClass = string.Empty;
        string strStatus = string.Empty;
        switch (LeaveStatus)
        {
            case 0:
                cssClass = "NA";
                strStatus = "ANT";
                break;
            case 1:
                strStatus = cssClass = "Pending";
                break;
            case 2:
                strStatus = "Approved";
                cssClass = "Approved memo";
                break;
            case 3:
                strStatus = "Rejected";
                cssClass = "Rejected memo";
                break;
        }
        if (isText)
            return strStatus;
        else
            return cssClass;
    }

    public string GetStatusDescriptionByID(int statusid)
    {
        return obj.GetStatusDescriptionByID(statusid);
    }

    // GET SHORT STRING
    public static string GetShortString(string strText, int length)
    {
        if (strText.Length <= length)
            return strText;
        else
        {
            strText = strText.Substring(0, length - 3) + "...";
            return strText;
        }
    }

    #endregion Custom Function

    #region EventHandler

    // Leave apply
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            var halfday = CheckBoxhalfday.Checked;
            DateTime dt1 = DateTime.ParseExact(TxtFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            DateTime dt2 = DateTime.ParseExact(TxtToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            TimeSpan span = dt2.Subtract(dt1);
            int days = span.Days + 1;
            int IsProbation = 0;
            int.TryParse(Convert.ToString(Session["IsProbationLeave"]), out IsProbation);
            if (halfday == true && days > 1)
            {
                lblErrorMessage.Text = "Half day leave can only be apply for one day.";
                lblErrorMessage.CssClass = "error";
                lblErrorMessage.Visible = true;
            }
            else
            {
                if (IsProbation == 0)
                {
                    switch (Convert.ToInt32(DdlLeaveType.SelectedValue))
                    {
                        case 1:
                            if (Convert.ToDateTime(TxtToDate.Text).Date >= DateTime.Now.Date && Convert.ToDateTime(TxtFromDate.Text).Date >= DateTime.Now.Date)
                            {
                                if (obj.checkleavestatus(Convert.ToDateTime(TxtFromDate.Text), Convert.ToDateTime(TxtToDate.Text), Page.User.Identity.Name) == false)
                                {
                                    ApplyLeaveMethod();
                                }
                                else
                                {
                                    lblErrorMessage.Text = "You have already applied leave for the same date .";
                                    lblErrorMessage.CssClass = "error";
                                    lblErrorMessage.Visible = true;
                                }
                            }
                            else
                            {
                                ApplyLeaveMethod();
                            }
                            break;

                        case 3:
                            if (Convert.ToDateTime(TxtToDate.Text).Date <= DateTime.Now.Date)
                            {
                                if (obj.checkleavestatus(Convert.ToDateTime(TxtFromDate.Text), Convert.ToDateTime(TxtToDate.Text), Page.User.Identity.Name) == false)
                                {
                                    ApplyLeaveMethod();
                                }
                                else
                                {
                                    lblErrorMessage.Text = "You have already applied leave for the same date .";
                                    lblErrorMessage.CssClass = "error";
                                    lblErrorMessage.Visible = true;
                                }
                            }
                            else
                            {
                                lblErrorMessage.Text = "Sick leave can only be apply on past days.";
                                lblErrorMessage.CssClass = "error";
                                lblErrorMessage.Visible = true;
                            }
                            break;
                        case 5:
                            if (Convert.ToDateTime(TxtToDate.Text).Date >= DateTime.Now.Date && Convert.ToDateTime(TxtFromDate.Text).Date >= DateTime.Now.Date)
                            {
                                if (obj.checkleavestatus(Convert.ToDateTime(TxtFromDate.Text), Convert.ToDateTime(TxtToDate.Text), Page.User.Identity.Name) == false)
                                {
                                    ApplyLeaveMethod();
                                }
                                else
                                {
                                    lblErrorMessage.Text = "You have already applied leave for the same date .";
                                    lblErrorMessage.CssClass = "error";
                                    lblErrorMessage.Visible = true;
                                }
                            }
                            else
                            {
                                ApplyLeaveMethod();
                            }
                            break;
                        default:
                            lblErrorMessage.Text = "Sorry you can only apply casual , earned & sick leave .";
                            lblErrorMessage.CssClass = "error";
                            lblErrorMessage.Visible = true;
                            break;
                    }
                }
                else
                {
                    if (obj.checkleavestatus(Convert.ToDateTime(TxtFromDate.Text), Convert.ToDateTime(TxtToDate.Text), Page.User.Identity.Name) == false)
                    {
                        ApplyLeaveMethod();
                    }
                    else
                    {
                        lblErrorMessage.Text = "You have already applied leave for the same date .";
                        lblErrorMessage.CssClass = "error";
                        lblErrorMessage.Visible = true;
                    }
                }
            }


        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    //apply leave method
    public void ApplyLeaveMethod()
    {
        var leaveobj = new LeaveStatu();
        var TotalLeaves = Convert.ToDecimal(Session["TotalLeaves"]);
        decimal CasualLeavesLeft = Convert.ToDecimal(Session["CasualLeavesLeft"]);
        decimal SickLeavesLeft = Convert.ToDecimal(Session["SickLeavesLeft"]);
        decimal PaidLeavesLeft = Convert.ToDecimal(Session["PaidLeavesLeft"]);
        var halfday = CheckBoxhalfday.Checked;
        DateTime dt1 = DateTime.ParseExact(TxtFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        DateTime dt2 = DateTime.ParseExact(TxtToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
        TimeSpan span = dt2.Subtract(dt1);
        int days = span.Days + 1;
        int IsProbationLeave = 0;
        int.TryParse(Convert.ToString(Session["IsProbationLeave"]), out IsProbationLeave);

        if (TotalLeaves <= 0)
        {
            if (IsProbationLeave == 1)
            {
                leaveobj.fkLeaveType = 1;
            }
            else
            {
                leaveobj.fkLeaveType = 5;
            }
            leaveobj.IsLWP = true;
            //leaveobj.IsLWP = false;
            leaveobj.ELC = 0;
            leaveobj.IsELCFlag = false;
        }
        else
        {
            leaveobj.fkLeaveType = Convert.ToInt32(DdlLeaveType.SelectedValue);
            if (IsProbationLeave == 0)
            {
                if (leaveobj.fkLeaveType == 1)
                {
                    decimal Casual_PaidLeavesLeft = Decimal.Add(CasualLeavesLeft, PaidLeavesLeft);
                    if (Casual_PaidLeavesLeft > 0)
                    {
                        if (Casual_PaidLeavesLeft < days)
                        {
                            if (halfday == true && Casual_PaidLeavesLeft < 1)
                            {
                                leaveobj.ELC = 0;
                                leaveobj.IsELCFlag = false;
                                leaveobj.IsLWP = false;
                            }
                            else
                            {
                                leaveobj.IsELCFlag = true;
                                leaveobj.ELC = days - Casual_PaidLeavesLeft;
                                leaveobj.IsLWP = false;
                            }

                        }
                        else
                        {
                            leaveobj.ELC = 0;
                            leaveobj.IsELCFlag = false;
                            leaveobj.IsLWP = false;
                        }
                    }
                    else
                    {
                        leaveobj.ELC = 0;
                        leaveobj.IsELCFlag = false;
                        leaveobj.IsLWP = true;
                    }

                }
                else if (leaveobj.fkLeaveType == 3)
                {

                    decimal Sick_PaidLeavesLeft = Decimal.Add(SickLeavesLeft, PaidLeavesLeft);
                    if (Sick_PaidLeavesLeft > 0)
                    {
                        if (Sick_PaidLeavesLeft < days)
                        {
                            if (halfday == true && Sick_PaidLeavesLeft < 1)
                            {
                                leaveobj.ELC = 0;
                                leaveobj.IsELCFlag = false;
                                leaveobj.IsLWP = false;
                            }
                            else
                            {
                                leaveobj.IsELCFlag = true;
                                leaveobj.ELC = days - Sick_PaidLeavesLeft;
                                leaveobj.IsLWP = false;
                            }
                        }
                        else
                        {
                            leaveobj.ELC = 0;
                            leaveobj.IsELCFlag = false;
                            leaveobj.IsLWP = false;
                        }
                    }
                    else
                    {
                        leaveobj.ELC = 0;
                        leaveobj.IsELCFlag = false;
                        leaveobj.IsLWP = true;
                    }

                }
                else if (leaveobj.fkLeaveType == 5)
                {
                    decimal Casual_PaidLeavesLeft = Decimal.Add(CasualLeavesLeft, PaidLeavesLeft);
                    if (Casual_PaidLeavesLeft > 0)
                    {
                        if (Casual_PaidLeavesLeft < days)
                        {
                            if (halfday == true && Casual_PaidLeavesLeft < 1)
                            {
                                leaveobj.ELC = 0;
                                leaveobj.IsELCFlag = false;
                                leaveobj.IsLWP = false;
                            }
                            else
                            {
                                leaveobj.IsELCFlag = true;
                                leaveobj.ELC = days - Casual_PaidLeavesLeft;
                                leaveobj.IsLWP = false;
                            }
                        }
                        else
                        {
                            leaveobj.ELC = 0;
                            leaveobj.IsELCFlag = false;
                            leaveobj.IsLWP = false;
                        }
                    }
                    else
                    {
                        leaveobj.ELC = 0;
                        leaveobj.IsELCFlag = false;
                        leaveobj.IsLWP = true;
                    }
                }
                else
                {
                    leaveobj.ELC = 0;
                    leaveobj.IsELCFlag = false;
                    leaveobj.IsLWP = false;
                }

            }
            else
            {
                decimal TotalLeave = Convert.ToDecimal(Session["TotalLeaves"]);
                if (TotalLeave < days)
                {
                    if (halfday == true && TotalLeave < 1)
                    {
                        leaveobj.ELC = 0;
                        leaveobj.IsELCFlag = false;
                        leaveobj.IsLWP = false;
                    }
                    else
                    {
                        leaveobj.IsELCFlag = true;
                        leaveobj.ELC = days - Convert.ToDecimal(Session["TotalLeaves"]);
                        leaveobj.IsLWP = false;
                    }
                }
                else
                {
                    leaveobj.IsELCFlag = false;
                    leaveobj.ELC = 0;
                    leaveobj.IsLWP = false;
                }
            }
        }
        if (IsProbationLeave == 1)
        {
            leaveobj.IsProbationLeave = Convert.ToBoolean(IsProbationLeave);
        }
        else
        {
            leaveobj.IsProbationLeave = false;
        }
        leaveobj.Emp_id = Page.User.Identity.Name;
        leaveobj.IsHalfDay = Convert.ToBoolean(halfday);
        leaveobj.Department = obj.Department(Page.User.Identity.Name);
        leaveobj.FromDate = Convert.ToDateTime(TxtFromDate.Text);
        leaveobj.ToDate = Convert.ToDateTime(TxtToDate.Text);
        leaveobj.LeaveReason = TxtLeaveReason.Text;
        leaveobj.FirstLineManager_id = obj.GetReportingManager(leaveobj.Emp_id);
        leaveobj.FirstLineManagerStatus = 1;
        leaveobj.FirstLineMangerComment = string.Empty;
        leaveobj.SecondLineManager_id = obj.GetReportingManager(leaveobj.FirstLineManager_id);
        leaveobj.SecondLineManagerStatus = 1;
        leaveobj.SecondLineManagerComment = string.Empty;

        if (Page.User.Identity.Name != obj.GetHrid())
            leaveobj.Hr_id = obj.GetHrid();

        leaveobj.Hr_Status = 1;
        leaveobj.Hr_Comment = string.Empty;
        leaveobj.EmpLeaveStatus = 1;
        leaveobj.LeaveAppliedDate = DateTime.Now;
        leaveobj.CreatedOn = DateTime.Now;
        leaveobj.CreatedBy = Page.User.Identity.Name;
        if (Convert.ToInt32(DdlLeaveType.SelectedValue) == 1 || Convert.ToInt32(DdlLeaveType.SelectedValue) == 5)
        {
            DateTime FLDecision_dt, SLDecision_dt, HRRDecision_dt;
            FLDecision_dt = GetDecisionDate(DateTime.Now.AddDays(1), Convert.ToDateTime(TxtFromDate.Text));
            SLDecision_dt = GetDecisionDate(FLDecision_dt.AddDays(1), Convert.ToDateTime(TxtFromDate.Text));
            HRRDecision_dt = GetDecisionDate(SLDecision_dt.AddDays(1), Convert.ToDateTime(TxtFromDate.Text));
            leaveobj.FLDecisiondate = FLDecision_dt;
            leaveobj.SLDecisiondate = SLDecision_dt;
            leaveobj.HRRDecisiondate = HRRDecision_dt;
        }
        int id = obj.ApplyLeave(leaveobj);
        if (id != 0)
        {
            int status = obj.SaveLeaveDetails(id, leaveobj);
            LeaveApplicationStatus(id, "ApplicationSubmission");
            lblErrorMessage.Text = "Your Leave is Applied successfully.";
            lblErrorMessage.CssClass = "success";
            lblErrorMessage.Visible = true;
            AuthenticateUser();
            clear();
        }
        else
        {
            lblErrorMessage.Text = "OOPS Something went wrong please contact your administrator!";
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }
    //public void ApplyLeaveMethod()
    //{
    //    var leaveobj = new LeaveStatu();
    //    var TotalLeaves = Convert.ToDecimal(Session["TotalLeaves"]);

    //    var halfday = CheckBoxhalfday.Checked;
    //    DateTime dt1 = DateTime.ParseExact(TxtFromDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
    //    DateTime dt2 = DateTime.ParseExact(TxtToDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture);
    //    TimeSpan span = dt2.Subtract(dt1);
    //    int days = span.Days + 1;
    //    int IsProbationLeave = 0;
    //    int.TryParse(Convert.ToString(Session["IsProbationLeave"]), out IsProbationLeave);

    //    if (TotalLeaves <= 0)
    //    {
    //        if (IsProbationLeave == 1)
    //        {
    //            leaveobj.fkLeaveType = 1;
    //        }
    //        else
    //        {
    //            leaveobj.fkLeaveType = 5;
    //        }
    //        leaveobj.IsLWP = true;
    //        //leaveobj.IsLWP = false;
    //        leaveobj.ELC = 0;
    //        leaveobj.IsELCFlag = false;
    //    }
    //    else
    //    {
    //        leaveobj.fkLeaveType = Convert.ToInt32(DdlLeaveType.SelectedValue);

    //        if (IsProbationLeave == 0)
    //        {
    //            if (leaveobj.fkLeaveType == 1)
    //            {
    //                decimal CasualLeavesLeft = Convert.ToDecimal(Session["CasualLeavesLeft"]);
    //                if (CasualLeavesLeft > 0)
    //                {
    //                    if (CasualLeavesLeft < days)
    //                    {
    //                        if (halfday == true && CasualLeavesLeft < 1)
    //                        {
    //                            leaveobj.ELC = 0;
    //                            leaveobj.IsELCFlag = false;
    //                            leaveobj.IsLWP = false;
    //                        }
    //                        else
    //                        {
    //                            leaveobj.IsELCFlag = true;
    //                            leaveobj.ELC = days - Convert.ToDecimal(Session["CasualLeavesLeft"]);
    //                            leaveobj.IsLWP = false;
    //                        }

    //                    }
    //                    else
    //                    {
    //                        leaveobj.ELC = 0;
    //                        leaveobj.IsELCFlag = false;
    //                        leaveobj.IsLWP = false;
    //                    }
    //                }
    //                else
    //                {
    //                    leaveobj.ELC = 0;
    //                    leaveobj.IsELCFlag = false;
    //                    leaveobj.IsLWP = true;
    //                }

    //            }
    //            else if (leaveobj.fkLeaveType == 3)
    //            {
    //                decimal SickLeavesLeft = Convert.ToDecimal(Session["SickLeavesLeft"]);
    //                if (SickLeavesLeft > 0)
    //                {
    //                    if (SickLeavesLeft < days)
    //                    {
    //                        if (halfday == true && SickLeavesLeft < 1)
    //                        {
    //                            leaveobj.ELC = 0;
    //                            leaveobj.IsELCFlag = false;
    //                            leaveobj.IsLWP = false;
    //                        }
    //                        else
    //                        {
    //                            leaveobj.IsELCFlag = true;
    //                            leaveobj.ELC = days - Convert.ToDecimal(Session["SickLeavesLeft"]); ;
    //                            leaveobj.IsLWP = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        leaveobj.ELC = 0;
    //                        leaveobj.IsELCFlag = false;
    //                        leaveobj.IsLWP = false;
    //                    }
    //                }
    //                else
    //                {
    //                    leaveobj.ELC = 0;
    //                    leaveobj.IsELCFlag = false;
    //                    leaveobj.IsLWP = true;
    //                }

    //            }
    //            else if (leaveobj.fkLeaveType == 5)
    //            {
    //                decimal PaidLeavesLeft = Convert.ToDecimal(Session["PaidLeavesLeft"]);
    //                if (PaidLeavesLeft > 0)
    //                {
    //                    if (PaidLeavesLeft < days)
    //                    {
    //                        if (halfday == true && PaidLeavesLeft < 1)
    //                        {
    //                            leaveobj.ELC = 0;
    //                            leaveobj.IsELCFlag = false;
    //                            leaveobj.IsLWP = false;
    //                        }
    //                        else
    //                        {
    //                            leaveobj.IsELCFlag = true;
    //                            leaveobj.ELC = days - Convert.ToDecimal(Session["PaidLeavesLeft"]);
    //                            leaveobj.IsLWP = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        leaveobj.ELC = 0;
    //                        leaveobj.IsELCFlag = false;
    //                        leaveobj.IsLWP = false;
    //                    }
    //                }
    //                else
    //                {
    //                    leaveobj.ELC = 0;
    //                    leaveobj.IsELCFlag = false;
    //                    leaveobj.IsLWP = true;
    //                }
    //            }
    //            else
    //            {
    //                leaveobj.ELC = 0;
    //                leaveobj.IsELCFlag = false;
    //                leaveobj.IsLWP = false;
    //            }
    //        }

    //        else
    //        {
    //            decimal TotalLeave = Convert.ToDecimal(Session["TotalLeaves"]);
    //            if (TotalLeave < days)
    //            {
    //                if (halfday == true && TotalLeave < 1)
    //                {
    //                    leaveobj.ELC = 0;
    //                    leaveobj.IsELCFlag = false;
    //                    leaveobj.IsLWP = false;
    //                }
    //                else
    //                {
    //                    leaveobj.IsELCFlag = true;
    //                    leaveobj.ELC = days - Convert.ToDecimal(Session["TotalLeaves"]);
    //                    leaveobj.IsLWP = false;
    //                }

    //            }
    //            else
    //            {
    //                leaveobj.IsELCFlag = false;
    //                leaveobj.ELC = 0;
    //                leaveobj.IsLWP = false;
    //            }

    //        }

    //    }
    //    //else
    //    //{
    //    //    leaveobj.fkLeaveType = Convert.ToInt32(DdlLeaveType.SelectedValue);

    //    //    if (IsProbationLeave == 0)
    //    //    {
    //    //        if (leaveobj.fkLeaveType == 1)
    //    //        {
    //    //            decimal CasualLeavesLeft = Convert.ToDecimal(Session["CasualLeavesLeft"]);
    //    //            if (CasualLeavesLeft < days)
    //    //            {
    //    //                if (halfday == true && CasualLeavesLeft < 1)
    //    //                {
    //    //                    leaveobj.ELC = 0;
    //    //                    leaveobj.IsELCFlag = false;
    //    //                    leaveobj.IsLWP = false;
    //    //                }
    //    //                else
    //    //                {
    //    //                    leaveobj.IsELCFlag = false;
    //    //                    leaveobj.ELC = days - Convert.ToDecimal(Session["CasualLeavesLeft"]);
    //    //                    leaveobj.IsLWP = true;
    //    //                }

    //    //            }
    //    //            else
    //    //            {
    //    //                leaveobj.ELC = 0;
    //    //                leaveobj.IsELCFlag = false;
    //    //                leaveobj.IsLWP = false;
    //    //            }
    //    //        }
    //    //        else if (leaveobj.fkLeaveType == 3)
    //    //        {
    //    //            decimal SickLeavesLeft = Convert.ToDecimal(Session["SickLeavesLeft"]);
    //    //            if (SickLeavesLeft < days)
    //    //            {
    //    //                if (halfday == true && SickLeavesLeft < 1)
    //    //                {
    //    //                    leaveobj.ELC = 0;
    //    //                    leaveobj.IsELCFlag = false;
    //    //                    leaveobj.IsLWP = false;
    //    //                }
    //    //                else
    //    //                {
    //    //                    leaveobj.IsELCFlag = false;
    //    //                    leaveobj.ELC = days - Convert.ToDecimal(Session["SickLeavesLeft"]); ;
    //    //                    leaveobj.IsLWP = true;
    //    //                }
    //    //            }
    //    //            else
    //    //            {
    //    //                leaveobj.ELC = 0;
    //    //                leaveobj.IsELCFlag = false;
    //    //                leaveobj.IsLWP = false;
    //    //            }
    //    //        }
    //    //        else if (leaveobj.fkLeaveType == 5)
    //    //        {
    //    //            decimal PaidLeavesLeft = Convert.ToDecimal(Session["PaidLeavesLeft"]);
    //    //            if (PaidLeavesLeft < days)
    //    //            {
    //    //                if (halfday == true && PaidLeavesLeft < 1)
    //    //                {
    //    //                    leaveobj.ELC = 0;
    //    //                    leaveobj.IsELCFlag = false;
    //    //                    leaveobj.IsLWP = false;
    //    //                }
    //    //                else
    //    //                {
    //    //                    leaveobj.IsELCFlag = false;
    //    //                    leaveobj.ELC = days - Convert.ToDecimal(Session["PaidLeavesLeft"]);
    //    //                    leaveobj.IsLWP = true;
    //    //                }
    //    //            }
    //    //            else
    //    //            {
    //    //                leaveobj.ELC = 0;
    //    //                leaveobj.IsELCFlag = false;
    //    //                leaveobj.IsLWP = false;
    //    //            }
    //    //        }
    //    //        else
    //    //        {
    //    //            leaveobj.ELC = 0;
    //    //            leaveobj.IsELCFlag = false;
    //    //            leaveobj.IsLWP = false;
    //    //        }
    //    //    }

    //    //    else
    //    //    {
    //    //        decimal TotalLeave = Convert.ToDecimal(Session["TotalLeaves"]);
    //    //        if (TotalLeave < days)
    //    //        {                    
    //    //            if (halfday == true && TotalLeave < 1)
    //    //            {
    //    //                leaveobj.ELC = 0;
    //    //                leaveobj.IsELCFlag = false;
    //    //                leaveobj.IsLWP = false;
    //    //            }
    //    //            else
    //    //            {
    //    //                leaveobj.IsELCFlag = true;
    //    //                leaveobj.ELC = days - Convert.ToDecimal(Session["AllowedTotalLeaves"]);
    //    //                leaveobj.IsLWP = false;
    //    //            }

    //    //        }
    //    //        else
    //    //        {
    //    //            leaveobj.IsELCFlag = false;
    //    //            leaveobj.ELC = 0;
    //    //            leaveobj.IsLWP = false;
    //    //        }

    //    //    }

    //    //}
    //    // int IsProbationLeave = Convert.ToInt32(Session["IsProbationLeave"]);

    //    if (IsProbationLeave == 1)
    //    {
    //        leaveobj.IsProbationLeave = Convert.ToBoolean(IsProbationLeave);
    //    }
    //    else
    //    {
    //        leaveobj.IsProbationLeave = false;
    //    }
    //    leaveobj.Emp_id = Page.User.Identity.Name;

    //    leaveobj.IsHalfDay = Convert.ToBoolean(halfday);
    //    //leaveobj.fkLeaveType = Convert.ToInt32(DdlLeaveType.SelectedValue);
    //    leaveobj.Department = obj.Department(Page.User.Identity.Name);
    //    leaveobj.FromDate = Convert.ToDateTime(TxtFromDate.Text);
    //    leaveobj.ToDate = Convert.ToDateTime(TxtToDate.Text);
    //    leaveobj.LeaveReason = TxtLeaveReason.Text;
    //    leaveobj.FirstLineManager_id = obj.GetReportingManager(leaveobj.Emp_id);
    //    leaveobj.FirstLineManagerStatus = 1;
    //    leaveobj.FirstLineMangerComment = string.Empty;
    //    leaveobj.SecondLineManager_id = obj.GetReportingManager(leaveobj.FirstLineManager_id);
    //    leaveobj.SecondLineManagerStatus = 1;
    //    leaveobj.SecondLineManagerComment = string.Empty;

    //    if (Page.User.Identity.Name != obj.GetHrid())
    //        leaveobj.Hr_id = obj.GetHrid();

    //    leaveobj.Hr_Status = 1;
    //    leaveobj.Hr_Comment = string.Empty;
    //    leaveobj.EmpLeaveStatus = 1;
    //    leaveobj.LeaveAppliedDate = DateTime.Now;
    //    leaveobj.CreatedOn = DateTime.Now;
    //    leaveobj.CreatedBy = Page.User.Identity.Name;
    //    if (Convert.ToInt32(DdlLeaveType.SelectedValue) == 1)
    //    {
    //        DateTime FLDecision_dt, SLDecision_dt, HRRDecision_dt;
    //        FLDecision_dt = GetDecisionDate(DateTime.Now.AddDays(1), Convert.ToDateTime(TxtFromDate.Text));
    //        SLDecision_dt = GetDecisionDate(FLDecision_dt.AddDays(1), Convert.ToDateTime(TxtFromDate.Text));
    //        HRRDecision_dt = GetDecisionDate(SLDecision_dt.AddDays(1), Convert.ToDateTime(TxtFromDate.Text));
    //        leaveobj.FLDecisiondate = FLDecision_dt;
    //        leaveobj.SLDecisiondate = SLDecision_dt;
    //        leaveobj.HRRDecisiondate = HRRDecision_dt;
    //    }

    //    int id = obj.ApplyLeave(leaveobj);
    //    if (id != 0)
    //    {

    //        LeaveApplicationStatus(id, "ApplicationSubmission");

    //        lblErrorMessage.Text = "Your Leave is Applied successfully.";
    //        lblErrorMessage.CssClass = "success";
    //        lblErrorMessage.Visible = true;
    //        AuthenticateUser();
    //        clear();
    //        //clearSession();

    //    }
    //    else
    //    {
    //        lblErrorMessage.Text = "OOPS Something went wrong please contact your administrator!";
    //        lblErrorMessage.CssClass = "error";
    //        lblErrorMessage.Visible = true;
    //    }
    //}

    public DateTime GetDecisionDate(DateTime applieddate, DateTime fromdate)
    {
        DateTime resultdate = applieddate;
        for (DateTime applieddate_dt = applieddate; applieddate_dt.Date < fromdate.Date;)
        {
            if (obj.checkholidaystatusbydate(applieddate_dt) == false && applieddate_dt.DayOfWeek.ToString().ToLower() != "saturday" && applieddate_dt.DayOfWeek.ToString().ToLower() != "sunday")
            {
                resultdate = applieddate_dt;
                break;
            }
            applieddate_dt = applieddate_dt.AddDays(1);
        }
        return resultdate;
    }

    // Change status and make Comment
    protected void LstViewViewAppliedLeaveStatus_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            var ddlFirstLineManagerStatus = (DropDownList)e.Item.FindControl("DdlFirstLineManagerStatus");
            var ddlSecoundLineManagerStatus = (DropDownList)e.Item.FindControl("DdlSecoundLineManagerStatus");
            var ddlHrManagerStatus = (DropDownList)e.Item.FindControl("DdlHrManagerStatus");
            var ddlAdmin = (DropDownList)e.Item.FindControl("DdlAdmin");
            var lblErrorMsgFirstLineManager = (Label)e.Item.FindControl("LblErrorMsgFirstLineManager");
            var lblErrorMsgSecoundLineManager = (Label)e.Item.FindControl("LblErrorMsgSecoundLineManager");
            var lblErrorMsgHrManager = (Label)e.Item.FindControl("LblErrorMsgHrManager");
            var lblErrorMsgAdmin = (Label)e.Item.FindControl("LblErrorMsgAdmin");

            var txtFirstLineManagerComment = (TextBox)e.Item.FindControl("TxtFirstLineManagerComment");
            var txtSecondLineManagerComment = (TextBox)e.Item.FindControl("TxtSecondLineManagerComment");
            var txtHrcomment = (TextBox)e.Item.FindControl("TxtHrcomment");
            var txtAdminComment = (TextBox)e.Item.FindControl("TxtAdminComment");
            var chkBoxAdminSendMail = (CheckBox)e.Item.FindControl("ChkBoxAdminSendMail");

            switch (e.CommandName)
            {
                case "FirstLineManager":
                    if (Convert.ToInt32(ddlFirstLineManagerStatus.SelectedValue) != 1 && txtFirstLineManagerComment.Text.Trim() != string.Empty)
                    {
                        obj.UpdateEmpLeaveStatus(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(ddlFirstLineManagerStatus.SelectedValue), txtFirstLineManagerComment.Text, "FirstLineManager", Page.User.Identity.Name);
                        LeaveApplicationStatus(Convert.ToInt32(e.CommandArgument.ToString()), "FirstLineManagerDecision");
                        BindListView("ViewAppliedLeaveStatus");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlFirstLineManagerStatus.SelectedValue) != 1)
                        {
                            lblErrorMsgFirstLineManager.Text = "Comment is required.";
                            lblErrorMsgFirstLineManager.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblErrorMsgFirstLineManager.Text = string.Empty;
                        }
                    }
                    break;

                case "SecoundLineManager":
                    if (Convert.ToInt32(ddlSecoundLineManagerStatus.SelectedValue) != 1 && txtSecondLineManagerComment.Text.Trim() != string.Empty)
                    {
                        obj.UpdateEmpLeaveStatus(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(ddlSecoundLineManagerStatus.SelectedValue), txtSecondLineManagerComment.Text, "SecoundLineManager", Page.User.Identity.Name);
                        LeaveApplicationStatus(Convert.ToInt32(e.CommandArgument.ToString()), "SecondLineManagerDecision");
                        BindListView("ViewAppliedLeaveStatus");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlSecoundLineManagerStatus.SelectedValue) != 1)
                        {
                            lblErrorMsgSecoundLineManager.Text = "Comment is required.";
                            lblErrorMsgSecoundLineManager.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblErrorMsgSecoundLineManager.Text = string.Empty;
                        }
                    }
                    break;

                case "HrManager":
                    if (Convert.ToInt32(ddlHrManagerStatus.SelectedValue) != 1 && txtHrcomment.Text.Trim() != string.Empty)
                    {
                        obj.UpdateEmpLeaveStatus(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(ddlHrManagerStatus.SelectedValue), txtHrcomment.Text, "HrManager", Page.User.Identity.Name);
                        LeaveApplicationStatus(Convert.ToInt32(e.CommandArgument.ToString()), "HrDecision");
                        BindListView("ViewAppliedLeaveStatus");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlHrManagerStatus.SelectedValue) != 1)
                        {
                            lblErrorMsgHrManager.Text = "Comment is required.";
                            lblErrorMsgHrManager.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblErrorMsgHrManager.Text = string.Empty;
                        }
                    }
                    break;

                case "Admin":
                    if (Convert.ToInt32(ddlAdmin.SelectedValue) != 1 && txtAdminComment.Text.Trim() != string.Empty)
                    {
                        obj.UpdateEmpLeaveStatus(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(ddlAdmin.SelectedValue), txtAdminComment.Text, "Admin", Page.User.Identity.Name);
                        if (chkBoxAdminSendMail.Checked)
                            LeaveApplicationStatus(Convert.ToInt32(e.CommandArgument.ToString()), "AdminDecision");

                        BindListView("ViewAppliedLeaveStatus");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlAdmin.SelectedValue) != 1)
                        {
                            lblErrorMsgAdmin.Text = "Comment is required.";
                            lblErrorMsgAdmin.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblErrorMsgAdmin.Text = string.Empty;
                        }
                    }
                    break;
            }

            for (int i = 0; i < LstViewViewAppliedLeaveStatus.Items.Count; i++)
            {
                if (i != e.Item.DataItemIndex)
                {
                    ((Label)LstViewViewAppliedLeaveStatus.Items[i].FindControl("LblErrorMsgFirstLineManager")).Text = ((Label)LstViewViewAppliedLeaveStatus.Items[i].FindControl("LblErrorMsgSecoundLineManager")).Text = ((Label)LstViewViewAppliedLeaveStatus.Items[i].FindControl("LblErrorMsgHrManager")).Text = ((Label)LstViewViewAppliedLeaveStatus.Items[i].FindControl("LblErrorMsgAdmin")).Text = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    // Distingush section and access for firstline , secondline , hr and admin manager
    protected void LstViewViewAppliedLeaveStatus_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var ddlFirstLineManagerStatus = (DropDownList)e.Item.FindControl("DdlFirstLineManagerStatus");
                var ddlSecoundLineManagerStatus = (DropDownList)e.Item.FindControl("DdlSecoundLineManagerStatus");
                var ddlHrManagerStatus = (DropDownList)e.Item.FindControl("DdlHrManagerStatus");
                var ddlAdmin = (DropDownList)e.Item.FindControl("DdlAdmin");

                var lblFirstLineManagerStatus = (Label)e.Item.FindControl("LblFirstLineManagerStatus");
                var lblSecondLineManagerStatus = (Label)e.Item.FindControl("LblSecondLineManagerStatus");
                var lblHrStatus = (Label)e.Item.FindControl("LblHr_Status");

                var lblFromDate = (Label)e.Item.FindControl("LblFromDate");
                var lblLeaveAppliedDate = (Label)e.Item.FindControl("LblLeaveAppliedDate");

                var lblEmpLeaveStatus = (Label)e.Item.FindControl("LblEmpLeaveStatus");

                var firstLineManagerid = (Label)e.Item.FindControl("LblFirstLineMaganerid");
                var secoundLineManagerid = (Label)e.Item.FindControl("LblSecoundLineManagerid");
                var hrid = (Label)e.Item.FindControl("LblHrid");

                var lblFirstLineStatusComment = (Label)e.Item.FindControl("LblFirstLineStatusComment");
                var lblSecondLineStatusComment = (Label)e.Item.FindControl("LblSecondLineStatusComment");
                var lblHrStatusComment = (Label)e.Item.FindControl("LblHrStatusComment");
                var lblAdminStatusComment = (Label)e.Item.FindControl("LblAdminStatusComment");

                var txtFirstLineManagerComment = (TextBox)e.Item.FindControl("TxtFirstLineManagerComment");
                var txtSecondLineManagerComment = (TextBox)e.Item.FindControl("TxtSecondLineManagerComment");
                var txtHrcomment = (TextBox)e.Item.FindControl("TxtHrcomment");
                var txtAdminComment = (TextBox)e.Item.FindControl("TxtAdminComment");

                txtAdminComment.Text = txtFirstLineManagerComment.Text = txtSecondLineManagerComment.Text = txtHrcomment.Text = string.Empty;
                var lnkFirstLineManagerComment = (LinkButton)e.Item.FindControl("LnkFirstLineManagerComment");
                var lnkBtnSecondLineManagerComment = (LinkButton)e.Item.FindControl("LnkBtnSecondLineManagerComment");
                var lnkBtnHrManagerComment = (LinkButton)e.Item.FindControl("LnkBtnHrManagerComment");
                var lnkBtnAdmin = (LinkButton)e.Item.FindControl("LnkBtnAdmin");

                var chkBoxAdminSendMail = (CheckBox)e.Item.FindControl("ChkBoxAdminSendMail");

                if (e.Item.FindControl("LblSrNo") != null)
                    ((Label)e.Item.FindControl("LblSrNo")).Text = (e.Item.DataItemIndex + 1).ToString();
                //First Line Manager
                if (firstLineManagerid.Text.ToLower().Trim() == Page.User.Identity.Name.ToLower().Trim() && Convert.ToInt32(lblFirstLineManagerStatus.Text) == 1 && DateTime.Now.Date <= Convert.ToDateTime(lblFromDate.Text).Date)
                {
                    ddlFirstLineManagerStatus.Visible = txtFirstLineManagerComment.Visible = lnkFirstLineManagerComment.Visible = true;
                    ddlFirstLineManagerStatus.Items.FindByValue(lblFirstLineManagerStatus.Text).Selected = true;
                    lblFirstLineStatusComment.Visible = false;
                }

                //Secound Line Manager
                if (secoundLineManagerid.Text.ToLower().Trim() == Page.User.Identity.Name.ToLower().Trim() && Convert.ToInt32(lblSecondLineManagerStatus.Text) == 1 && DateTime.Now.Date <= Convert.ToDateTime(lblFromDate.Text).Date)
                {
                    ddlSecoundLineManagerStatus.Visible = txtSecondLineManagerComment.Visible = lnkBtnSecondLineManagerComment.Visible = true;
                    ddlSecoundLineManagerStatus.Items.FindByValue(lblSecondLineManagerStatus.Text).Selected = true;
                    lblSecondLineStatusComment.Visible = false;
                }

                //Hr Manager
                if (hrid.Text.ToLower().Trim() == Page.User.Identity.Name.ToLower().Trim() && Convert.ToInt32(lblHrStatus.Text) == 1 && DateTime.Now.Date <= Convert.ToDateTime(lblFromDate.Text).Date)
                {
                    if (Convert.ToInt32(lblFirstLineManagerStatus.Text) == 1 && Convert.ToInt32(lblSecondLineManagerStatus.Text) == 1)
                        return;
                    else
                    {
                        ddlHrManagerStatus.Visible = txtHrcomment.Visible = lnkBtnHrManagerComment.Visible = true;
                        lblHrStatusComment.Visible = false;
                        DisableHRRStatus(lblFirstLineManagerStatus.Text, lblSecondLineManagerStatus.Text, ddlHrManagerStatus);
                    }
                }

                // Admin
                if (Page.User.IsInRole("Admin"))
                {
                    ddlAdmin.Visible = lnkBtnAdmin.Visible = txtAdminComment.Visible = chkBoxAdminSendMail.Visible = true;
                    ddlAdmin.Items.FindByValue(lblEmpLeaveStatus.Text).Selected = true;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    //Disable HRR Status
    public void DisableHRRStatus(string FMstatus, string SMstatus, DropDownList Ddlhr)
    {
        switch (Convert.ToInt32(SMstatus))
        {
            case 0:
                break;
            case 1:
                switch (Convert.ToInt32(FMstatus))
                {
                    case 0:
                        break;
                    case 1:
                        break;
                    case 2:
                        for (int i = 0; i < Ddlhr.Items.Count; i++)
                        {
                            Ddlhr.Items.FindByValue("1").Enabled = false;
                            Ddlhr.Items.FindByValue("3").Enabled = false;
                            Ddlhr.Items.FindByValue("2").Selected = true;
                        }
                        break;
                    case 3:
                        for (int i = 0; i < Ddlhr.Items.Count; i++)
                        {
                            Ddlhr.Items.FindByValue("1").Enabled = false;
                            Ddlhr.Items.FindByValue("2").Enabled = false;
                            Ddlhr.Items.FindByValue("3").Selected = true;
                        }
                        break;
                }

                break;
            case 2:
                for (int i = 0; i < Ddlhr.Items.Count; i++)
                {
                    Ddlhr.Items.FindByValue("1").Enabled = false;
                    Ddlhr.Items.FindByValue("3").Enabled = false;
                    Ddlhr.Items.FindByValue("2").Selected = true;
                }
                break;
            case 3:
                for (int i = 0; i < Ddlhr.Items.Count; i++)
                {
                    Ddlhr.Items.FindByValue("1").Enabled = false;
                    Ddlhr.Items.FindByValue("2").Enabled = false;
                    Ddlhr.Items.FindByValue("3").Selected = true;
                }
                break;
        }
    }

    protected void LnkBtnleave_Click(object sender, EventArgs e)
    {
        try
        {
            checksender(((LinkButton)sender).ID);
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    #endregion EventHandler

    //public void LeaveApplicationStatus(int id, string ApplicationStatus)
    //{
    //    LeaveStatu empleavestatusobj = enititiesobj.LeaveStatus.FirstOrDefault(c => c.ID == id);
    //    string EmailSubject, DepartmentManager, EmailBodycontent;
    //    string EmailSentTo = EmailSubject = DepartmentManager = EmailBodycontent = string.Empty;
    //    try
    //    {
    //        if (empleavestatusobj != null)
    //        {
    //            //if (obj.GetDepartmentMangerid(empleavestatusobj.Department) != null)
    //            //    DepartmentManager = obj.GetDepartmentMangerid(empleavestatusobj.Department);

    //            DepartmentManager = obj.GetReportingManager(empleavestatusobj.SecondLineManager_id);

    //            switch (ApplicationStatus)
    //            {
    //                case "ApplicationSubmission":
    //                    //EmailSentTo = "Applicant, First Line Manager, Second Line Manager, Leaves, Department Manager";
    //                    var applicant = enititiesobj.Employees.FirstOrDefault(c => c.Emp_Id == empleavestatusobj.Emp_id);
    //                    string applicantname = empleavestatusobj.Emp_id;
    //                    if (applicant != null)
    //                        applicantname = applicant.FullName;

    //                    EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //     obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
    //                    EmailSubject = "Leave application submitted by ###applicant### for ###Startdate###";
    //                    EmailSubject = EmailSubject.Replace("###applicant###", applicantname);
    //                    EmailSubject = EmailSubject.Replace("###Startdate###", Convert.ToDateTime(empleavestatusobj.FromDate).ToLongDateString());
    //                    EmailBodycontent = EmailBody(empleavestatusobj, "submitted", "FSLineManager");
    //                    if (!string.IsNullOrEmpty(EmailSentTo))
    //                        Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
    //                    break;

    //                case "FirstLineManagerDecision":
    //                    // EmailSentTo = "First Line Manager, Second Line Manager, Leaves, Department Manager";
    //                    if ((empleavestatusobj.FirstLineManager_id == "soumyajit_chakraborty" && empleavestatusobj.FirstLineManagerStatus != 1) || (empleavestatusobj.FirstLineManager_id == "divya_chakraborty" && empleavestatusobj.FirstLineManagerStatus != 1))
    //                    {
    //                        EmailSubject = "Leave application ###Status###";

    //                        EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id),
    //                                                          obj.GetEmployeeOfficialEmailId(
    //                                                              empleavestatusobj.FirstLineManager_id),
    //                                                          obj.GetEmployeeOfficialEmailId(
    //                                                              empleavestatusobj.SecondLineManager_id),
    //                                                          obj.GetEmployeeOfficialEmailId(DepartmentManager));

    //                        if (Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus) == 2)
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
    //                        }
    //                        else
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
    //                        }

    //                        if (!string.IsNullOrEmpty(EmailSentTo))
    //                            Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");

    //                    }
    //                    else
    //                    {
    //                        EmailSentTo = EmailSentToFunction(string.Empty, obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //                            obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
    //                        if (Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus) != 1)
    //                        {
    //                            if (Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus) == 2)
    //                            {
    //                                EmailSubject = "Leave application approved by ###ManagerName###";
    //                                EmailBodycontent = EmailBody(empleavestatusobj, "approved", "FSLineManager");
    //                            }
    //                            else
    //                            {
    //                                EmailSubject = "Leave application rejected by ###ManagerName###";
    //                                EmailBodycontent = EmailBody(empleavestatusobj, "rejected", "FSLineManager");
    //                            }
    //                            EmailSubject = EmailSubject.Replace("###ManagerName###",
    //                                                                obj.GetFullName(empleavestatusobj.FirstLineManager_id));
    //                            if (!string.IsNullOrEmpty(EmailSentTo))
    //                                Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
    //                        }
    //                    }
    //                    break;

    //                case "SecondLineManagerDecision":
    //                    //EmailSentTo = "First Line Manager, Second Line Manager, Leaves, Department Manager";

    //                    if ((empleavestatusobj.SecondLineManager_id == "soumyajit_chakraborty" && empleavestatusobj.SecondLineManagerStatus != 1) || (empleavestatusobj.SecondLineManager_id == "divya_chakraborty" && empleavestatusobj.SecondLineManagerStatus != 1))
    //                    {
    //                        EmailSubject = "Leave application ###Status###";

    //                        EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //                                                          obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));

    //                        if (Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus) == 2)
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
    //                        }
    //                        else
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
    //                        }
    //                        if (!string.IsNullOrEmpty(EmailSentTo))
    //                            Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
    //                    }
    //                    else
    //                    {
    //                        string emailfrom = Page.User.Identity.Name;
    //                        if (Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus) != 1)
    //                        {
    //                            if (Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus) == 2)
    //                            {
    //                                EmailSentTo = EmailSentToFunction(string.Empty, obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //                                                                  obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
    //                                EmailSubject = "Leave application approved by ###ManagerName###";
    //                                EmailBodycontent = EmailBody(empleavestatusobj, "approved", "FSLineManager");
    //                                EmailSubject = EmailSubject.Replace("###ManagerName###", obj.GetFullName(empleavestatusobj.SecondLineManager_id));
    //                            }
    //                            else
    //                            {
    //                                if (Convert.ToInt32(empleavestatusobj.Hr_Status) == 3)
    //                                {
    //                                    EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id),
    //                                                            obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //                                                            obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
    //                                    EmailSubject = "Leave application Rejected";
    //                                    EmailSubject = EmailSubject.Replace("###ManagerName###", obj.GetFullName(empleavestatusobj.Hr_id));
    //                                    EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
    //                                    emailfrom = empleavestatusobj.Hr_id;
    //                                    if (!string.IsNullOrEmpty(EmailSentTo))
    //                                        Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
    //                                }
    //                                EmailSentTo = string.Empty;
    //                                EmailSentTo = EmailSentToFunction(string.Empty, obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //                                                                  obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
    //                                EmailSubject = "Leave application rejected by ###ManagerName###";
    //                                EmailBodycontent = EmailBody(empleavestatusobj, "rejected", "FSLineManager");
    //                                EmailSubject = EmailSubject.Replace("###ManagerName###", obj.GetFullName(empleavestatusobj.SecondLineManager_id));
    //                            }
    //                            if (!string.IsNullOrEmpty(EmailSentTo))
    //                                Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"],
    //                                                 String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
    //                        }
    //                    }
    //                    break;

    //                case "HrDecision":
    //                    EmailSubject = "Leave application ###Status###";
    //                    EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //     obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
    //                    if (Convert.ToInt32(empleavestatusobj.Hr_Status) != 1)
    //                    {
    //                        if (Convert.ToInt32(empleavestatusobj.Hr_Status) == 2)
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
    //                        }
    //                        else
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
    //                        }

    //                        if (!string.IsNullOrEmpty(EmailSentTo))
    //                            Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
    //                    }
    //                    break;
    //                case "AdminDecision":
    //                    EmailSubject = "Leave application ###Status###";
    //                    EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
    //     obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
    //                    if (Convert.ToInt32(empleavestatusobj.EmpLeaveStatus) != 1)
    //                    {
    //                        if (Convert.ToInt32(empleavestatusobj.EmpLeaveStatus) == 2)
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
    //                        }
    //                        else
    //                        {
    //                            EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
    //                            EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
    //                        }

    //                        if (!string.IsNullOrEmpty(EmailSentTo))
    //                            Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
    //                    }
    //                    break;
    //            }
    //        }
    //    }
    //    catch(Exception ex)
    //    {

    //    }
    //}

    public void LeaveApplicationStatus(int id, string ApplicationStatus)
    {
        LeaveStatu empleavestatusobj = enititiesobj.LeaveStatus.FirstOrDefault(c => c.ID == id);
        string EmailSubject, DepartmentManager, EmailBodycontent;
        string EmailSentTo = EmailSubject = DepartmentManager = EmailBodycontent = string.Empty;
        try
        {
            if (empleavestatusobj != null)
            {
                //if (obj.GetDepartmentMangerid(empleavestatusobj.Department) != null)
                //    DepartmentManager = obj.GetDepartmentMangerid(empleavestatusobj.Department);

                DepartmentManager = obj.GetReportingManager(empleavestatusobj.Emp_id);

                switch (ApplicationStatus)
                {
                    case "ApplicationSubmission":
                        //EmailSentTo = "Applicant, First Line Manager, Second Line Manager, Leaves, Department Manager";
                        var applicant = enititiesobj.Employees.FirstOrDefault(c => c.Emp_Id == empleavestatusobj.Emp_id);
                        string applicantname = empleavestatusobj.Emp_id;
                        if (applicant != null)
                            applicantname = applicant.FullName;

                        EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
         obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
                        EmailSubject = "Leave application submitted by ###applicant### for ###Startdate###";
                        EmailSubject = EmailSubject.Replace("###applicant###", applicantname);
                        EmailSubject = EmailSubject.Replace("###Startdate###", Convert.ToDateTime(empleavestatusobj.FromDate).ToLongDateString());
                        EmailBodycontent = EmailBody(empleavestatusobj, "submitted", "FSLineManager");
                        if (!string.IsNullOrEmpty(EmailSentTo))
                            Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
                        break;

                    case "FirstLineManagerDecision":
                        // EmailSentTo = "First Line Manager, Second Line Manager, Leaves, Department Manager";
                        if ((empleavestatusobj.FirstLineManager_id == "soumyajit_chakraborty" && empleavestatusobj.FirstLineManagerStatus != 1) || (empleavestatusobj.FirstLineManager_id == "divya_chakraborty" && empleavestatusobj.FirstLineManagerStatus != 1))
                        {
                            EmailSubject = "Leave application ###Status###";

                            EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id),
                                                              obj.GetEmployeeOfficialEmailId(
                                                                  empleavestatusobj.FirstLineManager_id),
                                                              obj.GetEmployeeOfficialEmailId(
                                                                  empleavestatusobj.SecondLineManager_id),
                                                              obj.GetEmployeeOfficialEmailId(DepartmentManager));

                            if (Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus) == 2)
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
                            }
                            else
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
                            }

                            if (!string.IsNullOrEmpty(EmailSentTo))
                                Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");

                        }
                        else
                        {
                            EmailSentTo = EmailSentToFunction(string.Empty, obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
                                obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
                            if (Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus) != 1)
                            {
                                if (Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus) == 2)
                                {
                                    EmailSubject = "Leave application approved by ###ManagerName###";
                                    EmailBodycontent = EmailBody(empleavestatusobj, "approved", "FSLineManager");
                                }
                                else
                                {
                                    EmailSubject = "Leave application rejected by ###ManagerName###";
                                    EmailBodycontent = EmailBody(empleavestatusobj, "rejected", "FSLineManager");
                                }
                                EmailSubject = EmailSubject.Replace("###ManagerName###",
                                                                    obj.GetFullName(empleavestatusobj.FirstLineManager_id));
                                if (!string.IsNullOrEmpty(EmailSentTo))
                                    Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
                            }
                        }
                        break;

                    case "SecondLineManagerDecision":
                        //EmailSentTo = "First Line Manager, Second Line Manager, Leaves, Department Manager";

                        if ((empleavestatusobj.SecondLineManager_id == "soumyajit_chakraborty" && empleavestatusobj.SecondLineManagerStatus != 1) || (empleavestatusobj.SecondLineManager_id == "divya_chakraborty" && empleavestatusobj.SecondLineManagerStatus != 1))
                        {
                            EmailSubject = "Leave application ###Status###";

                            EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
                                                              obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));

                            if (Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus) == 2)
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
                            }
                            else
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
                            }
                            if (!string.IsNullOrEmpty(EmailSentTo))
                                Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
                        }
                        else
                        {
                            string emailfrom = Page.User.Identity.Name;
                            if (Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus) != 1)
                            {
                                if (Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus) == 2)
                                {
                                    EmailSentTo = EmailSentToFunction(string.Empty, obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
                                                                      obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
                                    EmailSubject = "Leave application approved by ###ManagerName###";
                                    EmailBodycontent = EmailBody(empleavestatusobj, "approved", "FSLineManager");
                                    EmailSubject = EmailSubject.Replace("###ManagerName###", obj.GetFullName(empleavestatusobj.SecondLineManager_id));
                                }
                                else
                                {
                                    if (Convert.ToInt32(empleavestatusobj.Hr_Status) == 3)
                                    {
                                        EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id),
                                                                obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
                                                                obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
                                        EmailSubject = "Leave application Rejected";
                                        EmailSubject = EmailSubject.Replace("###ManagerName###", obj.GetFullName(empleavestatusobj.Hr_id));
                                        EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
                                        emailfrom = empleavestatusobj.Hr_id;
                                        if (!string.IsNullOrEmpty(EmailSentTo))
                                            Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
                                    }
                                    EmailSentTo = string.Empty;
                                    EmailSentTo = EmailSentToFunction(string.Empty, obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
                                                                      obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
                                    EmailSubject = "Leave application rejected by ###ManagerName###";
                                    EmailBodycontent = EmailBody(empleavestatusobj, "rejected", "FSLineManager");
                                    EmailSubject = EmailSubject.Replace("###ManagerName###", obj.GetFullName(empleavestatusobj.SecondLineManager_id));
                                }
                                if (!string.IsNullOrEmpty(EmailSentTo))
                                    Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"],
                                                     String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
                            }
                        }
                        break;

                    case "HrDecision":
                        EmailSubject = "Leave application ###Status###";
                        EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
         obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
                        if (Convert.ToInt32(empleavestatusobj.Hr_Status) != 1)
                        {
                            if (Convert.ToInt32(empleavestatusobj.Hr_Status) == 2)
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
                            }
                            else
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
                            }

                            if (!string.IsNullOrEmpty(EmailSentTo))
                                Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
                        }
                        break;
                    case "AdminDecision":
                        EmailSubject = "Leave application ###Status###";
                        EmailSentTo = EmailSentToFunction(obj.GetEmployeeOfficialEmailId(empleavestatusobj.Emp_id), obj.GetEmployeeOfficialEmailId(empleavestatusobj.FirstLineManager_id),
         obj.GetEmployeeOfficialEmailId(empleavestatusobj.SecondLineManager_id), obj.GetEmployeeOfficialEmailId(DepartmentManager));
                        if (Convert.ToInt32(empleavestatusobj.EmpLeaveStatus) != 1)
                        {
                            if (Convert.ToInt32(empleavestatusobj.EmpLeaveStatus) == 2)
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Accepted");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Accepted", "Hr");
                            }
                            else
                            {
                                EmailSubject = EmailSubject.Replace("###Status###", "Rejected");
                                EmailBodycontent = EmailBody(empleavestatusobj, "Rejected", "Hr");
                            }

                            if (!string.IsNullOrEmpty(EmailSentTo))
                                Common.SendEmail(EmailSentTo, System.Configuration.ConfigurationManager.AppSettings["Leaves"], String.Empty, EmailSubject, EmailBodycontent, "Leave Tracker");
                        }
                        break;
                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public string EmailSentToFunction(string Employee, string FLManager, string SLManager, string DManager)
    {
        string EmailTo = string.Empty;

        if (!string.IsNullOrEmpty(Employee))
        {
            EmailTo += Employee;
        }
        if (!string.IsNullOrEmpty(FLManager))
        {
            if (!string.IsNullOrEmpty(EmailTo.Trim()))
                EmailTo += " , " + FLManager;
            else
                EmailTo += FLManager;
        }
        if (!string.IsNullOrEmpty(SLManager))
        {
            if (!string.IsNullOrEmpty(EmailTo.Trim()))
                EmailTo += " , " + SLManager;
            else
                EmailTo += SLManager;
        }
        if (!string.IsNullOrEmpty(DManager))
        {
            if (!string.IsNullOrEmpty(EmailTo.Trim()))
                EmailTo += " , " + DManager;
            else
                EmailTo += DManager;
        }

        return EmailTo;
    }

    public string EmailBody(LeaveStatu empleavestatusobj, string ApplicationStatus, string Senderidentity)
    {
        string mailbody = string.Empty;
        if (empleavestatusobj != null)
        {
            switch (Senderidentity)
            {
                case "FSLineManager":

                    //check for application status /submitted or decision taken by the first or second line manager 
                    if (ApplicationStatus == "submitted")
                    {
                        var employeedetail = enititiesobj.Employees.FirstOrDefault(c => c.Emp_Id == empleavestatusobj.Emp_id);
                        mailbody = System.IO.File.ReadAllText(Server.MapPath("~/Email/MailSubmissionTemplate.htm"));
                        mailbody = mailbody.Replace("###status###", ApplicationStatus);
                        mailbody = mailbody.Replace("###Employeename###", (employeedetail.FullName == null) ? "Not Mention" : employeedetail.FullName);
                        mailbody = mailbody.Replace("###designation###", employeedetail.tbl_E_Designation.Description);
                        mailbody = mailbody.Replace("###department###", employeedetail.tbl_E_Department.DeptName);
                        mailbody = mailbody.Replace("###Appliedon###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.LeaveAppliedDate)));
                        mailbody = mailbody.Replace("###LeaveType###", empleavestatusobj.tbl_E_LeaveType.Description);
                        mailbody = mailbody.Replace("###DateFrom###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.FromDate)));
                        mailbody = mailbody.Replace("###DateTo###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.ToDate)));
                        mailbody = mailbody.Replace("###Fromday###", Convert.ToDateTime(empleavestatusobj.FromDate).DayOfWeek.ToString());
                        mailbody = mailbody.Replace("###Today###", Convert.ToDateTime(empleavestatusobj.ToDate).DayOfWeek.ToString());

                        if (empleavestatusobj.tbl_E_LeaveType.Description.ToLower().Trim() == "half day")
                            mailbody = mailbody.Replace("###TotalDays###", "Half");
                        else
                            mailbody = mailbody.Replace("###TotalDays###", Convert.ToString((int)(Convert.ToDateTime(empleavestatusobj.ToDate).Subtract(Convert.ToDateTime(empleavestatusobj.FromDate))).TotalDays + 1));
                        mailbody = mailbody.Replace("###LeaveReason###", empleavestatusobj.LeaveReason);
                        //DataSet ds = new DataSet();
                        //var sobj = new TimelogSoapClient();
                        //var info = sobj.GetLastTwoDaysProjectInfo(Page.User.Identity.Name);
                        //var doc = new System.Xml.XmlDocument();
                        //doc.LoadXml(info.InnerXml);
                        //ds.ReadXml(new StringReader(doc.OuterXml));
                        //if (empleavestatusobj.fkLeaveType == 1 || empleavestatusobj.fkLeaveType == 2)
                        //{
                        //    string date1, date2, pname1, pname2, worknote1, worknote2, whr1, whr2;
                        //    date1 = String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[0]["logdate"].ToString()));
                        //    date2 = String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[1]["logdate"].ToString()));
                        //    pname1 = ds.Tables[0].Rows[0]["projectName"].ToString();
                        //    pname2 = ds.Tables[0].Rows[1]["projectName"].ToString();
                        //    whr1 = " Online:" + ds.Tables[0].Rows[0]["actonline"].ToString() + "  " + "Offline:" + ds.Tables[0].Rows[0]["expoffline"].ToString() + "  " + "FixedBid:" + ds.Tables[0].Rows[0]["expfixedbid"].ToString();
                        //    whr2 = " Online:" + ds.Tables[0].Rows[1]["actonline"].ToString() + "  " + "Offline:" + ds.Tables[0].Rows[1]["expoffline"].ToString() + "  " + "FixedBid:" + ds.Tables[0].Rows[1]["expfixedbid"].ToString();
                        //    worknote1 = ds.Tables[0].Rows[0]["worknote"].ToString();
                        //    worknote2 = ds.Tables[0].Rows[1]["worknote"].ToString();
                        //    string Strtable = "<table><tr><td><b>Date</b></td><td><b>Project Name</b></td><td><b>Hrs. Worked</b></td><td><b>Work Notes</b></td></tr><tr><td> " + date1 + "</td><td>" + pname1 + "</td><td>" + whr1 + "</td><td>" + worknote1 + "</td></tr><tr><td> " + date2 + "</td><td>" + pname2 + "</td><td>" + whr2 + "</td><td>" + worknote2 + "</td></tr></table>";
                        //    mailbody = mailbody.Replace("###table###", Strtable);
                        //}
                        //else
                        //{
                        mailbody = mailbody.Replace("###table###", "");
                        //}

                        if (empleavestatusobj.tbl_E_LeaveType.Description.ToLower().Trim() == "half day")
                            mailbody = mailbody.Replace("###TotalDays###", "Half");
                        else
                            mailbody = mailbody.Replace("###TotalDays###", Convert.ToString((int)(Convert.ToDateTime(empleavestatusobj.ToDate).Subtract(Convert.ToDateTime(empleavestatusobj.FromDate))).TotalDays + 1));
                        mailbody = mailbody.Replace("###LeaveReason###", empleavestatusobj.LeaveReason);

                        //doc.LoadXml(info.InnerXml);
                        //ds.ReadXml(new StringReader(doc.OuterXml));
                        //if (empleavestatusobj.fkLeaveType == 1 || empleavestatusobj.fkLeaveType == 2)
                        //{
                        //    string date1, date2, pname1, pname2, worknote1, worknote2, whr1, whr2;
                        //    date1 = String.Format("{0:MMMMd,yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[0]["logdate"].ToString()));
                        //    date2 = String.Format("{0:MMMMd,yyyy}", Convert.ToDateTime(ds.Tables[0].Rows[1]["logdate"].ToString()));
                        //    pname1 = ds.Tables[0].Rows[0]["projectName"].ToString();
                        //    pname2 = ds.Tables[0].Rows[1]["projectName"].ToString();
                        //    string online1 = "0.00";
                        //    string online2 = "0.00";
                        //    if (ds.Tables[0].Rows[0]["actonline"].ToString() != string.Empty)
                        //        online1 = ds.Tables[0].Rows[0]["actonline"].ToString();
                        //    if (ds.Tables[0].Rows[1]["actonline"].ToString() != string.Empty)
                        //        online2 = ds.Tables[0].Rows[1]["actonline"].ToString();
                        //    whr1 = " Expected:" + ds.Tables[0].Rows[0]["exponline"].ToString() + "  " + "Offline:" + ds.Tables[0].Rows[0]["expoffline"].ToString() + "  " + "Online:" + online1;
                        //    whr2 = " Expected:" + ds.Tables[0].Rows[1]["exponline"].ToString() + "  " + "Offline:" + ds.Tables[0].Rows[1]["expoffline"].ToString() + "  " + "Online:" + online2;
                        //    worknote1 = ds.Tables[0].Rows[0]["worknote"].ToString();
                        //    worknote2 = ds.Tables[0].Rows[1]["worknote"].ToString();
                        //    string Strtable = "<table><tr><td><b>Date</b></td><td><b>Project Name</b></td><td><b>Hrs. Worked</b></td><td><b>Work Notes</b></td></tr><tr><td> " + date1 + "</td><td>" + pname1 + "</td><td>" + whr1 + "</td><td>" + worknote1 + "</td></tr><tr><td> " + date2 + "</td><td>" + pname2 + "</td><td>" + whr2 + "</td><td>" + worknote2 + "</td></tr></table>";
                        //    mailbody = mailbody.Replace("###table###", Strtable);
                        //}
                        //else
                        //{
                        mailbody = mailbody.Replace("###table###", "");
                        //}
                    }
                    else
                    {
                        var employeedata = enititiesobj.Employees.FirstOrDefault(c => c.Emp_Id == empleavestatusobj.Emp_id);
                        mailbody = System.IO.File.ReadAllText(Server.MapPath("~/Email/MailTemplate.htm"));
                        mailbody = mailbody.Replace("###status###", ApplicationStatus);
                        mailbody = mailbody.Replace("###Employeename###", (employeedata.FullName == null) ? "Not Mention" : employeedata.FullName);
                        mailbody = mailbody.Replace("###designation###", employeedata.tbl_E_Designation.Description);
                        mailbody = mailbody.Replace("###department###", employeedata.tbl_E_Department.DeptName);
                        mailbody = mailbody.Replace("###Appliedon###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.LeaveAppliedDate)));
                        mailbody = mailbody.Replace("###LeaveType###", empleavestatusobj.tbl_E_LeaveType.Description);
                        mailbody = mailbody.Replace("###DateFrom###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.FromDate)));
                        mailbody = mailbody.Replace("###DateTo###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.ToDate)));
                        mailbody = mailbody.Replace("###Fromday###", Convert.ToDateTime(empleavestatusobj.FromDate).DayOfWeek.ToString());
                        mailbody = mailbody.Replace("###Today###", Convert.ToDateTime(empleavestatusobj.ToDate).DayOfWeek.ToString());

                        if (empleavestatusobj.tbl_E_LeaveType.Description.ToLower().Trim() == "half day")
                            mailbody = mailbody.Replace("###TotalDays###", "Half");
                        else
                            mailbody = mailbody.Replace("###TotalDays###", Convert.ToString((int)(Convert.ToDateTime(empleavestatusobj.ToDate).Subtract(Convert.ToDateTime(empleavestatusobj.FromDate))).TotalDays + 1));
                        mailbody = mailbody.Replace("###LeaveReason###", empleavestatusobj.LeaveReason);

                        if (obj.GetFullName(empleavestatusobj.FirstLineManager_id) != null)
                        {
                            mailbody = mailbody.Replace("##FLM###", obj.GetFullName(empleavestatusobj.FirstLineManager_id) + ":");
                            if (!string.IsNullOrEmpty(empleavestatusobj.FirstLineMangerComment))
                                mailbody = mailbody.Replace("###FLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus)) + " [Comment: " + empleavestatusobj.FirstLineMangerComment + "]");
                            else
                                mailbody = mailbody.Replace("###FLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus)));
                        }
                        else
                        {
                            mailbody = mailbody.Replace("##FLM###", string.Empty);
                            mailbody = mailbody.Replace("###FLMStatus###", string.Empty);
                        }

                        if (obj.GetFullName(empleavestatusobj.SecondLineManager_id) != null)
                        {
                            mailbody = mailbody.Replace("###SLM###", obj.GetFullName(empleavestatusobj.SecondLineManager_id) + ":");
                            if (!string.IsNullOrEmpty(empleavestatusobj.SecondLineManagerComment))
                                mailbody = mailbody.Replace("###SLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus)) + " [Comment: " + empleavestatusobj.SecondLineManagerComment + "]");
                            else
                                mailbody = mailbody.Replace("###SLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus)));
                        }
                        else
                        {
                            mailbody = mailbody.Replace("###SLM###", string.Empty);
                            mailbody = mailbody.Replace("###SLMStatus###", string.Empty);
                        }

                        if (obj.GetFullName(empleavestatusobj.Hr_id) != null)
                        {
                            mailbody = mailbody.Replace("###HRRep###", obj.GetFullName(empleavestatusobj.Hr_id) + ":");
                            if (!string.IsNullOrEmpty(empleavestatusobj.Hr_Comment))
                                mailbody = mailbody.Replace("###HRStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.Hr_Status)) + " [Comment: " + empleavestatusobj.Hr_Comment + "]");
                            else
                                mailbody = mailbody.Replace("###HRStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.Hr_Status)));
                        }
                        else
                        {
                            mailbody = mailbody.Replace("###HRRep###", string.Empty);
                            mailbody = mailbody.Replace("###HRStatus###", string.Empty);
                        }

                        mailbody = mailbody.Replace("###HrManager###", string.Empty);
                        mailbody = mailbody.Replace("###HrManagerstatus###", string.Empty);
                    }
                    //end implementation
                    break;

                case "Hr":
                    var empdata = enititiesobj.Employees.FirstOrDefault(c => c.Emp_Id == empleavestatusobj.Emp_id);
                    mailbody = System.IO.File.ReadAllText(Server.MapPath("~/Email/MailTemplate.htm"));
                    mailbody = mailbody.Replace("###status###", ApplicationStatus);
                    mailbody = mailbody.Replace("###Employeename###", (empdata.FullName == null) ? "Not Mention" : empdata.FullName);
                    mailbody = mailbody.Replace("###designation###", empdata.tbl_E_Designation.Description);
                    mailbody = mailbody.Replace("###department###", empdata.tbl_E_Department.DeptName);
                    mailbody = mailbody.Replace("###Appliedon###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.LeaveAppliedDate)));
                    mailbody = mailbody.Replace("###LeaveType###", empleavestatusobj.tbl_E_LeaveType.Description);
                    mailbody = mailbody.Replace("###DateFrom###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.FromDate)));
                    mailbody = mailbody.Replace("###DateTo###", String.Format("{0:MMMM  d, yyyy}", Convert.ToDateTime(empleavestatusobj.ToDate)));
                    mailbody = mailbody.Replace("###Fromday###", Convert.ToDateTime(empleavestatusobj.FromDate).DayOfWeek.ToString());
                    mailbody = mailbody.Replace("###Today###", Convert.ToDateTime(empleavestatusobj.ToDate).DayOfWeek.ToString());

                    if (empleavestatusobj.tbl_E_LeaveType.Description.ToLower().Trim() == "half day")
                        mailbody = mailbody.Replace("###TotalDays###", "Half");
                    else
                        mailbody = mailbody.Replace("###TotalDays###", Convert.ToString((int)(Convert.ToDateTime(empleavestatusobj.ToDate).Subtract(Convert.ToDateTime(empleavestatusobj.FromDate))).TotalDays + 1));
                    mailbody = mailbody.Replace("###LeaveReason###", empleavestatusobj.LeaveReason);

                    if (obj.GetFullName(empleavestatusobj.FirstLineManager_id) != null)
                    {
                        mailbody = mailbody.Replace("##FLM###", obj.GetFullName(empleavestatusobj.FirstLineManager_id) + ":");
                        if (!string.IsNullOrEmpty(empleavestatusobj.FirstLineMangerComment))
                            mailbody = mailbody.Replace("###FLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus)) + " [Comment: " + empleavestatusobj.FirstLineMangerComment + "]");
                        else
                            mailbody = mailbody.Replace("###FLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.FirstLineManagerStatus)));
                    }
                    else
                    {
                        mailbody = mailbody.Replace("##FLM###", string.Empty);
                        mailbody = mailbody.Replace("###FLMStatus###", string.Empty);
                    }

                    if (obj.GetFullName(empleavestatusobj.SecondLineManager_id) != null)
                    {
                        mailbody = mailbody.Replace("###SLM###", obj.GetFullName(empleavestatusobj.SecondLineManager_id) + ":");
                        if (!string.IsNullOrEmpty(empleavestatusobj.SecondLineManagerComment))
                            mailbody = mailbody.Replace("###SLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus)) + " [Comment: " + empleavestatusobj.SecondLineManagerComment + "]");
                        else
                            mailbody = mailbody.Replace("###SLMStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.SecondLineManagerStatus)));
                    }
                    else
                    {
                        mailbody = mailbody.Replace("###SLM###", string.Empty);
                        mailbody = mailbody.Replace("###SLMStatus###", string.Empty);
                    }

                    if (obj.GetFullName(empleavestatusobj.Hr_id) != null)
                    {
                        mailbody = mailbody.Replace("###HRRep###", obj.GetFullName(empleavestatusobj.Hr_id) + ":");
                        if (!string.IsNullOrEmpty(empleavestatusobj.Hr_Comment))
                            mailbody = mailbody.Replace("###HRStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.Hr_Status)) + " [Comment: " + empleavestatusobj.Hr_Comment + "]");
                        else
                            mailbody = mailbody.Replace("###HRStatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.Hr_Status)));
                    }
                    else
                    {
                        mailbody = mailbody.Replace("###HRRep###", string.Empty);
                        mailbody = mailbody.Replace("###HRStatus###", string.Empty);
                    }

                    if (obj.GetFullName(empleavestatusobj.Admin_id) != null && (!string.IsNullOrEmpty(empleavestatusobj.Admin_Comment)))
                    {
                        mailbody = mailbody.Replace("###HrManager###", obj.GetFullName(empleavestatusobj.Admin_id) + ":");
                        mailbody = mailbody.Replace("###HrManagerstatus###", obj.GetStatusDescriptionByID(Convert.ToInt32(empleavestatusobj.EmpLeaveStatus)) + " [Comment: " + empleavestatusobj.Admin_Comment + "]");
                    }
                    else
                    {
                        mailbody = mailbody.Replace("###HrManager###", string.Empty);
                        mailbody = mailbody.Replace("###HrManagerstatus###", string.Empty);
                    }
                    break;
            }
        }
        else
            mailbody = string.Empty;

        return mailbody;
    }

    public string Getemployeecode(string employeeid)
    {
        var empobj = new HRMSEntities1();
        return empobj.Employees.Where(c => c.Emp_Id == employeeid).Select(c => c.Emp_Code).FirstOrDefault();
    }

    protected int GetTotalDays(DateTime fromdate, DateTime todate)
    {
        return (int)(todate.Subtract(fromdate)).TotalDays + 1;
    }
    protected double GetTotalDaysCount(DateTime fromdate, DateTime todate,string halfDay)
    {
        if(halfDay=="True")
        {
            return Convert.ToDouble("0.5");
        }
        else 
        return (double)(todate.Subtract(fromdate)).TotalDays + 1;
    }
    protected string GetAppliedFor(string status)
    {
        if (status == "False")
        {
            return "Full Day";
        }
        else
        {
            return "Half Day";
        }

    }

    public void ViewAppliedLeaveStatusFilter(int status)
    {
        List<LeaveStatu> leavestatusobj = obj.GetAllLeaveStatus();
        string LineManager = checklinemangers(leavestatusobj.Any(c => c.FirstLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.SecondLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.Hr_id == Page.User.Identity.Name));
        int approvedleaves, rejectedleaves, totalleaves, antleaves;
        int pendingleaves = approvedleaves = rejectedleaves = totalleaves = antleaves = 0;
        switch (LineManager)
        {
            case "Admin":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.OrderByDescending(c => c.LeaveAppliedDate).Where(c => c.EmpLeaveStatus == status).ToList() : leavestatusobj.OrderByDescending(c => c.LeaveAppliedDate).ToList();
                pendingleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 3);
                antleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 0);
                totalleaves = leavestatusobj.Count();
                break;

            case "F&SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).Where(c => c.FirstLineManagerStatus == status || c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).ToList();

                var result_temp = leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).ToList();
                pendingleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                pendingleaves = pendingleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);

                approvedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                approvedleaves = approvedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);

                rejectedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                rejectedleaves = rejectedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);

                antleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                antleaves = antleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = result_temp.Count();
                break;

            case "FLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).Where(c => c.FirstLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name);
                break;

            case "SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).Where(c => c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name);
                break;

            case "HRR":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).Where(c => c.Hr_Status == status).ToList() : leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderByDescending(c => c.LeaveAppliedDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 1);
                approvedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 2);
                rejectedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 3);
                antleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 0);
                totalleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name);
                break;
        }
        LiteralPending.Text = pendingleaves.ToString();
        LiteralApproved.Text = approvedleaves.ToString();
        LiteralRejected.Text = rejectedleaves.ToString();
        LiteralTotalLeaves.Text = totalleaves.ToString();
        LiteralANT.Text = antleaves.ToString();
        LstViewViewAppliedLeaveStatus.DataBind();
    }

    public void ViewAppliedLeaveStatusFilter1(int status)
    {
        List<LeaveStatu> leavestatusobj = obj.GetAllLeaveStatus();
        string LineManager = checklinemangers(leavestatusobj.Any(c => c.FirstLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.SecondLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.Hr_id == Page.User.Identity.Name));
        int approvedleaves, rejectedleaves, totalleaves, antleaves;
        int pendingleaves = approvedleaves = rejectedleaves = totalleaves = antleaves = 0;
        switch (LineManager)
        {
            case "Admin":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.OrderByDescending(c => c.FromDate).Where(c => c.EmpLeaveStatus == status).ToList() : leavestatusobj.OrderByDescending(c => c.FromDate).ToList();
                pendingleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 3);
                antleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 0);
                totalleaves = leavestatusobj.Count();
                break;

            case "F&SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).Where(c => c.FirstLineManagerStatus == status || c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).ToList();

                var result_temp = leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).ToList();
                pendingleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                pendingleaves = pendingleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);

                approvedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                approvedleaves = approvedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);

                rejectedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                rejectedleaves = rejectedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);

                antleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                antleaves = antleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = result_temp.Count();
                break;

            case "FLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).Where(c => c.FirstLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name);
                break;

            case "SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).Where(c => c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name);
                break;

            case "HRR":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).Where(c => c.Hr_Status == status).ToList() : leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderByDescending(c => c.FromDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 1);
                approvedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 2);
                rejectedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 3);
                antleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 0);
                totalleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name);
                break;
        }
        LiteralPending.Text = pendingleaves.ToString();
        LiteralApproved.Text = approvedleaves.ToString();
        LiteralRejected.Text = rejectedleaves.ToString();
        LiteralTotalLeaves.Text = totalleaves.ToString();
        LiteralANT.Text = antleaves.ToString();
        LstViewViewAppliedLeaveStatus.DataBind();
    }

    public void ViewAppliedLeaveStatusFilter1as(int status)
    {
        List<LeaveStatu> leavestatusobj = obj.GetAllLeaveStatus();
        string LineManager = checklinemangers(leavestatusobj.Any(c => c.FirstLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.SecondLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.Hr_id == Page.User.Identity.Name));
        int approvedleaves, rejectedleaves, totalleaves, antleaves;
        int pendingleaves = approvedleaves = rejectedleaves = totalleaves = antleaves = 0;
        switch (LineManager)
        {
            case "Admin":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.OrderBy(c => c.FromDate).Where(c => c.EmpLeaveStatus == status).ToList() : leavestatusobj.OrderBy(c => c.FromDate).ToList();
                pendingleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 3);
                antleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 0);
                totalleaves = leavestatusobj.Count();
                break;

            case "F&SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).Where(c => c.FirstLineManagerStatus == status || c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).ToList();

                var result_temp = leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).ToList();
                pendingleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                pendingleaves = pendingleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);

                approvedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                approvedleaves = approvedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);

                rejectedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                rejectedleaves = rejectedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);

                antleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                antleaves = antleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = result_temp.Count();
                break;

            case "FLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).Where(c => c.FirstLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name);
                break;

            case "SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).Where(c => c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name);
                break;

            case "HRR":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).Where(c => c.Hr_Status == status).ToList() : leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderBy(c => c.FromDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 1);
                approvedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 2);
                rejectedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 3);
                antleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 0);
                totalleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name);
                break;
        }
        LiteralPending.Text = pendingleaves.ToString();
        LiteralApproved.Text = approvedleaves.ToString();
        LiteralRejected.Text = rejectedleaves.ToString();
        LiteralTotalLeaves.Text = totalleaves.ToString();
        LiteralANT.Text = antleaves.ToString();
        LstViewViewAppliedLeaveStatus.DataBind();
    }

    public void ViewAppliedLeaveStatusFilteras(int status)
    {
        List<LeaveStatu> leavestatusobj = obj.GetAllLeaveStatus();
        string LineManager = checklinemangers(leavestatusobj.Any(c => c.FirstLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.SecondLineManager_id == Page.User.Identity.Name), leavestatusobj.Any(c => c.Hr_id == Page.User.Identity.Name));
        int approvedleaves, rejectedleaves, totalleaves, antleaves;
        int pendingleaves = approvedleaves = rejectedleaves = totalleaves = antleaves = 0;
        switch (LineManager)
        {
            case "Admin":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.OrderBy(c => c.LeaveAppliedDate).Where(c => c.EmpLeaveStatus == status).ToList() : leavestatusobj.OrderBy(c => c.LeaveAppliedDate).ToList();
                pendingleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 3);
                antleaves = leavestatusobj.Count(c => c.EmpLeaveStatus == 0);
                totalleaves = leavestatusobj.Count();
                break;

            case "F&SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).Where(c => c.FirstLineManagerStatus == status || c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).ToList();

                var result_temp = leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name || c.SecondLineManager_id == Page.User.Identity.Name).ToList();
                pendingleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                pendingleaves = pendingleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);

                approvedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                approvedleaves = approvedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);

                rejectedleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                rejectedleaves = rejectedleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);

                antleaves = result_temp.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                antleaves = antleaves + result_temp.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = result_temp.Count();
                break;

            case "FLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).Where(c => c.FirstLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.FirstLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name && c.FirstLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.FirstLineManager_id == Page.User.Identity.Name);
                break;

            case "SLM":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).Where(c => c.SecondLineManagerStatus == status).ToList() : leavestatusobj.Where(c => c.SecondLineManager_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 1);
                approvedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 2);
                rejectedleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 3);
                antleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name && c.SecondLineManagerStatus == 0);
                totalleaves = leavestatusobj.Count(c => c.SecondLineManager_id == Page.User.Identity.Name);
                break;

            case "HRR":
                LstViewViewAppliedLeaveStatus.DataSource = status != 0 ? leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).Where(c => c.Hr_Status == status).ToList() : leavestatusobj.Where(c => c.Hr_id == Page.User.Identity.Name).OrderBy(c => c.LeaveAppliedDate).ToList();

                pendingleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 1);
                approvedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 2);
                rejectedleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 3);
                antleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name && c.Hr_Status == 0);
                totalleaves = leavestatusobj.Count(c => c.Hr_id == Page.User.Identity.Name);
                break;
        }
        LiteralPending.Text = pendingleaves.ToString();
        LiteralApproved.Text = approvedleaves.ToString();
        LiteralRejected.Text = rejectedleaves.ToString();
        LiteralTotalLeaves.Text = totalleaves.ToString();
        LiteralANT.Text = antleaves.ToString();
        LstViewViewAppliedLeaveStatus.DataBind();
    }

    public string checklinemangers(bool checkflm, bool checkslm, bool checkhrr)
    {
        string result = string.Empty;
        if (Page.User.IsInRole("Admin"))
            result = "Admin";
        else
        {
            if (checkslm == true && checkflm == true)
                result = "F&SLM";
            else
            {
                if (checkflm)
                    result = "FLM";
                else
                {
                    if (checkslm)
                        result = "SLM";
                    else
                    {
                        if (checkhrr)
                            result = "HRR";
                    }
                }
            }
        }
        return result;
    }

    protected void DdlFilters_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (DdlShowRecord.SelectedIndex != 0)
            {
                DataPagerManageleavestatus.PageSize = Convert.ToInt32(DdlShowRecord.Text);
                DataPagerManageleavestatus.Visible = true;
                DataPagerManageleavestatus1.PageSize = Convert.ToInt32(DdlShowRecord.Text);
                DataPagerManageleavestatus1.Visible = true;
            }
            else
            {
                if (DataPagerManageleavestatus.TotalRowCount != 0)
                {
                    DataPagerManageleavestatus.SetPageProperties(0, DataPagerManageleavestatus.TotalRowCount, false);
                    DataPagerManageleavestatus.PageSize = DataPagerManageleavestatus.TotalRowCount;
                    DataPagerManageleavestatus.Visible = false;
                    DataPagerManageleavestatus1.SetPageProperties(0, DataPagerManageleavestatus.TotalRowCount, false);
                    DataPagerManageleavestatus1.PageSize = DataPagerManageleavestatus.TotalRowCount;
                    DataPagerManageleavestatus1.Visible = false;
                }
            }
            ViewAppliedLeaveStatusFilter(Convert.ToInt32(DdlFilters.SelectedValue));
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    protected void DdlShowRecord_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (DdlShowRecord.SelectedIndex != 0)
            {
                DataPagerManageleavestatus.PageSize = Convert.ToInt32(DdlShowRecord.Text);
                DataPagerManageleavestatus.Visible = true;
                DataPagerManageleavestatus1.PageSize = Convert.ToInt32(DdlShowRecord.Text);
                DataPagerManageleavestatus1.Visible = true;
            }
            else
            {
                if (DataPagerManageleavestatus.TotalRowCount != 0)
                {
                    DataPagerManageleavestatus.SetPageProperties(0, DataPagerManageleavestatus.TotalRowCount, false);
                    DataPagerManageleavestatus.PageSize = DataPagerManageleavestatus.TotalRowCount;
                    DataPagerManageleavestatus.Visible = false;
                    DataPagerManageleavestatus1.SetPageProperties(0, DataPagerManageleavestatus.TotalRowCount, false);
                    DataPagerManageleavestatus1.PageSize = DataPagerManageleavestatus.TotalRowCount;
                    DataPagerManageleavestatus1.Visible = false;
                }
            }
            ViewAppliedLeaveStatusFilter(Convert.ToInt32(DdlFilters.SelectedValue));
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    protected void LstViewViewAppliedLeaveStatus_PagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
    {
        try
        {
            DataPagerManageleavestatus.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            DataPagerManageleavestatus1.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            ViewAppliedLeaveStatusFilter(Convert.ToInt32(DdlFilters.SelectedValue));
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    // Change status and make Comment
    protected void LstViewMyActionItems_ItemCommand(object sender, ListViewCommandEventArgs e)
    {
        try
        {
            var ddlFirstLineManagerStatus = (DropDownList)e.Item.FindControl("DdlFirstLineManagerStatus");
            var ddlSecoundLineManagerStatus = (DropDownList)e.Item.FindControl("DdlSecoundLineManagerStatus");
            var ddlHrManagerStatus = (DropDownList)e.Item.FindControl("DdlHrManagerStatus");
            var lblErrorMsgFirstLineManager = (Label)e.Item.FindControl("LblErrorMsgFirstLineManager");
            var lblErrorMsgSecoundLineManager = (Label)e.Item.FindControl("LblErrorMsgSecoundLineManager");
            var lblErrorMsgHrManager = (Label)e.Item.FindControl("LblErrorMsgHrManager");

            var txtFirstLineManagerComment = (TextBox)e.Item.FindControl("TxtFirstLineManagerComment");
            var txtSecondLineManagerComment = (TextBox)e.Item.FindControl("TxtSecondLineManagerComment");
            var txtHrcomment = (TextBox)e.Item.FindControl("TxtHrcomment");
            switch (e.CommandName)
            {
                case "FirstLineManager":
                    if (Convert.ToInt32(ddlFirstLineManagerStatus.SelectedValue) != 1 && txtFirstLineManagerComment.Text.Trim() != string.Empty)
                    {
                        obj.UpdateEmpLeaveStatus(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(ddlFirstLineManagerStatus.SelectedValue), txtFirstLineManagerComment.Text, "FirstLineManager", Page.User.Identity.Name);
                        LeaveApplicationStatus(Convert.ToInt32(e.CommandArgument.ToString()), "FirstLineManagerDecision");
                        BindListView("LnkBtnMyActionItems");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlFirstLineManagerStatus.SelectedValue) != 1)
                        {
                            lblErrorMsgFirstLineManager.Text = "Comment is required.";
                            lblErrorMsgFirstLineManager.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblErrorMsgFirstLineManager.Text = string.Empty;
                        }
                    }
                    break;

                case "SecoundLineManager":
                    if (Convert.ToInt32(ddlSecoundLineManagerStatus.SelectedValue) != 1 && txtSecondLineManagerComment.Text.Trim() != string.Empty)
                    {
                        obj.UpdateEmpLeaveStatus(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(ddlSecoundLineManagerStatus.SelectedValue), txtSecondLineManagerComment.Text, "SecoundLineManager", Page.User.Identity.Name);
                        LeaveApplicationStatus(Convert.ToInt32(e.CommandArgument.ToString()), "SecondLineManagerDecision");
                        BindListView("LnkBtnMyActionItems");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlSecoundLineManagerStatus.SelectedValue) != 1)
                        {
                            lblErrorMsgSecoundLineManager.Text = "Comment is required.";
                            lblErrorMsgSecoundLineManager.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblErrorMsgSecoundLineManager.Text = string.Empty;
                        }
                    }
                    break;

                case "HrManager":
                    if (Convert.ToInt32(ddlHrManagerStatus.SelectedValue) != 1 && txtHrcomment.Text.Trim() != string.Empty)
                    {
                        obj.UpdateEmpLeaveStatus(Convert.ToInt32(e.CommandArgument.ToString()), Convert.ToInt32(ddlHrManagerStatus.SelectedValue), txtHrcomment.Text, "HrManager", Page.User.Identity.Name);
                        LeaveApplicationStatus(Convert.ToInt32(e.CommandArgument.ToString()), "HrDecision");
                        BindListView("LnkBtnMyActionItems");
                    }
                    else
                    {
                        if (Convert.ToInt32(ddlHrManagerStatus.SelectedValue) != 1)
                        {
                            lblErrorMsgHrManager.Text = "Comment is required.";
                            lblErrorMsgHrManager.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            lblErrorMsgHrManager.Text = string.Empty;
                        }
                    }
                    break;
            }
            for (int i = 0; i < LstViewMyActionItems.Items.Count; i++)
            {
                if (i != e.Item.DataItemIndex)
                {
                    ((Label)LstViewMyActionItems.Items[i].FindControl("LblErrorMsgFirstLineManager")).Text = ((Label)LstViewMyActionItems.Items[i].FindControl("LblErrorMsgSecoundLineManager")).Text = ((Label)LstViewMyActionItems.Items[i].FindControl("LblErrorMsgHrManager")).Text = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    // Distingush section and access for firstline , secondline , hr and admin manager
    protected void LstViewMyActionItems_ItemDataBound(object sender, ListViewItemEventArgs e)
    {
        try
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                var ddlFirstLineManagerStatus = (DropDownList)e.Item.FindControl("DdlFirstLineManagerStatus");
                var ddlSecoundLineManagerStatus = (DropDownList)e.Item.FindControl("DdlSecoundLineManagerStatus");
                var ddlHrManagerStatus = (DropDownList)e.Item.FindControl("DdlHrManagerStatus");

                var lblFirstLineManagerStatus = (Label)e.Item.FindControl("LblFirstLineManagerStatus");
                var lblSecondLineManagerStatus = (Label)e.Item.FindControl("LblSecondLineManagerStatus");
                var lblHrStatus = (Label)e.Item.FindControl("LblHr_Status");

                var lblFromDate = (Label)e.Item.FindControl("LblFromDate");
                var lblLeaveAppliedDate = (Label)e.Item.FindControl("LblLeaveAppliedDate");

                var lblEmpLeaveStatus = (Label)e.Item.FindControl("LblEmpLeaveStatus");

                var firstLineManagerid = (Label)e.Item.FindControl("LblFirstLineMaganerid");
                var secoundLineManagerid = (Label)e.Item.FindControl("LblSecoundLineManagerid");
                var hrid = (Label)e.Item.FindControl("LblHrid");

                var lblFirstLineStatusComment = (Label)e.Item.FindControl("LblFirstLineStatusComment");
                var lblSecondLineStatusComment = (Label)e.Item.FindControl("LblSecondLineStatusComment");
                var lblHrStatusComment = (Label)e.Item.FindControl("LblHrStatusComment");

                var txtFirstLineManagerComment = (TextBox)e.Item.FindControl("TxtFirstLineManagerComment");
                var txtSecondLineManagerComment = (TextBox)e.Item.FindControl("TxtSecondLineManagerComment");
                var txtHrcomment = (TextBox)e.Item.FindControl("TxtHrcomment");

                txtFirstLineManagerComment.Text = txtSecondLineManagerComment.Text = txtHrcomment.Text = string.Empty;

                var lnkFirstLineManagerComment = (LinkButton)e.Item.FindControl("LnkFirstLineManagerComment");
                var lnkBtnSecondLineManagerComment = (LinkButton)e.Item.FindControl("LnkBtnSecondLineManagerComment");
                var lnkBtnHrManagerComment = (LinkButton)e.Item.FindControl("LnkBtnHrManagerComment");

                if (e.Item.FindControl("LblSrNo") != null)
                    ((Label)e.Item.FindControl("LblSrNo")).Text = (e.Item.DataItemIndex + 1).ToString();
                //First Line Manager
                if (firstLineManagerid.Text.ToLower().Trim() == Page.User.Identity.Name.ToLower().Trim() && Convert.ToInt32(lblFirstLineManagerStatus.Text) == 1 && DateTime.Now.Date <= Convert.ToDateTime(lblFromDate.Text).Date)
                {
                    ddlFirstLineManagerStatus.Visible = txtFirstLineManagerComment.Visible = lnkFirstLineManagerComment.Visible = true;
                    ddlFirstLineManagerStatus.Items.FindByValue(lblFirstLineManagerStatus.Text).Selected = true;
                    lblFirstLineStatusComment.Visible = false;
                }

                //Secound Line Manager
                if (secoundLineManagerid.Text.ToLower().Trim() == Page.User.Identity.Name.ToLower().Trim() && Convert.ToInt32(lblSecondLineManagerStatus.Text) == 1 && DateTime.Now.Date <= Convert.ToDateTime(lblFromDate.Text).Date)
                {
                    ddlSecoundLineManagerStatus.Visible = txtSecondLineManagerComment.Visible = lnkBtnSecondLineManagerComment.Visible = true;
                    ddlSecoundLineManagerStatus.Items.FindByValue(lblSecondLineManagerStatus.Text).Selected = true;
                    lblSecondLineStatusComment.Visible = false;
                }

                //Hr Manager
                if (hrid.Text.ToLower().Trim() == Page.User.Identity.Name.ToLower().Trim() && Convert.ToInt32(lblHrStatus.Text) == 1 && DateTime.Now.Date <= Convert.ToDateTime(lblFromDate.Text).Date)
                {
                    if (Convert.ToInt32(lblFirstLineManagerStatus.Text) == 1 && Convert.ToInt32(lblSecondLineManagerStatus.Text) == 1)
                        return;
                    else
                    {
                        ddlHrManagerStatus.Visible = txtHrcomment.Visible = lnkBtnHrManagerComment.Visible = true;
                        lblHrStatusComment.Visible = false;
                        DisableHRRStatus(lblFirstLineManagerStatus.Text, lblSecondLineManagerStatus.Text, ddlHrManagerStatus);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Text = ex.Message;
            lblErrorMessage.CssClass = "error";
            lblErrorMessage.Visible = true;
        }
    }

    protected void LstViewEmployeeLeaveStatus_Sorting(object sender, ListViewSortEventArgs e)
    {
        ImageButton imAppliedOn = LstViewEmployeeLeaveStatus.FindControl("imAppliedOn") as ImageButton;
        ImageButton imDate = LstViewEmployeeLeaveStatus.FindControl("imDate") as ImageButton;

        string DefaultSortIMG = "~/image/dsc.jpg";

        string imgUrl = "~/image/asc.jpg";

        if (ViewState["SortExpression"] != null)
        {

            if (ViewState["SortExpression"].ToString() == e.SortExpression)
            {
                ViewState["SortExpression"] = null;
                imgUrl = DefaultSortIMG;
            }
            else
            {
                ViewState["SortExpression"] = e.SortExpression;
            }
        }
        else
        {
            ViewState["SortExpression"] = e.SortExpression;
        }

        switch (e.SortExpression)
        {
            case "AppliedOn":
                if (imDate != null)
                    imDate.ImageUrl = DefaultSortIMG;
                if (imAppliedOn != null)
                    imAppliedOn.ImageUrl = imgUrl;
                break;
            case "Date":
                if (imAppliedOn != null)
                    imAppliedOn.ImageUrl = DefaultSortIMG;
                if (imDate != null)
                    imDate.ImageUrl = imgUrl;
                break;
        }
        string order = "";
        if (ViewState["SortExpression"] != null)
        {
            order = "ASC";
        }
        else
        {
            order = "DESC";
        }
        PnlApplyLeave.Visible = PnlLeaveCommentStatus.Visible = PnlCalendar.Visible = PnlMyActionItems.Visible = false;
        PnlViewLeaveStatus.Visible = true;
        BindList("ViewLeaveStatus", order, e.SortExpression);
    }

    protected void LstViewViewAppliedLeaveStatus_Sorting(object sender, ListViewSortEventArgs e)
    {
        ImageButton imAppliedOn = LstViewViewAppliedLeaveStatus.FindControl("imAppliedOn") as ImageButton;
        ImageButton imDate = LstViewViewAppliedLeaveStatus.FindControl("imDate") as ImageButton;

        string DefaultSortIMG = "~/image/dsc.jpg";
        string imgUrl = "~/image/asc.jpg";
        if (ViewState["SortExpression"] != null)
        {
            if (ViewState["SortExpression"].ToString() == e.SortExpression)
            {
                ViewState["SortExpression"] = null;
                imgUrl = DefaultSortIMG;
            }
            else
            {
                ViewState["SortExpression"] = e.SortExpression;
            }
        }
        else
        {
            ViewState["SortExpression"] = e.SortExpression;
        }

        switch (e.SortExpression)
        {
            case "AppliedOn":
                if (imDate != null)
                    imDate.ImageUrl = DefaultSortIMG;
                if (imAppliedOn != null)
                    imAppliedOn.ImageUrl = imgUrl;
                break;
            case "Date":
                if (imAppliedOn != null)
                    imAppliedOn.ImageUrl = DefaultSortIMG;
                if (imDate != null)
                    imDate.ImageUrl = imgUrl;
                break;
        }

        string order = "";
        order = (ViewState["SortExpression"] != null) ? "ASC" : "DESC";
        PnlApplyLeave.Visible = PnlViewLeaveStatus.Visible = PnlCalendar.Visible = PnlMyActionItems.Visible = false;
        PnlLeaveCommentStatus.Visible = true;
        BindList("ViewAppliedLeaveStatus", order, e.SortExpression);
    }

    protected void LstViewMyActionItems_Sorting(object sender, ListViewSortEventArgs e)
    {
        var imAppliedOn = LstViewMyActionItems.FindControl("imAppliedOn") as ImageButton;
        var imDate = LstViewMyActionItems.FindControl("imDate") as ImageButton;

        string DefaultSortIMG = "~/image/dsc.jpg";
        string imgUrl = "~/image/asc.jpg";

        if (ViewState["SortExpression"] != null)
        {
            if (ViewState["SortExpression"].ToString() == e.SortExpression)
            {
                ViewState["SortExpression"] = null;
                imgUrl = DefaultSortIMG;
            }
            else
            {
                ViewState["SortExpression"] = e.SortExpression;
            }
        }
        else
        {
            ViewState["SortExpression"] = e.SortExpression;
        }

        switch (e.SortExpression)
        {
            case "AppliedOn":
                if (imDate != null)
                    imDate.ImageUrl = DefaultSortIMG;
                if (imAppliedOn != null)
                    imAppliedOn.ImageUrl = imgUrl;
                break;
            case "Date":
                if (imAppliedOn != null)
                    imAppliedOn.ImageUrl = DefaultSortIMG;
                if (imDate != null)
                    imDate.ImageUrl = imgUrl;
                break;
        }

        string order = "";
        order = (ViewState["SortExpression"] != null) ? "ASC" : "DESC";
        PnlApplyLeave.Visible = PnlViewLeaveStatus.Visible = PnlCalendar.Visible = PnlLeaveCommentStatus.Visible = false;
        PnlMyActionItems.Visible = true;
        BindList("LnkBtnMyActionItems", order, e.SortExpression);
    }

    public void AuthenticateUser()
    {
        var enititiesobj = new HRMSEntities1();
        Employee objEmployee = obj.GetEmployee(Page.User.Identity.Name.Trim().ToLower());
        var check = Request.RawUrl;
        int ID = objEmployee.ID;
        //For Probation period 
        var IsProbationLeave = enititiesobj.Emp_ProbationStatus(ID).FirstOrDefault();
        Session["IsProbationLeave"] = IsProbationLeave;
        int IsProbation = 0;
        int.TryParse(Convert.ToString(Session["IsProbationLeave"]), out IsProbation);
        var command = enititiesobj.Database.Connection.CreateCommand();
        if (IsProbation == 0)
        {
            //command.CommandText = "[Emp_GetEmployeePendingLeaves]";
            command.CommandText = "[Emp_GetEmployeePendingLeaves]";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Userid", ID));
            enititiesobj.Database.Connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Session["SickLeavesLeft"] = Convert.ToDecimal(reader["SickLeavesLeft"]);
                Session["SickLeaveTaken"] = Convert.ToDecimal(reader["SickLeaveTaken"]);
                Session["CasualLeavesLeft"] = Convert.ToDecimal(reader["CasualLeavesLeft"]);
                Session["CasualLeaveTaken"] = Convert.ToDecimal(reader["CasualLeaveTaken"]);
                Session["PaidLeavesLeft"] = Convert.ToDecimal(reader["PaidLeavesLeft"]);
                Session["PaidLeaveTaken"] = Convert.ToDecimal(reader["PaidLeaveTaken"]);
                Session["TotalLeaves"] = Convert.ToDecimal(reader["TotalLeavesLeft"]);
                Session["AllowedTotalLeaves"] = Convert.ToDecimal(reader["AllowedTotalLeaves"]);
                Session["LWP"] = Convert.ToDecimal(reader["LWP"]);
                Session["TotalLeaveTaken"] = Convert.ToDecimal(reader["TotalLeaveTaken"]);
                Session["AllowedCasualLeaves"] = Convert.ToDecimal(reader["AllowedCasualLeaves"]);
                Session["AllowedSickLeaves"] = Convert.ToDecimal(reader["AllowedSickLeaves"]);
                Session["AllowedPaidLeaves"] = Convert.ToDecimal(reader["AllowedPaidLeaves"]);
            }
        }
        else
        {
            command.CommandText = "[Emp_GetEmployeePendingLeaves_Probation]";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@Userid", ID));
            enititiesobj.Database.Connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                Session["AllowedTotalLeaves"] = Convert.ToDecimal(reader["AllowedTotalLeaves"]);
                Session["TotalLeaves"] = Convert.ToDecimal(reader["PaidLeavesLeft"]);
                Session["TotalLeaveTaken"] = Convert.ToDecimal(reader["TotalLeaveTaken"]);
                Session["LWP"] = Convert.ToDecimal(reader["LWP"]);
            }
        }
        if (objEmployee != null)
        {
            List<EmployeeRole> erole = obj.GetEmployeeRoleByID(objEmployee.ID);
            string EmpRole = "User";
            if (erole.Count() != 0)
            {
                EmpRole = erole.Select(q => q.tbl_E_Role.RoleName).Aggregate((a, b) => a + "," + b);
            }
            //Session["EmployeeName"] = EmployeeName;
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, objEmployee.Emp_Id, DateTime.Now, DateTime.Now.AddMinutes(20), false, EmpRole, FormsAuthentication.FormsCookiePath);
            //For security reasons we may hash the cookies
            string hashCookies = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashCookies);
            // add the cookie to user browser
            Response.Cookies.Add(cookie);
            assignNewSessionValue();
        }
    }


    public void assignNewSessionValue()
    {

        if (Session["IsProbationLeave"].ToString() == "0")
        {
            var iid = Request.QueryString["empid"];
            var SickLeavesLeft = Session["SickLeavesLeft"].ToString();
            var CasualLeavesLeft = Session["CasualLeavesLeft"].ToString();
            var PaidLeavesLeft = Session["PaidLeavesLeft"].ToString();
            var SickLeaveTaken = Session["SickLeaveTaken"].ToString();
            var CasualLeaveTaken = Session["CasualLeaveTaken"].ToString();
            var PaidLeaveTaken = Session["PaidLeaveTaken"].ToString();
            var TotalLeaves = Session["TotalLeaves"].ToString();
            var AllowedTotalLeaves = Session["AllowedTotalLeaves"].ToString();
            var LWP = Session["LWP"].ToString();
            var TotalLeaveTaken = Session["TotalLeaveTaken"].ToString();
            var AllowedCasualLeaves = Session["AllowedCasualLeaves"].ToString();
            var AllowedSickLeaves = Session["AllowedSickLeaves"].ToString();
            var AllowedPaidLeaves = Session["AllowedPaidLeaves"].ToString();
            //lblTotalLeavescount.Text = TotalLeaves;
            lblPaidLeavesLeftcount.Text = PaidLeavesLeft;
            lblPaidLeavesTakencount.Text = PaidLeaveTaken;
            lblCasualLeavesLeft.Text = CasualLeavesLeft;
            lblCasualLeavesTaken.Text = CasualLeaveTaken;
            lblSickLeavesLeft.Text = SickLeavesLeft;
            lblSickLeavesTaken.Text = SickLeaveTaken;
            //  lblAllowedTotalLeaves.Text = AllowedTotalLeaves;
            lblLWP.Text = LWP;
            lblAllowedLeaves.Text = AllowedTotalLeaves;
            lblAllowedCasualLeaves.Text = AllowedCasualLeaves;
            lblAllowedSickLeaves.Text = AllowedSickLeaves;
            lblAllowedPaidLeaves.Text = AllowedPaidLeaves;
            lblTotalLeaveTaken.Text = TotalLeaveTaken;
            if (obj.DiplayLeaveType() != null)
            {
                DdlLeaveType.DataSource = obj.DiplayLeaveType();

                DdlLeaveType.DataTextField = "Description";
                DdlLeaveType.DataValueField = "ID";
                DdlLeaveType.DataBind();
                DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
                DdlLeaveType.Items.Insert(3, new ListItem("Earned Leave", "5"));
            }
            else
                DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
            clear();
        }
        else
        {
            var AllowedTotalLeaves = Session["AllowedTotalLeaves"].ToString();
            var TotalLeavesLeft = Session["TotalLeaves"].ToString();
            var TotalLeaveTaken = Session["TotalLeaveTaken"].ToString();
            var LWP = Session["LWP"].ToString();
            lblAllowedTotalLeaves.Text = AllowedTotalLeaves;
            lblTotalLeaveTaken.Text = TotalLeaveTaken;
            lblTotalLeavesLeft.Text = TotalLeavesLeft;
            lblProbLWP.Text = LWP;
            if (obj.DiplayProbLeaveType() != null)
            {
                DdlLeaveType.DataSource = obj.DiplayProbLeaveType();

                DdlLeaveType.DataTextField = "Description";
                DdlLeaveType.DataValueField = "ID";
                DdlLeaveType.DataBind();
                DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
            }
            else
                DdlLeaveType.Items.Insert(0, new ListItem("---Select---", "0"));
            clear();
        }
    }

}
