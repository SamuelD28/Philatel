using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilStJ;

namespace Philatel
{
    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////// Types et fonctions utilitaires (dans la classe statique Timbre)
    ////////////

    [Serializable]
    public enum Coin { SupérieurGauche, SupérieurDroit, InférieurGauche, InférieurDroit }

    [Serializable]
    public enum Oblitération { Aucune, Normale }  // Pourrait un jour contenir Spéciale, Erronné, etc. ?

    static public class Timbre
    {
        public static string ValeurEnString(double p_valeur)
            => (p_valeur < 1.0) ? $"{(int)Math.Round(p_valeur * 100)} ¢" : $"{p_valeur:C}";
        // N.B. Il manque probablement un test pour savoir s'il faut espace avant ¢ (il suffirait de
        //      regarder si :C en met un)

        public static string CoinEnString(Coin p_coin)
        {
            switch (p_coin)
            {
                case Coin.SupérieurGauche: return "supérieur gauche";
                case Coin.SupérieurDroit: return "supérieur droit";
                case Coin.InférieurGauche: return "inférieur gauche";
                case Coin.InférieurDroit: return "inférieur droit";
                default: Debug.Assert(false); return "";
            }
        }

        public static string OblitérationEnString(Oblitération p_oblitération)
        {
            switch (p_oblitération)
            {
                case Oblitération.Aucune: return "non oblitéré";
                case Oblitération.Normale: return "oblitéré";
                default: Debug.Assert(false); return "";
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////// Deux types d'articles de base (leurs fabriques et commandes suivent)
    ////////////

    [Serializable]
    public class TimbreSeul : ArticlePhilatélique
    {
        public TimbreSeul(int p_numéro, string p_motif, string p_tailleEtForme, DateTime? p_parution,
                          double p_valeurTimbre, Oblitération p_oblitération, double? p_prixPayé)
            : base(p_numéro, p_motif, p_tailleEtForme, p_parution, p_prixPayé)
        {
            ValeurTimbre = p_valeurTimbre;
            Oblitération = p_oblitération;
        }

        public override string Catégorie => "Timbre seul";

        public double ValeurTimbre { get; }
        public Oblitération Oblitération { get; }

        public override string Description()
            => Catégorie + " " + Timbre.OblitérationEnString(Oblitération);

        public override string ValeurNominale()
            => Timbre.ValeurEnString(ValeurTimbre);
    }

    [Serializable]
    public class BlocDeCoin : ArticlePhilatélique
    {
        public BlocDeCoin(int p_numéro, string p_motif, string p_tailleEtForme, DateTime? p_parution,
                          Coin p_coin, double p_valeurTimbre, int p_nbTimbres, double? p_prixPayé)
            : base(p_numéro, p_motif, p_tailleEtForme, p_parution, p_prixPayé)
        {
            Coin = p_coin;
            ValeurTimbre = p_valeurTimbre;
            NbTimbres = p_nbTimbres;
        }

        public override string Catégorie => "Bloc de coin";

        public Coin Coin { get; }

        public double ValeurTimbre { get; }

        public int NbTimbres { get; }

        public override string Description()
            => Catégorie + " " + Timbre.CoinEnString(Coin);

        public override string ValeurNominale()
            => $"{NbTimbres} × {Timbre.ValeurEnString(ValeurTimbre)}";
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////// Les fabriques des deux types d'articles de base
    ////////////

    public class FabriqueTimbreSeul : IFabriqueCommande
    {
        // Singleton :
        public static FabriqueTimbreSeul Instance { get; } = new FabriqueTimbreSeul();

		public string DescriptionDuType =>"Timbre seul";

		private FabriqueTimbreSeul()
        { }

        public ICommande CréerCommandeAjouter()
            => new CommandeAjoutTS();

        public ICommande CréerCommandeModifier(ArticlePhilatélique p_articleCourant)
            => new CommandeModificationTS(p_articleCourant as TimbreSeul);

        public ICommande CréerCommandeSupprimer(ArticlePhilatélique p_articleCourant)
            => new CommandeSuppression(p_articleCourant); // Approche générale
    }

    public class FabriqueBlocDeCoin : IFabriqueCommande
    {
        // Singleton :
        public static FabriqueBlocDeCoin Instance { get; } = new FabriqueBlocDeCoin();

		private FabriqueBlocDeCoin()
        { }

		string IFabriqueCommande.DescriptionDuType => "Bloc en coin";

		public ICommande CréerCommandeAjouter()
            => new CommandeAjoutBC();

        public ICommande CréerCommandeModifier(ArticlePhilatélique p_articleCourant)
            => new CommandeModificationBC(p_articleCourant as BlocDeCoin);

        public ICommande CréerCommandeSupprimer(ArticlePhilatélique p_articleCourant)
            => new CommandeSuppressionBC(p_articleCourant as BlocDeCoin); // Particulier
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////
    //////////// Les commandes des deux types d'articles de base
    ////////////

	[Serializable]
    public class CommandeAjoutTS : CommandeAjout
    {
        public override DlgSaisieArticle CréerDlgSaisie()
            => new DlgSaisieTimbreSeul(TypeDeSaisie.Ajout, null);
    }

	[Serializable]
    public class CommandeModificationTS : CommandeModification
    {
        public CommandeModificationTS(TimbreSeul p_article)
            : base(p_article)
        { }

        public override DlgSaisieArticle CréerDlgSaisie(ArticlePhilatélique p_article)
            => new DlgSaisieTimbreSeul(TypeDeSaisie.Modification, p_article as TimbreSeul);
    }

	[Serializable]
    public class CommandeAjoutBC : CommandeAjout
    {
        public override DlgSaisieArticle CréerDlgSaisie()
            => new DlgSaisieBlocDeCoin(TypeDeSaisie.Ajout, null);
    }

	[Serializable]
    public class CommandeModificationBC : CommandeModification
    {
        public CommandeModificationBC(BlocDeCoin p_article)
            : base(p_article)
        { }

        public override DlgSaisieArticle CréerDlgSaisie(ArticlePhilatélique p_article)
            => new DlgSaisieBlocDeCoin(TypeDeSaisie.Modification, p_article as BlocDeCoin);
    }

	[Serializable]
    public class CommandeSuppressionBC : CommandeSuppression
    {
        public CommandeSuppressionBC(BlocDeCoin p_article)
            : base(p_article)
        { }

        public override bool ConfirmerSuppression(ArticlePhilatélique p_article)
        {
            return DialogResult.OK ==
                new DlgSaisieBlocDeCoin(TypeDeSaisie.Suppression, p_article as BlocDeCoin).ShowDialog();
        }
    }
}
