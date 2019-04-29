using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentsAccessor
{
    public class Assignment : TableEntity
    {
        public Guid Guid { get; set; }
    }
}