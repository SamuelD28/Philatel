﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilStJ;

namespace Philatel
{
    public partial class DlgSaisieBlocDeCoin : DlgSaisieArticle
    {
		public DlgSaisieBlocDeCoin(TypeDeSaisie p_opération, BlocDeCoin p_blocDeCoin)
            : base(p_opération, p_blocDeCoin)
        {
            InitializeComponent();
            CorrecteurDécimal.Corriger(textBoxValeurTimbre);

            InitialiserTitre(p_opération);

            if (p_blocDeCoin != null)
            {
                textBoxValeurTimbre.Text = $"{p_blocDeCoin.ValeurTimbre:F2}";
                CocherBoutonRadio(groupBoxCoins, (int)p_blocDeCoin.Coin);
                textBoxNombreTimbres.Text = p_blocDeCoin.NbTimbres.ToString();
            }

            if (p_opération == TypeDeSaisie.Suppression)
            {
                textBoxValeurTimbre.Enabled = false;
                groupBoxCoins.Enabled = false;
                textBoxNombreTimbres.Enabled = false;

                BoutonOK.Text = "Supprimer";
            }
        }

		public DlgSaisieBlocDeCoin(TypeDeSaisie ajout, ArticlePhilatélique m_article)
			:base(ajout, m_article)
		{
			InitializeComponent();
            InitialiserTitre(ajout);
		}

		public override bool FinirValidation(string p_motif, string p_tailleEtForme, DateTime? p_parution, double? p_prixPayé)
        {
            double valeurTimbre = DoubleAvecMinimum(textBoxValeurTimbre, 0.01, "Valeur d'un timbre");
            int nbTimbres = Int32AvecMinimum(textBoxNombreTimbres, 1, "Nombre de timbres");
            int codeCoin = NoRadio(groupBoxCoins, "Coins");

            Article = new BlocDeCoin(
                (Article != null) ? Article.Numéro : Document.Instance.NuméroNouvelArticle(),
                p_motif, p_tailleEtForme, p_parution, (Coin)codeCoin, valeurTimbre, nbTimbres, p_prixPayé);
            return true;
        }

        public void InitialiserTitre(TypeDeSaisie p_typeDeSaisie)
        {
            switch (p_typeDeSaisie)
            {
                case TypeDeSaisie.Ajout: Text = "Ajout d'un bloc de coin"; break;
                case TypeDeSaisie.Modification: Text = "Modification d'un bloc de coin"; break;
                case TypeDeSaisie.Suppression: Text = "Suppression d'un bloc de coin"; break;
                case TypeDeSaisie.Autre: Debug.Assert(false, "Opération non implémentée"); break;
            }
        }
    }
}
