using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cifrados
{
    class Cesar
    {
        public string Nombrearchivo;
        public string Rutaarchivo;
        public string Rutaserver;
        public string Direccionrecorrido;
        public string Clave;
        public int posicionbuffer;
        const int largobuffer = 150;
        public byte[] bufferlectura = new byte[largobuffer];
        public byte[] bufferescritura = new byte[largobuffer];

        public Cesar(string clave, string nombrearchivo, string rutaarchivo, string rutaserver)
        {
            Clave = clave.ToLower();
            Nombrearchivo = nombrearchivo;
            Rutaarchivo = rutaarchivo;
            Rutaserver = rutaserver;
            posicionbuffer = 0;
        }

        public bool ValidarClave(char[]clave)
        {
            var valida = false;
            for (int i = 0; i < clave.Length; i++)
            {
                for (int j = 0; j < i; j++)
                {
                    if (clave[i]==clave[j])
                    {
                        valida = false;
                        j = clave.Length;
                        i = clave.Length;
                    }
                    else
                    {
                        valida = true;
                    }
                }
            }
            return valida;
        }

        public void Cifrar()
        {
            var alfabeto = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789";
            var pos = Clave.Length;
            char[] alfabetomod = new char[alfabeto.Length];
            for (int i = 0; i < Clave.Length; i++)
            {
                alfabetomod[i] = Clave[i];
            }
            for (int i = 0; i < alfabeto.Length; i++)
            {
                if (!(alfabetomod.Contains(alfabeto[i])))
                {
                    alfabetomod[pos] = alfabeto[i];
                    pos++;
                }
            }
            using (var file=new FileStream(Rutaarchivo,FileMode.Open))
            {
                using(var reader=new BinaryReader(file))
                {
                    while (reader.BaseStream.Position!=reader.BaseStream.Length)
                    {
                        bufferescritura = new byte[largobuffer];
                        bufferlectura = new byte[largobuffer];
                        bufferlectura = reader.ReadBytes(largobuffer);
                        posicionbuffer = 0;
                        for (int i = 0; i < bufferlectura.Length; i++)
                        {
                            for (int j = 0; j < alfabeto.Length; j++)
                            {
                                if (alfabeto.Contains((char)bufferlectura[i]))
                                {
                                    if ((char)bufferlectura[i] == alfabeto[j])
                                    {
                                        bufferescritura[posicionbuffer] += (byte)alfabetomod[j];
                                        posicionbuffer++;
                                        j = alfabeto.Length;
                                    }
                                }
                                else
                                {
                                    bufferescritura[posicionbuffer] += (byte)alfabetomod[j];
                                    posicionbuffer++;
                                    j = alfabeto.Length;
                                }
                            }
                        }
                        escribir();
                    }                    
                }
            }
            File.Delete(Rutaarchivo);
        }
        public void escribir()
        {
            using(var file=new FileStream(Rutaserver+Nombrearchivo+".csr",FileMode.Append))
            {
                using(var writer=new BinaryWriter(file))
                {
                    for (int i = 0; i < posicionbuffer; i++)
                    {
                        writer.Write(bufferescritura[i]);
                    }
                }
            }
        }
        public void Descifrar()
        {
            var alfabeto = "abcdefghijklmnñopqrstuvwxyzABCDEFGHIJKLMNÑOPQRSTUVWXYZ0123456789";
            var pos = Clave.Length;
            char[] alfabetomod = new char[alfabeto.Length];
            for (int i = 0; i < Clave.Length; i++)
            {
                alfabetomod[i] = Clave[i];
            }
            for (int i = 0; i < alfabeto.Length; i++)
            {
                if(!(alfabetomod.Contains(alfabeto[i])))
                {
                    alfabetomod[pos] = alfabeto[i];
                    pos++;
                }
            }
            using (var file=new FileStream(Rutaarchivo,FileMode.Open))
            {
                using(var reader = new BinaryReader(file))
                {
                    while (reader.BaseStream.Position!=reader.BaseStream.Length)
                    {
                        bufferescritura = new byte[largobuffer];
                        bufferlectura = reader.ReadBytes(largobuffer);
                        posicionbuffer = 0;
                        for (int i = 0; i < bufferlectura.Length; i++)
                        {
                            for (int j = 0; j < alfabeto.Length; j++)
                            {
                                if(alfabetomod.Contains((char)bufferlectura[i]))
                                {
                                    if ((char)bufferlectura[i]==alfabetomod[i])
                                    {
                                        bufferescritura[posicionbuffer] += (byte)alfabeto[j];
                                        posicionbuffer++;
                                        j = alfabeto.Length;
                                    }
                                }
                                else
                                {
                                    bufferescritura[posicionbuffer] += (byte)bufferlectura[i];
                                    posicionbuffer++;
                                    j = alfabeto.Length;
                                }
                            }
                        }
                        escribirdes();
                    }
                }
            }
            File.Delete(Rutaarchivo);
        }
        public void escribirdes()
        {
            using (var file = new FileStream(Rutaserver + Nombrearchivo + ".txt", FileMode.Append))
            {
                using (var writer = new BinaryWriter(file))
                {
                    for (int i = 0; i < posicionbuffer; i++)
                    {
                        writer.Write(bufferescritura[i]);
                    }
                }
            }
        }
    }
}
