using CsvHelper;
using PID.Common;

namespace PID.Common
{
    public interface IPIDMapper
    {
        IPIDEntity Map(CsvReader stream);
    }



}
