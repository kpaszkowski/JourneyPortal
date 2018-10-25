using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Chat
{
    public class UserChatViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Login")]
        public string UserName { get; set; }
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Rola")]
        public string UserRole { get; set; }
    }
}