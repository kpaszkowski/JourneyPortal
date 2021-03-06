﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace JourneyPortal.ViewModels.Forum
{
    public class CreateCategoryViewModel
    {
        [Display(Name = "Nazwa")]
        [Required(AllowEmptyStrings = false ,ErrorMessage = "Należy podać nazwę kategorii.")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Należy podać opis kategorii.")]
        public string Description { get; set; }
    }
}