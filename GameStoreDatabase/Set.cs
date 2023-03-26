using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStoreDatabase
{
    public class Set
    {
        public string Name { get; set; }
        public string Id { get; set; }

        public Set(string name, string id)
        {
            Name = name;
            Id = id;
        }
    }
}
