using System;
using System.Text;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NetworkSellFood
{
	public static class AutoSendEmail
	{
		/// <summary>
		/// Gets the email address.
		/// </summary>
		/// <value>The email address.</value>
		public static string EmailAddress {
			get {
				return WebConfigurationManager.AppSettings.Get ("EmailAddress");
			}
		}

		/// <summary>
		/// Gets the smtp address.
		/// </summary>
		/// <value>The smtp address.</value>
		public static string SmtpAddress {
			get {
				return WebConfigurationManager.AppSettings.Get ("SmtpAddress");
			}
		}

		/// <summary>
		/// Gets the smtp user.
		/// </summary>
		/// <value>The smtp user.</value>
		public static string SmtpUser {
			get {
				return WebConfigurationManager.AppSettings.Get ("SmtpUser");
			}
		}

		/// <summary>
		/// Gets the smtp password.
		/// </summary>
		/// <value>The smtp password.</value>
		public static string SmtpPassword {
			get {
				return WebConfigurationManager.AppSettings.Get ("SmtpPasswd");
			}
		}

		/// <summary>
		/// Sends the email.
		/// </summary>
		/// <returns><c>true</c>, if email was sent, <c>false</c> otherwise.</returns>
		/// <param name="Subject">Subject. 标题</param>
		/// <param name="Context">Context.正文</param>
		/// <param name="To">To.目的邮箱</param>
		public static async  void SendEmail (string Subject, string Context, string To, bool IsHtml = false)
		{
			using (SmtpClient smtp = new SmtpClient ()) {
				string[] smtpserver = SmtpAddress.Split (':');
				smtp.Host = smtpserver [0];
				if (smtpserver.Length < 2)
					smtp.Port = 25;
				else
					smtp.Port = Convert.ToInt32 (smtpserver [1]);
				try {
					smtp.Credentials = new NetworkCredential (SmtpUser, SmtpPassword);
					MailAddress addr = new MailAddress (To);
					MailAddress addrfrom = new MailAddress (EmailAddress,"网站用户服务中心",Encoding.UTF8);
					using (MailMessage msg = new MailMessage ()) {						
						msg.Subject = Subject;
						msg.SubjectEncoding = Encoding.UTF8;
						msg.From = addrfrom;
						msg.To.Add (addr);
						msg.Sender = addrfrom;
						msg.Body = Context;
						msg.BodyEncoding = Encoding.UTF8;
						msg.IsBodyHtml = IsHtml;
						await smtp.SendMailAsync (msg);
					}
				} catch (Exception) {
				}
			}
		}
	}
}

