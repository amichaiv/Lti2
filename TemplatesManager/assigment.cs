using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace TemplatesManager
{
    class Assigment : TableEntity
    {
        public Guid Guid { get; set; }

        public Assigment(string lmsName, string courseId, string linkId)
        {
            PartitionKey = $"{lmsName}";
            RowKey = $"{courseId}_{linkId}";
        }

        public void GenerateGuid()
        {
            Guid = Guid.NewGuid();
        }
    }
}
