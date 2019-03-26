﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilStJ;
using static UtilStJ.MB;

namespace Philatel
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
            new TrieurListe(listViewArticles, "ttdN").TrierSelonColonne(0);

            Text = InfoApp.Nom;
            m_doc.Changement += MettreÀJour; // Système d'inscription en observateur

            LesFabriques.CompléterLeMenu(opérationsAjouterToolStripMenuItem, OpérationsAjouter);
            MettreÀJour(null);
        }

        Document m_doc = Document.Instance;
        ArticlePhilatélique m_articleCourant = null;
        Stack<ICommande> m_commandesAnnulables = new Stack<ICommande>();  // Commandes pour le Annuler/Ctrl+Z

        public void MettreÀJour(Document p_sujet)
        {
            listViewArticles.Items.Clear();

            foreach (var article in m_doc.TousLesArticles())
            {
                ListViewItem lvi = new ListViewItem(article.Description());
                lvi.Tag = article.Numéro;
                lvi.SubItems.Add(article.Motif);
                lvi.SubItems.Add(article.Parution.HasValue ? article.Parution.Value.ToShortDateString() : "");
                lvi.SubItems.Add($"{article.PrixPayé:C}");
                listViewArticles.Items.Add(lvi);
            }

            listViewArticles_SelectedIndexChanged(null, null);
        }

        private void aideÀproposToolStripMenuItem_Click(object sender, EventArgs e)
            => new DlgÀPropos().ShowDialog();

        private void fichierQuitterToolStripMenuItem_Click(object sender, EventArgs e)
            => Close();

        private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e)
            => m_doc.Fermer();

        private void listViewArticles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listViewArticles.SelectedItems.Count == 0)
            {
                textBoxDétails.Text = "";
                m_articleCourant = null;
            }
            else
            {
                int noArticle = (int)listViewArticles.SelectedItems[0].Tag;
                m_articleCourant = m_doc.ArticleSelonNuméro(noArticle);
                textBoxDétails.Text = m_articleCourant.ToString().Replace("\n", "\r\n"); ;
            }

            ActiverDésactiverContrôlesPourLaListe();
        }

        private void ActiverDésactiverContrôlesPourLaListe()
        {
            bool actif = m_articleCourant != null;

            buttonAfficher.Enabled = actif;
            buttonModifier.Enabled = actif;
            buttonSupprimer.Enabled = actif;
        }

        private void ActiverDésactiverMenus(object sender, EventArgs e)
        {
            opérationsAnnulerToolStripMenuItem.Enabled = m_commandesAnnulables.Count != 0;

            bool actif = m_articleCourant != null;

            opérationsModifierToolStripMenuItem.Enabled = actif;
            opérationsSupprimerToolStripMenuItem.Enabled = actif;
        }

        private void opérationsAnnulerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var commandeAnnuler = m_commandesAnnulables.Pop();
            commandeAnnuler.Annuler();
        }

        public void OpérationsAjouter(object p_sender, EventArgs p_e)
        {
            var tsi = (ToolStripMenuItem)p_sender;
            var fab = (IFabriqueCommande)tsi.Tag;
            var commande = fab.CréerCommandeAjouter();

            if (commande.Exécuter())
                m_commandesAnnulables.Push(commande);
        }

        private void opérationsModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fab = LesFabriques.FabriqueDe(m_articleCourant.GetType());
            var commande = fab.CréerCommandeModifier(m_articleCourant);

            if (commande.Exécuter())
                m_commandesAnnulables.Push(commande);
         }

        private void opérationsSupprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fab = LesFabriques.FabriqueDe(m_articleCourant.GetType());
            var commande = fab.CréerCommandeSupprimer(m_articleCourant);

            if (commande.Exécuter())
                m_commandesAnnulables.Push(commande);
        }

        private void buttonAfficher_Click(object sender, EventArgs e)
        {
            Avertir($"#{m_articleCourant.Numéro} {m_articleCourant.Description()}" +
                    " : Paresse ! (il faudrait tout afficher, sauf le numéro, interne)");
        }// J'aurais pu mettre « => », mais je continue la règle « plus qu'une ligne == bloc ».


        // On aurait pu associer directement ces trois événements aux fonctions déjà existantes...
        private void buttonModifier_Click(object sender, EventArgs e)
            => opérationsModifierToolStripMenuItem_Click(sender, e);
        
        private void buttonSupprimer_Click(object sender, EventArgs e)
            => opérationsSupprimerToolStripMenuItem_Click(sender, e);

        private void listViewArticles_DoubleClick(object sender, EventArgs e)
            => buttonModifier_Click(sender, e);  // Ou ... afficher ...

        // Cette méthode est ajoutée pour que les raccourcis clavier soient actifs ou pas, correctement.
        // Dans les premières versions de .NET (et dans les MFC, et peut-être ailleurs), les raccourcis
        // clavier généraient l'évènement DropDownOpening du menu concerné, mais ce n'est pas le cas
        // avec les ToolStripMenuItem, alors on active manuellement la mise à jour quand une touche
        // qui pourrait être un raccourci est détectée. On pourrait faire ça encore plus « subtilement »
        // si la mise à jour est longue, en détectant seulement les raccourcis vraiment utilisés, etc.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Control) == Keys.Control)
                ActiverDésactiverMenus(null, null);

            // Ou, s'il y a des raccourcis avec Alt, comme Alt+F2, etc. : 
            // if (... || (keyData & Keys.Alt) == Keys.Alt || ...) 
            // Ou, s'il y des raccourcis simples comme F2, etc., sans modificateur :
            // if (... || (F1 <= keyData && keyData <= F12) || ...)  

            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}