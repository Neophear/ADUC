using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Profile;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Net;

namespace Stiig
{
    public class Utilities
    {
        public static string GetInfoFromDBOrCache(string name)
        {
            if (HttpRuntime.Cache.Get(name) == null)
            {
                object value = GetInfo(name);

                HttpRuntime.Cache.Insert(name, value, null, DateTime.UtcNow.Add(new TimeSpan(0, 0, 30)), System.Web.Caching.Cache.NoSlidingExpiration);
            }

            return HttpRuntime.Cache.Get(name).ToString();
        }
        public static object GetInfo(string name)
        {
            DataAccessLayer dal = new DataAccessLayer();

            dal.AddParameter("@Name", name, DbType.String);
            string value = dal.ExecuteScalar("SELECT dbo.GetInfo(@Name)").ToString();
            dal.ClearParameters();

            return value;
        }
        public static string GetSearchOU()
        {
            return Roles.IsUserInRole("Admin") ? "DC = NRU-INET,DC = LOCAL" : "OU = Brugere,DC = NRU-INET,DC = LOCAL";
        }
        public static CustomError CheckPassword(string password, string accountname)
        {
            int counter = 0;
            CustomError errors = new CustomError();

            Regex hasCapitalLetter = new Regex("[A-Z]");
            Regex hasSmallLetter = new Regex("[a-z]");
            Regex hasNumber = new Regex("[0-9]");
            Regex hasSpecialChar = new Regex(@"[\W]");

            if (hasCapitalLetter.IsMatch(password)) { counter++; }
            if (hasSmallLetter.IsMatch(password)) { counter++; }
            if (hasNumber.IsMatch(password)) { counter++; }
            if (hasSpecialChar.IsMatch(password)) { counter++; }

            if (password.ToLower().Contains(accountname.ToLower()))
                errors.Add(CustomError.ErrorType.PasswordIncludesAccount);

            //foreach (string part in fullname.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries))
            //{
            //    if (part.Length >= 3 && password.ToLower().Contains(part.ToLower()))
            //    {
            //        errors.Add(CustomError.ErrorType.PasswordIncludesNames);
            //        break;
            //    }
            //}

            if (password.Length < 8)
                errors.Add(CustomError.ErrorType.PasswordLessThan8);
            else if (counter < 3)
                errors.Add(CustomError.ErrorType.PasswordNotComplex);

            return errors;
        }

        /// <summary>
        /// Cuts a string to a specified length depending on if it should cut at whole words or not. Fx:
        /// string txt = "I want to cut this string!"
        /// CutText(txt, "...", 10, true)
        /// returns:
        /// "I want to..."
        /// </summary>
        /// <param name="text">String to cut</param>
        /// <param name="addString">String to add after the cut has been made. Fx. "..."</param>
        /// <param name="maxLength">Max length the resulting string shall be without the addString</param>
        /// <param name="wholeWords">True to leave whole words and false to ignore whole words and just cut</param>
        /// <returns></returns>
        public static string CutText(string text, string addString, int maxLength, bool wholeWords)
        {
            string endString = "";

            if (text.Length <= maxLength)
                endString = text;
            else
            {
                if (!wholeWords)
                {
                    endString = text.Substring(0, maxLength);
                    endString = endString.Trim();
                }
                else
                {
                    endString = text.Substring(0, maxLength + 1);
                    int lastIndexOfSpace = endString.LastIndexOf(" ");

                    endString = endString.Remove(lastIndexOfSpace);
                }

                endString += addString;
            }

            return endString;
        }
        /// <summary>
        /// Returns a more userfriendly time.
        /// </summary>
        /// <param name="time">The DateTime to modify</param>
        /// <returns></returns>
        public static string GetFriendlyTime(DateTime time)
        {
            string modifieddate = "{0} {1} siden";
            TimeSpan diff = DateTime.Now - time;
            int years = (int)(diff.Days / 365.242199);
            int months = diff.Days / 30;
            int weeks = diff.Days / 7;

            if (years > 0)
                modifieddate = string.Format(modifieddate, years, "år");
            else if (months > 0)
                modifieddate = string.Format(modifieddate, months, months > 1 ? "måneder" : "måned");
            else if (weeks > 0)
                modifieddate = string.Format(modifieddate, weeks, weeks > 1 ? "uger" : "uge");
            else if (diff.Days > 1)
                modifieddate = string.Format(modifieddate, diff.Days, "dage");
            else if (diff.Days == 1)
                modifieddate = "I går";
            else if (diff.Hours > 0)
                modifieddate = string.Format(modifieddate, diff.Hours, diff.Hours > 1 ? "timer" : "time");
            else if (diff.Minutes > 0)
                modifieddate = string.Format(modifieddate, diff.Minutes, diff.Minutes > 1 ? "minutter" : "minut");
            else if (diff.Seconds > 0)
                modifieddate = string.Format(modifieddate, diff.Seconds, diff.Seconds > 1 ? "sekunder" : "sekund");
            else
                modifieddate = "Nu";

            return modifieddate;
        }
        /// <summary>
        /// Removes the end of the string.
        /// </summary>
        /// <param name="Text">The string that has to be cut</param>
        /// <param name="TextToRemove">The end string to remove</param>
        /// <returns></returns>
        public static string RemoveEndString(string Text, string TextToRemove)
        {
            string result = Text;

            if (Text.EndsWith(TextToRemove))
            {
                result = Text.Remove(Text.Length - TextToRemove.Length);
            }

            return result;
        }
        public static string RemoveExcessWhitespaces(string input)
        {
            input = input.Trim();
            while (input.Contains("  "))
                input = input.Replace("  ", " ");

            return input;
        }
        public static string StripHTML(string htmlString, string htmlPlaceHolder = "", bool stripExcessSpaces = true)
        {
            string pattern = @"<(.|\n)*?>";
            string sOut = System.Text.RegularExpressions.Regex.Replace(htmlString, pattern, htmlPlaceHolder);
            sOut = sOut.Replace("&nbsp;", "");
            sOut = sOut.Replace("&amp;", "&");

            if (stripExcessSpaces)
            {
                // If there is excess whitespace, this will remove
                // like "THE      WORD".
                char[] delim = { ' ' };
                string[] lines = sOut.Split(delim, StringSplitOptions.RemoveEmptyEntries);

                sOut = "";
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (string s in lines)
                {
                    sb.Append(s);
                    sb.Append(" ");
                }

                return sb.ToString().Trim();
            }
            else
            {
                return sOut;
            }
        }
        public static string ToggleHtmlBR(string text, bool isOn)
        {
            string outS = "";

            if (isOn)
                outS = text.Replace("\n", "<br />");
            else
            {
                outS = text.Replace("<br />", "\n");
                outS = text.Replace("<br>", "\n");
                outS = text.Replace("<br >", "\n");
            }

            return outS;
        }

        public static string HTMLToBB(string OriginalText)
        {
            string text = OriginalText;

            List<Regex> regexlist = new List<Regex>();

            regexlist.Add(new Regex(@"<(?<tag>b)>(?<text>[\w\W]*?)</b>"));
            regexlist.Add(new Regex(@"<(?<tag>i)>(?<text>[\w\W]*?)</i>"));
            regexlist.Add(new Regex(@"<(?<tag>u)>(?<text>[\w\W]*?)</u>"));
            regexlist.Add(new Regex(@"<(?<tag>s)>(?<text>[\w\W]*?)</s>"));

            Regex quotewithauthor = new Regex("<table\\ cellpadding=\"2\"><tr><td\\ class=\"quote\">Citat\\ af\\ (?<author>.*?)<br\\ /><i>(?<text>[\\w\\W]*?)</i></td></tr></table>");
            Regex quote = new Regex("<table\\ cellpadding=\"2\"><tr><td\\ class=\"quote\"><i>(?<text>[\\w\\W]*?)</i></td></tr></table>");
            Regex urlwithtitle = new Regex("<a href=\"(?<link>[\\w\\W]*?)\">(?<text>[\\w\\W]*?)</a>");
            Regex url = new Regex("<a\\ href=\"(?<link>[\\w\\W]*?)\">\\1</a>");
            Regex size = new Regex("<span\\ style=\"font-size:(?<size>\\d*?)pt;\">(?<text>[\\w\\W]*?)</span>");
            Regex color = new Regex("<span\\ style=\"color:(?<color>.*?);\">(?<text>[\\w\\W]*?)</span>");

            text = quotewithauthor.Replace(text, @"[quote=$1]$2[/quote]");
            text = quote.Replace(text, @"[quote]$1[/quote]");
            text = url.Replace(text, @"[url]$1[/url]");
            text = urlwithtitle.Replace(text, @"[url=$1]$2[/url]");
            text = size.Replace(text, @"[size=$1]$2[/size]");
            text = color.Replace(text, @"[color=$1]$2[/color]");

            foreach (Regex regex in regexlist)
                text = regex.Replace(text, @"[$1]$2[/$1]");

            text = text.Replace("<table cellpadding=\"2\"><tr><td class=\"quote\"><i>", "[quote]").Replace("</i></td></tr></table>", "[/quote]");
            text = text.Replace("<img src=\"", "[img]").Replace("\"/>", "[/img]");
            text = text.Replace("<table cellpadding=\"2\"><tr><td class=\"code\">", "[code]").Replace("</td></tr></table>", "[/code]");
            text = text.Replace("<br />", "");
            text = text.Replace("&#60;", "<");
            text = text.Replace("&#62;", ">");

            return text;
        }
        public static string BBToHTML(string OriginalText)
        {
            string text;
            List<Regex> regexlist = new List<Regex>();
            text = OriginalText;

            regexlist.Add(new Regex(@"\[(?<tag>b)\](?<text>[\w\W]*?)\[/b\]"));
            regexlist.Add(new Regex(@"\[(?<tag>i)\](?<text>[\w\W]*?)\[/i\]"));
            regexlist.Add(new Regex(@"\[(?<tag>u)\](?<text>[\w\W]*?)\[/u\]"));
            regexlist.Add(new Regex(@"\[(?<tag>s)\](?<text>[\w\W]*?)\[/s\]"));

            Regex quote = new Regex(@"\[quote\](?<text>[\w\W]*?)\[/quote\]");
            Regex quotewithauthor = new Regex(@"\[quote=(?<author>.*?)\](?<text>[\w\W]*?)\[/quote\]");
            Regex code = new Regex(@"\[code\](?<text>[\w\W]*?)\[/code\]");
            Regex urlWithProtocol = new Regex(@"\[url\]((?:f|ht)tps?://[\w\W]*?)\[/url\]");
            Regex url = new Regex(@"\[url\](?<link>[\w\W]*?)\[/url\]");
            Regex urlwithtitleWithProtocol = new Regex(@"\[url=((?:f|ht)tps?://(?:[\w\W]*?))\](.*?)\[/url\]");
            Regex urlwithtitle = new Regex(@"\[url=([\w\W]*?)\](.*?)\[/url\]");
            Regex image = new Regex(@"\[img\](?<link>[\w\W]*?)\[/img\]");
            Regex size = new Regex(@"\[size=(?<size>\d*?)\](?<text>[\w\W]*?)\[/size\]");
            Regex color = new Regex(@"\[color=(?<color>[\w\W]*?)\](?<text>[\w\W]*?)\[/color\]");

            text = text.Replace("<", "&#60;");
            text = text.Replace(">", "&#62;");

            text = ToggleHtmlBR(text, true);

            foreach (Regex regex in regexlist)
                text = regex.Replace(text, "<$1>$2</$1>");

            text = quote.Replace(text, "<table cellpadding=\"2\"><tr><td class=\"quote\"><i>$1</i></td></tr></table>");
            text = code.Replace(text, "<table cellpadding=\"2\"><tr><td class=\"code\">$1</td></tr></table>");
            text = quotewithauthor.Replace(text, "<table cellpadding=\"2\"><tr><td class=\"quote\">Citat af $1<br /><i>$2</i></td></tr></table>");
            text = urlWithProtocol.Replace(text, "<a href=\"$1\">$1</a>");
            text = url.Replace(text, "<a href=\"$1\">$1</a>");
            text = urlwithtitleWithProtocol.Replace(text, "<a href=\"$1\">$2</a>");
            text = urlwithtitle.Replace(text, "<a href=\"$1\">$2</a>");
            text = image.Replace(text, "<img src=\"$1\"/>");
            text = size.Replace(text, "<span style=\"font-size:$1pt;\">$2</span>");
            text = color.Replace(text, "<span style=\"color:$1;\">$2</span>");

            return text;
        }

        public static void SendMail(string Name, string MA, string Password)
        {
            string smtp = ConfigurationManager.AppSettings["Host"];
            int port = int.Parse(ConfigurationManager.AppSettings["Port"]);
            string from = ConfigurationManager.AppSettings["Username"];
            string password = ConfigurationManager.AppSettings["Password"];
            string _to = "00" + MA + "@mil.dk";
            string subject = "Ny kode til din internet konto(NRU-INET & Guest_WiFi) ved Trænregimentet på Aalborg kaserne.";
            StringBuilder sb = new StringBuilder();
            sb.Append("Hej ");
            sb.AppendLine(Name);
            sb.AppendLine();
            sb.AppendLine("Du har fået ny kode til NRU-INET & Guest_WiFi");
            sb.AppendLine();
            sb.Append("Brugernavn: ");
            sb.AppendLine(MA);
            sb.Append("Kodeord: ");
            sb.AppendLine(Password);
            sb.AppendLine();
            sb.AppendLine("Venlig hilsen");
            sb.AppendLine("IT Sektionen");
            sb.AppendLine("Trænregimentet Aalborg Kaserner");

            string body = sb.ToString();
            //string body = "Brugernavn: " + MA + " Kodeord: " + Password;

            MailMessage message = new MailMessage();
            message.From = new MailAddress(from);
            message.To.Add(new MailAddress(_to));
            message.Subject = subject;
            message.Body = body;

            SmtpClient client = new SmtpClient();
            client.Host = smtp;
            client.Port = port;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
            client.Credentials = new NetworkCredential(from, password);
            client.Send(message);
        }
    }
}