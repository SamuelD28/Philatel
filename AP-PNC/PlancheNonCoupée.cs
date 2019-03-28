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
		public PlancheNonCoupée(int p_numéro, string p_motif, string p_tailleEtForme, DateTime? p_parution,
            int p_nombreTimbres, double p_valeurPlanche, int p_nombreTimbresDifférents,
			string p_nomDesigner, int p_nombreLignes, int p_nombreColonnes, double? p_prixPayé)
			: base(p_numéro, p_motif, p_tailleEtForme, p_parution, p_prixPayé)
		{
			ValeurPlanche = p_valeurPlanche;
			NombreTimbres = p_nombreTimbres;
			NombreTimbresDifférent = p_nombreTimbresDifférents;
			NomDesigner = p_nomDesigner;
			NombreLignes = p_nombreLignes;
			NombreColonnes = p_nombreColonnes;
		}

		public double ValeurPlanche { get; }
		public int NombreTimbres { get; }
		public int NombreTimbresDifférent { get; }
		public string NomDesigner { get; }
		public int NombreLignes { get; }
		public int NombreColonnes { get; }

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

		public string DescriptionDuType => "Planche non coupée (PNC)";
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
