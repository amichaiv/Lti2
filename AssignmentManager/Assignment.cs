using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsManager
{
    public class Assignment : TableEntity
    {
        public Guid Guid { get; set; }
        public string CourseName { get; set; }
        public string LmsName { get; set; }
        public string LtiName { get; set; }
    }
}
