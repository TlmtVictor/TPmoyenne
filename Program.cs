using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace HNI_TPmoyennes
{
    class Program
    {
        static void Main(string[] args)
        {
            // Création d'une classe
            Classe sixiemeA = new Classe("6eme A");

            // Ajout des élèves à la classe
            sixiemeA.ajouterEleve("Jean", "RAGE");
            sixiemeA.ajouterEleve("Paul", "HAAR");
            sixiemeA.ajouterEleve("Sibylle", "BOQUET");
            sixiemeA.ajouterEleve("Annie", "CROCHE");
            sixiemeA.ajouterEleve("Alain", "PROVISTE");
            sixiemeA.ajouterEleve("Justin", "TYDERNIER");
            sixiemeA.ajouterEleve("Sacha", "TOUILLE");
            sixiemeA.ajouterEleve("Cesar", "TICHO");
            sixiemeA.ajouterEleve("Guy", "DON");

            // Ajout de matières étudiées par la classe
            sixiemeA.ajouterMatiere("Francais");
            sixiemeA.ajouterMatiere("Anglais");
            sixiemeA.ajouterMatiere("Physique/Chimie");
            sixiemeA.ajouterMatiere("Histoire");
            Random random = new Random();

            // Ajout de 5 notes à chaque élève et dans chaque matière
            for (int ieleve = 0; ieleve < sixiemeA.eleves.Count; ieleve++)
            {
                for (int matiere = 0; matiere < sixiemeA.matieres.Count; matiere++)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        sixiemeA.eleves[ieleve].ajouterNote(new Note(matiere, (float)((6.5 +
                       random.NextDouble() * 34)) / 2.0f));
                        // Note minimale = 3
                    }
                }
            }

            // Pour tout afficher tous les élèves et toutes les notes 
            // Pour aller chercher chaque elève de la classe
            foreach (Eleve e in sixiemeA.eleves)
                {
                    Console.WriteLine(e.Nom + " " + e.Prenom);
                    Console.WriteLine();

                    // Pour chaque matière
                    foreach(string matiere in sixiemeA.matieres)
                    {
                        int index = sixiemeA.matieres.IndexOf(matiere);
                        float moyenne = e.MoyenneMatiereEleve(index);
                        Console.WriteLine("  - Moyenne en " + matiere + " : " + moyenne);
                    }

                    // Moyenne générale de l’élève
                    Console.WriteLine("  --- Moyenne générale : " + e.MoyenneGenerale());
                    Console.WriteLine();
                    Console.WriteLine();

            }

            Console.WriteLine("Résultats classe");
            Console.WriteLine();

            // Pour chaque matière, moyenne de la classe
            foreach (string matiere in sixiemeA.matieres)
            {
                int index = sixiemeA.matieres.IndexOf(matiere);
                float moyenneClasse = sixiemeA.MoyenneMatiereClasse(index);
                Console.WriteLine("--Moyenne de la classe en" + matiere + " : " + moyenneClasse);
                Console.WriteLine();
            }

            // Moyenne générale de la classe
            Console.WriteLine("---Moyenne générale de la classe" + sixiemeA.nomClasse + " : " + sixiemeA.MoyenneGeneraleClasse());
            Console.Read();
        }
    }


    // Création des classes et des méthodes qu'ils manquent pour faire fonctionner le code déjà forunit plus haut 
    public class Eleve
    {
        public string Nom { get; set; }            // Propriété nom
        public string Prenom { get; set; }         // Propriété prénom
        public List<Note> notes;                   // Liste des notes de l'élève
        public Eleve (string prenom, string nom)   // Constructeur : on initialise nom, prénom et la liste des notes vide 
        {
        Nom = nom;
        Prenom = prenom;
        notes = new List<Note>();                  // Liste vide au départ
        }

        public void ajouterNote(Note note)        // Méthode pour ajouter une note à l'élève
        {
            if (notes.Count < 200)
                notes.Add(note);                  // prends la note reçue en paramètre (nommée note),et add dans la liste notes de l’élève
            else
                Console.WriteLine("Limite de notes atteinte pour l'élève " + Prenom + Nom);
        }

        public float MoyenneMatiereEleve(int matiere)   // Méthode calculer la moyenne d'un élève dans une matière
        {
            // notes est la liste de toutes les notes de l'élève
            // where filtre les notes
            // select extraire la note de la liste
            // ToList mets les notes dans une nouvelle liste
            var notesMatiere = notes.Where(n => n.matiere == matiere).Select(n => n.note).ToList();
            // si aucune note on mets un 0
            if (notesMatiere.Count == 0)
                return 0;
            // nom de la liste.average --> calcul la moyenne de la liste
            float moyenne = notesMatiere.Average();
            // Multiplie par 100 pour garder deux chiffres significatifs
            // Math.Truncate() tronque (coupe sans arrondir) à l’entier inférieur, fonctionne sur entier only
            return (float)Math.Truncate(moyenne * 100) / 100;   
        }

        public float MoyenneGenerale()                 // Calculer la moyenne générale de l'élève (moyenne des moyennes par matière)
        {   
            // select la matière
            // Distinct permet d'éliminer les doublons et créer un objet avec composant unique
            var matieresDistinctes = notes.Select(n => n.matiere).Distinct();  // Trouver toutes les matières concernées par les notes

            // création variable pour calcul de la moyenne generale
            float sommeMoyennes = 0;
            int nbMatieres = 0;
            // boucle var matiere qui prends une valeur de matieresDistinctes
            foreach (int matiere in matieresDistinctes)
            {   // ajoute matiere à la somme des moyennes
                sommeMoyennes += MoyenneMatiereEleve(matiere);
                // compter le nombre de notes
                nbMatieres = nbMatieres + 1;
            }

            float MoyenneGenerale = sommeMoyennes / nbMatieres;
            return (float)Math.Truncate(MoyenneGenerale * 100) / 100;
        }

    }

    public class Classe
    {
            public string nomClasse { get; set; }       // Nom de la classe (ex: "6eme A")
            public List<Eleve> eleves;                  // Liste des élèves (class) dans cette classe
            public List<string> matieres;               // Liste des matières (string) enseignées dans cette classe
            public Classe(string nom)                   // Constructeur : appelé quand on fait `new Classe("6eme A")
            {
                nomClasse = nom;                        // On attribue le nom donné
                eleves = new List<Eleve>();             // On crée une liste vide d'élèves
                matieres = new List <string>();         // On crée une liste vide de matières
            }

        public void ajouterEleve(string prenom, string nom)       // Méthode pour ajouter un eleve à la classe 
        {
            if (eleves.Count < 30)
                eleves.Add(new Eleve(prenom, nom));
            else
                Console.WriteLine("Limite d'élèves atteintes");
        }
        
        public void ajouterMatiere(string nouvelleMatiere)        // Méthode pour ajouter une matière à la classe 
        {
            if (matieres.Count < 10)
                matieres.Add(nouvelleMatiere);
            else
                Console.WriteLine("Limite de matières atteinte");
        }
        

        public float MoyenneMatiereClasse(int matiere)    // Méthode pour calculer
        {                                                 // Ici on spécifie matière entre parenthèse car cette méthode a besoin de se paramètre
            // initalisation à zéro des variables que je vais use pour la méthode
            float sommeMoyennes = 0;
            int nbEleves = 0;

            foreach (Eleve eleve in eleves)
            {
                float moyenneEleve = eleve.MoyenneMatiereEleve(matiere);
                sommeMoyennes += moyenneEleve;
                // ajouter +1 dès qu'il y a une somme de faite
                nbEleves++;
            }

            float MoyenneMatiereClasse = sommeMoyennes / nbEleves;
            return (float)Math.Truncate(MoyenneMatiereClasse * 100) / 100;
        }

        public float MoyenneGeneraleClasse()         // Méthode pour calculer moyenne de la classe 
        {                                            // Rien dans les paranthèses car on a déjà les élèments (à éclaircir)
                                                     // Méthode sont
            float sommeMoyennes = 0;
            int nbEleves = 0;

            foreach (Eleve eleve in eleves)
            {
                // Appelle la moyenne générale de chaque élève
                float moyenneEleve = eleve.MoyenneGenerale(); 
                sommeMoyennes += moyenneEleve;
                nbEleves++;
            }

            float MoyenneMatiereClasse = sommeMoyennes / nbEleves;
            return (float)Math.Truncate(MoyenneMatiereClasse * 100) / 100;
        }
    }  
}



