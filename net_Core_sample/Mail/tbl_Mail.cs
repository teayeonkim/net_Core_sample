using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mail
{
    public class tbl_Mail
    {
        [Key]
        public int num { get; set; }

        public string ToMail { get; set; }

        public string FoMail { get; set; }

        public string title { get; set; }

        public string Content { get; set; }

        public string errMsg { get; set; }

        public DateTime regdate { get; set; }

    }
}
