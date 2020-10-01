using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingProfileP1.Models;
using DatingProfileP1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace DatingProfileP1.Controllers
{
    public class MessagesController : Controller
    {
        private BlindDatingContext _context;
        private UserManager<IdentityUser> _userManager;

        public MessagesController(BlindDatingContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Inbox");
        }

        public IActionResult Inbox()
        {
            DatingProfile profile = _context.DatingProfile.FirstOrDefault(id => id.UserAccountId == _userManager.GetUserId(User));
            InboxViewModel inbox = new InboxViewModel();
            inbox.mailMessages = _context.MailMessage.Where(to => to.ToProfileId == profile.Id).ToList();

            List<DatingProfile> fromList = new List<DatingProfile>();
            foreach (var msg in inbox.mailMessages)
            {
                fromList.Add(_context.DatingProfile.FirstOrDefault(from => from.Id == msg.FromProfileId));                 
            }

            inbox.fromProfiles = fromList;
            return View(inbox);
        }

        public IActionResult newMessage(int id)
        {
            ViewBag.ToProfileId = id;
            return View();
        }
        [HttpPost]
        [Authorize]
        public IActionResult Send([Bind("MessageTitle, MessageText")] MailMessage mail, int toProfileId)
        {
            DatingProfile fromUser = _context.DatingProfile.FirstOrDefault(p => p.UserAccountId == _userManager.GetUserId(User));
            mail.FromProfileId = fromUser.Id;
            mail.IsRead = false;
            mail.FromProfile = fromUser;
            mail.ToProfileId = toProfileId;

            _context.Add(mail);
            _context.SaveChanges();

            return RedirectToAction("Browse", "DatingProfiles");
        }

        public IActionResult Read(int id)
        {
            MailMessage mail = _context.MailMessage.FirstOrDefault(m => m.Id == id);
           mail.IsRead = true;
            _context.Update(mail);
            _context.SaveChanges();
            return View(mail);
        }

    }
}
