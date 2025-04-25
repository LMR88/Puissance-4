using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exo : MonoBehaviour
{
      private List<int> paquet = new List<int>();
        private int nbCartes = 1000;
    
        private void InitPaquet()
        {
            for (int i = 0; i < nbCartes; i++)
            {
                if (i%2 == 0) paquet.Add(i);
            }
        }
    
        private List<int> PaquetMelange(List<int> paquetToMelange)
        {
            for (int i = 0; i < paquetToMelange.Count; i++)
            {
                int rnd = Random.Range(0, paquetToMelange.Count);
    
                (paquetToMelange[i], paquetToMelange[rnd]) = (paquetToMelange[rnd], paquetToMelange[i]);
            }
            return paquetToMelange;
        }
        private bool IsPaquetTrie(List<int> paquetToCheck)
        {
            for (int i = 0; i < paquetToCheck.Count-1; i++)
            {
                if (paquetToCheck[i] > paquetToCheck[i + 1]) return false;
            }
            return true;
        }
    
        private List<int> PaquetTrie(List<int> paquetToTrie)
        {
            while (!IsPaquetTrie(paquetToTrie))
            {
                for (int i = 0; i < paquetToTrie.Count-1; i++)
                {
                    if (paquetToTrie[i] > paquetToTrie[i + 1])
                    {
                        (paquetToTrie[i], paquetToTrie[i + 1]) = (paquetToTrie[i + 1], paquetToTrie[i]);
                    }
                }
            }
            return paquetToTrie;
        }
        
        private int IsInPaquet(int valeur, List<int> liste, int debut, int fin)
        {
            if (liste == null || liste.Count == 0) return -1;

            int gauche = debut;
            int droite = fin - 1;

            while (gauche <= droite)
            {
                int milieu = (gauche + droite) / 2;

                if (liste[milieu] == valeur)
                {
                    return milieu; 
                }
                else if (liste[milieu] < valeur)
                {
                    gauche = milieu + 1;
                }
                else
                {
                    droite = milieu - 1;
                }
            }
    
            if (droite < 0) return gauche;
            if (gauche >= liste.Count) return droite;

            int diffGauche = Mathf.Abs(liste[gauche] - valeur);
            int diffDroite = Mathf.Abs(liste[droite] - valeur);

            return (diffGauche < diffDroite) ? gauche : droite;
        }

    
        private void Start()
        {
            InitPaquet();
            paquet = PaquetMelange(paquet);
            paquet = PaquetTrie(paquet);
            IsInPaquet(1, paquet, 0, paquet.Count);
        }
}
