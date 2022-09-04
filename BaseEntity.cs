using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.EntityTypes
{
    public class BaseEntity
    {
        public int ID { get; set; }
    }
    public class Dictionary: BaseEntity
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
