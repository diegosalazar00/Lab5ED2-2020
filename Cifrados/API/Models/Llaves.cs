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

       /* public void ArchivoARuta(IFormFile Archivo)
        {​​
            var nombre = Archivo.FileName;
            using (var reader = new BinaryReader(Archivo.OpenReadStream()))
            {​​
                using (var st = new FileStream($"Temporal\\{​​nombre}​​.txt", FileMode.OpenOrCreate))
                {​​
                    using (var w = new BinaryWriter(st))
                    {​​
                        var bl = 10000;
                        var bf = new byte[bl];
                        bf = reader.ReadBytes(bl);
                        foreach (var car in bf)
                        {​​
                            w.Write(car);
                        }​​
                    }​​
                reader.Close();
                }​​
            }​​

        }​​*/
    }
}
