using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Philatel
{
    /// <summary>
    /// Conserve et permet de récupérer des fabriques pour des commandes.
    /// Offre aussi une opération pour ajouter les commandes au menu du programme (ce n'est pas tellement
    /// « fabrique » alors aurait pu être dans un truc séparé, ou alors on pourrait changer le nom de la
    /// classe pour quelque chose de moins précis que LesFabriques !)
    /// </summary>
    static class LesFabriques
    {
        static Dictionary<Type, IFabriqueCommande> m_fabriques = new Dictionary<Type, IFabriqueCommande>();

        public static void Ajouter(Type p_typeArticle, IFabriqueCommande p_fabrique) 
            => m_fabriques.Add(p_typeArticle, p_fabrique);

        public static IFabriqueCommande FabriqueDe(Type p_type) 
            => m_fabriques[p_type];

        public static void CompléterLeMenu(ToolStripMenuItem p_menu, EventHandler p_eh)  // Pas très « fabrique », et pas vraiment général (c'est lié au menu WinForm, etc.). 
        {                                                                                // Pour faire plus propre, on pourrait proposer un itérateur dans les fabriques, ou un modèle Visiteur...
            foreach (var fab in m_fabriques)                                             // Mais on n'a pas vu ces patrons pour le moment...
            {
                var tsi = new ToolStripMenuItem(fab.Value.DescriptionPourMenu(), null, p_eh);
                tsi.Tag = fab.Value; // .Key est l'identificateur, donc .Value est la fabrique
                p_menu.DropDownItems.Add(tsi);
            }
        }
    }

    /// <summary>
    /// Abstract Factory pour des commandes, et un peu plus (la description de la commande pour le menu).
    /// </summary>
    public interface IFabriqueCommande
    {
        string DescriptionPourMenu();

        ICommande CréerCommandeAjouter();
        ICommande CréerCommandeModifier(ArticlePhilatélique p_article);
        ICommande CréerCommandeSupprimer(ArticlePhilatélique p_article);
    }
}