using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;
using PID.Common;

namespace PID.Data
{
    public class SummaryRecord : IPIDEntity
    {

        [Index(0)]
        public string ReportingMonth { get; set; }

        [Index(1)]
        public string ReportingUnit { get; set; }

        [Index(2)]
        public string ProjectNumber { get; set; }

        [Index(3)]
        public string ProjectName { get; set; }

        [Index(4)]
        public string ChangeCount { get; set; }
    }
}
