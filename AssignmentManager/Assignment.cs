﻿using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace AssignmentManager
{
    class Assignment : TableEntity
    {
        public Guid Guid { get; set; }
    }
}
