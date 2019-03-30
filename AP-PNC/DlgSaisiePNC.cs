using Philatel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UtilStJ;
using System.Diagnostics;


namespace AP_PNC
{
    public partial class DlgSaisiePNC : DlgSaisieArticle
    {
		public DlgSaisiePNC(TypeDeSaisie p_saisie, ArticlePhilatélique p_article) : base(p_saisie, p_article)
		{
			InitializeComponent();
		}

		public DlgSaisiePNC(TypeDeSaisie p_opération, PlancheNonCoupée p_pnc) 
            : base(p_opération, p_pnc)
        {
            InitializeComponent();

            switch (p_opération)
            {
                case TypeDeSaisie.Ajout: Text = "Ajout d'une planche non coupée"; break;
                case TypeDeSaisie.Modification: Text = "Modification d'une planche non coupée"; break;
                case TypeDeSaisie.Autre: Debug.Assert(false, "Opération non implémentée"); break;
            }

            if (p_pnc != null)
            {
                textBoxValeurPlanche.Text = $"{p_pnc.ValeurPlanche}";
                textBoxNbTimbres.Text = $"{p_pnc.NombreTimbres}";
                textBoxNbTimbresDifférents.Text = $"{p_pnc.NombreTimbresDifférent}";
                textBoxNomDesigner.Text = $"{p_pnc.NomDesigner}";
                textBoxNbLignes.Text = $"{p_pnc.NombreLignes}";
                textBoxNbColonnes.Text = $"{p_pnc.NombreColonnes}";
            }
        }

        public override bool FinirValidation(string p_motif, string p_tailleEtForme, DateTime? p_parution, double? p_prixPayé)
        {
            int nombreTimbres = Int32AvecMinimum(textBoxNbTimbres, 1, "Nombre de timbres dans la planche");
            double valeurPlanche = DoubleAvecMinimum(textBoxValeurPlanche, 0.01, "Valeur de la planche");
            int nombreTimbresDifférents = Int32AvecMinimum(textBoxNbTimbresDifférents, 1, "Nombre de timbres différent dans la planche");
            string nomDesigner = StringAvecLongueurMinimum(textBoxNomDesigner, 3, "Nom du designer (3 charactères min)");
            int nombreLignes = Int32AvecMinimum(textBoxNbLignes, 1, "Nombre de ligne dans la planche");
            int nombreColonnes = Int32AvecMinimum(textBoxNbColonnes, 1, "Nombre de colonne dans la planche");

            Article = new PlancheNonCoupée(
                (Article != null) ? Article.Numéro : Document.Instance.NuméroNouvelArticle(), p_motif, p_tailleEtForme, p_parution, 
                nombreTimbres, valeurPlanche, nombreTimbresDifférents, nomDesigner, nombreLignes, nombreColonnes, p_prixPayé);

            return true;
        }
    }
}
