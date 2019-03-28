using System;
using System.Collections;
using System.Collections.Generic;
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

		private LesFabriques(){}

        private Dictionary<Type, IFabriqueCommande> m_fabriques = new Dictionary<Type, IFabriqueCommande>();

        public void Ajouter(Type p_typeArticle, IFabriqueCommande p_fabrique) => m_fabriques.Add(p_typeArticle, p_fabrique);

        public IFabriqueCommande FabriqueDe(Type p_type) => m_fabriques[p_type];

		public IEnumerator<IFabriqueCommande> GetEnumerator() => m_fabriques.Values.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); //Pourquoi implementer sa?

		private class LesFrabriquesEnumerator : IEnumerator<IFabriqueCommande>
		{
			private Dictionary<Type, IFabriqueCommande>.ValueCollection m_fabriquesCommandes;
			private int CursorPosition { get; set; }

			public LesFrabriquesEnumerator(Dictionary<Type, IFabriqueCommande>.ValueCollection fabriqueCommandes)
			{
				m_fabriquesCommandes = fabriqueCommandes;
			}

			public IFabriqueCommande Current => throw new NotImplementedException();

			object IEnumerator.Current => throw new NotImplementedException();

			public void Dispose()
			{
				throw new NotImplementedException();
			}

			public bool MoveNext()
			{
				throw new NotImplementedException();
			}

			public void Reset()
			{
				throw new NotImplementedException();
			}
		}
	}

    /// <summary>
    /// Abstract Factory pour des commandes, et un peu plus (la description de la commande pour le menu).
    /// </summary>
    public interface IFabriqueCommande
    {
        string DescriptionPourMenu();

        ICommande CréerCommandeAjouter();
        ICommande CréerCommandeModifier(ArticlePhilatélique p_article);
        ICommande CréerCommandeSupprimer(ArticlePhilatélique p_article);
    }
}