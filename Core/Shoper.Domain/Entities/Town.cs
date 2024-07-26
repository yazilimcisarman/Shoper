using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Domain.Entities
{
    public class Town
    {
        public int Id { get; set; }
        public int TownId { get; set; }
        public int CityId { get; set; }
        public string Townname { get; set; }
    }
}
