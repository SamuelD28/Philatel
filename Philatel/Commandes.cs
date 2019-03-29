using System;
using System.Collections.Generic;
using System.Windows.Forms;
using UtilStJ;
using static UtilStJ.MB;

namespace Philatel
{
	/// Interface ICommande :
	/// <summary>
	/// interface de toutes les commandes. La fonction Exécuter renvoie un bool qui permet de
	/// savoir s'il y a vraiment eu opération et donc s'il faut permettre le Annuler (Undo)
	/// </summary>
	public interface ICommande
	{
		bool Exécuter();
		void Annuler();
		void Rétablir();
		// ICommande Cloner();  // Pas utile dans cette version
	}

	[Serializable]
	public class CommandeEffacerTout : ICommande
	{
		Stack<ArticlePhilatélique> m_article;

		public bool Exécuter()
		{
			try
			{
				m_article = new Stack<ArticlePhilatélique>(Document.Instance.TousLesArticles());
				GestionCommandes.GetInstance().PousserCommandeAnnulable(this);
				GestionCommandes.GetInstance().ViderCommandeRétablissante();
				Document.Instance.Vider();
				Document.Instance.Enregistrer();
				return true;
			}
			catch
			{
				return false;
			}
		}

		public void Annuler()
		{
			GestionCommandes.GetInstance().PousserCommandeRétablissante(this);
			Document.Instance.Remplir(m_article);
		}

		public void Rétablir()
		{
			Exécuter();
		}
	}

    [Serializable]
    public abstract class CommandeAjoutInitialisé : ICommande
    {
        public void Annuler()
        {
            throw new NotImplementedException();
        }

        public bool Exécuter()
        {
            throw new NotImplementedException();
        }

        public void Rétablir()
        {
            throw new NotImplementedException();
        }
    }

    /// Classe abstraite CommandeAjout : 
    /// <summary>
    /// permet l'ajout d'un article (abstraite car on doit définir CréerDlgSaisie).
    /// </summary>
    [Serializable]
	public abstract class CommandeAjout : ICommande
	{
		ArticlePhilatélique m_article;

		public bool Exécuter() 
		{
			DlgSaisieArticle d = CréerDlgSaisie();

			if (d.ShowDialog() == DialogResult.Cancel)
				return false;

			m_article = d.Extraire();
			GestionCommandes.GetInstance().PousserCommandeAnnulable(this);
			GestionCommandes.GetInstance().ViderCommandeRétablissante();
			Document.Instance.Ajouter(m_article);
			Document.Instance.Enregistrer();
			return true;
		}

		public void Annuler()
		{
			Document.Instance.RetirerArticle(m_article.Numéro);
			GestionCommandes.GetInstance().PousserCommandeRétablissante(this);
		}
		
		public void Rétablir() => Document.Instance.Ajouter(m_article);

		public abstract DlgSaisieArticle CréerDlgSaisie(); 
	}

	/// Classe abstraite CommandeModification : 
	/// <summary>
	/// permet la modification d'un article (abstraite car on doit définir CréerDlgSaisie).
	/// </summary>
	[Serializable]
	public abstract class CommandeModification : ICommande
	{
		public CommandeModification(ArticlePhilatélique p_articleCourant)
		{
			m_articleOrignal = p_articleCourant;
		}

		ArticlePhilatélique m_articleOrignal;
		ArticlePhilatélique m_articleModifier;

		public bool Exécuter()
		{
			DlgSaisieArticle d = CréerDlgSaisie(m_articleOrignal);

			if (d.ShowDialog() == DialogResult.Cancel)
				return false;

			m_articleModifier = d.Extraire();

			GestionCommandes.GetInstance().PousserCommandeAnnulable(this);
			GestionCommandes.GetInstance().ViderCommandeRétablissante();
			Document.Instance.Modifier(m_articleModifier);
			Document.Instance.Enregistrer();
			return true;
		}

		public void Annuler()
		{
			GestionCommandes.GetInstance().PousserCommandeRétablissante(this);
			Document.Instance.Modifier(m_articleOrignal);
		}

		public abstract DlgSaisieArticle CréerDlgSaisie(ArticlePhilatélique p_article);  

		public void Rétablir()
		{
			Document.Instance.Modifier(m_articleModifier);
		}
	}

	/// Classe CommandeSuppression :
	/// <summary>
	/// permet la suppression d'un article (pas abstraite mais on peut en dériver si on n'aime pas la façon
	/// dont est fait la confirmation dans la fonction ConfirmerSuppression de base).
	/// </summary>
	[Serializable]
	public class CommandeSuppression : ICommande
	{
		public CommandeSuppression(ArticlePhilatélique p_articleCourant)
		{
			m_article = p_articleCourant;
		}

		ArticlePhilatélique m_article;

		public bool Exécuter()
		{
			if (!ConfirmerSuppression(m_article))
				return false;

			GestionCommandes.GetInstance().PousserCommandeAnnulable(this);
			GestionCommandes.GetInstance().ViderCommandeRétablissante();
			Document.Instance.RetirerArticle(m_article.Numéro);
			Document.Instance.Enregistrer();
			return true;
		}

		public void Annuler()
		{
			GestionCommandes.GetInstance().PousserCommandeRétablissante(this);
			Document.Instance.Ajouter(m_article);
		}

		public virtual bool ConfirmerSuppression(ArticlePhilatélique p_article)
		{
			return ConfirmerOuiNon("Êtes-vous sûr de vouloir retirer\n\n" +
								   p_article.ToString() +
								   "\n\n?");
		}

		public void Rétablir()
		{
			Document.Instance.RetirerArticle(m_article.Numéro);
		}
	}
}
