using System;
using System.Collections;
using System.Collections.Generic;

namespace Piles
{
    // Non fonctionnelle et pas utilisée dans le projet, nous utilisons une stack

    public class PileLimitée<T> : IEnumerable<T>
    {
        const int limiteMax = 100;
        private T[] Valeurs;
        private int dessus = -1;

        public PileLimitée()
        {
            Capacité = limiteMax;
            Valeurs = new T[Capacité];
        }

        public PileLimitée(int limite)
        {
            if (limite <= 0)
                throw new ArgumentOutOfRangeException();

            Capacité = limite;
            Valeurs = new T[Capacité];
        }

        public int Capacité { get; private set; }
        public int NbÉléments { get => dessus == -1 ? 0 : dessus + 1; }
        public bool Pleine { get => NbÉléments == Capacité; }
        public bool Vide { get => dessus == -1; }

        public bool Empiler(T valeur)
        {
            if (Pleine)
            {
                for (int i = 0; i < Capacité - 1; i++)
                {
                    Valeurs[i] = Valeurs[i + 1];
                }
                
                Valeurs[dessus] = valeur;
                return true;
            }
            else
            {
                ++dessus;
                Valeurs[dessus] = valeur;
                return false;
            }
        }

        public T Dépiler()
        {
            if (Vide)
                throw new InvalidOperationException();

            var valeur = Valeurs[dessus];
            Valeurs[dessus] = default(T);
            dessus--;

            return valeur;
        }

        public void Vider()
        {
            dessus = -1;
        }

        public ICollection ToArray()
        {
            var array = new T[NbÉléments];
            int cpt = NbÉléments - 1;

            for (int i = 0; i < NbÉléments; i++)
            {
                array[i] = Valeurs[cpt];
                cpt--;
            }

            return array;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ItérateurPile(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class ItérateurPile : IEnumerator<T>
        {
            PileLimitée<T> pile;
            private T[] valeurs;
            private int cpt;


            internal ItérateurPile(PileLimitée<T> pileLimité)
            {
                pile = pileLimité;
                this.valeurs = pileLimité.Valeurs;
                Reset();
            }

            public T Current
            {
                get
                {
                    return valeurs[cpt];
                }
            }

            object IEnumerator.Current => this.Current;

            public bool MoveNext()
            {
                if (pile.Vide)
                {
                    return false;
                }
                return (++cpt) < pile.NbÉléments;
            }

            public void Reset()
            {
                this.cpt = -1;
            }

            #region IDisposable Support
            private bool disposedValue = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!disposedValue)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    disposedValue = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~ItérateurPile() {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
            #endregion
        }
    }
}
