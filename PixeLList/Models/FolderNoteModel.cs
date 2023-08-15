using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixeLList.Models
{
    public class FolderNoteModel
    {
        public int ID { get; set; }
        public string FolderName { get; set; }
        public Color FolderColor { get; set; }
        public DateTime FolderCreateTime { get; set; }


    }
}
