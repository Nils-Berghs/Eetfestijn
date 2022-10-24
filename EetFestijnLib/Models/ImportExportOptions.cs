using System;
using System.Collections.Generic;
using System.Text;

namespace be.berghs.nils.EetFestijnLib.Models
{
    public class ImportExportOptions
    {
        public string FileName { get; set; }

        public string Filter { get; }

        public bool IsConfirmed { get; set; }

        public ImportExportOptions(string fileName, string filter):this(filter)
        {
            FileName = fileName;
            
        }

        public ImportExportOptions(string filter)
        {
            Filter = filter;
        }
    }
}
