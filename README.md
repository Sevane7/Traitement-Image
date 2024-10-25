# Projet de Traitement d'Images en C#

## Description du Projet
Ce projet est un programme de traitement d'images développé en C#. Il permet aux utilisateurs de manipuler des images et d'appliquer diverses opérations, notamment :
- Agrandissement et réduction d'image
- Application de filtres
- Rotation d'images
- Stéganographie (encodage et décodage d'images)
- Génération de fractales

Le programme propose une interface utilisateur via un menu permettant d'interagir facilement avec les images.

## Structure du Code
Le programme est structuré autour de deux classes principales : `MyImage` et `Pixel`, chacune jouant un rôle essentiel dans le traitement des images.

### Classe `Pixel`
La classe `Pixel` représente un pixel individuel avec ses trois composantes de couleur (Rouge, Vert, Bleu) codées sur une échelle de 0 à 255. Elle offre des propriétés en lecture et en écriture pour chaque composante, ainsi que des méthodes pour manipuler les pixels de manière flexible.

### Classe `MyImage`
La classe `MyImage` gère les informations d'une image, telles que la taille, la largeur, la hauteur, les bits par couleur, et un tableau de pixels. Elle inclut des méthodes pour :
- Charger une image bitmap
- Manipuler les pixels
- Effectuer des opérations telles que convolution et rotation

#### Principales Méthodes
- **Rotation(angle)** : Fait tourner l'image selon un angle donné.
- **Convolution(noyau)** : Applique une convolution à l'image avec un noyau spécifié, permettant d'appliquer divers filtres.
- **Redim(haut, larg)** : Redimensionne l'image selon des dimensions données.
- **Encodage(encod1)** : Encode une image dans une autre pour la stéganographie.
- **Decodage()** : Décode deux images encodées.
- **Fractale(zoom, zoom_x, zoom_y, puissance)** : Génère une fractale de Mandelbrot en fonction des paramètres de zoom et de puissance.

## Interface Utilisateur
L'interface utilisateur est basée sur un menu texte interactif, qui guide l'utilisateur à travers les différentes opérations disponibles. Le menu propose :
1. Sélection d'une image pour agrandissement.
2. Application d'un filtre (uniforme, Gauss, détection des bords, etc.).
3. Encodage d'une image dans une autre, pour la stéganographie.
4. Décodage d'images encodées.
5. Génération de fractales.

### Utilisation
1. **Lancer le programme** pour afficher un message de bienvenue.
2. **Choisir une image** depuis le dossier `./Images`.
3. **Suivre les étapes** du menu pour appliquer les opérations souhaitées (agrandissement, filtres, encodage, fractales, etc.).

## Exemple d'Exécution
1. **Agrandir une Image** : Saisir le coefficient d'agrandissement pour redimensionner l'image.
2. **Appliquer un Filtre** : Choisir un filtre parmi les options proposées pour appliquer un effet à l'image.
3. **Stéganographie** : Sélectionner une image de support et une image à encoder, puis appliquer la méthode `Encodage`. La méthode `Decodage` permet d'extraire les images encodées.
4. **Fractales** : Générer et stocker trois images fractales avec différents paramètres de zoom et de puissance.

## Conclusion
Ce programme offre une variété d'outils pour le traitement d'images, allant de simples transformations géométriques à la génération de fractales et à la stéganographie. L'interface utilisateur et la structure du code rendent cet outil accessible et facile à utiliser pour réaliser des manipulations d'images diverses.

## Auteurs
Ce projet a été développé en C# dans le cadre d'un projet de traitement d'images.

## Licence
Ce projet est sous licence libre. Utilisation et modifications sont autorisées.
