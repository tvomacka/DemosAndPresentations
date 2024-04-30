namespace RefactorMe
{
    internal class Exercise3
    {
        /*
         * Cviceni 3
         *
         * 1. Vyzkoumejte, co je ucelem metod Applesauce a Applesauce2.
         * 2. Pokryjte 100 % metod jednotkovymi testy.
         * 3. Provedte refactoring metod tak, aby byly funkcionalni a odstrante code smells.
         */

        public static void EntryPoint()
        {
            var a = new int[] { 1, 2, 3, 4, 5 };
            Applesauce2(a);

            Console.WriteLine(string.Join(", ", a));
        }

        public static void Applesauce(int[] aaa, int[] a, int[] b)
        {
            int i = 0;
            int j = 0;
        
            while ((i < a.Length) && (j < b.Length))
            { 
                if (a[i] < b[j])
                {
                    aaa[i + j] = a[i];
                    i++;
                }
                else
                {
                    aaa[i + j] = b[j];
                    j++;
                }
            }
            if (i < a.Length)
            {
                while (i < a.Length)
                {
                    aaa[i + j] = a[i];
                    i++;
                }
            }
            else
            {
                while (j < b.Length)
                {
                    aaa[i + j] = b[j];
                    j++;
                }
            }
        }

        public static void Applesauce2(int[] intArray)
        {
            if (intArray.Length <= 1) 
                return;
            int a = intArray.Length / 2; 
            int[] b = new int[a]; 
            for (int i = 0; i < a; i++)
                b[i] = intArray[i];
            int[] c = new int[intArray.Length - a]; 
            for (int i = a; i < intArray.Length; i++)  
                c[i - a] = intArray[i];
            Applesauce2(b); // 
            Applesauce2(c);
            Applesauce(intArray, b, c); 
        }

        #region Hint

        // https://www.itnetwork.cz/algoritmy/razeni/algoritmus-merge-sort-trideni-cisel-podle-velikosti

        #endregion
    }
}
