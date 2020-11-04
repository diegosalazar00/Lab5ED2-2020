using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cifrados
{
    class Espiral
    {
        public string Nombrearchivo;
        public string Rutaarchivo;
        public string Rutaserver;
        public string Direccionrecorrido;
        public int Clave;
        public int Posbufferescritura;
        const int largobuffer = 150;
        public byte[] bufferescritura = new byte[largobuffer];
        public byte[] bufferlectura = new byte[largobuffer];
        int pos = 0;
        char[,] matriz;

        public Espiral(int clave, string direccion, string nombrearchivo, string rutaarchivo, string rutaserver)
        {
            Clave = clave;
            Direccionrecorrido = direccion.ToUpper();
            Nombrearchivo = nombrearchivo;
            Rutaarchivo = rutaarchivo;
            Rutaserver = rutaserver;
            Posbufferescritura = 0;
        }

        public void Cifrar()
        {
            using (var file = new FileStream(Rutaarchivo, FileMode.Open))
            {
                using(var reader=new BinaryReader(file))
                {
                    while (reader.BaseStream.Position!=reader.BaseStream.Length)
                    {
                        matriz = new char[Clave, calcularfilas(Clave, largobuffer)];
                        bufferlectura = new byte[largobuffer];
                        bufferlectura = reader.ReadBytes(largobuffer);
                        Posbufferescritura = 0;
                        pos = 0;
                        var Valorcolumna = matriz.GetLength(0);
                        var Valorfila = matriz.GetLength(1);
                        var xarribaabajo = 0;
                        var xabajoarriba = 0;
                        var yderizq = 0;
                        var yizqder = 0;
                        var area = matriz.GetLength(0) * matriz.GetLength(1);
                        for (int i = 0; i < matriz.GetLength(1); i++)
                        {
                            for (int j = 0; j < matriz.GetLength(0); j++)
                            {
                                if (pos<bufferlectura.Length)
                                {
                                    matriz[j, i] = (char)bufferlectura[pos];
                                    pos++;
                                }
                                else
                                {
                                    matriz[j, i] = '$';
                                }
                            }
                        }
                        switch (Direccionrecorrido)
                        {
                            case "D":
                                xabajoarriba = 0;
                                xarribaabajo = matriz.GetLength(0) - 1;
                                yizqder = 0;
                                yderizq = matriz.GetLength(1) - 1;
                                while (area > Posbufferescritura)
                                {//izq -> der
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = yizqder; i < Valorcolumna; i++)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[i, yizqder];
                                            }
                                            Posbufferescritura++;
                                        }
                                        yizqder++;
                                    }
                                    //arriba->abajo
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = yizqder; i < Valorfila; i++)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[xarribaabajo, i];
                                            }
                                            Posbufferescritura++;
                                        }
                                        xarribaabajo--;
                                        Valorcolumna -= 1;
                                    }
                                    //der->izq
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = xarribaabajo; i >= xabajoarriba; i--)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[i, yderizq];
                                            }
                                            Posbufferescritura++;
                                        }
                                        yderizq--;
                                        Valorfila -= 1;
                                    }
                                    //abajo->arriba
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = yderizq; i >= yizqder; i--)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[xabajoarriba, i];
                                            }
                                            Posbufferescritura++;
                                        }
                                        xabajoarriba++;
                                    }
                                }
                                break;
                            case "I":
                                xabajoarriba = matriz.GetLength(0) - 1;
                                xarribaabajo = 0;
                                yizqder = matriz.GetLength(1) - 1;
                                yderizq = 0;
                                while (area > Posbufferescritura)
                                {//arriba->abajo
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = yderizq; i < Valorfila; i++)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[xarribaabajo, i];
                                            }
                                            Posbufferescritura++;
                                        }
                                        xarribaabajo++;
                                    }
                                    //izq->der
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = xarribaabajo; i < Valorcolumna; i++)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[i, yizqder];
                                            }
                                            Posbufferescritura++;
                                        }
                                        yizqder--;
                                    }
                                    //abajo->arriba
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = yizqder; i >= yderizq; i--)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[xabajoarriba, i];
                                            }
                                            Posbufferescritura++;
                                        }
                                        xabajoarriba--;
                                        Valorcolumna -= 1;
                                    }
                                    //der->izq
                                    if (area > Posbufferescritura)
                                    {
                                        for (int i = xabajoarriba; i >= xarribaabajo; i--)
                                        {
                                            if (Posbufferescritura < largobuffer)
                                            {
                                                bufferescritura[Posbufferescritura] = (byte)matriz[i, yderizq];
                                            }
                                            Posbufferescritura++;
                                        }
                                        yderizq++;
                                        Valorfila -= 1;
                                    }
                                }
                                break;
                        }
                        Escribir();                        
                    }
                }           
            }
            File.Delete(Rutaarchivo);
        }
        public void Escribir()
        {
            using(var file=new FileStream(Rutaserver+Nombrearchivo+".rt",FileMode.Append))
            {
                using (var writer = new BinaryWriter(file))
                {
                    writer.Write(bufferescritura);
                }
            }
        }
        public int calcularfilas(int columnas, int longitudtexto)
        {
            var filas = 0;
            filas = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(longitudtexto) / Convert.ToDouble(columnas)));
            return filas;
        }
        public void Descifrar()
        {
            var pos = 0;
            var valorcolumna = 0;
            var valorfila = 0;
            var xarribaabajo = 0;
            var xabajoarriba = 0;
            var yderizq = 0;
            var yizqder = 0;
            var carmatriz = 0;
            var area = 0;
            using (var file = new FileStream(Rutaarchivo, FileMode.Open))
            {
                using(var reader=new BinaryReader(file))
                {
                    while (reader.BaseStream.Position!=reader.BaseStream.Length)
                    {
                        bufferlectura = new byte[largobuffer];
                        bufferescritura = new byte[largobuffer];
                        bufferlectura = reader.ReadBytes(largobuffer);
                        matriz = new char[calcularcolumnas(Clave, largobuffer), Clave];
                        area = matriz.GetLength(0) * matriz.GetLength(1);
                        valorcolumna = matriz.GetLength(0);
                        valorfila = matriz.GetLength(1);
                        Posbufferescritura = 0;
                        pos = 0;
                        switch (Direccionrecorrido)
                        {
                            case "D":
                                xarribaabajo = 0;
                                xabajoarriba = matriz.GetLength(0) - 1;
                                yderizq = 0;
                                yizqder = matriz.GetLength(1) - 1;
                                while (carmatriz<matriz.Length&&pos<bufferlectura.Length)
                                {
                                    if (pos<bufferlectura.Length)
                                    {
                                        for (int i = yderizq; i < valorfila; i++)
                                        {
                                            if (pos<bufferlectura.Length)
                                            {
                                                matriz[xarribaabajo, i] = (char)bufferlectura[pos];
                                                pos++;
                                                carmatriz++;
                                            }
                                        }
                                        xarribaabajo++;
                                    }
                                    if (pos<bufferlectura.Length)
                                    {
                                        for (int i = xarribaabajo; i < valorcolumna; i++)
                                        {
                                            if (pos<bufferlectura.Length)
                                            {
                                                matriz[i, yizqder] = (char)bufferlectura[pos];
                                                pos++;
                                                carmatriz++;
                                            }
                                        }
                                        yizqder--;
                                    }
                                    if (pos<bufferlectura.Length)
                                    {
                                        for (int i = yizqder; i >= yderizq; i--)
                                        {
                                            if (pos<bufferlectura.Length)
                                            {
                                                matriz[xabajoarriba, i] = (char)bufferlectura[pos];
                                                pos++;
                                                carmatriz++;
                                            }
                                        }
                                        xabajoarriba--;
                                        valorcolumna -= 1;
                                    }
                                    if (pos<bufferlectura.Length)
                                    {
                                        for (int i = yizqder; i >= yderizq; i--)
                                        {
                                            if (pos<bufferlectura.Length)
                                            {
                                                matriz[xabajoarriba, i] = (char)bufferlectura[pos];
                                                pos++;
                                                carmatriz++;
                                            }
                                        }
                                        xabajoarriba--;
                                        valorcolumna -= 1;
                                    }
                                    if (pos<bufferlectura.Length)
                                    {
                                        for (int i = xabajoarriba; i >= xarribaabajo; i--)
                                        {
                                            if (pos<bufferlectura.Length)
                                            {
                                                matriz[i, yderizq] = (char)bufferlectura[pos];
                                                pos++;
                                                carmatriz++;
                                            }
                                        }
                                        yderizq++;
                                        valorfila -= 1;
                                    }
                                }
                                break;
                            case "I":
                                xabajoarriba = 0;
                                xarribaabajo = matriz.GetLength(0) - 1;
                                yizqder = 0;
                                yderizq = matriz.GetLength(1) - 1;
                                while (carmatriz<matriz.Length&&pos<bufferlectura.Length)
                                {
                                    if (pos<bufferlectura.Length)
                                    {
                                        for (int i = yizqder; i < valorcolumna; i++)
                                        {
                                            if (pos<bufferlectura.Length)
                                            {
                                                matriz[i, yizqder] = (char)bufferlectura[pos];
                                                pos++;
                                                carmatriz++;
                                            }
                                        }
                                    }
                                    yizqder++;
                                }
                                if (pos<bufferlectura.Length)
                                {
                                    for (int i = yizqder; i < valorfila; i++)
                                    {
                                        if (pos<bufferlectura.Length)
                                        {
                                            matriz[xarribaabajo, i] = (char)bufferlectura[pos];
                                            pos++;
                                            carmatriz++;
                                        }
                                    }
                                    xarribaabajo--;
                                    valorcolumna -= 1;
                                }
                                if (pos<bufferlectura.Length)
                                {
                                    for (int i = xarribaabajo; i >= xabajoarriba; i--)
                                    {
                                        if (pos<bufferlectura.Length)
                                        {
                                            matriz[i, yderizq] = (char)bufferlectura[pos];
                                            pos++;
                                            carmatriz++;
                                        }
                                    }
                                    yderizq--;
                                    valorfila -= 1;
                                }
                                if (pos<bufferlectura.Length)
                                {
                                    for (int i = yderizq; i >= yizqder; i--)
                                    {
                                        if (pos<bufferlectura.Length)
                                        {
                                            matriz[xabajoarriba, i] = (char)bufferlectura[pos];
                                            pos++;
                                            carmatriz++;
                                        }
                                    }
                                    xabajoarriba++;
                                }
                                break;
                        }
                        if (pos==bufferlectura.Length&&carmatriz<matriz.Length)
                        {
                            for (int i = 0; i < matriz.GetLength(0); i++)
                            {
                                for (int j = 0; j < matriz.GetLength(1); j++)
                                {
                                    if (matriz[i,j]=='\0')
                                    {
                                        matriz[i, j] = '$';
                                    }
                                }
                            }
                        }
                        for (int i = 0; i < matriz.GetLength(0); i++)
                        {
                            for (int j = 0; j < matriz.GetLength(1); j++)
                            {
                                if (Posbufferescritura>=largobuffer)
                                {
                                    Escribirdes();
                                    Posbufferescritura = 0;
                                }
                                bufferescritura[Posbufferescritura] = (byte)matriz[i, j];
                                Posbufferescritura++;
                            }
                        }
                        Escribirdes();
                    }
                }
            }
            File.Delete(Rutaarchivo);
        }
        public void Escribirdes()
        {
            using(var file=new FileStream(Rutaserver+Nombrearchivo+".txt",FileMode.Append))
            {
                using(var writer=new BinaryWriter(file))
                {
                    for (int i = 0; i < bufferescritura.Length; i++)
                    {
                        if ((char)bufferescritura[i]!='$')
                        {
                            writer.Write(bufferescritura[i]);
                        }
                    }
                }
            }
        }
        private int calcularcolumnas(int filas, int longitudtexto)
        {
            var columnas = 0;
            columnas = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(longitudtexto) / Convert.ToDouble(filas)));
            return columnas;

        }
    }
}
