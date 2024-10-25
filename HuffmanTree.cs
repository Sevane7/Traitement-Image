using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProblemeScientifique
{
    public class HuffmanTree
    {
        Noeud root;
        private Dictionary <Pixel, int> way_frequency;

        public HuffmanTree(Noeud root, Dictionary<Pixel, int> way_frequency)
        {
            this.root = root;
            this.way_frequency = way_frequency;
        }

        public Noeud Root
        {
            get { return this.root; }
            set { this.root = value; }
        }
        public Dictionary <Pixel, int> Way_Frequency 
        { 
            get { return this.way_frequency; } 
            set { this.way_frequency= value; }
        }



    }
}
