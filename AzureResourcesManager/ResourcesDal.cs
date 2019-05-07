using System;
using System.Collections.Generic;
using System.Text;

namespace AzureAccessor
{
    public class ResourcesDal
    {
        public IEnumerable<Resource> GetResources()
        {
            var resources = new List<Resource>
            {
                new Resource
                {
                    Name = "Azure Functions",
                    Category = "Featured",
                    Summary = "Azure Functions",
                    IconUrl =
                        "https://docs.microsoft.com/en-us/learn/achievements/data-ai/evolving-world-of-data-badge.svg",
                    Url = "https://docs.microsoft.com/learn/modules/evolving-world-of-data"
                },
                new Resource
                {
                    Name = "Virtual Machines",
                    Category = "Featured",
                    Summary = "Virtual Machines",
                    IconUrl =
                        "https://docs.microsoft.com/en-us/learn/achievements/data-ai/evolving-world-of-data-badge.svg",
                    Url = "https://docs.microsoft.com/learn/modules/evolving-world-of-data"
                },
                new Resource
                {
                    Name = "Storage Accounts",
                    Category = "Featured",
                    Summary = "Storage Accounts",
                    IconUrl =
                        "https://docs.microsoft.com/en-us/learn/achievements/data-ai/evolving-world-of-data-badge.svg",
                    Url = "https://docs.microsoft.com/learn/modules/evolving-world-of-data"
                },
                new Resource
                {
                    Name = "Azure SQL Database",
                    Category = "Featured",
                    Summary = "Azure SQL Database",
                    IconUrl =
                        "https://docs.microsoft.com/en-us/learn/achievements/data-ai/evolving-world-of-data-badge.svg",
                    Url = "https://docs.microsoft.com/learn/modules/evolving-world-of-data"
                },
                new Resource
                {
                    Name = "App Service",
                    Category = "Featured",
                    Summary = "App Service",
                    IconUrl =
                        "https://docs.microsoft.com/en-us/learn/achievements/data-ai/evolving-world-of-data-badge.svg",
                    Url = "https://docs.microsoft.com/learn/modules/evolving-world-of-data"
                }
            };

            return resources;
        }
    }
}
