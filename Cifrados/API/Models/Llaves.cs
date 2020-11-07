using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace API.Models
{
    public class Llaves
    {
        public string words { get; set; }
        public int levels { get; set; }

        public int rows { get; set; }
        public int columns { get; set; }

    }
}
