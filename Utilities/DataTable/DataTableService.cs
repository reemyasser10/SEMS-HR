using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.DataTable
{
    public class DataTable<T> where T : class
    {
        public DataTableResult<T> LoadTable(DtParameters dtParameters, IEnumerable<T> resultData, int filteredCount, int totalCount)
        {
            return new DataTableResult<T>
            {
                Data = resultData,
                DtParameters = dtParameters,
                FilteredResultsCount = filteredCount,
                TotalResultsCount = totalCount
            };
        }

        public DtResult<T> ReturnTable(DataTableResult<T> dataTableResult)
        {
            return new DtResult<T>
            {
                Draw = dataTableResult.DtParameters.Draw,
                RecordsTotal = dataTableResult.TotalResultsCount,
                RecordsFiltered = dataTableResult.FilteredResultsCount,
                Data = dataTableResult.Data
            };
        }
    }

}
