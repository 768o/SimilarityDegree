using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommend
{
    public class User
    {
        public string Id { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public List<Item> UserDegrees { get; set; } = new List<Item>();
        public List<Item> RecommendItems { get; set; } = new List<Item>();
    }
}
