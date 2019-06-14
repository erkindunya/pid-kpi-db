using System;
using CsvHelper;
using PID.Common;
using PID.Data;

namespace PID.Data.Mappers
{
    public class DetailedDataMapper : IPIDMapper
    {
        public IPIDEntity Map(CsvReader csv)
        {
            if (csv == null)
            {
                throw new ArgumentNullException(nameof(csv));
            }

            var record = new DetailedRecord()
            {
                ReportingUnit = csv.GetField<string>(0),
                ReportingUnitName = csv.GetField<string>(1),
                ProjectNumber = csv.GetField<string>(2),
                ProjectName = csv.GetField<string>(3),
                KeyMemberName = csv.GetField<string>(4),
                OracleEmployeeNumber = csv.GetField<string>(5),
                KeyMemberRole = csv.GetField<string>(6),
                CurrentlyActive = csv.GetField<string>(7),
                EffectiveDatesFrom = csv.GetField<string>(8),
                EffectiveDatesTo = csv.GetField<string>(9),
                ActionedByName = csv.GetField<string>(10),
                ActionedByOracleEmployeeNumber = csv.GetField<string>(11),
                Use_In_PID = csv.GetField<string>(12)
            };

            return record;
        }
    }



}
