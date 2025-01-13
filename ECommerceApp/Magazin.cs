using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp
{
    internal class Magazin
    {

        public void DeschidereAplicatie()
        {
            int identitate = IdentitateUtilizator();
            if(identitate == 0)
            {
                MagazinClient magazinClient = new MagazinClient();
                magazinClient.MagazinC();
            }
            else if(identitate == 1) 
            {
                MagazinAdmin magazinAdmin = new MagazinAdmin();
                magazinAdmin.Magazin();
            }
            else
            {
                Console.WriteLine("Nu puteti accesa aplicatia!");
            }
        }
        private int IdentitateUtilizator()
        {
            Console.WriteLine("Sunteti client? ");
            string raspuns = Console.ReadLine().ToLower();
            if (raspuns == "da")
                return 0;
            Console.WriteLine("Sunteti administrator?");
            raspuns = Console.ReadLine();
            if (raspuns == "da")
                return 1;
            return -1;
        }
    }
}
