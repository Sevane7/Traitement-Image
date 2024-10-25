using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Numerics;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    public class MyImage
    {
        private int file_size;
        private int largeur;
        private int hauteur;
        private int bitsParCouleur = 24;
        private Pixel [,] pixels;

        /// <summary>
        /// Constructeur en fonctions d'une matrice de pixel
        /// </summary>
        /// <param name="pixels"></param>
        public MyImage(Pixel[,] pixels)
        {
            this.pixels = pixels;
            this.hauteur = pixels.GetLength(0);
            this.largeur= pixels.GetLength(1);
            this.file_size = pixels.GetLength(0) * pixels.GetLength(1) * this.bitsParCouleur;
        }

        /// <summary>
        /// Constructeur en fonction d'un fichier
        /// </summary>
        /// <param name="myfile"></param>
        public MyImage(string myfile)
        {
            //bytes est un vecteur composé d'octets représentant les métadonnées et les données de l'image
            byte[] bytes = File.ReadAllBytes(myfile);

            byte[] size = new byte[] { bytes[2], bytes[3], bytes[4], bytes[5] };
            this.file_size = Convertir_Endian_To_Int(size);

            byte[] larg = new byte[] { bytes[18], bytes[19], bytes[20], bytes[21] };
            this.largeur = Convertir_Endian_To_Int(larg);

            byte[] haut = new byte[] { bytes[22], bytes[23], bytes[24], bytes[25] };
            this.hauteur = Convertir_Endian_To_Int(haut);

            byte[] bitsparcoul = new byte[] { bytes[28], bytes[29] };
            this.bitsParCouleur = Convertir_Endian_To_Int(bitsparcoul);

            //On rempli la matrice de pixel avec les informations :
            //    - hauteur et largeur pour les dimensions
            //    - 3 bytes consécutifs représentent 1 pixels
            
            this.pixels = new Pixel [this.hauteur, this.largeur];

            // i percours les bytes de la 54ème position jusqu'au dernier, soit toute l'image

            int index = 54;
            for(int i = 0; i < this.pixels.GetLength(0); i++)
            {
                // j allant de 54 jusqu'à la fin de la largeur
                for(int j = 0; j < this.pixels.GetLength(1); j++)
                {
                    this.pixels[i, j] = new Pixel(bytes[index], bytes[index + 1], bytes[index + 2]);
                    index += 3;
                }
            }
        }

        /// <summary>
        /// Propriété en lecture et écriture de file_size
        /// </summary>
        public int File_Size
        {
            get { return this.file_size; }
            set { this.file_size = value;}
        }

        /// <summary>
        /// Propriété en lecture et écriture de la largeur 
        /// </summary>
        public int Largeur
        {
            get { return this.largeur; }
            set { this.largeur = value;}
        }

        /// <summary>
        /// Propriété en lecture et écriture de la hauteur
        /// </summary>
        public int Hauteur
        {
            get { return this.hauteur; }
            set { this.hauteur = value;}
        }

        /// <summary>
        /// Propriété en écriture et en lecture de la matrice de pixel
        /// </summary>
        public Pixel[,] MatPix
        {
            get { return this.pixels; }
            set { this.pixels = value; }
        } 

        /// <summary>
        /// Propriété en lecture et écriture de BitsParCouleur
        /// </summary>
        public int BitsParCouleur
        {
            get { return this.bitsParCouleur; }
            set { this.bitsParCouleur = value; }
        }

        /// <summary>
        /// Affiche la taille, la largeur et la hauteur dans une console.
        /// </summary>
        public void ToString()
        {
            Console.WriteLine($" taille : {this.file_size}\n largeur : {this.largeur}\n hauteur : {this.hauteur}\n Bits par couleur : {this.bitsParCouleur}");
        }

        /// <summary>
        /// prend une instance de MyImage et la transforme en fichier binaire respectant la structure du fichier.bmp
        /// </summary>
        /// <param name="file"></param>
        public void From_image_to_file(string file)
        {
            List <byte> byts_list = new List<byte> { };
            byte[] byts_tab = null;

            //Header

            //Type
            byts_list.Add(66);
            byts_list.Add(77);

            //Size
            byte[] size = this.Convertir_Int_To_Endian(this.file_size, 4);
            for(int i = 0; i < size.Length; i++) { byts_list.Add(size[i]); }

            //champ réservé
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //taille de l'offset
            byts_list.Add(54);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Headder Info

            //Taille du Header
            byts_list.Add(40);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Largeur
            byte[] larg = this.Convertir_Int_To_Endian(this.largeur, 4);
            for (int i = 0; i < larg.Length; i++) { byts_list.Add(larg[i]); }

            //Hauteur
            byte[] haut = this.Convertir_Int_To_Endian(this.hauteur, 4);
            for (int i = 0; i < haut.Length; i++) { byts_list.Add(haut[i]); }

            //Nombre de plan par image
            byts_list.Add(1);
            byts_list.Add(0);

            //Nombre de bits par pixel
            byts_list.Add((byte)this.bitsParCouleur);
            byts_list.Add(0);

            //Compression
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Taille de l'image incluant le remplissage
            for (int i = 0; i < size.Length; i++) 
            {
                if (i == 0) byts_list.Add((byte)(size[i] - 54));
                else byts_list.Add(size[i]);

                //byts_list.Add((byte)( i == 0 ? (size[i] - 54) : (size[i])));
            }

            //Résolution horizontale
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            
            //Résolution verticale
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            
            //Nombre de coueur de la palette
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Nombre de couleurs importantes de la palette
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);
            byts_list.Add(0);

            //Image

            //Parcours la matrice de pixel de image
            //ajoute les bites correspondants dans la liste
            for(int i = 0; i < this.hauteur; i++)
            {
                for(int j = 0; j < this.largeur; j++)
                {
                    byts_list.Add(this.pixels[i,j].Blue);
                    byts_list.Add(this.pixels[i,j].Green);
                    byts_list.Add(this.pixels[i,j].Red);
                }
            }

            byts_tab = new byte[byts_list.Count];
            for (int i = 0; i < byts_list.Count; i++)
            {
                byts_tab[i] = byts_list[i];
            }

            File.WriteAllBytes(file, byts_tab);
            
        }

        /// <summary>
        /// Retourne un int. Converti un little endian en décimale
        /// Parours le tableau dans le sens inverse, car little endian
        /// Multiplie sa valeur par la puissance de 256 correspondante, en fonction de la position de l'élément dans le tableau
        /// Retourn la somme de toutes ces valeurs
        /// </summary>
        /// <param name="tab"></param>
        /// <returns></returns>
        public int Convertir_Endian_To_Int(byte[] tab)
        {
            int result = 0;

            for (int i = 0; i < tab.Length; i++)
                result += tab[i] * (int)Math.Pow(256, i);

            return result;
        }

        /// <summary>
        /// Retourne un tableau de byte. Converti un int en little endian
        /// </summary>
        /// <param name="value"></param>
        /// <param name="size_endian"></param>
        /// <returns></returns>
        public byte[] Convertir_Int_To_Endian(int value, int size_endian)
        {
            byte[] result = new byte[size_endian];

            result = BitConverter.GetBytes(value);

            if(!BitConverter.IsLittleEndian)
            {
                Array.Reverse(result);
            }

            return result;
        }

        /// <summary>
        /// Transforme l'instance de l'image en nuance de Gris
        /// Utilise la méthode Grey() de la classe pixel
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public void Nuance_De_Gris()
        {
            for(int i = 0; i < MatPix.GetLength(0); i++)
            {
                for(int j = 0; j < MatPix.GetLength(1); j++)
                {
                    MatPix[i, j] = MatPix[i, j].Grey();
                }
            }
        }

        /// <summary>
        /// Retourne une image agrandie de l'instance par un coef
        /// </summary>
        /// <param name="coef"></param>
        /// <returns></returns>
        public MyImage Agrandissement(int coef)
        {
            Pixel[,] pix_ag = new Pixel[this.hauteur * coef, this.largeur * coef];

            for (int i = 0; i < this.hauteur; i++)
            {
                for (int j = 0; j < this.largeur; j++)
                {                   
                    for(int k = 0; k < coef; k++)
                    {
                        for(int l = 0; l < coef; l++)
                        {
                            pix_ag[i * coef + k,j * coef + l] = this.pixels[i,j];
                        }                      
                    }
                    
                }
            }
            MyImage res_ag = new MyImage(pix_ag);
            return res_ag;
        }

        /// <summary>
        /// Retourne une image zommée au centre par un certain coef
        /// Utilise la méthode Agrandissement
        /// </summary>
        /// <param name="coef"></param>
        /// <returns></returns>
        public MyImage Zoom(int coef)
        {
            MyImage Image_ag = this.Agrandissement(coef);

            Pixel[,] respix = new Pixel[this.hauteur, this.largeur]; 

            int H_dep = (Image_ag.MatPix.GetLength(0) - respix.GetLength(0)) / 2;
            int L_dep = (Image_ag.MatPix.GetLength(1) - respix.GetLength(1)) / 2;

            for(int i = 0; i < respix.GetLength(0); i++)
            {   
                for(int j = 0; j< respix.GetLength(1); j++)
                {
                    respix[i , j] = Image_ag.MatPix[H_dep + i,L_dep + j];
                }
            }
            MyImage res = new MyImage(respix);
            return res;
        }

        /// <summary>
        /// Retourne une image avec un certain angle de rotation
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public MyImage RotationS(double angle)
        {
            int centrex = this.largeur / 2;
            int centrey = this.hauteur / 2;

            //double norme = Math.Sqrt(Math.Pow(centrex, 2) + Math.Pow(centrey, 2));
            //int normefinale = (int)(Math.Round((norme * 2) + 1));
            //Pixel[,] Imageretournee = new Pixel[normefinale, normefinale];
            double cosangle = Math.Cos(((double)angle) / 180 * Math.PI);
            double sinangle = Math.Sin(((double)angle) / 180 * Math.PI);

            int largeurfinale = (int)Math.Round(largeur * cosangle + hauteur * sinangle);
            int hauteurfinale = (int)Math.Round(largeur * sinangle + hauteur * cosangle);

            Pixel[,] Imageretournee = new Pixel[hauteurfinale, largeurfinale];

            for(int a = 0; a < Imageretournee.GetLength(0); a++)
            {
                for(int b = 0;  b < Imageretournee.GetLength(1); b++) Imageretournee[a, b] = new Pixel(0, 0, 0);
            }

            MyImage res = new MyImage(Imageretournee);

            //matrice de passage
            for (int i = 0; i < Imageretournee.GetLength(0); i++)
            {
                for (int j = 0; j < Imageretournee.GetLength(1); j++)
                {
                    int x = j - centrex;
                    int y = i - centrey;
                    
                    int nouveauX = (int)(x * cosangle - y * sinangle);
                    int nouveauY = (int)(x * sinangle + y * cosangle);

                    int pos_i = (hauteurfinale - hauteur) / 2 + nouveauX;
                    int pos_j = (largeurfinale - largeur) / 2 + nouveauY;

                    if (pos_i >= 0 
                        && pos_i < this.hauteur
                        && pos_j >= 0
                        && pos_j < this.largeur) 
                        Imageretournee[i, j] = this.pixels[pos_i, pos_j];

                }
            }

            return res;
        }

        /// <summary>
        /// Tourne l'instance de l'image avec un angle quelconque
        /// </summary>
        /// <param name="angle"></param>
        public void Rotation(float angle)
        {
            int centrex = largeur / 2;
            int centrey = hauteur / 2;

            //double norme = Math.Sqrt(Math.Pow(centrex, 2) + Math.Pow(centrey, 2));
            //int normefinale = (int)(Math.Round((norme * 2) + 1));
            //Pixel[,] Imageretournee = new Pixel[normefinale, normefinale];

            double cosangle = Math.Cos(((float)angle) / 180 * Math.PI);
            double sinangle = Math.Sin(((float)angle) / 180 * Math.PI);

            int largeurfinale = (int)Math.Round(Math.Abs(largeur * cosangle) + Math.Abs(hauteur * sinangle));
            int hauteurfinale = (int)Math.Round(Math.Abs(largeur * cosangle) + Math.Abs(hauteur * sinangle));

            while (largeurfinale % 4 != 0) largeurfinale++;
            while (hauteurfinale % 4 != 0) hauteurfinale++;

            int centrex_f = largeurfinale / 2;
            int centrey_f = hauteurfinale / 2;

            Pixel[,] Imageretournee = new Pixel[hauteurfinale, largeurfinale];

            for (int i = 0; i < Imageretournee.GetLength(0); i++)
            {
                for (int j = 0; j < Imageretournee.GetLength(1); j++)
                {
                    int y = i - centrey_f;
                    int x = j - centrex_f;

                    int nouveauX = (int)(x * cosangle - y * sinangle) + centrex;
                    int nouveauY = (int)(x * sinangle + y * cosangle) + centrey;

                    //int pos_i = (hauteurfinale - hauteur) / 2 + nouveauY;
                    //int pos_j = (largeurfinale - largeur) / 2 + nouveauX;

                    int pos_i = nouveauY;
                    int pos_j = nouveauX;

                    if (pos_j >= 0 && pos_j < largeur && pos_i >= 0 && pos_i < hauteur) Imageretournee[i, j] = MatPix[pos_i, pos_j];
                    else Imageretournee[i, j] = new Pixel(0, 0, 0);
                }
            }
            this.largeur = largeurfinale;
            this.hauteur = hauteurfinale;
            this.file_size = hauteurfinale * largeurfinale * this.bitsParCouleur;
            this.pixels = Imageretournee;
        }

        /// <summary>
        /// Retourne une liste de pixel
        /// Effectue la multiplication du voisinage d'un pixel et d'un noyeau (stocke le tout sous forme de liste) 
        /// </summary>
        /// <param name="noyeau"></param>
        /// <returns></returns>
        public List <Pixel> Pixelvoisin_X_noyeau(double[,] noyeau, int pos_H, int pos_L)
        {
            List<Pixel> pixelvoisin = new List<Pixel>();

            for (int x = - noyeau.GetLength(0) / 2; x <= noyeau.GetLength(0) / 2; x++)
            {
                for (int y = - noyeau.GetLength(1) / 2; y <= noyeau.GetLength(1) / 2; y++)
                    pixelvoisin.Add(this.pixels[pos_H + x, pos_L + y].Mult(noyeau[(int)(noyeau.GetLength(0) / 2 + x), (int)(noyeau.GetLength(1) / 2 + y)]));
            }
            return pixelvoisin;
        }

        /// <summary>
        /// Retourne une image avec détection des bords
        /// </summary>
        /// <param name="noyeau"></param>
        /// <returns></returns>
        public MyImage Convolution(double[,] noyeau)
        {
            Pixel[,] pixel_res = new Pixel [Hauteur,Largeur]; 

            for(int i = 0; i < Hauteur; i++)
            {
                for (int j = 0; j < Largeur; j++)
                    pixel_res[i, j] = new Pixel(0, 0, 0);
            }

            //Borne sans les bords
            for (int i = noyeau.GetLength(0) / 2; i < Hauteur - (noyeau.GetLength(0) / 2); i++)
            {
                for (int j = noyeau.GetLength(1) / 2; j < Largeur - (noyeau.GetLength(1) / 2); j++)
                {
                    double red = 0, green = 0, blue = 0;

                    //opération de convolution 
                    for (int x = - noyeau.GetLength(0) / 2; x <= noyeau.GetLength(0) / 2; x++)
                    {
                        for (int y = - noyeau.GetLength(1) / 2; y <= noyeau.GetLength(1) / 2; y++)
                        {
                            red += this.MatPix[i + x, j + y].Red * noyeau[(int)(noyeau.GetLength(0) / 2 + x), (int)(noyeau.GetLength(1) / 2 + y)];
                            green += this.MatPix[i + x, j + y].Green * noyeau[(int)(noyeau.GetLength(0) / 2 + x), (int)(noyeau.GetLength(1) / 2 + y)];
                            blue += this.MatPix[i + x, j + y].Blue * noyeau[(int)(noyeau.GetLength(0) / 2 + x), (int)(noyeau.GetLength(1) / 2 + y)];
                        }
                    }
                    
                    //limite une couleur entre 0 et 255
                    red = Math.Min(Math.Max(red, 0), 255);
                    green = Math.Min(Math.Max(green, 0), 255);
                    blue = Math.Min(Math.Max(blue, 0), 255);

                    pixel_res[i, j] = new Pixel((byte)blue, (byte)green, (byte)red);
                }
            }

            MyImage res = new MyImage(pixel_res);

            return res;
        }

        /// <summary>
        /// Retourne une image filtrée
        /// </summary>
        /// <param name="type_de_floutage"> 1 : flou uniforme</param 1 >
        /// <returns></returns>
        public MyImage Filtre(int type_de_filtre)
        {
            Pixel[,] pixel_res = this.pixels;

            switch (type_de_filtre)
            {
                //Flou uniforme
                case 1:
                {
                    //noyau
                    double[,] noyeau = new double[3, 3];
                    for (int i = 0; i < noyeau.GetLength(0); i++)
                    {
                        for (int j = 0; j < noyeau.GetLength(1); j++)
                        {
                            noyeau[i, j] = 1.0 / 9.0;
                        }
                    }

                    //Obtention d'image
                    for (int i = 1; i < Hauteur - 1; i++)
                    {
                        for (int j = 1; j < Largeur - 1; j++)
                        {
                            pixel_res[i,j] = this.pixels[i, j].SommePix(Pixelvoisin_X_noyeau(noyeau, i, j));
                        }
                    }
                    break;
                }
                    
                //Flou de Gauss
                case 2:
                {
                    //noyeau 
                    double[,] noyeau = new double[3, 3];
                    noyeau[0, 0] = 1.0 / 16.0;
                    noyeau[0, 1] = 2.0 / 16.0;
                    noyeau[0, 2] = 1.0 / 16.0;
                    noyeau[1, 0] = 2.0 / 16.0;
                    noyeau[1, 1] = 4.0 / 16.0;
                    noyeau[1, 2] = 2.0 / 16.0;
                    noyeau[2, 0] = 1.0 / 16.0;
                    noyeau[2, 1] = 2.0 / 16.0;
                    noyeau[2, 2] = 1.0 / 16.0;

                    //Obtention d'image
                    for (int i = 1; i < Hauteur - 1; i++)
                    {
                        for (int j = 1; j < Largeur - 1; j++)
                        {
                                pixel_res[i,j] = this.pixels[i, j].SommePix(Pixelvoisin_X_noyeau(noyeau, i, j));
                        }
                    }

                    break;
                }

                //Détection des contour
                case 3:
                {                        
                    //noyeau horizontale
                    double[,] noyeau_h = { {-1.0, 0.0, 1.0},
                                           {-2.0, 0.0, 2.0},
                                           {-1.0, 0.0, 1.0} };

                    //noyeau verticale
                    double[,] noyeau_v = { {-1.0, -2.0, -1.0 },
                                           {0.0, 0.0, 0.0 },
                                           {1.0, 2.0, 1.0 } };
                    
                    //Images intermédiares
                    MyImage Avec_noyeau_h = this.Convolution(noyeau_h);
                    MyImage Avec_noyeau_v = this.Convolution(noyeau_v);

                    //Remplissage pixel_res
                    for(int i = 0; i < Hauteur; i++)
                    {
                        for(int j = 0; j < Largeur; j++)
                        {
                            pixel_res[i, j] = new Pixel(0, 0, 0);
                            pixel_res[i,j].Sum(Avec_noyeau_h.MatPix[i,j], Avec_noyeau_v.MatPix[i,j]);
                            //pixel_res[i, j] = pixel_res[i, j].Grey();
                        }
                    }
            
                    break;
                }

                //Renforcement de bord
                case 4:
                {
                    //noyeau 
                    double[,] noyeau = { { 0.0, 1.0, 0.0},
                                         {1.0, -4.0, 1.0 }, 
                                         { 0.0, 1.0, 0.0 } };

                    //Remplissage pixel_res
                    pixel_res = this.Convolution(noyeau).MatPix;
                    break;
                }
                
                case 5:
                {
                    //noyeau 
                    double[,] noyeau = { { -2.0, -1.0, 0.0 },
                                            { -1.0, 1.0, 1.0},
                                            { 0.0, 1.0, 2.0 } };

                    //remplissage pixel_res
                    pixel_res = this.Convolution(noyeau).MatPix;
                    break;
                }
            }
            MyImage ImageFiltre = new MyImage(pixel_res);
            return ImageFiltre;
        }

        /// <summary>
        /// Créer une nouvelle image redimensionnées avec haut et larg pris en paramètres
        /// </summary>
        /// <param name="haut"></param>
        /// <param name="larg"></param>
        public MyImage Redim(int haut, int larg)
        {
            Pixel[,] interm = new Pixel[haut, larg];

            for (int i = 0; i < haut; i++)
            {
                for(int j = 0; j < larg; j++)
                    interm[i,j] = new Pixel(0,0,0);
            }


            //coordonées du centre de l'instense
            int centre_h = this.hauteur / 2;
            int centre_l = this.largeur / 2;

            for(int i = 0; i < haut; i++)
            {
                for(int j = 0; j < larg; j++)
                {
                    //ok pour encoder une image dans une plus petite
                    int pos_i = centre_h - haut / 2 + i;
                    int pos_j = centre_l - larg / 2 + j;

                    if(pos_i >= 0 && pos_j >= 0 && pos_i < this.hauteur && pos_j < this.largeur) interm[i, j] = this.pixels[pos_i, pos_j];

                }
            }

            MyImage res = new MyImage(interm);
            return res;

        }

        /// <summary>
        /// Retourne une image encodée avec cette instance et une image prise en paramètre
        /// </summary>
        /// <param name="image1"></param>
        public MyImage Encodage(MyImage encod1)
        {
            //dimensionne encod avec les dimensions de l'instance

            int h = Math.Max(encod1.Hauteur, this.Hauteur);
            int w = Math.Max(encod1.Largeur, this.Largeur);

            MyImage resized_encod = encod1.Redim(h, w);
            MyImage resized_image = this.Redim(h, w);

            Pixel[,] pixel_res = new Pixel[h, w];

            for (int i = 0; i < h; i++)
            {
                for(int j = 0; j < w; j++)
                {
                    Pixel value = resized_image.MatPix[i, j];
                    Pixel value2 = resized_encod.MatPix[i, j];

                    string blue_value = value.Hexa()[0];
                    string blue_value2 = value2.Hexa()[0];

                    string blue_res_string = blue_value + blue_value2;
                    byte blue_res = Convert.ToByte(blue_res_string, 2);

                    string green_value = value.Hexa()[1];
                    string green_value2 = value2.Hexa()[1];

                    string green_res_string = green_value + green_value2;
                    byte green_res = Convert.ToByte(green_res_string, 2);

                    string red_value = value.Hexa()[2];
                    string red_value2 = value2.Hexa()[2];

                    string red_res_string = red_value + red_value2;
                    byte red_res = Convert.ToByte(red_res_string, 2);

                    pixel_res[i,j] = new Pixel(blue_res, green_res, red_res);
                }
            }

            MyImage res = new MyImage(pixel_res);

            return res;
        }

        /// <summary>
        /// retourne un tableau de 2 images encodée
        /// </summary>
        /// <param name="encodee"></param>
        /// <returns></returns>
        public MyImage[] Decodage()
        {
            MyImage[] res = new MyImage[2];

            Pixel[,] pix_retouvee_1 = new Pixel[this.hauteur, this.largeur]; 
            Pixel[,] pix_retouvee_2 = new Pixel[this.hauteur, this.largeur];        

            for(int i = 0; i < this.hauteur; i++)
            {
                for(int j = 0; j < this.largeur; j++)
                {
                    Pixel current_pix = this.pixels[i, j];

                    string B1 = current_pix.Hexa2()[0, 0];
                    string B2 = current_pix.Hexa2()[0, 1];
                    byte Blue1_res = Convert.ToByte(B1, 2);
                    byte Blue2_res = Convert.ToByte(B2, 2);

                    string G1 = current_pix.Hexa2()[1, 0];
                    string G2 = current_pix.Hexa2()[1, 1];
                    byte Green1_res = Convert.ToByte(G1, 2);
                    byte Green2_res = Convert.ToByte(G2, 2);

                    string R1 = current_pix.Hexa2()[2, 0];
                    string R2 = current_pix.Hexa2()[2, 1];
                    byte Red1_res = Convert.ToByte(R1, 2);
                    byte Red2_res = Convert.ToByte(R2, 2);

                    pix_retouvee_1[i, j] = new Pixel(Blue1_res, Green1_res, Red1_res);
                    pix_retouvee_2[i, j] = new Pixel(Blue2_res, Green2_res, Red2_res);
                }
            }

            MyImage retouvee1 = new MyImage(pix_retouvee_1);
            MyImage retouvee2 = new MyImage(pix_retouvee_2);

            res[0] = retouvee1;
            res[1] = retouvee2;

            return res;
        } 

        /// <summary>
        /// Retourne une image de fractale de Mandelbrot
        /// Avec la possibilité de faire un certain zoon à un certain point de l'image
        /// Il y puissance élément de fractale  
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="zoom_x"></param>
        /// <param name="zoom_y"></param>
        /// <param name="puissance"></param>
        /// <returns></returns>
        public static MyImage Fractale(double zoom, double zoom_x, double zoom_y, int puissance)
        {
            int width = 500;
            double centre_l = width / 2;

            int height = 500;
            double centre_h = height / 2;

            Pixel[,] pix_frac = new Pixel [height, width];

            int itter_max = 100;

            for (int i = 0;  i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    double pos_i = ((i - centre_h) / (zoom * centre_h) + zoom_x);
                    double pos_j = ((j - centre_l) / (centre_l * zoom) + zoom_y);
                    Complex c = new Complex(pos_j, pos_i);
                    Complex z = new Complex(0, 0);
                    int itter = 0; 

                    //tant que le nombre d'ittération max n'est pas atteint, in incrémente la suite z 
                    //de même, tant que le module de z est inférieur à 2, on incrémente z.

                    while(z.Magnitude <= 2 && itter < itter_max)
                    {
                        z = Complex.Pow(z * z, puissance) + c;
                        itter++;
                    }

                    if(itter  == itter_max) pix_frac[i,j] = new Pixel (0,0,0);

                    //0 au vert pour mettre en violet 
                    //else pix_frac[i,j] = new Pixel ( (byte)(itter * 255 /  itter_max), 0, (byte)(itter * 255 / itter_max));
                    else pix_frac[i,j] = new Pixel ( (byte)((10 * itter) % 256), (byte)((8 * itter) % 256), (byte)((5 * itter) % 256));
                }
            }

            MyImage frac = new MyImage(pix_frac);
            return frac;
        }

        /// <summary>
        /// Retourne une image de fractale avec 2000 ittération et un rayon d'échappement < 50
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="zoom_x"></param>
        /// <param name="zoom_y"></param>
        /// <param name="puissance"></param>
        /// <returns></returns>
        public static MyImage Fractale_1(double zoom, double zoom_x, double zoom_y, int puissance)
        {
            int width = 500;
            double centre_l = width / 2;

            int height = 500;
            double centre_h = height / 2;

            Pixel[,] pix_frac = new Pixel[height, width];

            int itter_max = 2000;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Complex c = new Complex(((i - centre_h) / (zoom * centre_h) + zoom_x), ((j - centre_l) / (centre_l * zoom) + zoom_y));
                    Complex z = new Complex(0, 0);
                    int itter = 0;

                    //tant que le nombre d'ittération max n'est pas atteint, in incrémente la suite z 
                    //de même, tant que le module de z est inférieur à 50, on incrémente z.

                    while (z.Magnitude <= 50 && itter < itter_max)
                    {
                        z = Complex.Cos(z / c);
                        itter++;
                    }

                    if (itter == itter_max) pix_frac[i, j] = new Pixel(0, 0, 0);
                    else pix_frac[i, j] = new Pixel((byte)((10 * itter) % 256), (byte)((8 * itter) % 256), (byte)((3 * itter) % 256));
                }
            }

            MyImage frac = new MyImage(pix_frac);
            return frac;
        }

        /// <summary>
        /// retourne une image de fractale avec 300 iteerations max et la le rayon d'échappement inférieur à 200
        /// f(z, c) = sin(z) + 1 / c²
        /// </summary>
        /// <param name="zoom"></param>
        /// <param name="zoom_x"></param>
        /// <param name="zoom_y"></param>
        /// <param name="puissance"></param>
        /// <returns></returns>
        public static MyImage Fractale_2(double zoom, double zoom_x, double zoom_y, int puissance)
        {
            int width = 500;
            double centre_l = width / 2;

            int height = 500;
            double centre_h = height / 2;

            Pixel[,] pix_frac = new Pixel[height, width];

            int itter_max = 300;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Complex c = new Complex(((i - centre_h) / (zoom * centre_h) + zoom_x), ((j - centre_l) / (centre_l * zoom) + zoom_y));
                    Complex z = new Complex(1, 0.1);
                    int itter = 0;

                    //tant que le nombre d'ittération max n'est pas atteint, in incrémente la suite z 
                    //de même, tant que le module de z est inférieur à 200, on incrémente z.

                    while (z.Magnitude <= 200 && itter < itter_max)
                    {
                        z = Complex.Pow(Complex.Sinh(z) + 1 / (Complex.Pow(c, 2)), puissance);
                        itter++;
                    }

                    if (itter == itter_max) pix_frac[i, j] = new Pixel(0, 0, 0);

                    //0 au vert pour mettre en violet 
                    //else pix_frac[i,j] = new Pixel ( (byte)(itter * 255 /  itter_max), 0, (byte)(itter * 255 / itter_max));
                    else pix_frac[i, j] = new Pixel((byte)((1 * itter) % 256), (byte)((2 * itter) % 256), (byte)((30 * itter) % 256));
                }
            }

            MyImage frac = new MyImage(pix_frac);
            return frac;
        }

        /// <summary>
        /// Réduit le bruit d'une image (un pixel devient la moyenne de ses pixels voisins)
        /// </summary>
        public void Bruit()
        {
            Pixel[,] res = new Pixel[this.hauteur, this.largeur];
            for (int i = 0; i < this.hauteur ; i++)
            {
                for (int j =0; j < this.largeur ; j++) res[i, j] = new Pixel(0, 0, 0);
            }

            for (int i = 1; i < this.hauteur - 1; i++)
            {
                for(int j = 1; j < this.largeur - 1; j++)
                {
                    Pixel sum = new Pixel(0, 0, 0);
                    int b = 0;
                    int g = 0;
                    int r = 0;
                    for(int k = - 1; k <= 1; k++)
                    {
                        for(int l = - 1; l <= 1; l++)
                        {
                            int x = i + k;
                            int y = j + l;
                            b += MatPix[x, y].Blue;
                            g += MatPix[x, y].Green;
                            r += MatPix[x, y].Red;
                        }
                    }
                    sum.Blue = (byte)(b / 9);
                    sum.Green = (byte)(g / 9);
                    sum.Red = (byte)(r / 9);

                    res[i,j] = sum;
                }
            }
            this.pixels = res;
        }

    }
}
