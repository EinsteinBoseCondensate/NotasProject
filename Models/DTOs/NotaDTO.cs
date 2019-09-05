using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotasProject.Models.DTOs
{
    public class NotaDTO
    {
        public DateTime LUDT { get; set; }
        /// <summary>
        /// Creation Date Time
        /// </summary>
        public DateTime CDT { get; set; }
        /// <summary>
        /// NoteContent
        /// </summary>
        public string NoteContent { get; set; }
        public bool Anchor { get; set; }
        public int NotaId { get; set; }
    }
    public class CreateNotaDTO
    {
        public string NoteContent { get; set; }
        public bool Anchor { get; set; }
    }
}