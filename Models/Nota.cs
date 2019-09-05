using NotasProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotasProject.Models
{
    public class Nota
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NotaId { get; set; }
        /// <summary>
        /// Last Update Date Time
        /// </summary>
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
        public string Nonce { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
