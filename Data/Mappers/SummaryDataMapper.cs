using System;
using CsvHelper;
using PID.Common;
using PID.Data;

namespace PID.Data.Mappers
{
    public class SummaryDataMapper : IPIDMapper
    {
        public IPIDEntity Map(CsvReader csv)
        {
            if (csv == null)
            {
                throw new ArgumentNullException(nameof(csv));
            }

            var record = new SummaryRecord()
            {
                ReportingMonth = csv.GetField<string>(0),
                ReportingUnit = csv.GetField<string>(1),
                ProjectNumber = csv.GetField<string>(2),
                ProjectName = csv.GetField<string>(3),
                ChangeCount = csv.GetField<string>(4)
            };

            return record;
        }
    }



}
