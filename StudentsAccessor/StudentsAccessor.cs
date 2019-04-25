using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace StudentsAccessor
{
    public class StudentsAccessor
    {
        //[FunctionName("GetStudents")]
        //public static async Task<IActionResult> GetStudents(
        //   ILogger log)
        //{
        //    TableContinuationToken token = null;
        //    var memberships = new List<Membership>();

        //    do
        //    {

        //        var emptyQuery = new TableQuery<Membership>();
        //        var queryResult = await students.ExecuteQuerySegmentedAsync(emptyQuery, token);
        //        memberships.AddRange(queryResult.Results);
        //        token = queryResult.ContinuationToken;

        //    } while (token != null);

        //    return new OkObjectResult(memberships);
        //}
    }
}
