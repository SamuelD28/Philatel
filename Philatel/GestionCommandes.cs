using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Philatel
{
	public class GestionCommandes
	{
		//Propriétés publics
		public bool AucuneAnnulables => Annulables.Count == 0;
		public bool AucuneRétablissantes => Rétablissantes.Count == 0;

		//Propriété privés
		public Stack<ICommande> Annulables { get; private set; }
		public Stack<ICommande> Rétablissantes { get; private set; }
		private static GestionCommandes Instance = null;

		private GestionCommandes(){
			Annulables = new Stack<ICommande>();
			Rétablissantes = new Stack<ICommande>();
		}

		public static GestionCommandes GetInstance()
		{
			if (Instance is null)
				Instance = new GestionCommandes();

			return Instance;
		}

		public bool InitialiserAnnulables(Stack<ICommande> annulables)
		{
			if (AucuneAnnulables)
			{
				Annulables = annulables;
				return true;
			}

			return false;
		}

		public ICommande RetirerCommandeRétablissante() => Rétablissantes.Pop();

		public ICommande RetirerCommandeAnnulable() => Annulables.Pop();

		public void PousserCommandeRétablissante(ICommande commande) => Rétablissantes.Push(commande);

		public void ViderCommandeRétablissante() => Rétablissantes.Clear();

		public void PousserCommandeAnnulable(ICommande commande) => Annulables.Push(commande);
	}
}
