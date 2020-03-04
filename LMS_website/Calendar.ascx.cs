using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BAL;
public partial class Calender : System.Web.UI.UserControl
{
	private HRMSEntities1 objhrms = new HRMSEntities1();
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			Calendar1.DayRender += new DayRenderEventHandler(this.Calendar1_DayRender);

			Calendar1.OtherMonthDayStyle.ForeColor = System.Drawing.Color.Black;
			Calendar1.TitleStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml("#E56717");
			Calendar1.TitleStyle.BackColor = System.Drawing.Color.White;
			Calendar1.DayStyle.BackColor = System.Drawing.Color.White;
			Calendar1.ShowDayHeader = true;
			Calendar1.ShowTitle = true;
			Calendar1.ShowGridLines = true;
			Calendar1.ShowNextPrevMonth = true;
			if (Page.User.IsInRole("Admin"))
				Dddepartment.Visible = true;
			else
			{
				var id = objhrms.Employees.FirstOrDefault(c => c.Emp_Id == Page.User.Identity.Name).ID.ToString();
				if (objhrms.tbl_E_Department.Any(c => c.DepartmentHead == id))
				{
					Dddepartment.Visible = true;
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
	private void Calendar1_DayRender(Object source, DayRenderEventArgs e)
	{
		try
		{

			var holidaylist =
			   objhrms.tbl_E_CalendarHoliday.FirstOrDefault(
				   c => c.IsDeleted == false && c.IsPublished == true && c.OccasionDate == e.Day.Date);
			var firstdateofthemonth = GetFirstDayOfMonth(e.Day.Date);
			if (e.Day.IsWeekend)
			{
				e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#f1f1f1");
				e.Cell.ToolTip = "Weekend";
			}
			if (e.Day.IsToday)
			{
				e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFACD");
				e.Cell.ForeColor = System.Drawing.Color.Black;
			}
			//var lastdate = GetLastDayofMonth(e.Day.Date);
			if (firstdateofthemonth == e.Day.Date)
			{
				e.Cell.Controls.Add(
						new LiteralControl(
							"<div style='padding-right:20px;color:#222222;margin-top:-16px;font-weight:bold;font-size:12px;'>" +
							e.Day.Date.ToString("MMMM").ToUpper() + "<br/></div>"));
			}

			if (Page.User.IsInRole("Admin"))
			{
				List<LeaveStatu> adminresult;
				if (Dddepartment.SelectedItem.Text == "Show All")
					adminresult = objhrms.LeaveStatus.Where(c => (c.FromDate <= e.Day.Date && c.ToDate >= e.Day.Date)).ToList();
				else
					adminresult = objhrms.LeaveStatus.Where(c => c.Department == Dddepartment.SelectedValue && (c.FromDate <= e.Day.Date && c.ToDate >= e.Day.Date)).ToList();

				if (adminresult.Count != 0)
				{
					string status = string.Empty;
					foreach (var items in adminresult)
					{
						var firstOrDefault = objhrms.Employees.FirstOrDefault(c => c.Emp_Id == items.Emp_id);
						if (firstOrDefault != null)
							//        status = status + "<tr valign='top' style='height:20px;'><td>" +
							//                StrSplitMethod(firstOrDefault.FullName) +
							//                 "</td>" +
							//                 "<td>" + ApplyCss(Convert.ToInt32(items.EmpLeaveStatus)) + "</td></tr>";
							//}
							status = status + "<tr valign='top' style='height:20px;'><td>" +
								  StrSplitMethod(firstOrDefault.FullName) +
									 "</td>" +
			"<td class='calendar' " + "dayofweek=" + "'" + e.Day.Date.DayOfWeek + "'" + "msg=" + "'" + "Applied On" + ":&nbsp;&nbsp;" + Convert.ToDateTime(items.LeaveAppliedDate).ToString("dd MMMM, yyyy") + "<br/>" + GetEmployeename(items.FirstLineManager_id) + ":&nbsp;&nbsp;" + ApplyCssForpopup(Convert.ToInt32(items.FirstLineManagerStatus)) + "<br/>" + GetEmployeename(items.SecondLineManager_id) + ":&nbsp;&nbsp;" + ApplyCssForpopup(Convert.ToInt32(items.SecondLineManagerStatus)) + "<br/>" + GetEmployeename(items.Hr_id) + ":&nbsp;&nbsp;" + ApplyCssForpopup(Convert.ToInt32(items.Hr_Status)) + "'>" + ApplyCss(Convert.ToInt32(items.EmpLeaveStatus)) + "</td></tr>";
					}
					if (holidaylist != null)
					{
						e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCEEFF");
						e.Cell.Controls.Add(new LiteralControl("<div style='min-height:20px; text-align:center; font-size:10px; font-family:Verdana; font-weight: normal; color:Black; '>" + holidaylist.OccasionName + "</div>"));
						e.Cell.ToolTip = "Holiday";
					}
					e.Cell.Controls.Add(
						new LiteralControl(
							"<div style='min-height:88px;'><table style='text-align:left; font-size:10px; font-family:Verdana; font-weight: normal; color:#238EC6; padding-right:10px;'>" +
							status + "</table></div>"));

				}
				else
				{
					if (holidaylist != null)
					{
						e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCEEFF");
						e.Cell.Controls.Add(new LiteralControl("<div style='min-height:88px; text-align:center; font-size:10px; font-family:Verdana; font-weight: normal; color:Black; '>" + holidaylist.OccasionName + "</div>"));
						e.Cell.ToolTip = "Holiday";
					}
					else
						e.Cell.Controls.Add(new LiteralControl("<div style='min-height:88px;'> </div>"));
				}
			}
			else
			{
				List<LeaveStatu> result;
				var firstOrDefault = objhrms.Employees.FirstOrDefault(c => c.Emp_Id == Page.User.Identity.Name);
				string empid = Convert.ToString(firstOrDefault.ID);
				if (objhrms.tbl_E_Department.Any(c => c.DepartmentHead == empid))
				{
					result = Dddepartment.SelectedItem.Text == "Show All" ? objhrms.LeaveStatus.Where(c => (c.FromDate <= e.Day.Date && c.ToDate >= e.Day.Date)).ToList() : objhrms.LeaveStatus.Where(c => c.Department == Dddepartment.SelectedValue && (c.FromDate <= e.Day.Date && c.ToDate >= e.Day.Date)).ToList();
				}
				else
				{
					var departmentname = firstOrDefault.fkDepartment.ToString();
					result = objhrms.LeaveStatus.Where(c => c.Department == departmentname && (c.FromDate <= e.Day.Date && c.ToDate >= e.Day.Date)).ToList();
				}
				if (result.Count != 0)
				{
					string status = string.Empty;
					foreach (var items in result)
					{
						var orDefault = objhrms.Employees.FirstOrDefault(c => c.Emp_Id == items.Emp_id);
						if (orDefault != null)
							status = status + "<tr valign='top' style='height:20px;'><td>" +
								  StrSplitMethod(orDefault.FullName) +
									 "</td>" +
								//					 "<td class='leavestatus' msg=''>" + ApplyCss(Convert.ToInt32(items.EmpLeaveStatus)) + "</td></tr>";
				"<td class='calendar' " + "dayofweek=" + "'" + e.Day.Date.DayOfWeek + "'" + "msg=" + "'" + "Applied On" + ":&nbsp;&nbsp;" + Convert.ToDateTime(items.LeaveAppliedDate).ToString("dd MMMM, yyyy") + "<br/>" + GetEmployeename(items.FirstLineManager_id) + ":&nbsp;&nbsp;" + ApplyCssForpopup(Convert.ToInt32(items.FirstLineManagerStatus)) + "<br/>" + GetEmployeename(items.SecondLineManager_id) + ":&nbsp;&nbsp;" + ApplyCssForpopup(Convert.ToInt32(items.SecondLineManagerStatus)) + "<br/>" + GetEmployeename(items.Hr_id) + ":&nbsp;&nbsp;" + ApplyCssForpopup(Convert.ToInt32(items.Hr_Status)) + "'>" + ApplyCss(Convert.ToInt32(items.EmpLeaveStatus)) + "</td></tr>";
					}
					if (holidaylist != null)
					{
						e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCEEFF");
						e.Cell.Controls.Add(new LiteralControl("<div style='min-height:20px; text-align:center; font-size:10px; font-family:Verdana; font-weight: normal; color:Black; '>" + holidaylist.OccasionName + "</div>"));
						e.Cell.ToolTip = "Holiday";
					}
					e.Cell.Controls.Add(
						new LiteralControl(
							"<div style='min-height:88px;'><table style='text-align:left; font-size:10px; font-family:Verdana; font-weight: normal; color:#238EC6; padding-right:10px;'>" +
							status + "</table></div>"));

				}
				else
				{
					if (holidaylist != null)
					{
						e.Cell.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCEEFF");
						e.Cell.Controls.Add(new LiteralControl("<div style='min-height:88px; text-align:center; font-size:10px; font-family:Verdana; font-weight: normal; color:Black; '>" + holidaylist.OccasionName + "</div>"));
						e.Cell.ToolTip = "Holiday";
					}
					else
						e.Cell.Controls.Add(new LiteralControl("<div style='min-height:88px;'> </div>"));
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


	public string StrSplitMethod(string text)
	{
		string[] str = text.Split((string[])null, StringSplitOptions.RemoveEmptyEntries);
		return "<span title='" + text + "'>" + str[0] + " " + str[1].Substring(0, 1) + "." + " </span>";
	}

	public string ApplyCss(int id)
	{

		switch (id)
		{
			case 0:
				return "<span title='Action Not Taken' class='ANT'>ANT</span>";

			case 1:
				return "<span title='Pending' class='Pending'>Pending</span>";

			case 2:
				return "<span title='Approved' class='Approved'>Approved</span>";

			case 3:
				return "<span title='Rejected' class='Rejected'>Rejected</span>";

			default:
				return string.Empty;
		}

	}

	public string ApplyCssForpopup(int id)
	{
		switch (id)
		{
			case 0:
				return "Action Not Taken";

			case 1:
				return "Pending";

			case 2:
				return "Approved";

			case 3:
				return "Rejected";

			default:
				return string.Empty;
		}
	}

	public string GetEmployeename(string emp_id)
	{
		var firstOrDefault = objhrms.Employees.FirstOrDefault(c => c.Emp_Id == emp_id);
		if (firstOrDefault != null)
			return firstOrDefault.FullName;
		else
			return emp_id;
	}

	//override protected void OnInit(EventArgs e)
	//{
	//    try
	//    {
	//        //
	//        // CODEGEN: This call is required by the ASP.NET Web Form Designer.
	//        //
	//        InitializeComponent();
	//        base.OnInit(e);
	//    }
	//    catch (Exception ex)
	//    {
	//        lblErrorMessage.Text = ex.Message;
	//        lblErrorMessage.CssClass = "error";
	//        lblErrorMessage.Visible = true;
	//    }
	//}

	///// <summary>
	///// Required method for Designer support - do not modify
	///// the contents of this method with the code editor.
	///// </summary>
	//private void InitializeComponent()
	//{
	//    try
	//    {
	//        this.Calendar1.DayRender += new System.Web.UI.WebControls.DayRenderEventHandler(this.Calendar1_DayRender);
	//        this.Load += new System.EventHandler(this.Page_Load);
	//    }
	//    catch (Exception ex)
	//    {
	//        lblErrorMessage.Text = ex.Message;
	//        lblErrorMessage.CssClass = "error";
	//        lblErrorMessage.Visible = true;
	//    }
	//}

	private DateTime GetLastDayofMonth(DateTime dtDate)
	{
		DateTime dtToDate = dtDate;
		dtToDate = dtToDate.AddMonths(1);
		dtToDate = dtToDate.AddDays(-(dtToDate.Day));
		return dtToDate;
	}

	/// <summary>
	/// Get the first day of the month for
	/// any full date submitted
	/// </summary>
	/// <param name="dtDate"></param>
	/// <returns></returns>

	private DateTime GetFirstDayOfMonth(DateTime dtDate)
	{
		// set return value to the first day of the month
		// for any date passed in to the method
		// create a datetime variable set to the passed in date
		DateTime dtFrom = dtDate;
		// remove all of the days in the month
		// except the first day and set the
		// variable to hold that date
		dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));
		// return the first day of the month
		return dtFrom;
	}


	/// <summary>
	/// Get the first day of the month for a
	/// month passed by it's integer value
	/// </summary>
	/// <param name="iMonth"></param>
	/// <returns></returns>

	private DateTime GetFirstDayOfMonth(int iMonth)
	{
		// set return value to the last day of the month
		// for any date passed in to the method
		// create a datetime variable set to the passed in date
		DateTime dtFrom = new DateTime(DateTime.Now.Year, iMonth, 1);
		// remove all of the days in the month
		// except the first day and set the
		// variable to hold that date
		dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));
		// return the first day of the month
		return dtFrom;
	}


	/// <summary>
	/// Get the last day of the month for any
	/// full date
	/// </summary>
	/// <param name="dtDate"></param>
	/// <returns></returns>

	private DateTime GetLastDayOfMonth(DateTime dtDate)
	{
		// set return value to the last day of the month
		// for any date passed in to the method
		// create a datetime variable set to the passed in date
		DateTime dtTo = dtDate;
		// overshoot the date by a month
		dtTo = dtTo.AddMonths(1);
		// remove all of the days in the next month
		// to get bumped down to the last day of the
		// previous month
		dtTo = dtTo.AddDays(-(dtTo.Day));
		// return the last day of the month
		return dtTo;
	}


	/// <summary>
	/// Get the last day of a month expressed by it's
	/// integer value
	/// </summary>
	/// <param name="iMonth"></param>
	/// <returns></returns>

	private DateTime GetLastDayOfMonth(int iMonth)
	{
		// set return value to the last day of the month
		// for any date passed in to the method
		// create a datetime variable set to the passed in date
		DateTime dtTo = new DateTime(DateTime.Now.Year, iMonth, 1);
		// overshoot the date by a month
		dtTo = dtTo.AddMonths(1);
		// remove all of the days in the next month
		// to get bumped down to the last day of the
		// previous month
		dtTo = dtTo.AddDays(-(dtTo.Day));
		// return the last day of the month
		return dtTo;
	}

}