﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Philatel;
using UtilStJ;
//using UtilStJ;

namespace PhilatelPPJO
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PPJO : ArticlePhilatélique
    {
        public PPJO(int p_numéro, string p_motif, string p_tailleEtForme,  
					DateTime? p_parution, double p_valeurTimbres, double? p_prixPayé)
            : base(p_numéro, p_motif, p_tailleEtForme, p_parution, p_prixPayé)
        {
            ValeurTimbres = p_valeurTimbres;
        }

        public override string Catégorie
            => "Pli premier jour officiel";

        public double ValeurTimbres { get; }

        public override string Description()
            => Catégorie;

        public override string ValeurNominale()
            => Timbre.ValeurEnString(ValeurTimbres);
    }

    /// <summary>
    /// 
    /// </summary>
    public class FabriquePPJO : IFabriqueCommande
    {
        // Singleton :
        public static FabriquePPJO InstanceFabrique { get; } = new FabriquePPJO();

		private FabriquePPJO()
        { }

		public string DescriptionDuType => "Pli premier jour officiel (PPJO)";

        public ICommande CréerCommandeAjouter()
           => new CommandeAjoutPPJO();

		public ICommande CréerCommandeAjouter(ArticlePhilatélique p_article)
			=> new CommandeAjoutPPJO(p_article);

        public ICommande CréerCommandeModifier(ArticlePhilatélique p_articleCourant)
            => new CommandeModificationPPJO(p_articleCourant as PPJO);

        public ICommande CréerCommandeSupprimer(ArticlePhilatélique p_articleCourant)
            => new CommandeSuppression(p_articleCourant);

	}
    
    // NOTEZ QUE LES COMMANDES NE SONT PAS DES TYPES PUBLICS

    /// <summary>
    /// 
    /// </summary>
	[Serializable]
    class CommandeAjoutPPJO : CommandeAjout
    {
		private ArticlePhilatélique m_article;

		public CommandeAjoutPPJO(ArticlePhilatélique p_article = null)
		{
			m_article = p_article;

		}

        public override DlgSaisieArticle CréerDlgSaisie() => new DlgSaisiePPJO(TypeDeSaisie.Ajout, m_article);
    }

    /// <summary>
    /// 
    /// </summary>
	[Serializable]
    class CommandeModificationPPJO : CommandeModification
    {
        public CommandeModificationPPJO(PPJO p_article)
            : base(p_article)
        { }

        public override DlgSaisieArticle CréerDlgSaisie(ArticlePhilatélique p_article)
            => new DlgSaisiePPJO(TypeDeSaisie.Modification, p_article as PPJO);
    }
}
