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

            comboBoxMotifs.Items.Clear();
            foreach (string motif in Document.Instance.ObtenirTousLesMotifs())
            {
                comboBoxMotifs.Items.Add(motif);
            }

            if (Article != null)
                InitialiserLesChamps();

            if (p_opération != TypeDeSaisie.Ajout && p_opération != TypeDeSaisie.Modification)
                DésactiverLesChamps();
        }

        protected ArticlePhilatélique Article { get; set; }

        public ArticlePhilatélique Extraire()
            => Article;

        private void InitialiserLesChamps()
        {
            comboBoxMotifs.SelectedIndex = comboBoxMotifs.FindStringExact(Article.Motif);

            textBoxMotif.Text = null;

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

            textBoxTailleEtForme.Text = Article.TailleEtForme;

            textBoxPrixPayé.Text = Article.PrixPayé.ToString() ?? String.Empty;  
        }

        private void DésactiverLesChamps()
        {
            textBoxMotif.Enabled = false;
            dateTimeParution.Enabled = false;
            textBoxPrixPayé.Enabled = false;
        }

        protected override bool ChampsValides() // Appelé d'un template method, mais est aussi un TM !
        {
            string motif;

            if (string.IsNullOrEmpty(textBoxMotif.Text) && comboBoxMotifs.SelectedIndex == -1)
            {
                MB.Avertir("Choisir une option de motif ou en saisir un nouveau");
                return false;
            }
            
            if (!string.IsNullOrEmpty(textBoxMotif.Text))
            {
                string motifFormaté = Char.ToUpper(textBoxMotif.Text[0]) + textBoxMotif.Text.Substring(1).ToLower();
                motif = motifFormaté;
                Document.Instance.AjouterMotif(motifFormaté);
            }

            else
            {
                motif = comboBoxMotifs.SelectedItem.ToString();
            }

            string tailleEtForme = StringAvecLongueurMinimum(textBoxTailleEtForme, 1, "Taille et forme");
                  
            DateTime? parution = dateTimeParution.Checked ? dateTimeParution.Value : (DateTime?)null;

            double? prixPayé = DoubleAvecMinimumOuNull(textBoxPrixPayé, 0.0, "Prix payé");
            // S'il y en avait beaucoup, il faudrait probablement créer un BaseDeArticlePhilatélique

            return FinirValidation(motif, tailleEtForme, parution, prixPayé); // TM...
        }

        // Pour Template Method ChampsValides, doit faire un set sur Article
        public virtual bool FinirValidation(string p_motif, string p_tailleEtForme, DateTime? p_parution, double? p_prixPayé)
        {
            throw new NotImplementedException(); // Doit être définie dans la classe dérivée (devait être
        }                                        //  abstraite mais on ne peut pas pour l'éditeur visuel)
    }
}
