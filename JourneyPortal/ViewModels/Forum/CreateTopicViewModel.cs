using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class CreateTopicViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać nazwę tematu.")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać opis tematu.")]
        public string Description { get; set; }
    }
}