﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CorporationHR.Models
{
    public class ClearenceModel
    {
        [Key]
        public int ClearenceId { get; set; }
        public string ClearenceName { get; set; }
    }
}