using System.ComponentModel.Design;

namespace ProblemeScientifique
{
    internal class Program
    {
        static void Menu()
        {
            Console.WriteLine("Bonjour,\n" +
                "Bienvenue dans la solution du Problême scientifique de :\n" +
                "AZIZI Salma\nADEL Mehdi\nBASTIAN Sevane.\n\n" +
                "Permettez que nous manipulions quelques images ensemble :)\n\n " +
                "Quelle image voulez vous agrandir ? \n\n");
                
            string name = Console.ReadLine();
            while (name != "lac" && name != "coco" && name != "lac") name = Console.ReadLine();

            MyImage choice = new MyImage($"./Image./{name}.bmp");
            Console.WriteLine("De quel coefficient ? \n");
            int coef = Convert.ToInt32(Console.ReadLine());
            MyImage Ag = choice.Agrandissement(coef);
            Ag.From_image_to_file($"./Image./{name}_Ag{coef}.bmp");
            Console.WriteLine("Si vous permettez, nous avons aussi pris la peine d'innover un peu cette méthode et avons créer une méthode qui zoom dans l'image : \n");
            MyImage Zoom = choice.Zoom(coef);
            Zoom.From_image_to_file($"./Image./{name}_Zoom{coef}.bmp");
            Console.WriteLine("Nous vous prions de bien vouloir aller vérifier dans bin -> debug -> Images\n\n" +
                "Passons aux filtres, quelle image voulez vous filtrer ?\n");
            name = Console.ReadLine();
            while (name != "lac" && name != "coco" && name != "lac") name = Console.ReadLine();
            choice = new MyImage($"./Image./{name}.bmp");
            Console.WriteLine("Notez le filtre que vous vouler appliquer : \n\n" +
                "1- Fou uniforme\n" +
                "2- Flou de Gauss\n" +
                "3- Detection des bords\n" +
                "4- Renforcement des bords\n" +
                "5-Repossage\n" +
                "0 pour passer à la méthode suivante.\n");
            int filtre = Convert.ToInt32(Console.ReadLine());
            while(filtre != 0)
            {
                MyImage Im_Fil = choice.Filtre(filtre);
                Im_Fil.From_image_to_file($"./Image./{name}_Filtre{filtre}.bmp");
                Console.WriteLine("Notez le filtre que vous vouler appliquer : \n\n" +
                "1- Fou uniforme\n" +
                "2- Flou de Gauss\n" +
                "3- Detection des bords\n" +
                "4- Renforcement des bords\n" +
                "5-Repossage\n" +
                "0 pour passer à la méthode suivante.\n");
                filtre = Convert.ToInt32(Console.ReadLine());
            }

            Console.WriteLine("C'est le moment de tourner une image !! \n\n" +
                "Quelle image voulez vous tourner? \n\n");

            name = Console.ReadLine();
            while (name != "lac" && name != "coco" && name != "lac") name = Console.ReadLine();
            choice = new MyImage($"./Image./{name}.bmp");

            Console.WriteLine("De quel degré ? \n ");
            float degre = float.Parse(Console.ReadLine());
            choice.Rotation(degre);
            choice.From_image_to_file($"./Image./{name}_Rot{degre}.bmp");

            Console.WriteLine("Et voila, l'image tournée se trouve dans le dossier Image !\n" +
                "Passons maintenant à la steganographie : \n" +
                "Taper le nom de l'image que vous voulez comme support d'encodage et en deuxieme l'image que vous voulez encoder.\n\n" +
                "Exemple : Si vous voulez encoder coco dans lac, il faut d'abord taper lac puis coco\n");
            name = Console.ReadLine();
            while (name != "lac" && name != "coco" && name != "lac") name = Console.ReadLine();
            choice = new MyImage($"./Image./{name}.bmp");
            string name_2 = Console.ReadLine();
            while (name_2 != "lac" && name_2 != "coco" && name_2 != "lac") name_2 = Console.ReadLine();
            MyImage choice_2 = new MyImage($"./Image./{name_2}.bmp");
            choice = choice.Encodage(choice_2);
            choice.From_image_to_file($"./Image./{name}_Encodé_Par_{name_2}.bmp");
            MyImage [] Decod1 = choice.Decodage();
            Decod1[0].From_image_to_file($"./Image./Decodé_0.bmp");
            Decod1[1].From_image_to_file($"./Image./Decodé_2.bmp");

            Console.WriteLine("Nous suppose que vous voulez les décoder !\n Nous avons pris la liberté de la faire ;) \n\n" +
                "Toutes ces images sons dans le même fichier Images que tout à l'heure.\n\n" +
                "Et enfin, le clou du spectacle, les fractales !! \n " +
                "3 fractales différentes sont en cours de création.\n" +
                "Oui Oui, 3 fractales !!\n" +
                "Elles seront stockées... vous savez où!");

            MyImage Fractale = MyImage.Fractale(1.0, 0.0, -0.5, 1);
            MyImage Fractale_1 = MyImage.Fractale_1(-0.80, 0.0, 0.0, 1);
            MyImage Fractale_2 = MyImage.Fractale_2(-2.20, 0.0, -0.0, 1);
            Fractale.From_image_to_file("./Image/Fractale.bmp");
            Fractale_1.From_image_to_file("./Image/Fractale_1.bmp");
            Fractale_2.From_image_to_file("./Image/Fractale_2.bmp");

            Console.WriteLine("Cette présentation a été conçue dans le but de plaire,\n" +
                "nous espérons que vous avez appréciez nos efforts!\n\n" +
                "Bien cordialement,");
        }
        static void Main(string[] args)
        {
            MyImage coco = new MyImage("./Image/coco.bmp");
            //MyImage lena = new MyImage("./Image/lena.bmp");
            MyImage lac = new MyImage("./Image/lac.bmp");


            //coco.Rotation(30.7f);
            //coco.From_image_to_file("./Image/cocoRot.bmp");

            //MyImage Fractale = MyImage.Fractale(1000.0, 0.06783611264225832, 0.6617460391250546);

            //coco.Bruit();
            //MyImage cocoBord = coco.Filtre(3);

            //coco.From_image_to_file("./Image/cocoBruit.bmp");

            //MyImage cocoZoom = coco.Zoom(2);
            //MyImage cocoAg = coco.Agrandissement(2);

            //cocoAg.From_image_to_file("./Image/cocoAg2.bmp", cocoAg);
            //cocoZoom.From_image_to_file("./Image/cocoZoom2.bmp", cocoZoom);

            //MyImage cocofiltre = coco.Filtre(5);
            //MyImage lacfiltre = lac.Filtre(5);
            //MyImage lenafiltre = lena.Filtre(5);

            //MyImage CocoEncodLena = lena.Encodage(coco);
            //MyImage LenaEncodCoco = coco.Encodage(lena);
            //MyImage LacEncodLena = lena.Encodage(lac);
            //MyImage LenaEncodLac = lac.Encodage(lena);
            //MyImage CocoEncodLac = lac.Encodage(coco);
            //MyImage LacEncodCoco = coco.Encodage(lac);

            //CocoEncodLac.From_image_to_file("./Image/CocoEncodLac.bmp");
            //LenaEncodLac.From_image_to_file("./Image/LenaEncodLac.bmp");
            //CocoEncodLena.From_image_to_file("./Image/CocoEncodLena.bmp");
            //LacEncodLena.From_image_to_file("./Image/LacEncodLena.bmp");
            //LenaEncodCoco.From_image_to_file("./Image/LenaEncodCoco.bmp");
            //LacEncodCoco.From_image_to_file("./Image/LacEncodCoco.bmp");

            //MyImage[] lacDecod = LacEncodCoco.Decodage();
            //lacDecod[0].From_image_to_file("./Image/CocoDencodCoco.bmp");
            //lacDecod[1].From_image_to_file("./Image/CocoDencodLac.bmp");
            //MyImage[] lacDecod2 = LenaEncodLac.Decodage();
            //MyImage[] lenaDecod = LacEncodLena.Decodage();
            //MyImage[] lenaDecod2 = CocoEncodLena.Decodage();
            //MyImage[] CocoDecod = LacEncodCoco.Decodage();
            //MyImage[] CocoDecod2 = LenaEncodCoco.Decodage();

            //MyImage lenaDecodcoco = lenaDecod2[1];
            //MyImage lenaDecodlena = lenaDecod2[0];

            //lenaDecodcoco.From_image_to_file("./Image/lenaDecoCoco.bmp");
            //lenaDecodlena.From_image_to_file("./Image/lenaDecodlena.bmp");
            Menu();
            Console.ReadKey();
        }
    }
}