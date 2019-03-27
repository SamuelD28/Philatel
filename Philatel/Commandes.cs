using System;
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

    /// Classe abstraite CommandeAjout : 
    /// <summary>
    /// permet l'ajout d'un article (abstraite car on doit définir CréerDlgSaisie).
    /// </summary>
    public abstract class CommandeAjout : ICommande
    {
        int m_numéroArticleAjouté;
		ArticlePhilatélique m_article;

        public bool Exécuter() // Template Method (appelle une Factory Method)
        {
            DlgSaisieArticle d = CréerDlgSaisie();

            if (d.ShowDialog() == DialogResult.Cancel)
                return false;

            m_article = d.Extraire();
            m_numéroArticleAjouté = m_article.Numéro;

            Document.Instance.Ajouter(m_article);
            return true;
        }

        public void Annuler() => Document.Instance.RetirerArticle(m_numéroArticleAjouté);

        public abstract DlgSaisieArticle CréerDlgSaisie();  // Factory Method

		public void Rétablir()
		{
			Document.Instance.Ajouter(m_article);
		}
	}

    /// Classe abstraite CommandeModification : 
    /// <summary>
    /// permet la modification d'un article (abstraite car on doit définir CréerDlgSaisie).
    /// </summary>
    public abstract class CommandeModification : ICommande
    {
        public CommandeModification(ArticlePhilatélique p_articleCourant)
        {
            m_article = p_articleCourant;
        }

        ArticlePhilatélique m_article;

        public bool Exécuter() // Template Method (appelle une Factory Method)
        {
            DlgSaisieArticle d = CréerDlgSaisie(m_article);

            if (d.ShowDialog() == DialogResult.Cancel)
                return false;

            Document.Instance.RetirerArticle(m_article.Numéro);
            Document.Instance.Ajouter(d.Extraire());
            return true;
        }

        public void Annuler()
        {
            Document.Instance.RetirerArticle(m_article.Numéro);
            Document.Instance.Ajouter(m_article);
        }

        public abstract DlgSaisieArticle CréerDlgSaisie(ArticlePhilatélique p_article);  // Factory Method

		public void Rétablir()
		{
			throw new NotImplementedException();
		}
	}

    /// Classe CommandeSuppression :
    /// <summary>
    /// permet la suppression d'un article (pas abstraite mais on peut en dériver si on n'aime pas la façon
    /// dont est fait la confirmation dans la fonction ConfirmerSuppression de base).
    /// </summary>
    public class CommandeSuppression : ICommande
    {
        public CommandeSuppression(ArticlePhilatélique p_articleCourant)
        {
            m_article = p_articleCourant;
        }

        ArticlePhilatélique m_article;

        public bool Exécuter()  // Template Method (ConfirmerSuppression peut être supplantée)
        {
            if (!ConfirmerSuppression(m_article))
                return false;

            Document.Instance.RetirerArticle(m_article.Numéro);
            return true;
        }

        public void Annuler() => Document.Instance.Ajouter(m_article);

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
