using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Philatel
{
    public struct FabriqueEtArticle
    {
        public IFabriqueCommande Fabrique;
        public ArticlePhilatélique Article;

        public FabriqueEtArticle(IFabriqueCommande p_fabrique, ArticlePhilatélique p_article)
        {
            Fabrique = p_fabrique;
            Article = p_article;
        }
    }
}
