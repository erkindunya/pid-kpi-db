using CsvHelper.Configuration.Attributes;
using PID.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace PID.Data
{
    public class DetailedRecord : IPIDEntity
    {
        [Index(0)]
        public string ReportingUnit;
        [Index(1)]
        public string ReportingUnitName;
        [Index(2)]
        public string ProjectNumber;
        [Index(3)]
        public string ProjectName;
        [Index(4)]
        public string KeyMemberName;
        [Index(5)]
        public string OracleEmployeeNumber;
        [Index(6)]
        public string KeyMemberRole;
        [Index(7)] public string CurrentlyActive;
        [Index(8)] public string EffectiveDatesFrom;
        [Index(9)] public string EffectiveDatesTo;
        [Index(10)] public string ActionedByName;
        [Index(11)] public string ActionedByOracleEmployeeNumber;
        [Index(12)] public string Use_In_PID;

    }
}
