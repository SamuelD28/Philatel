using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Philatel
{
    [Serializable]
    public abstract class ArticlePhilatélique
    {
        public ArticlePhilatélique(int p_numéro, string p_motif, DateTime? p_parution, double p_prixPayé)
        {
            Numéro = p_numéro;
            Motif = p_motif;
            Parution = p_parution;
            PrixPayé = p_prixPayé;
        }

        public int Numéro  { get; }
        public string Motif { get; }
        public DateTime? Parution  { get; }
        public double PrixPayé { get; }

        /// <summary>
        /// Met des \r\n entre les lignes...
        /// </summary>
        /// <returns></returns>
        public override string ToString()  // DP Template Method 
        {                                                                  // Exemple :
            return Description() + "\n" +                                  // Bloc de coin, coin sup-gauche
                   ValeurNominale() + "\n" +                               // 4 x 52 ¢
                   Motif + "\n" +                                          // Face de la Reine
                   "Paru le " +
                   (Parution.HasValue ? Parution.Value.ToShortDateString() // Paru le 2007-08-25
                                      : "(inconnu)") + "\n" +
                   $"Payé {PrixPayé:C}";                                   // Payé 2,27 $
        }

		public override int GetHashCode()
		{
			return	Numéro * 7 +
					Motif.Length * 11 +
					Parution.Value.DayOfYear * 17 +
					(int)PrixPayé * 23;
		}

		public override bool Equals(object obj)
		{
			if (obj is null)
				return false;

			if(obj is ArticlePhilatélique)
			{
				ArticlePhilatélique temp = (ArticlePhilatélique)obj;
				return	temp.Numéro == Numéro && 
						temp.Motif == Motif && 
						temp.Parution == Parution && 
						temp.PrixPayé == PrixPayé;
			}

			return false ;
		}

		public abstract string Description();
        public abstract string Catégorie { get; }
        public abstract string ValeurNominale();
    }
}
