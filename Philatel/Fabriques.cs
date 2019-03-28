using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Philatel
{
    /// <summary>
    /// Conserve et permet de récupérer des fabriques pour des commandes.
    /// Offre aussi une opération pour ajouter les commandes au menu du programme (ce n'est pas tellement
    /// « fabrique » alors aurait pu être dans un truc séparé, ou alors on pourrait changer le nom de la
    /// classe pour quelque chose de moins précis que LesFabriques !)
    /// </summary>
    class LesFabriques : IEnumerable<IFabriqueCommande>
    {
		public static LesFabriques GetInstance()
		{
			if (Instance is null)
			{
				Instance = new LesFabriques();
			}
			return Instance;
		}

		private static LesFabriques Instance = null;

		private LesFabriques(){
			m_fabriques = new Dictionary<Type, IFabriqueCommande>();
		}

        private Dictionary<Type, IFabriqueCommande> m_fabriques;

        public void Ajouter(Type p_typeArticle, IFabriqueCommande p_fabrique) => m_fabriques.Add(p_typeArticle, p_fabrique);

        public IFabriqueCommande FabriqueDe(Type p_type) => m_fabriques[p_type];

		public IEnumerator<IFabriqueCommande> GetEnumerator() => new LesFrabriquesEnumerator(m_fabriques);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 

		private class LesFrabriquesEnumerator : IEnumerator<IFabriqueCommande>
		{
			private IFabriqueCommande[] FabriqueCommandes { get; }
			private int CursorPosition { get; set; }
			private int Count { get; }

			public LesFrabriquesEnumerator(Dictionary<Type, IFabriqueCommande> fabriqueCommandes)
			{
				CursorPosition = -1;
				Count = fabriqueCommandes.Count;

				FabriqueCommandes = new IFabriqueCommande[Count];
				fabriqueCommandes.Values.CopyTo(FabriqueCommandes, 0);

				Array.Sort(FabriqueCommandes, new ComparateurAlphabétique());
			}

			public IFabriqueCommande Current => FabriqueCommandes[CursorPosition];

			object IEnumerator.Current => FabriqueCommandes[CursorPosition];

			public void Dispose()
			{

			}

			public bool MoveNext()
			{
				CursorPosition++;
				return CursorPosition < Count;
			}

			public void Reset() => CursorPosition = -1;
		}

		private class ComparateurAlphabétique : IComparer<IFabriqueCommande>
		{
			public int Compare(IFabriqueCommande x, IFabriqueCommande y)
			{
				return x.DescriptionDuType.CompareTo(y.DescriptionDuType);
			}
		}
	}

    /// <summary>
    /// Abstract Factory pour des commandes, et un peu plus (la description de la commande pour le menu).
    /// </summary>
    public interface IFabriqueCommande
    {
        string DescriptionDuType { get; }
        ICommande CréerCommandeAjouter();
        ICommande CréerCommandeModifier(ArticlePhilatélique p_article);
        ICommande CréerCommandeSupprimer(ArticlePhilatélique p_article);
    }
}