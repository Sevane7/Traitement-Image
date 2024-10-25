using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    public class Noeud
    {
        private Pixel pix;
        private int freq;
        private Noeud gauche;
        private Noeud droite;

        public Noeud(Pixel pix, int freq)
        {
            this.pix = pix;
            this.freq = freq;
        }
        public Noeud(Noeud gauche, Noeud droite) 
        {
            this.gauche = gauche;
            this.droite = droite;
            this.freq = this.gauche.freq + this.droite.freq;
        }
        public Pixel Pix
        {
            get { return this.pix; }
            set { this.pix = value; }
        }
        public int Freq
        {
            get { return this.Freq; }
            set { this.freq = value; }
        }
        public Noeud Gauche
        {
            get { return this.gauche; }
            set { this.gauche = value; }
        }
        public Noeud Droite
        {
            get { return this.droite; }
            set { this.droite = value; }
        }
    }
}
