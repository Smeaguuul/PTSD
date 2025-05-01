using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Field
    {
        public Field(int id)
        {
            this.Id = id;
        }
        public Match CurrentMatch { get; set; }
        public int Id { get; set; }

        public Field() { }
    }
}
