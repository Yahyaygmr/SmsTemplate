using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MultiSms.Interfaces;
using MultiSms.Models;
using Sms.Models;
using Sms.Models.Context;

namespace Sms.Controllers
{
    public class ClientController : Controller
    {
        private readonly AppDbContext _context;

        public ClientController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Client
        public async Task<IActionResult> Index()
        {
            return _context.Client != null ?
                        View(await _context.Client.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Client'  is null.");
        }

        // GET: Client/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        public IActionResult Create()
        {
            return View();
        }

        //******************************************************************************************************
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

        //**********************************************************************

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Client client)
        {
            if (ModelState.IsValid)
            {
                string clientMessage = "Merhaba " + client.Name.ToUpper() + " Siparişiniz Alınmıştır. Siparişinizin durumunu " + client.Id + " sipariş numarası ile aşşağıdaki linkten takip edebilirsiniz. https://yahyayagmur.com";
                string phoneNumb = "90" + client.PhoneNumber;
                _context.Add(client);
                await _context.SaveChangesAsync();

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
                ss += $"<mp><msg><![CDATA[{clientMessage}]]></msg><no>{phoneNumb}</no></mp>";
                ss += "</body>";
                ss += "</mainbody>";

                XMLPOST("https://api.netgsm.com.tr/sms/send/xml", ss);

                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Client/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,PhoneNumber")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Client/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Client == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Id == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Client/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Client == null)
            {
                return Problem("Entity set 'AppDbContext.Client'  is null.");
            }
            var client = await _context.Client.FindAsync(id);
            if (client != null)
            {
                _context.Client.Remove(client);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return (_context.Client?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
