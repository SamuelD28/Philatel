using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilStJ;

namespace Philatel
{
    public partial class DlgSaisieArticle : DialogueOkAnnuler
    {
        /// <summary>
        /// Ce constructeur est pour l'éditeur visuel seulement, il ne devrait pas être utilisé autrement.
        /// </summary>
        protected DlgSaisieArticle()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Vrai constructeur à utiliser
        /// </summary>
        /// <param name="p_opération">Le type d'opération désiré</param>
        /// <param name="p_article">L'article à traiter (normalement null pour les ajouts)</param>
        protected DlgSaisieArticle(TypeDeSaisie p_opération, ArticlePhilatélique p_article)
        {
            InitializeComponent();
            CorrecteurDécimal.Corriger(textBoxPrixPayé);

            Article = p_article;

            if (Article != null)
                InitialiserLesChamps();

            if (p_opération != TypeDeSaisie.Ajout && p_opération != TypeDeSaisie.Modification)
                DésactiverLesChamps();
        }

        protected ArticlePhilatélique Article  { get; set; }

        public ArticlePhilatélique Extraire()
            => Article;

        private void InitialiserLesChamps()
        {
            textBoxMotif.Text = Article.Motif;

            if (Article.Parution.HasValue)
            {
                dateTimeParution.Checked = true;
                dateTimeParution.Value = Article.Parution.Value;
            }
            else
            {
                dateTimeParution.Checked = false;
                dateTimeParution.Value = DateTime.Today;
            }

            textBoxPrixPayé.Text = $"{Article.PrixPayé:F2}";
        }

        private void DésactiverLesChamps()
        {
            textBoxMotif.Enabled = false;
            dateTimeParution.Enabled = false;
            textBoxPrixPayé.Enabled = false;
        }

        protected override bool ChampsValides() // Appelé d'un template method, mais est aussi un TM !
        {
            string motif = StringNonVide(textBoxMotif, "Motif");
            DateTime? parution = dateTimeParution.Checked ? dateTimeParution.Value : (DateTime?)null;
            double prixPayé = DoubleAvecMinimum(textBoxPrixPayé, 0.0, "Prix payé");
            // S'il y en avait beaucoup, il faudrait probablement créer un BaseDeArticlePhilatélique

            return FinirValidation(motif, parution, prixPayé); // TM...
        }

        // Pour Template Method ChampsValides, doit faire un set sur Article
        public virtual bool FinirValidation(string p_motif, DateTime? p_parution, double p_prixPayé)
        {
            throw new NotImplementedException(); // Doit être définie dans la classe dérivée (devait être
        }                                        //  abstraite mais on ne peut pas pour l'éditeur visuel)
    }
}
