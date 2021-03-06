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
		//Propriété privée : Principe de plus grande restriction
		private Document m_doc = Document.Instance;
		private GestionCommandes m_gestionCommandes = GestionCommandes.GetInstance();
		private ArticlePhilatélique m_articleCourant = null;

		public FormPrincipal()
		{
			InitializeComponent();
			new TrieurListe(listViewArticles, "ttdN").TrierSelonColonne(0);

			ActiverDésactiverMenus(null);

			Text = InfoApp.Nom;
			m_doc.Changement += MettreÀJourListe; // Système d'inscription en observateur
			m_doc.Changement += ActiverDésactiverMenus; //On desactive/active les menus chaques fois que la liste change


			CompléterLeMenu(opérationsAjouterToolStripMenuItem, OpérationsAjouter);
			MettreÀJourListe(null);

		}


		/*-Méthodes diverses sur le formulaire principal-*/

		// Cette méthode est ajoutée pour que les raccourcis clavier soient actifs ou pas, correctement.
		// Dans les premières versions de .NET (et dans les MFC, et peut-être ailleurs), les raccourcis
		// clavier généraient l'évènement DropDownOpening du menu concerné, mais ce n'est pas le cas
		// avec les ToolStripMenuItem, alors on active manuellement la mise à jour quand une touche
		// qui pourrait être un raccourci est détectée. On pourrait faire ça encore plus « subtilement »
		// si la mise à jour est longue, en détectant seulement les raccourcis vraiment utilisés, etc.
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if ((keyData & Keys.Control) == Keys.Control)
				ActiverDésactiverMenus(m_doc);

			// Ou, s'il y a des raccourcis avec Alt, comme Alt+F2, etc. : 
			// if (... || (keyData & Keys.Alt) == Keys.Alt || ...) 
			// Ou, s'il y des raccourcis simples comme F2, etc., sans modificateur :
			// if (... || (F1 <= keyData && keyData <= F12) || ...)  

			return base.ProcessCmdKey(ref msg, keyData);
		}

		/// <summary>
		/// Méthode appelé quand le formulaire ferme.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FormPrincipal_FormClosing(object sender, FormClosingEventArgs e) => m_doc.Fermer();


		/*--Opération Modifiante effectuer dans le menu--*/

		public void OpérationsAjouter(object p_sender, EventArgs p_e)
		{

			ToolStripMenuItem tsi = (ToolStripMenuItem)p_sender;

			FabriqueEtArticle fabriqueEtArticle = (FabriqueEtArticle)tsi.Tag;

			IFabriqueCommande fab = (IFabriqueCommande)fabriqueEtArticle.Fabrique;
			ICommande commande = fab.CréerCommandeAjouter(fabriqueEtArticle.Article);
			commande.Exécuter();


		}

		private void opérationsAnnulerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			m_gestionCommandes.RetirerCommandeAnnulable().Annuler();
		}

		private void opérationsModifierToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IFabriqueCommande fab = LesFabriques.GetInstance().FabriqueDe(m_articleCourant.GetType());
			ICommande commande = fab.CréerCommandeModifier(m_articleCourant);
			commande.Exécuter();
		}

		private void opérationsSupprimerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			IFabriqueCommande fab = LesFabriques.GetInstance().FabriqueDe(m_articleCourant.GetType());
			ICommande commande = fab.CréerCommandeSupprimer(m_articleCourant);
			commande.Exécuter();
		}

		private void Rétablir_Btn_Click(object sender, EventArgs e)
		{
			//Numero C : A revoir
			if (!m_gestionCommandes.AucuneRétablissantes)
			{
				ICommande commandeRétablissante = m_gestionCommandes.RetirerCommandeRétablissante();
				commandeRétablissante.Rétablir();
				m_gestionCommandes.PousserCommandeAnnulable(commandeRétablissante);
			}
		}


		/*-Actions relatives au menu-*/

		private void aideÀproposToolStripMenuItem_Click(object sender, EventArgs e) => new DlgÀPropos().ShowDialog();

		private void fichierQuitterToolStripMenuItem_Click(object sender, EventArgs e) => Close();

		private void ActiverDésactiverMenus(Document p_doc)
		{
			bool actif = m_articleCourant != null;
			opérationsAnnulerToolStripMenuItem.Enabled = !m_gestionCommandes.AucuneAnnulables;
			opérationsModifierToolStripMenuItem.Enabled = actif;
			opérationsSupprimerToolStripMenuItem.Enabled = actif;

			Rétablir_Btn.Enabled = (!m_gestionCommandes.AucuneRétablissantes);
			effacertout.Enabled = m_doc.TousLesArticles().Count() > 0;
		}

		private string TrouvéRaccourciUnique(string raccourci, List<string> raccourcis_existants)
		{
			bool raccourciTrouver = false;
			string raccourciReconstituer = "";
			foreach (string mot in raccourci.Split(' '))
			{
				int indexRaccourci = raccourcis_existants.FindIndex(s => s.ToLower().Contains("&" + char.ToLower(mot[0])));
				if (indexRaccourci == -1 && !raccourciTrouver)
				{
					raccourciReconstituer += "&" + mot + " ";
					raccourciTrouver = true;
				}
				else
					raccourciReconstituer += mot + " ";
			}

			if (!raccourciTrouver)
			{
				char[] voyelles = new char[] { 'a', 'e', 'i', 'o', 'u', 'y' };
				foreach (char voyelle in voyelles)
				{
					if (raccourcis_existants.FindIndex(s => s.ToLower().Contains("&" + voyelle)) == -1 &&
						raccourciReconstituer.IndexOf(voyelle) != -1 &&
						!raccourciTrouver)
					{
						int index = raccourciReconstituer.IndexOf(voyelle);
						string debut = raccourci.Substring(0, index);
						string fin = raccourciReconstituer.Substring(index, raccourciReconstituer.Length - index);
						raccourciReconstituer = debut + "&" + fin;
						raccourciTrouver = true;
					}
				}
			}

			return raccourciReconstituer;
		}

		public void CompléterLeMenu(ToolStripMenuItem p_menu, EventHandler p_eh)
		{
			List<string> raccourcis_existants = new List<string>();
			foreach (IFabriqueCommande fab in LesFabriques.GetInstance())
			{
				string raccourci = TrouvéRaccourciUnique(fab.DescriptionDuType, raccourcis_existants);
				raccourcis_existants.Add(raccourci);
				var tsi = new ToolStripMenuItem(raccourci, null, p_eh);
				tsi.Tag = new FabriqueEtArticle(fab, null);
				tsi.ShowShortcutKeys = true;
				p_menu.DropDownItems.Add(tsi);
			}

			foreach (var article in Document.Instance.TroisDerniersArticles())
			{
				foreach (var fabrique in LesFabriques.GetInstance())
				{
					string raccourci = TrouvéRaccourciUnique(fabrique.DescriptionDuType, raccourcis_existants);
					raccourcis_existants.Add(raccourci);
					var tsi = new ToolStripMenuItem(raccourci + " " + article.Motif, null, p_eh);
					tsi.Tag = new FabriqueEtArticle(fabrique, article);
					tsi.ShowShortcutKeys = true;
					p_menu.DropDownItems.Add(tsi);
				}
			}
		}


		/*--Opération par les boutons sur le coté de la liste--*/

		private void effacertout_Click(object sender, EventArgs e)
		{
			if (ConfirmerOkAnnuler("Voulez-vous effacer toute la liste d'articles"))
				new CommandeEffacerTout().Exécuter();
		}

		private void buttonAfficher_Click(object sender, EventArgs e)
		{
			Avertir($"#{m_articleCourant.Numéro} {m_articleCourant.Description()}" +
					" : Paresse ! (il faudrait tout afficher, sauf le numéro, interne)");
		}// J'aurais pu mettre « => », mais je continue la règle « plus qu'une ligne == bloc ».

		private void buttonModifier_Click(object sender, EventArgs e) => opérationsModifierToolStripMenuItem_Click(sender, e);

		private void buttonSupprimer_Click(object sender, EventArgs e) => opérationsSupprimerToolStripMenuItem_Click(sender, e);


		/*-Méthode pour la liste d'article-*/

		private void ActiverDésactiverContrôlesPourLaListe()
		{
			bool actif = m_articleCourant != null;

			buttonAfficher.Enabled = actif;
			buttonModifier.Enabled = actif;
			buttonSupprimer.Enabled = actif;
		}

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

		private void listViewArticles_DoubleClick(object sender, EventArgs e) => buttonModifier_Click(sender, e);

		/// <summary>
		/// Méthode qui met à jour la liste d'article
		/// </summary>
		/// <param name="p_sujet"></param>
		public void MettreÀJourListe(Document p_sujet)
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
	}
}
