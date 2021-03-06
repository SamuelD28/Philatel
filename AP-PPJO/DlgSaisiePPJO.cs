﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Philatel;
using UtilStJ;

namespace PhilatelPPJO
{
    partial class DlgSaisiePPJO : DlgSaisieArticle
    {
		public DlgSaisiePPJO(TypeDeSaisie p_opération, PPJO p_ppjo)
            : base(p_opération, p_ppjo)
        {
            InitializeComponent();
            CorrecteurDécimal.Corriger(textBoxValeurTimbres);

            InitialiserTitre(p_opération);

            if (p_ppjo != null)
                textBoxValeurTimbres.Text = $"{p_ppjo.ValeurTimbres:F2}";
        }

		public DlgSaisiePPJO(TypeDeSaisie ajout, ArticlePhilatélique m_article)
			:base(ajout, m_article)
		{
			InitializeComponent();
            InitialiserTitre(ajout);
		}

		public override bool FinirValidation(string p_motif, string p_tailleEtForme, DateTime? p_parution, double? p_prixPayé)
        {
            double valeurTimbres = DoubleAvecMinimum(textBoxValeurTimbres, 0.01, "Valeur des timbres");

            Article = new PPJO(
                (Article != null) ? Article.Numéro : Document.Instance.NuméroNouvelArticle(),
                p_motif, p_tailleEtForme, p_parution, valeurTimbres, p_prixPayé);
            return true;
        }

        public void InitialiserTitre(TypeDeSaisie p_typeDeSaisie)
        {
            switch (p_typeDeSaisie)
            {
                case TypeDeSaisie.Ajout: Text = "Ajout d'un pli premier jour officiel"; break;
                case TypeDeSaisie.Modification: Text = "Modification d'un pli premier jour officiel"; break;
                case TypeDeSaisie.Autre: Debug.Assert(false, "Opération non implémentée"); break;
            }
        }
    }
}
