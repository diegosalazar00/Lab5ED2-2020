using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Text;

namespace Cifrados
{
    class ZigZag
    {
        public string Nombrearchivo;
        public string Rutaarchivo;
        public string Rutaserver;
        public string Rutaarchivocif;
        public int Filas;
        public char Relleno;
        public const int Largobuffer = 150;
        public string[,] Estructura;
        
        public ZigZag(string nombrearchivo, string rutaarchivo,string rutaserver, int clave)
        {
            Nombrearchivo = nombrearchivo;
            Rutaarchivo = rutaarchivo;
            Rutaserver = rutaserver;
            Filas = clave;
            Relleno = '^';
        }

        public void Cifrar()
        {
            var buffer = new byte[Largobuffer];
            var longitudcadena = 0;
            var olas = 0;
            var cantelementosolas = 0;
            var columnas = 0;
            using (var file=new FileStream(Rutaarchivo,FileMode.Open))
            {
                using(var reader=new BinaryReader(file))
                {
                    while(reader.BaseStream.Position!=reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(Largobuffer);
                        var caracteres = System.Text.ASCIIEncoding.ASCII.GetChars(buffer);
                        longitudcadena = caracteres.Length;
                        calcularcolumnas(ref columnas, ref cantelementosolas, ref olas, Convert.ToDouble(longitudcadena));
                        Estructura = new string[Filas, columnas];
                        var elementos = 0;
                        var contadorfila = 0;
                        var contadorcolumna = 0;
                        var olasaux = 0;
                        foreach (var caracter in caracteres)
                        {
                            if (elementos!='\r')
                            {
                                if (elementos!=cantelementosolas)
                                {
                                    if (contadorfila<Filas)
                                    {
                                        if (caracter=='\n')
                                        {
                                            Estructura[contadorfila, contadorcolumna] = "\n";
                                        }
                                        else
                                        {
                                            Estructura[contadorfila, contadorcolumna] = caracter.ToString();
                                        }
                                        contadorfila++;
                                        elementos++;
                                    }
                                    else
                                    {
                                        var diferencia = contadorfila + 1;
                                        diferencia -= Filas;
                                        if (caracter=='\n')
                                        {
                                            Estructura[(contadorfila - (diferencia * 2)), contadorcolumna] = "\n";
                                        }
                                        else
                                        {
                                            Estructura[(contadorfila - (diferencia * 2)), contadorcolumna] = caracter.ToString();
                                        }
                                        elementos++;
                                        contadorfila++;
                                        if (elementos==cantelementosolas)
                                        {
                                            elementos = 0;
                                            contadorfila = 0;
                                            olasaux++;
                                        }
                                    }
                                    contadorcolumna++;
                                }
                                else
                                {
                                    elementos = 0;
                                    olasaux++;
                                    contadorfila = 0;
                                }
                            }
                        }
                        if ((columnas-longitudcadena)!=0)
                        {
                            contadorfila = 0;
                            for (int i = (((olas-1)*cantelementosolas)); i < columnas; i++)
                            {
                                if (elementos != cantelementosolas)
                                {
                                    if (contadorfila < Filas)
                                    {
                                        if (Estructura[contadorfila, i] == null)
                                        {
                                            Estructura[contadorfila, i] = Relleno.ToString();
                                            elementos++;
                                        }
                                    }
                                    else
                                    {
                                        var diferencia = contadorfila + 1;
                                        diferencia -= Filas;
                                        if (Estructura[(contadorfila-(diferencia*2)),i]==null)
                                        {
                                            Estructura[(contadorfila - (diferencia * 2)), i] = Relleno.ToString();
                                            elementos++;
                                        }
                                    }
                                }
                                else
                                {
                                    i = columnas;
                                }
                                contadorfila++;
                            }
                        }
                        contadorfila = 0;
                        elementos = 0;
                        Escribir(columnas);
                    }
                }
            }
            File.Delete(Rutaarchivo);
        }
        public void calcularcolumnas(ref int columnas, ref int elementosola, ref int olas, double longitud)
        {
            var elementisolavoid = (Filas * 2) - 2;
            var olasvoid = (Math.Ceiling(longitud / elementisolavoid)).ToString("####");
            elementosola = int.Parse(elementisolavoid.ToString("####"));
            olas = int.Parse(olasvoid);
            columnas = elementosola * olas;
        }
        public void Escribir(int columnas)
        {
            using(var file=new FileStream(Rutaserver+Nombrearchivo+".zz",FileMode.Append))
            {
                using(var writer=new StreamWriter(file))
                {
                    for (int i = 0; i < Filas; i++)
                    {
                        for (int j = 0; j < columnas; j++)
                        {
                            if (Estructura[i,j]!=null)
                            {
                                writer.Write(Estructura[i, j]);
                            }
                        }
                    }
                }
            }
        }
        public bool Descifrar()
        {
            var correcto = false;
            var buffer = new byte[Largobuffer];
            var longitudcad = 0;
            var olas = 0;
            var cantelementosolas = 0;
            var columnas = 0;
            using(var file=new FileStream(Rutaarchivo,FileMode.Open))
            {
                using(var reader=new BinaryReader(file))
                {
                    while(reader.BaseStream.Position!=reader.BaseStream.Length)
                    {
                        buffer = reader.ReadBytes(Largobuffer);
                        var caracteres = System.Text.ASCIIEncoding.ASCII.GetString(buffer);
                        if (caracteres.Contains('/'))
                        {
                            caracteres = caracteres.Replace('/', (char)92);
                        }
                        longitudcad = caracteres.Length - 1;
                        calcularcolumnas(ref columnas, ref cantelementosolas, ref olas, Convert.ToDouble(longitudcad));
                        if (longitudcad==columnas)
                        {
                            Estructura = new string[Filas, columnas];
                            var cresta = caracteres.Substring(0, olas + 1);
                            var valle = caracteres.Substring((columnas - olas) + 1, olas);
                            caracteres = caracteres.Remove(0, olas + 1);
                            caracteres = caracteres.Remove((caracteres.Length - 1) - olas, olas);
                            List<string> niveles = new List<string>(Filas);
                            for (int i = 0; i < Filas; i++)
                            {
                                niveles.Add("");
                            }
                            niveles[0] = cresta;
                            niveles[Filas - 1] = valle;
                            var elementosrestantes = longitudcad - (olas * 2);
                            var rielesrestantes = (Filas - 2);
                            var longitudrieles = elementosrestantes / rielesrestantes;
                            for (int i = 0; i < rielesrestantes; i++)
                            {
                                niveles[i] = caracteres.Substring(0, longitudrieles);
                                caracteres = caracteres.Remove(0, longitudrieles);
                            }
                            var contadorfilas = 0;
                            var elementos = 0;
                            var olasaux = 0;
                            for (int i = 0; i < columnas; i++)
                            {
                                if (elementos!=cantelementosolas)
                                {
                                    if (contadorfilas<Filas)
                                    {
                                        if (contadorfilas==0)
                                        {
                                            Estructura[contadorfilas, i] = niveles[contadorfilas].Substring(0, 2);
                                            niveles[contadorfilas] = niveles[contadorfilas].Remove(1, 1);
                                        }
                                        else
                                        {
                                            Estructura[contadorfilas, i] = niveles[contadorfilas].Substring(0, 1);
                                            niveles[contadorfilas] = niveles[contadorfilas].Remove(0, 1);
                                        }
                                        contadorfilas++;
                                        elementos++;
                                    }
                                    else
                                    {
                                        var diferencia = contadorfilas + 1;
                                        diferencia -= Filas;
                                        Estructura[(contadorfilas - (diferencia * 2)), i] = niveles[contadorfilas - (diferencia * 2)].Substring(0, 1);
                                        niveles[contadorfilas - (diferencia * 2)] = niveles[contadorfilas - (diferencia * 2)].Remove(0, 1);
                                        elementos++;
                                        contadorfilas++;
                                        if (elementos==cantelementosolas)
                                        {
                                            elementos = 0;
                                            contadorfilas = 0;
                                            olasaux++;
                                        }
                                    }
                                }
                                else
                                {
                                    elementos = 0;
                                    olasaux++;
                                    contadorfilas = 0;
                                }
                            }
                            Escribirdes(columnas);
                        }
                        else
                        {
                            File.Delete(Rutaarchivo);
                            throw new Exception("Clave incorrecta");
                        }
                    }
                }
            }
            File.Delete(Rutaarchivo);
            return correcto;
        }
        public void Escribirdes(int columnas)
        {
            using (var file = new FileStream(Rutaserver + Nombrearchivo + ".txt", FileMode.Append))
            {
                using (var writer = new StreamWriter(file))
                {
                    for (int i = 0; i < Filas; i++)
                    {
                        for (int j = 0; j < columnas; j++)
                        {
                            if (Estructura[i, j] != null)
                            {
                                writer.Write(Estructura[i, j]);
                            }
                        }
                    }
                }
            }
        }
    }
}
