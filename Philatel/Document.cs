using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UtilStJ;
using static UtilStJ.MB;

namespace Philatel
{
	public class Document : DocumentObservable<Document>
	{
		//Propriétés public
		public const string NomFichierPhilatélie = "Philatélie.données";
		public const int NoPremierArticle = 1;
		public static Document Instance => m_docUnique.Value;

		//Propriétés privés : Applique le principe d plus grande restriction possible
		private static Lazy<Document> m_docUnique = new Lazy<Document>(() => new Document());
		private Stack<ICommande> m_commandesAnnulables = new Stack<ICommande>();
		private Stack<ICommande> m_commandesRétablissante = new Stack<ICommande>();
		private Stack<ArticlePhilatélique> m_articles;
		private int m_noProchainArticle;
		private bool m_docModifié = false;

		/// <summary>
		/// Constructeur privée utilisé par la classe singleton
		/// </summary>
		private Document()
		{
			try
			{
				Récupérer();
			}
			catch
			{
				AvertirCritique("La lecture de « {0} » a échoué.\n" +
								"Le programme va s'arrêter.\n",
								NomFichierPhilatélie);

				Environment.Exit(0);  // Permet d'arrêter le programme directement.
			}
		}

		/// <summary>
		/// Termine l'accès aux données et s'assure qu'elles sont bien enregistrées (un message est affichée
		/// si ce n'est pas le cas).
		/// </summary>
		public void Fermer()
		{
			if (m_docModifié)
			{
				try
				{
					Enregistrer();
				}
				catch
				{
					AvertirCritique(
						"***** ERREUR *****\n" +
						"Les données n'ont pas pu être enregistrées dans {0}.",
						NomFichierPhilatélie);
				}
			}
		}

		/// <summary>
		/// Permet de retrouver un type (sous forme System.Type) dans un assemblage qui est
		/// peut-être autre que l'assemblage courant. Le lien est fait simplement par le nom de
		/// l'assemblage et non par sa version (etc.). 
		/// N.B. On dirait que c'est ce que devrait faire par défaut la (dé)sérialisation, mais
		///      non, elle ne prend pas de chances si on ne l'aide pas...
		/// </summary>
		class LierAssemblagesSimplement : SerializationBinder
		{
			/// <summary>
			/// La désérialisation appelle cette fonction en donnant le nom (complet, avec version,
			/// etc.) de l'assemblage qu'elle pense responsable pour le type dont le nom est fourni
			/// par p_nomType. Elle retrouve l'assemblage (en considérant seulement le nom), puis
			/// elle y cherche et renvoie le System.Type du type recherché.
			/// </summary>
			/// <param name="p_nomAssemblageEtc">sous la forme "nom, Version=1.0.0.1, ...", on
			/// s'intéressera seulement à ce qui vient avant la première virgule</param>
			/// <param name="p_nomType">sous la forme "nomDuNamespace.nom"</param>
			/// <returns>le descripteur de type ou null si non trouvé</returns>
			public override Type BindToType(string p_nomAssemblageEtc, string p_nomType)
			{
				string nomBase = p_nomAssemblageEtc.Substring(0, p_nomAssemblageEtc.IndexOf(','));

				var assemblage = AppDomain.CurrentDomain.GetAssemblies()
					.FirstOrDefault(a => a.FullName.StartsWith(nomBase + ","));

				return assemblage?.GetType(p_nomType);
			}
		}

		private void Récupérer()
		{
			try
			{
				using (var ficArticles = File.OpenRead(NomFichierPhilatélie))
				{
					var formateur = new BinaryFormatter();
					formateur.Binder = new LierAssemblagesSimplement();
					m_articles = (Stack<ArticlePhilatélique>)formateur.Deserialize(ficArticles);
					m_noProchainArticle = (int)formateur.Deserialize(ficArticles);
					m_commandesAnnulables = (Stack<ICommande>)formateur.Deserialize(ficArticles);
				}
			}
			catch (FileNotFoundException)
			{
				m_articles = new Stack<ArticlePhilatélique>();
				m_noProchainArticle = NoPremierArticle;
			}
		}

		private void Enregistrer()
		{
			using (var ficArticles = File.Create(NomFichierPhilatélie))
			{
				var formateur = new BinaryFormatter();
				formateur.Serialize(ficArticles, m_articles);
				formateur.Serialize(ficArticles, m_noProchainArticle);
				formateur.Serialize(ficArticles, m_commandesAnnulables);
			}
		}

		/// <summary>
		/// Renvoie un accès (en lecture seule) à tous les articles (sans ordre particulier).
		/// </summary>
		/// <returns>un accès non modifiable à tous les articles (sans ordre particulier)</returns>
		public IEnumerable<ArticlePhilatélique> TousLesArticles() => m_articles;

		/// <summary>
		/// Renvoie un numéro d'article pas encore utilisé.
		/// </summary>
		/// <returns>un numéro d'article pas encore utilisé</returns>
		public int NuméroNouvelArticle() => m_noProchainArticle++;

		/// <summary>
		/// Renvoie l'article demandé (par son numéro) ou null s'il n'existe pas.
		/// </summary>
		/// <param name="p_numéro">le numéro de l'article désiré</param>
		/// <returns>l'article dont on a fourni le numéro ou null s'il n'existe pas</returns>
		public ArticlePhilatélique ArticleSelonNuméro(int p_numéro) => m_articles.SingleOrDefault(a => a.Numéro == p_numéro);

		/*-Opérations sur les différents articles-*/

		/// <summary>
		/// Ajoute l'article (les observateurs sont ensuite notifiés).
		/// </summary>
		/// <param name="p_article">l'article à ajouter</param>
		public void Ajouter(ArticlePhilatélique p_article)
		{
			m_articles.Push(p_article);
			m_docModifié = true;
			Notifier(this);
		}

		/// <summary>
		/// Inscrit la nouvelle version de l'article (les observateurs sont ensuite notifiés).
		/// (un article portant le même numéro doit déjà exister)
		/// </summary>
		/// <param name="p_article">l'article pour remplacer l'ancien</param>
		public void Modifier(ArticlePhilatélique p_article)
		{
			Stack<ArticlePhilatélique> tempStack = new Stack<ArticlePhilatélique>();

			foreach (ArticlePhilatélique article in m_articles)
			{
				if (article.Numéro == p_article.Numéro)
				{
					//Si les valeurs ont changés, on le remplace par le nouveau
					if (!article.Equals(p_article))
					{
						tempStack.Push(p_article);
					}
				}
				else
				{
					tempStack.Push(article);
				}
			}

			foreach (ArticlePhilatélique article in tempStack)
			{
				m_articles.Push(article);
			}

			m_docModifié = true;
			Notifier(this);
		}

		/// <summary>
		/// Retire un article des articles conservés.
		/// </summary>
		/// <param name="p_noArticle">le numéro de l'article à retirer</param>
		/// <returns>true si l'article existait et a été retiré, false sinon</returns>
		public bool RetirerArticle(int p_noArticle)
		{
			Stack<ArticlePhilatélique> tempStack = new Stack<ArticlePhilatélique>();

			foreach (ArticlePhilatélique article in m_articles)
			{
				if (article.Numéro != p_noArticle)
					tempStack.Push(article);
			}

			m_articles.Clear();

			foreach (ArticlePhilatélique article in tempStack)
			{
				m_articles.Push(article);
			}

			m_docModifié = true;
			Notifier(this);
			return true;
		}

		/// <summary>
		/// Permet de vider la liste d'articles
		/// </summary>
		public void Vider() => m_articles.Clear();

		/// <summary>
		/// Permet de remplir la liste d'article avec un IEnumerable
		/// </summary>
		/// <param name="articles">Liste d'article utilisé pour populé la liste courante</param>
		public void Remplir(IEnumerable<ArticlePhilatélique> articles) => m_articles = (Stack<ArticlePhilatélique>)articles;

		/**
		 * Numero D : A revoir avec le prof, je suis pas sur si on peut sortir
		 * la logique de la forme principal et la mettre ici mais sa fait du sens 
		 * car selon moi le document devrais gerer la liste de commande qui ont ete 
		 * effectuer. A revoir.
		 */

		internal bool AucuneCommandeAnnulable() => m_commandesAnnulables.Count == 0;

		internal bool AucuneCommandeRetablissante() => m_commandesRétablissante.Count == 0;

		internal ICommande RetirerCommandeRétablissante() => m_commandesRétablissante.Pop();

		internal ICommande RetirerCommandeAnnulable() => m_commandesAnnulables.Pop();

		internal void PousserCommandeRétablissante(ICommande commande) => m_commandesRétablissante.Push(commande);

		internal void ViderCommandeRétablissante() => m_commandesRétablissante.Clear();

		internal void PousserCommandeAnnulable(ICommande commande) => m_commandesAnnulables.Push(commande);

	}
}
