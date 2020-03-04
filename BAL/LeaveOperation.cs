using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Web;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Data.SqlClient;

namespace BAL
{
    public class LeaveOperation
    {
        HRMSEntities1 obj = new HRMSEntities1();

        public LeaveOperation()
        {
            obj = new HRMSEntities1();
        }

        public List<tbl_E_LeaveType> DiplayLeaveType()
        {
            // return obj.tbl_E_LeaveType.OrderBy(c => c.Description).ToList();
            var List = obj.tbl_E_LeaveType.OrderBy(c => c.Description).ToList();
            // List.RemoveAt(2);
            List.RemoveAll(t => t.ID == 2 || t.ID == 4 || t.ID == 6 || t.ID == 5);
            return List;
        }

        public List<tbl_E_LeaveType> DiplayProbLeaveType()
        {
            // return obj.tbl_E_LeaveType.OrderBy(c => c.Description).ToList();
            var List = obj.tbl_E_LeaveType.OrderBy(c => c.Description).ToList();
            // List.RemoveAt(2);
            List.RemoveAll(t => t.ID == 2 || t.ID == 4 || t.ID == 6 || t.ID == 3 || t.ID == 5);
            return List;
        }

        //Apply leave 
        public int ApplyLeave(LeaveStatu leavestatus)
        {
            //obj.LeaveStatus(leavestatus);
            obj.LeaveStatus.Add(leavestatus);
            obj.SaveChanges();
            return leavestatus.ID;
        }
        public int SaveLeaveDetails(int id, LeaveStatu leavestatus)
        {
            //obj.Leaveapplied.Add(leavestatus);
            //obj.SaveChanges();
            var command = obj.Database.Connection.CreateCommand();
            command.CommandText = "[Leaveapplied]";
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@Emp_id", leavestatus.Emp_id));
            command.Parameters.Add(new SqlParameter("@fkLeaveType", leavestatus.fkLeaveType));
            command.Parameters.Add(new SqlParameter("@Department", leavestatus.Department));
            command.Parameters.Add(new SqlParameter("@FromDate", leavestatus.FromDate));
            command.Parameters.Add(new SqlParameter("@ToDate", leavestatus.ToDate));
            command.Parameters.Add(new SqlParameter("@LeaveReason", leavestatus.LeaveReason));
            command.Parameters.Add(new SqlParameter("@FirstLineManager_id", leavestatus.FirstLineManager_id));
            command.Parameters.Add(new SqlParameter("@FirstLineManagerStatus", leavestatus.FirstLineManagerStatus));
            command.Parameters.Add(new SqlParameter("@FirstLineMangerComment", leavestatus.FirstLineMangerComment));
            command.Parameters.Add(new SqlParameter("@SecondLineManager_id", leavestatus.SecondLineManager_id));
            command.Parameters.Add(new SqlParameter("@SecondLineManagerStatus", leavestatus.SecondLineManagerStatus));
            command.Parameters.Add(new SqlParameter("@SecondLineManagerComment", leavestatus.SecondLineManagerComment));
            command.Parameters.Add(new SqlParameter("@Hr_id", leavestatus.Hr_id));
            command.Parameters.Add(new SqlParameter("@Hr_Comment", leavestatus.Hr_Comment));
            command.Parameters.Add(new SqlParameter("@Hr_Status", leavestatus.Hr_Status));
            command.Parameters.Add(new SqlParameter("@EmpLeaveStatus", leavestatus.EmpLeaveStatus));
            command.Parameters.Add(new SqlParameter("@LeaveAppliedDate", leavestatus.LeaveAppliedDate));
            command.Parameters.Add(new SqlParameter("@CreatedOn", leavestatus.CreatedOn));
            command.Parameters.Add(new SqlParameter("@CreatedBy", leavestatus.CreatedBy));
            command.Parameters.Add(new SqlParameter("@UpdatedOn", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@UpdatedBy", null));
            command.Parameters.Add(new SqlParameter("@Admin_id", null));
            command.Parameters.Add(new SqlParameter("@Admin_Comment", leavestatus.Admin_Comment));
            command.Parameters.Add(new SqlParameter("@FLDecisiondate", leavestatus.FLDecisiondate));
            command.Parameters.Add(new SqlParameter("@SLDecisiondate", leavestatus.SLDecisiondate));
            command.Parameters.Add(new SqlParameter("@HRRDecisiondate", leavestatus.HRRDecisiondate));
            command.Parameters.Add(new SqlParameter("@IsHalfDay", leavestatus.IsHalfDay));
            command.Parameters.Add(new SqlParameter("@IsProbationLeave", leavestatus.IsProbationLeave));
            command.Parameters.Add(new SqlParameter("@IsLWP", leavestatus.IsLWP));
            command.Parameters.Add(new SqlParameter("@ELC", leavestatus.ELC));
            command.Parameters.Add(new SqlParameter("@IsELCFlag", leavestatus.IsELCFlag));

            obj.Database.Connection.Open();
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
            }

            return leavestatus.ID;
        }

        //Get reporting manager 
        public string GetReportingManager(string emp_id)
        {
            try
            {
                var result = obj.HRMS_GetReportingManagerbyEmpid(emp_id).FirstOrDefault();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        //Get department
        public string Department(string empid)
        {
            Employee emp = obj.Employees.Where<Employee>(c => c.Emp_Id == empid).SingleOrDefault();
            return emp.fkDepartment.ToString();
        }

        public string GetDepartment(string id)
        {
            if (id != string.Empty)
            {
                int depid = Convert.ToInt32(id);
                var firstOrDefault = obj.tbl_E_Department.FirstOrDefault(c => c.ID == depid);
                if (firstOrDefault != null)
                    return firstOrDefault.DeptName;
                else
                {
                    return string.Empty;
                }
            }
            else
            {
                return string.Empty;
            }
        }

        //Get all leave status
        public List<LeaveStatu> GetAllLeaveStatus()
        {
            return obj.LeaveStatus.ToList();
        }

        //Get hr id
        public string GetHrid()
        {
            /* Commented On 27 April 2015 by Paramjeet Singh. Reason: To make the "Deepti Ma'am as a hr_representative to take decision on Applied Leaves.
           //return obj.Employees.Where<Employee>(c => c.fkDesignation == 44).Select(c => c.Emp_Id).FirstOrDefault();//.SingleOrDefault(); 
           */

            //return obj.Employees.Where<Employee>(c => c.ID == 72).Select(c => c.Emp_Id).SingleOrDefault();

            /* Commented on 21 August 2015 by Paramjet Singh. Reason: To get the Hr_Representative id on bases of role i.e. HRRep
            //return obj.Employees.Where<Employee>(c => c.fkDesignation == 67).Select(c => c.Emp_Id).FirstOrDefault();
             */

            string empid = string.Empty;
            var _role = obj.EmployeeRoles.FirstOrDefault(c => c.fkRole == 5);
            if (_role != null)
            {
                var _employee = obj.Employees.FirstOrDefault(c => c.ID == _role.fkEmployee);
                if (_employee != null)
                {
                    empid = _employee.Emp_Id;
                }
            }
            return empid;
        }

        //Get leave status by id
        public List<LeaveStatu> GetLeaveStatusByID(string emp_id)
        {
            return obj.LeaveStatus.Where<LeaveStatu>(c => c.Emp_id == emp_id).OrderByDescending(c => c.LeaveAppliedDate).ToList();
        }

        //Get full name of employee by empid
        public string GetFullName(string empid)
        {
            return obj.Employees.Where<Employee>(c => c.Emp_Id == empid).Select(c => c.FullName).SingleOrDefault();
        }

        //Get leave type by id
        public string GetLeaveType(int leavetypeid)
        {
            return obj.tbl_E_LeaveType.Where<tbl_E_LeaveType>(c => c.ID == leavetypeid).Select(c => c.Description).SingleOrDefault();
        }

        //Get employee by id
        public Employee GetEmployee(string empId)
        {
            return obj.Employees.Where<Employee>(e => e.Emp_Id == empId).SingleOrDefault();
        }

        public string GetEmployeeOfficialEmailId(string empid)
        {
            //return obj.Employees.Where(c => c.Emp_Id == empid).Select(c => c.OfficialEmail).FirstOrDefault();
            try
            {
                string officialemail = string.Empty;
                var employee = obj.Employees.FirstOrDefault(c => c.Emp_Id == empid);
                if (employee != null)
                    officialemail = employee.OfficialEmail;

                return officialemail;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        // Authenticate Employee
        public bool EmployeeAuthenticate(string empId, string password)
        {
            if (obj.Employees.Any(c => c.Emp_Id == empId && c.LoginPassword == password && c.EmpStatus == "Active" && c.IsDeleted == false)
                && obj.EmployeeRoles.Any(er => er.fkEmployee == obj.Employees.Where(e => e.Emp_Id == empId).FirstOrDefault().ID))
                return true;
            else
                return false;
        }

        // Get Employee Role By ID
        public List<EmployeeRole> GetEmployeeRoleByID(int fkemployeeid)
        {
            return obj.EmployeeRoles.Where<EmployeeRole>(c => c.fkEmployee == fkemployeeid).ToList();
        }

        public List<tbl_E_Status> GetAllStatus()
        {
            return obj.tbl_E_Status.Where<tbl_E_Status>(c => c.ID != 0).ToList();
        }

        // Update Employee Status

        //public void UpdateEmpLeaveStatus(int id, int status, string comment, string senderid, string Employeeid)
        //{
        //    LeaveStatu empleaveobj = obj.LeaveStatus.Where(c => c.ID == id).SingleOrDefault();
        //    switch (senderid)
        //    {
        //        case "FirstLineManager":
        //            if (empleaveobj.FirstLineManager_id == "soumyajit_chakraborty" || empleaveobj.FirstLineManager_id == "divya_chakraborty")
        //            {
        //                if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
        //                    empleaveobj.SecondLineManagerStatus = 0;
        //                if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
        //                    empleaveobj.Hr_Status = 0;

        //                empleaveobj.EmpLeaveStatus = status;
        //            }

        //            empleaveobj.FirstLineManagerStatus = status;
        //            empleaveobj.FirstLineMangerComment = comment;
        //            empleaveobj.UpdatedOn = DateTime.Now;
        //            empleaveobj.UpdatedBy = Employeeid;


        //            break;

        //        case "SecoundLineManager":
        //            if (empleaveobj.SecondLineManager_id == "soumyajit_chakraborty" || empleaveobj.SecondLineManager_id == "divya_chakraborty")
        //            {
        //                if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
        //                    empleaveobj.FirstLineManagerStatus = 0;
        //                if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
        //                    empleaveobj.Hr_Status = 0;
        //                empleaveobj.EmpLeaveStatus = status;
        //            }
        //            else
        //            {
        //                if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
        //                    empleaveobj.FirstLineManagerStatus = 0;
        //                if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 3 && status == 3)
        //                {
        //                    empleaveobj.Hr_Status = status;
        //                    empleaveobj.EmpLeaveStatus = status;
        //                }
        //            }
        //            empleaveobj.SecondLineManagerStatus = status;
        //            empleaveobj.SecondLineManagerComment = comment;
        //            empleaveobj.UpdatedOn = DateTime.Now;
        //            empleaveobj.UpdatedBy = Employeeid;

        //            break;

        //        case "HrManager":

        //            if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
        //                empleaveobj.SecondLineManagerStatus = 0;
        //            empleaveobj.Hr_Status = status;
        //            empleaveobj.Hr_Comment = comment;
        //            empleaveobj.EmpLeaveStatus = status;
        //            empleaveobj.UpdatedOn = DateTime.Now;
        //            empleaveobj.UpdatedBy = Employeeid;
        //            break;

        //        case "Admin":
        //            if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
        //                empleaveobj.Hr_Status = 0;
        //            if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
        //                empleaveobj.SecondLineManagerStatus = 0;
        //            if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
        //                empleaveobj.FirstLineManagerStatus = 0;
        //            empleaveobj.Admin_id = Employeeid;
        //            empleaveobj.Admin_Comment = comment;
        //            empleaveobj.EmpLeaveStatus = status;
        //            empleaveobj.UpdatedOn = DateTime.Now;
        //            empleaveobj.UpdatedBy = Employeeid;
        //            break;
        //    }
        //    obj.SaveChanges();
        //}


        public void UpdateEmpLeaveStatus(int id, int status, string comment, string senderid, string Employeeid)
        {
            LeaveStatu empleaveobj = obj.LeaveStatus.Where(c => c.ID == id).SingleOrDefault();

            switch (senderid)
            {
                case "FirstLineManager":
                    if (empleaveobj.FirstLineManager_id == "soumyajit_chakraborty" || empleaveobj.FirstLineManager_id == "divya_chakraborty")
                    {
                        if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
                            empleaveobj.SecondLineManagerStatus = 0;
                        if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
                            empleaveobj.Hr_Status = 0;

                        empleaveobj.EmpLeaveStatus = status;
                    }

                    empleaveobj.FirstLineManagerStatus = status;
                    empleaveobj.FirstLineMangerComment = comment;
                    empleaveobj.UpdatedOn = DateTime.Now;
                    empleaveobj.UpdatedBy = Employeeid;


                    break;

                case "SecoundLineManager":
                    if (empleaveobj.SecondLineManager_id == "soumyajit_chakraborty" || empleaveobj.SecondLineManager_id == "divya_chakraborty")
                    {
                        if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
                            empleaveobj.FirstLineManagerStatus = 0;
                        if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
                            empleaveobj.Hr_Status = 0;
                        empleaveobj.EmpLeaveStatus = status;
                    }
                    else
                    {
                        if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
                            empleaveobj.FirstLineManagerStatus = 0;
                        if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 3 && status == 3)
                        {
                            empleaveobj.Hr_Status = status;
                            empleaveobj.EmpLeaveStatus = status;
                        }
                    }
                    empleaveobj.SecondLineManagerStatus = status;
                    empleaveobj.SecondLineManagerComment = comment;
                    empleaveobj.UpdatedOn = DateTime.Now;
                    empleaveobj.UpdatedBy = Employeeid;

                    break;

                case "HrManager":

                    if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
                        empleaveobj.SecondLineManagerStatus = 0;
                    empleaveobj.Hr_Status = status;
                    empleaveobj.Hr_Comment = comment;
                    empleaveobj.EmpLeaveStatus = status;
                    empleaveobj.UpdatedOn = DateTime.Now;
                    empleaveobj.UpdatedBy = Employeeid;
                    break;

                case "Admin":
                    if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
                        empleaveobj.Hr_Status = 0;
                    if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
                        empleaveobj.SecondLineManagerStatus = 0;
                    if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
                        empleaveobj.FirstLineManagerStatus = 0;
                    empleaveobj.Admin_id = Employeeid;
                    empleaveobj.Admin_Comment = comment;
                    empleaveobj.EmpLeaveStatus = status;
                    empleaveobj.UpdatedOn = DateTime.Now;
                    empleaveobj.UpdatedBy = Employeeid;
                    break;
            }
            obj.SaveChanges();
            //update leavedetail table
            var empdeatilleaveobj = obj.DetailedLeaveStatus.Where(c => c.LeavestatusID == id).ToList();
            if(empdeatilleaveobj.Count>0)
            {
                foreach(var item in empdeatilleaveobj)
                {
                    switch (senderid)
                    {
                        case "FirstLineManager":
                            if (empleaveobj.FirstLineManager_id == "soumyajit_chakraborty" || empleaveobj.FirstLineManager_id == "divya_chakraborty")
                            {
                                if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
                                    item.SecondLineManagerStatus = 0;
                                if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
                                    item.Hr_Status = 0;

                                item.EmpLeaveStatus = status;
                            }

                            item.FirstLineManagerStatus = status;
                            item.FirstLineMangerComment = comment;
                            item.UpdatedOn = DateTime.Now;
                            item.UpdatedBy = Employeeid;


                            break;

                        case "SecoundLineManager":
                            if (empleaveobj.SecondLineManager_id == "soumyajit_chakraborty" || empleaveobj.SecondLineManager_id == "divya_chakraborty")
                            {
                                if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
                                    item.FirstLineManagerStatus = 0;
                                if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
                                    item.Hr_Status = 0;
                                item.EmpLeaveStatus = status;
                            }
                            else
                            {
                                if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
                                    item.FirstLineManagerStatus = 0;
                                if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 3 && status == 3)
                                {
                                    item.Hr_Status = status;
                                    item.EmpLeaveStatus = status;
                                }
                            }
                            item.SecondLineManagerStatus = status;
                            item.SecondLineManagerComment = comment;
                            item.UpdatedOn = DateTime.Now;
                            item.UpdatedBy = Employeeid;

                            break;

                        case "HrManager":

                            if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
                                item.SecondLineManagerStatus = 0;
                            item.Hr_Status = status;
                            item.Hr_Comment = comment;
                            item.EmpLeaveStatus = status;
                            item.UpdatedOn = DateTime.Now;
                            item.UpdatedBy = Employeeid;
                            break;

                        case "Admin":
                            if (Convert.ToInt32(empleaveobj.Hr_Status) == 1)
                                item.Hr_Status = 0;
                            if (Convert.ToInt32(empleaveobj.SecondLineManagerStatus) == 1)
                                item.SecondLineManagerStatus = 0;
                            if (Convert.ToInt32(empleaveobj.FirstLineManagerStatus) == 1)
                                item.FirstLineManagerStatus = 0;
                            item.Admin_id = Employeeid;
                            item.Admin_Comment = comment;
                            item.EmpLeaveStatus = status;
                            item.UpdatedOn = DateTime.Now;
                            item.UpdatedBy = Employeeid;
                            break;
                    }
                    obj.SaveChanges();
                }
            }

           
        }

        public string GetStatusDescriptionByID(int Statusid)
        {
            return obj.tbl_E_Status.Where<tbl_E_Status>(c => c.ID == Statusid).Select(c => c.Description).SingleOrDefault();
        }

        public string GetDepartmentMangerid(string department)
        {
            int? departmentid = Convert.ToInt32(department);
            var departmenthead = obj.tbl_E_Department.Where(c => c.ID == departmentid).Select(c => c.DepartmentHead).FirstOrDefault();
            int id = Convert.ToInt32(departmenthead);
            return obj.Employees.Where(c => c.ID == id).Select(c => c.Emp_Id).FirstOrDefault();
        }

        public bool checkstatusforapplyleave(string employeeid, DateTime today, DateTime from, Int32 leavetype)
        {
            bool result = false;
            var status = obj.Hrms_EmpCheckApplyLeaveStatus(today, from, employeeid, leavetype).Single().Value;
            if (status > 0)
                result = true;


            return result;

        }

        //more than one sudden leave in a month
        public int Checksuddenleavestatus(string empid, DateTime fromDate, DateTime toDate)
        {
            int result = obj.LeaveStatus.Count(c => c.Emp_id == empid && c.fkLeaveType == 2 && c.FromDate >= fromDate && c.ToDate <= toDate);
            return result;
        }


        public bool checkholidaystatusbydate(DateTime applieddate)
        {
            int result;
            result = obj.Hrms_checkholidaystatusbydate(applieddate).Single().Value;
            if (result == 1)
                return true;
            else
                return false;
        }

        public bool checkleavestatus(DateTime fromdate, DateTime todate, string empid)
        {
            bool result = false;
            for (DateTime i = fromdate; fromdate <= todate;)
            {
                if (obj.LeaveStatus.Any(c => c.Emp_id == empid && c.FromDate <= i && c.ToDate >= todate))
                {
                    var rejectLeaveExist = obj.LeaveStatus.Any(c => c.Emp_id == empid && c.FromDate <= i && c.ToDate >= todate && (c.EmpLeaveStatus == 3 || c.FirstLineManagerStatus == 3 || c.SecondLineManagerStatus == 3 || c.Hr_Status == 3));
                    var approvedLeaveExist = obj.LeaveStatus.Any(c => c.Emp_id == empid && c.FromDate <= i && c.ToDate >= todate && (c.EmpLeaveStatus == 2 || c.FirstLineManagerStatus == 2 || c.SecondLineManagerStatus == 2 || c.Hr_Status == 2));
                    //leave can apply on rejected date
                    if (rejectLeaveExist == true && approvedLeaveExist==false)
                    {
                        result = false;
                        fromdate = fromdate.AddDays(1);
                        break;
                    }
                    //leave can not apply on approved date
                    else if (approvedLeaveExist==true && rejectLeaveExist == false)
                    {
                        result = true;
                        break;
                    }
                    //leave can not apply on approved leave and rejected leave on same date
                    else if (rejectLeaveExist == true && approvedLeaveExist == true)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        result = true;
                        break;
                    }
                }
                fromdate = fromdate.AddDays(1);
            }
            return result;
        }

        #region Commented Code

        //public void LeaveApplicationStatus(int id, string ApplicationStatus)
        //{
        //    string EmailSentTo, EmailSubject;
        //    EmailSentTo = EmailSubject = string.Empty;
        //    LeaveStatu emp_leavestatusobj = obj.LeaveStatus.Where(c => c.ID == id).First();
        //    if (emp_leavestatusobj != null)
        //    {
        //        switch (ApplicationStatus)
        //        {
        //            case "ApplicationSubmission":
        //                string DepartmentManager = GetDepartmentMangerid(emp_leavestatusobj.Department);
        //                EmailSentTo = "Applicant, First Line Manager, Second Line Manager, Leaves, Department Manager";
        //                EmailSentTo = emp_leavestatusobj.Emp_id + "," + emp_leavestatusobj.FirstLineManager_id + "," + emp_leavestatusobj.SecondLineManager_id +
        //                    "," + "Leaves@softprodigy.com" + "," + DepartmentManager;
        //                EmailSubject = "Leave application submitted by ###applicant### for ###Startdate###";
        //                EmailSubject = EmailSubject.Replace("###applicant###", emp_leavestatusobj.Emp_id);
        //                EmailSubject = EmailSubject.Replace("###Startdate###", emp_leavestatusobj.FromDate.ToString());
        //                SendEmail(EmailSentTo, "", emp_leavestatusobj.Emp_id, EmailSubject, "emailbody", "LMS");

        //                break;

        //            case "ApplicationApproval":
        //                EmailSentTo = "First Line Manager, Second Line Manager, Leaves, Department Manager";
        //                EmailSubject = "Leave application approved by  ###ManagerName###";
        //                EmailSubject = EmailSubject.Replace("###ManagerName###", "ManagerName");
        //                break;

        //            case "ApplicationRejection":
        //                EmailSentTo = "First Line Manager, Second Line Manager, Leaves, Department Manager";
        //                EmailSubject = "Leave application rejected by  ###ManagerName###";
        //                EmailSubject = EmailSubject.Replace("###ManagerName###", "ManagerName");
        //                break;

        //            case "ApplicationAcceptance/RejectionMail":
        //                EmailSentTo = "First Line Manager, Second Line Manager, Leaves, Applicant, Department Manager";
        //                EmailSubject = "Leave application Accepted/Rejected";
        //                break;

        //            default:
        //                break;
        //        }
        //    }
        //}

        //public static void SendEmail(string emailTo, string cc, string emailFrom, string subject, string body, string displayFromText)
        //{
        //    MailMessage msg = new MailMessage();
        //    if (string.IsNullOrEmpty(emailFrom))
        //    {
        //        emailFrom = "do-not-reply@softprodigy.com";
        //        displayFromText = "LMS";
        //    }
        //    msg.From = new System.Net.Mail.MailAddress(emailFrom, displayFromText);
        //    msg.To.Add(emailTo);
        //    if (cc != string.Empty)
        //        msg.CC.Add(new System.Net.Mail.MailAddress(cc));
        //    msg.Subject = subject;
        //    msg.Body = body;
        //    msg.IsBodyHtml = true;
        //    SmtpClient smtp = new SmtpClient();
        //    smtp.EnableSsl = false;
        //    smtp.Send(msg);
        //}

        #endregion
    }
}
