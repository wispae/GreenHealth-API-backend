using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenHealth_API_backend.Models
{
    public interface IAiResult
    {
        public float Accuracy { get; set; }
        public int Output { get; set; }
		public string Species { get; set; }
		public string Kind { get; set; }
    }
}
