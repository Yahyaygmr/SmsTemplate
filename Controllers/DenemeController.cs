using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text;

namespace Sms.Controllers
{

    public class SmsController : Controller
    {
        private string XMLPOST(string postAddress, string xmlData)
        {
            try
            {
                using (var client = new WebClient())
                {
                    byte[] bPostArray = Encoding.UTF8.GetBytes(xmlData);
                    byte[] bResponse = client.UploadData(postAddress, "POST", bPostArray);
                    char[] sReturnChars = Encoding.UTF8.GetChars(bResponse);
                    string sWebPage = new string(sReturnChars);
                    return sWebPage;
                }
            }
            catch
            {
                return "-1";
            }
        }
        public IActionResult SmsSendAction()
        {
            string ss = "<?xml version='1.0' encoding='UTF-8'?>";
            ss += "<mainbody>";
            ss += "<header>";
            ss += "<company dil='TR'>Netgsm</company>";
            ss += "<usercode>usrcode</usercode>";
            ss += "<password>passwd</password>";
            ss += "<type>n:n</type>";
            ss += "<msgheader>header</msgheader>";
            ss += "</header>";
            ss += "<body>";
            ss += "<mp><msg><![CDATA[Mesaj1asasfdfgfdds]]></msg><no>905418145813</no></mp>";
            ss += "</body>";
            ss += "</mainbody>";

            string result = XMLPOST("https://api.netgsm.com.tr/sms/send/xml", ss);

            ViewBag.Result = result; // Sonucu görüntüye iletebilirsiniz

            return View();
        }
    }
}
