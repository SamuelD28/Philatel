using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Philatel
{
    public class PilePhilatel
    {
        private static Stack<ICommande> pileCommandes = new Stack<ICommande>();
        private static Stack<ICommande> pileDerniereCommandes = new Stack<ICommande>();
        private static ICommande derniereCommande;

        public static void AjouterCommande(ICommande p_commande)
        {
            pileCommandes.Push(p_commande);
        }

        public static void AnnulerDerniereCommande()
        {
            pileDerniereCommandes.Push(pileCommandes.Peek());
            pileCommandes.Pop();
        }

        public static void RetablirDerniereCommande()
        {
            pileCommandes.Push(pileDerniereCommandes.Peek());
            pileDerniereCommandes.Pop();
        }
    }
}
