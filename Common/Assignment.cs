using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace Common
{
    public class Assignment : TableEntity
    {
        public Guid Guid { get; set; }

        public LtiLaunchRequestData LtiLaunchRequestData { get; set; }
    }
}