using DatingProfileP1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingProfileP1.ViewModels
{
    public class InboxViewModel
    {
        public IEnumerable<MailMessage> mailMessages;
        public IEnumerable<DatingProfile> fromProfiles;

    }
}
