using Philatel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilStJ;

namespace AP_PNC
{
	[Serializable]
	public class PlancheNonCoupée : ArticlePhilatélique
	{
		public PlancheNonCoupée(int p_numéro, string p_motif, DateTime? p_parution,
			double p_prixPayé, int p_nombreTimbres, double p_valeurPlanche, int p_nombreTimbreDifférent,
			string p_nomDesigner, int p_nombreLigne, int p_nombreColonne)
			: base(p_numéro, p_motif, p_parution, p_prixPayé)
		{
			ValeurPlanche = p_valeurPlanche;
			NombreTimbre = p_nombreTimbres;
			NombreTimbreDifférent = p_nombreTimbreDifférent;
			NomDesigner = p_nomDesigner;
			NombreLigne = p_nombreLigne;
			NombreColonne = p_nombreColonne;
		}

		public double ValeurPlanche { get; }
		public int NombreTimbre { get; }
		public int NombreTimbreDifférent { get; }
		public string NomDesigner { get; }
		public int NombreLigne { get; }
		public int NombreColonne { get; }

		public override string Catégorie => "Planche de timbres non coupées";

		public override string Description() => Catégorie;

		public override string ValeurNominale() => ValeurPlanche.ToString();
	}

	public class FabriquePNC : IFabriqueCommande
	{
		public static FabriquePNC InstanceFabrique { get; } = new FabriquePNC();

		private FabriquePNC() { }

		public ICommande CréerCommandeAjouter() => new CommandeAjoutPNC();

		public ICommande CréerCommandeModifier(ArticlePhilatélique p_article) => new CommandeModificationPNC(p_article as PlancheNonCoupée);
		
		public ICommande CréerCommandeSupprimer(ArticlePhilatélique p_article) => new CommandeSuppression(p_article);

		public string DescriptionPourMenu() => "Planche non coupée (PNC)";
	}

	[Serializable]
	class CommandeAjoutPNC : CommandeAjout
	{
		public override DlgSaisieArticle CréerDlgSaisie() => new DlgSaisiePNC(TypeDeSaisie.Ajout, null);
	}

	[Serializable]
    class CommandeModificationPNC : CommandeModification
    {
        public CommandeModificationPNC(PlancheNonCoupée p_article): base(p_article){ }

        public override DlgSaisieArticle CréerDlgSaisie(ArticlePhilatélique p_article)
            => new DlgSaisiePNC(TypeDeSaisie.Modification, p_article as PlancheNonCoupée);  
    }
}
