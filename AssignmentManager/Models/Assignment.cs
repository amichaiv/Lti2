using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager.Models
{
    public class Assignment : TableEntity
    {
        public Guid Guid { get; set; }
        public string CourseName { get; set; }
        public string LmsInstanceName { get; set; }
        public string LmsProviderName { get; set; }
        public string LtiName { get; set; }
    }
}
