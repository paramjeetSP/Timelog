using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Web.Security;
/// <summary>
/// Summary description for Common
/// </summary>
public static class Common
{

    /// <summary>
    /// This is to get the thumbnail of the image using ImageResizer.
    /// </summary>
    /// <param name="pWidth">Width of the image.</param>
    /// <param name="pHeight">Height of the p.</param>
    /// <param name="FolderName">Name of the folder.</param>
    /// <param name="ImageName">Name of an image.</param>
    /// <returns>Returns the thumbnail image.</returns>
    public static string ImagePath(string width, string height, string folderName, string fileName)
    {
        string strPath = string.Empty;
        string strName = Convert.ToString(fileName);
        string imageAttributes = "?width=" + width + "&height=" + height + "&crop=auto&scale=both";
        if (!String.IsNullOrEmpty(fileName))
        {
            strPath = HttpContext.Current.Server.MapPath("~/" + folderName + "/" + fileName);
            if (File.Exists(strPath))
                return "~/" + folderName + "/" + fileName + imageAttributes;
            else
                return "~/" + folderName + "/noimage.png" + imageAttributes;
        }
        else
            return "~/" + folderName + "/noimage.png" + imageAttributes;
    }

    /// <summary>
    /// Binds the monthto DD LST.
    /// </summary>
    /// <param name="DDLst">The DD LST.</param>
    public static void BindMonthtoDDLst(DropDownList DDLst)
    {
        try
        {
            DDLst.Items.Clear();
            for (int Indexer = 1; Indexer < 13; Indexer++)
            {
                ListItem ObjLstItem = new ListItem();

                switch (Indexer)
                {
                    case 1:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Jan";
                        break;
                    case 2:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Feb";
                        break;
                    case 3:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Mar";
                        break;
                    case 4:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Apr";
                        break;
                    case 5:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "May";
                        break;
                    case 6:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Jun";
                        break;
                    case 7:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Jul";
                        break;
                    case 8:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Aug";
                        break;
                    case 9:
                        ObjLstItem.Value = "0" + Indexer.ToString();
                        ObjLstItem.Text = "Sep";
                        break;
                    case 10:
                        ObjLstItem.Value = Indexer.ToString();
                        ObjLstItem.Text = "Oct";
                        break;
                    case 11:
                        ObjLstItem.Value = Indexer.ToString();
                        ObjLstItem.Text = "Nov";
                        break;
                    case 12:
                        ObjLstItem.Value = Indexer.ToString();
                        ObjLstItem.Text = "Dec";
                        break;
                    default:
                        break;
                }
                DDLst.Items.Add(ObjLstItem);
            }
            int CurrentMonth = DateTime.Now.Month;
            string CurrentMonthString = Convert.ToString(CurrentMonth);
            if (CurrentMonth < 10)
            {
                CurrentMonthString = "0" + CurrentMonthString;
            }
            DDLst.SelectedValue = CurrentMonthString;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Binds the dateto DD LST.
    /// </summary>
    /// <param name="DDLst">The DD LST.</param>
    public static void BindDatetoDDLst(DropDownList DDLst)
    {
        try
        {
            DDLst.Items.Clear();
            for (int Indexer = 1; Indexer < 32; Indexer++)
            {
                ListItem ObjLstItem = new ListItem();
                if (Indexer < 10)
                {
                    ObjLstItem.Value = "0" + Indexer.ToString();
                    ObjLstItem.Text = "0" + Indexer.ToString();
                }
                else
                {
                    ObjLstItem.Value = Indexer.ToString();
                    ObjLstItem.Text = Indexer.ToString();
                }
                DDLst.Items.Add(ObjLstItem);
            }
            int CurrentDate = DateTime.Now.Day;
            if (CurrentDate < 10)
            {
                DDLst.SelectedValue = "0" + Convert.ToString(CurrentDate);
            }
            else
            {
                DDLst.SelectedValue = Convert.ToString(CurrentDate);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// Strips the HTML.This pattern Matches everything found inside html tags;(.|\n) - > Look for any character or a new line
    /// *?  -> 0 or more occurences, and make a non-greedy search meaning
    /// That the match will stop at the first available '>' it sees, and not at the last one
    /// (if it stopped at the last one we could have overlooked
    /// nested HTML tags inside a bigger HTML tag..)
    /// </summary>
    /// <param name="htmlString">The HTML string.</param>
    /// <returns></returns>
    public static string StripHTML(string htmlString)
    {
        string pattern = @"<(.|\n)*?>";
        return Regex.Replace(htmlString, pattern, string.Empty);
    }
    /// <summary>
    /// Convers to camel case.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns></returns>
    public static string ConvertToCamelCase(string value)
    {
        if (value == null)
            throw new ArgumentNullException("value");
        if (value.Length == 0)
            return value;
        StringBuilder result = new StringBuilder(value);
        result[0] = char.ToUpper(result[0]);
        for (int i = 1; i < result.Length; ++i)
        {
            if (char.IsWhiteSpace(result[i - 1]))
                result[i] = char.ToUpper(result[i]);
            else
                result[i] = char.ToLower(result[i]);
        }
        return result.ToString();
    }
    /// <summary>
    /// Gets the short string.
    /// </summary>
    /// <param name="strText">The STR text.</param>
    /// <param name="length">The length.</param>
    /// <returns></returns>
    public static string GetShortString(string strText, int length)
    {
        if (strText.Length <= length)
        {
            return strText;
        }
        else
        {
            strText = strText.Substring(0, length - 3) + "...";
            return strText;
        }
    }
    static byte[] bytes = ASCIIEncoding.ASCII.GetBytes("ZeroCool");
    /// <summary>
    /// Encrypt a string.
    /// </summary>
    /// <param name="originalString">The original string.</param>
    /// <returns>The encrypted string.</returns>
    /// <exception cref="ArgumentNullException">This exception will be 
    /// thrown when the original string is null or empty.</exception>
    public static string Encrypt(string originalString)
    {
        if (String.IsNullOrEmpty(originalString))
        {
            throw new ArgumentNullException
                   ("The string which needs to be encrypted can not be null.");
        }
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream memoryStream = new MemoryStream();
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            cryptoProvider.CreateEncryptor(bytes, bytes), CryptoStreamMode.Write);
        StreamWriter writer = new StreamWriter(cryptoStream);
        writer.Write(originalString);
        writer.Flush();
        cryptoStream.FlushFinalBlock();
        writer.Flush();
        return Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
    }
    /// <summary>
    /// Decrypt a crypted string.
    /// </summary>
    /// <param name="cryptedString">The crypted string.</param>
    /// <returns>The decrypted string.</returns>
    /// <exception cref="ArgumentNullException">This exception will be thrown 
    /// when the crypted string is null or empty.</exception>
    public static string Decrypt(string cryptedString)
    {
        if (String.IsNullOrEmpty(cryptedString))
        {
            throw new ArgumentNullException
               ("The string which needs to be decrypted can not be null.");
        }
        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream memoryStream = new MemoryStream
                (Convert.FromBase64String(cryptedString));
        CryptoStream cryptoStream = new CryptoStream(memoryStream,
            cryptoProvider.CreateDecryptor(bytes, bytes), CryptoStreamMode.Read);
        StreamReader reader = new StreamReader(cryptoStream);
        return reader.ReadToEnd();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="emailTo"></param>
    /// <param name="cc"></param>
    /// <param name="emailFrom"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="displayFromText"></param>

    public static void SendEmail(string emailTo, string cc, string emailFrom, string subject, string body, string displayFromText)
    {
        var msg = new MailMessage();
        //if (string.IsNullOrEmpty(emailFrom))
        //{
        //emailFrom = "do-not-reply@softprodigy.com";
        displayFromText = "Leave Tracker";
        //}
        //msg.From = new System.Net.Mail.MailAddress(displayFromText);
        msg.To.Add(emailTo);
        if (cc != string.Empty)
            msg.CC.Add(new System.Net.Mail.MailAddress(cc));
        msg.Bcc.Add(new MailAddress("aisha_nooreen@softprodigy.com"));
        msg.Subject = subject;
        msg.Body = body;
        msg.IsBodyHtml = true;
        var smtp = new SmtpClient();
        smtp.EnableSsl = false;
        smtp.Send(msg);
    }

    public static void SendEmail(string emailTo, string emailFrom, string subject, string body, string displayFromText)
    {
        var msg = new MailMessage();
        //if (string.IsNullOrEmpty(emailFrom))
        //{
        //emailFrom = "do-not-reply@softprodigy.com";
        displayFromText = "Leave Tracker";
        //}
        //msg.From = new System.Net.Mail.MailAddress(displayFromText);
        msg.To.Add(emailTo);
        msg.Subject = subject;
        msg.Body = body;
        msg.IsBodyHtml = true;
        var smtp = new SmtpClient();
        smtp.EnableSsl = false;
        smtp.Send(msg);
    }
}
