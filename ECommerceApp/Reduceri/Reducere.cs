using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Reduceri
{
    internal class Reducere
    {
        protected readonly string connString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source= Provider=Microsoft.ACE.OLEDB.12.0;Data Source=E:\VisualStudioCommunityProjects\ProiectPOO\ECommerceApp\ECommerceApp.accdb";

        public virtual void AplicareReducereProdus()
        {
            Console.WriteLine("Reducere");
        }
        
        public virtual void AplicareReducereCategorie()
        {
            Console.WriteLine("Reducere");
        }
        protected static string PrimaLitera(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;
                       
            return char.ToUpper(text[0]) + text.Substring(1).ToLower();
        }

    }
}
