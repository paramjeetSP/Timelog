using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
using System.Web.Security;
using System.Data;
using System.Data.SqlClient;

public partial class Default2 : System.Web.UI.Page
{
    LeaveOperation obj = new LeaveOperation();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["empid"] != null)
                AuthenticateUser();
            else
            {
                lblErrorMessage.Visible = true;
                lblErrorMessage.Text = "OOPS Something went wrong please contact your administrator";
                lblErrorMessage.CssClass = "loginerror";
            }
        }
        catch (Exception ex)
        {
            lblErrorMessage.Visible = true;
            lblErrorMessage.Text = ex.ToString();
            lblErrorMessage.CssClass = "loginerror";
        }

    }

    public void AuthenticateUser()
    {
        var enititiesobj = new HRMSEntities1();
        Employee objEmployee = obj.GetEmployee(Request.QueryString["empid"].ToString().Trim().ToLower());
        var check = Request.RawUrl;
        int ID = objEmployee.ID;
        // var totalcount = enititiesobj.Emp_GetEmployeePendingLeaves(ID).FirstOrDefault();    
        //Session["SickLeavesLeft"] = totalcount.SickLeavesLeft;
        //Session["CasualLeavesLeft"] = totalcount.CasualLeavesLeft;
        //Session["PaidLeavesLeft"] = totalcount.PaidLeavesLeft;
        //Session["TotalLeaves"] = totalcount.TotalLeavesLeft;
        //Session["AllowedTotalLeaves"] = totalcount.AllowedTotalLeaves;
        //Session["LWP"] = totalcount.LWP;
       
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
                Session["TotalLeaves"] = Convert.ToDecimal(reader["CasualLeavesLeft"]);
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
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                Response.Redirect("~/Default.aspx?id=" + '#' + Request.QueryString["id"]);
            else
                Response.Redirect("~/Default.aspx");

        }
    }
    protected void btnsend_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(TxtEmp_id.Text))
            Response.Redirect("Default2.aspx?empid=" + TxtEmp_id.Text);
    }
}