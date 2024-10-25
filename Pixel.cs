using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    public class Pixel
    {
        private byte blue;
        private byte red;
        private byte green;

        /// <summary>
        /// Pixel en BGR
        /// </summary>
        /// <param name="blue"></param>
        /// <param name="red"></param>
        /// <param name="green"></param>
        public Pixel(byte blue, byte green, byte red)
        {
            this.blue = blue;
            this.green = green;
            this.red = red;
        }

        /// <summary>
        /// Proprété en écriture et en lecture du bleu
        /// </summary>
        public byte Blue
        {
            get { return this.blue; }
            set { this.blue = value; }
        }

        /// <summary>
        /// Proprété en écriture et en lecture du rouge
        /// </summary>
        public byte Red
        {
            get { return this.red; }
            set { this.red = value; }
        }

        /// <summary>
        /// Proprété en écriture et en lecture du vert
        /// </summary>
        public byte Green
        {
            get { return this.green; }
            set { this.green = value; }
        }

        /// <summary>
        /// Retourne la version gris nuancé du pixel
        /// Chaque byte du pixel transformé devient la moyenne des trois bytes
        /// </summary>
        /// <returns></returns>
        public Pixel Grey()
        {
            byte moy = Convert.ToByte((this.blue + this.green + this.red) / 3);

            Pixel gris = new Pixel(moy, moy, moy);

            return gris;
        }

        /// <summary>
        /// Renvoie une matrice carrée du même pixel de dimmension : zoom
        /// </summary>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public Pixel [,] Zoom(int zoom)
        {
            Pixel[,] pixelZoom = new Pixel[zoom, zoom]; 

            for(int i = 0; i < pixelZoom.GetLength(0); i++)
            {
                for(int j = 0; j < pixelZoom.GetLength(1); j++)
                {
                    pixelZoom[i, j] = new Pixel(this.blue, this.red, this.green);
                }
            }

            return pixelZoom;
        }

        /// <summary>
        /// Multiplie chaque couleur du pixel par une valeur
        /// </summary>
        /// <param name="value"></param>
        public Pixel Mult(double value)
        {
            byte blue_mult = (byte)Math.Min(Math.Max(0, this.blue * value), 255);
            byte green_mult = (byte)Math.Min(Math.Max(0, this.green * value), 255);
            byte red_mult = (byte)Math.Min(Math.Max(0, this.red * value), 255);

            Pixel res = new Pixel(blue_mult, green_mult, red_mult);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne la somme de tous les pixel
        /// </summary>
        /// <param name="tabpix"></param>
        /// <returns></returns>
        public Pixel SommePix(List <Pixel> tabpix)
        {
            Pixel res = new Pixel(0,0,0);

            for(int i = 0; i < tabpix.Count; i++)
            {
                res.Blue += tabpix[i].Blue;
                res.Red += tabpix[i].Red;
                res.Green += tabpix[i].Green;
            }

            //res.Blue = (byte)(res.Blue / tabpix.Count);
            //res.Red = (byte)(res.Red / tabpix.Count);
            //res.Green = (byte)(res.Green / tabpix.Count);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel, prend un pixel en paramètre
        /// Retourne la somme de ces 2 pixels.
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public void Sum(Pixel a, Pixel b)
        {
            this.blue = (byte)Math.Min(Math.Max(a.Blue +  b.Blue, 0), 255);
            this.green = (byte)Math.Min(Math.Max(a.Green +  b.Green, 0), 255);
            this.red = (byte)Math.Min(Math.Max(a.Red + b.Red, 0), 255);
        }

        /// <summary>
        /// Le pixel devient la norme des deux autres
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public Pixel Norme(Pixel a, Pixel b)
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)Math.Sqrt(a.Blue * a.Blue + b.Blue * b.Blue);
            res.Green = (byte)Math.Sqrt(a.Green * a.Green + b.Green * b.Green);
            res.Red = (byte)Math.Sqrt(a.Red * a.Red + b.Red * b.Red);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne le log de celui ci pixel
        /// </summary>
        /// <returns></returns>
        public Pixel LogPix()
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)(255 / Math.Log(255) * Math.Log( this.blue + 1));
            res.Green = (byte)(255 / Math.Log(255) * Math.Log( this.green + 1));
            res.Red = (byte)(255 / Math.Log(255) * Math.Log( this.red + 1));

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne la valeur absolue de celui ci
        /// </summary>
        /// <returns></returns>
        public Pixel AbsolutePix()
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)(1.50 * this.blue + 50.0);
            res.Green = (byte)(1.50 * this.green + 50.0);
            res.Red = (byte)(1.50 * this.red + 50.0);

            return res;
        }

        /// <summary>
        /// Retourne une instance de pixel
        /// Retourne un pixel de couleur opposée
        /// </summary>
        /// <returns></returns>
        public Pixel Invere_BW()
        {
            Pixel res = new Pixel(0, 0, 0);

            res.Blue = (byte)(255 - this.blue);
            res.Green = (byte)(255 - this.green);
            res.Red = (byte)(255 - this.red);

            return res;
        }

        /// <summary>
        /// rend un pixel noir
        /// </summary>
        public void Black()
        {
            this.blue = 0;
            this.green = 0;
            this.red = 0;
        }

        /// <summary>
        /// rend un pixel blanc
        /// </summary>
        public void White()
        {
            this.blue = 255;
            this.green = 255;
            this.red = 255;
        }

        /// <summary>
        /// retourne un tableau de 3 string
        /// un élément correspond  aux 4 premiers chiffres de la valeur binaire d'une couleur 
        /// </summary>
        /// <returns></returns>
        public string[] Hexa()
        {
            string[] res = new string[3];

            //Conversion de chaque byte en binaire
            res[0] = Convert.ToString(this.blue, 2);
            res[1] = Convert.ToString(this.green, 2);
            res[2] = Convert.ToString(this.red, 2);

            //Ajoute des 0 devant la valeur binaire de talle sorte à ce que le length soit de 8
            for (int i = 0; i < 3; i++)
            {
                while (res[i].Length < 8)
                    res[i] = res[i].PadLeft(8, '0');
                res[i] = res[i].Substring(0, 4);
            }          

            return res;
        }

        /// <summary>
        /// retourne une matrice de string de dimensions [3,2]
        /// dimensions en colonnes pour les 2 images 
        /// les 3 lignes pour les 3 couleurs d'un pixel
        /// </summary>
        /// <returns></returns>
        public string[,] Hexa2()
        {
            string[,] res = new string[3, 2];

            //Conversion de chaque byte en binaire puis le split en 2 : de 0 à 4 et de 4 à 8
            res[0, 0] = Convert.ToString(this.blue, 2);
            res[0, 1] = Convert.ToString(this.blue, 2);
            res[1, 0] = Convert.ToString(this.green, 2);
            res[1, 1] = Convert.ToString(this.green, 2);
            res[2, 0] = Convert.ToString(this.red, 2);
            res[2, 1] = Convert.ToString(this.red, 2);

            //Ajoute des 0 devant la valeur binaire de talle sorte à ce que le length soit de 8
            for (int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 2; j++)
                {
                    if (res[i,j].Length < 8) res[i,j] = res[i,j].PadLeft(8, '0');

                    if (j == 0)
                    {
                        res[i, j] = res[i, j].Substring(0, 4);
                        res[i, j] = res[i, j].PadRight(8, '0');
                    }
                    if (j == 1)
                    {
                        res[i, j] = res[i, j].Substring(4, 4);
                        res[i, j] = res[i, j].PadRight(8, '0');
                    }
                }
            }

            return res;
        }
    }
}
