using Philatel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_PNC
{
    public class PlancheNonCoupée : ArticlePhilatélique
    {
        private object p_nomDesigner;

        public PlancheNonCoupée(int p_numéro, string p_motif, DateTime? p_parution, 
            double p_prixPayé, int p_nombreTimbres, double p_valeurPlanche, int p_nombreTimbreDifférent,
            string p_nomDesigner, int p_nombreLigne, int p_nombreColonne) 
            : base(p_numéro, p_motif, p_parution, p_prixPayé)
        {
            ValeurPlanche = p_valeurPlanche;
            NombreTimbre = p_nombreTimbres;
            NombreTimbreDifférent = p_nombreTimbreDifférent;
            NomDesigner = p_nomDesigner;
            NombreLigne = p_nombreLigne;
            NombreColonne = p_nombreColonne;
        }

        public double ValeurPlanche { get; }
        public int NombreTimbre { get; }
        public int NombreTimbreDifférent { get; }
        public string NomDesigner { get; }
        public int NombreLigne { get; }
        public int NombreColonne { get; }

        public override string Catégorie => "Planche de timbres non coupées";

        public override string Description() => Catégorie;

        public override string ValeurNominale()
        {
            return ValeurPlanche.ToString();
        }
    }
}
