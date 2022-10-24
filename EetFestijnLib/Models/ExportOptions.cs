using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class ExportOptions
    {
        public string FileName { get; set; }

        public string Filter { get; }

        public bool IsConfirmed { get; set; }

        public ExportOptions(string fileName, string filter)
        {
            FileName = fileName;
            Filter = filter;
        }
    }
}
