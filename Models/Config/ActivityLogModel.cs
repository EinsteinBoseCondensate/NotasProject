using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NotasProject.Models.Config
{
    public class ActivityLogModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ActivityLogModelId { get; set; }
        public DateTime LogTime { get; set; }
        public string Realm { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
    }
}