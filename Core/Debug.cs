using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace AMS2_core.Core
{
    public class Debug
    {
        public void CheckCategoriesInfoObject(Dictionary<string, List<string>> categoriesInfo)
        {

            //var a = categoriesInfo["root"];

            foreach (var categorie in categoriesInfo.Keys/*.Skip(2).Where(x => !x.Contains("20"))*/)
            {
                Console.WriteLine();

                foreach (var element in categoriesInfo[categorie])
                {
                    Console.WriteLine(element);
                }
            }
        }
    }
}
