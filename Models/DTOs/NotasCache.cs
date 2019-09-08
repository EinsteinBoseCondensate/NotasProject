using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotasProject.Models.DTOs
{
    public class NotasCache
    {
        public List<NotaDTO> anchored { get; set; }
        public List<NotaDTO> notAnchored { get; set; }
    }
}