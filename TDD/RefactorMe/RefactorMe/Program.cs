namespace RefactorMe;

class Program
{
    static string res = "";
    static int[] arr = new int[1000];
    static int cnt = 0;

    static void Main(string[] args)
    {
        bool r = true;
        while (r)
        {
            Console.WriteLine("============================");
            Console.WriteLine("1. Factorial");
            Console.WriteLine("2. Fibonacci");
            Console.WriteLine("3. Primes");
            Console.WriteLine("4. Sort Characters");
            Console.WriteLine("5. Exit");
            Console.WriteLine("============================");
            Console.Write("Choose option: ");
            string? inp = Console.ReadLine();

            if (inp == "1")
            {
                Console.Write("Enter a number: ");
                string? s = Console.ReadLine();
                int n = -1;
                try { n = int.Parse(s!); } catch { }
                if (n < 0)
                {
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    long result = 1;
                    for (int i = 2; i < n; i++)
                    {
                        result = result * i;
                    }
                    // duplicated output formatting
                    res = ">>> Result: " + result.ToString();
                    Console.WriteLine("============================");
                    Console.WriteLine(res);
                    Console.WriteLine("============================");
                }
            }
            else if (inp == "2")
            {
                Console.Write("Enter a number: ");
                string? s = Console.ReadLine();
                int n = -1;
                try { n = int.Parse(s!); } catch { }
                if (n < 0)
                {
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    // using static array and counter instead of a list
                    cnt = 0;
                    int a = 0;
                    int b = 1;
                    while (a < n)
                    {
                        arr[cnt] = a;
                        cnt = cnt + 1;
                        int temp = a;
                        a = b;
                        b = temp + b;
                    }
                    // building result string manually
                    res = ">>> Result: ";
                    for (int i = 0; i < cnt; i++)
                    {
                        if (i > 0)
                        {
                            res = res + ", ";
                        }
                        res = res + arr[i].ToString();
                    }
                    // duplicated output formatting
                    Console.WriteLine("============================");
                    Console.WriteLine(res);
                    Console.WriteLine("============================");
                }
            }
            else if (inp == "3")
            {
                Console.Write("Enter a number: ");
                string? s = Console.ReadLine();
                int n = -1;
                try { n = int.Parse(s!); } catch { }
                if (n < 0)
                {
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    cnt = 0;
                    for (int i = 3; i <= n; i++)
                    {
                        bool ip = true;
                        for (int j = 2; j <= i - 1; j++)
                        {
                            if (i % j == 0)
                            {
                                ip = false;
                                break;
                            }
                        }
                        if (ip)
                        {
                            arr[cnt] = i;
                            cnt = cnt + 1;
                        }
                    }
                    // building result string manually — duplicated from Fibonacci
                    res = ">>> Result: ";
                    for (int i = 0; i < cnt; i++)
                    {
                        if (i > 0)
                        {
                            res = res + ", ";
                        }
                        res = res + arr[i].ToString();
                    }
                    // duplicated output formatting
                    Console.WriteLine("============================");
                    Console.WriteLine(res);
                    Console.WriteLine("============================");
                }
            }
            else if (inp == "4")
            {
                Console.Write("Enter a string: ");
                string? s = Console.ReadLine();
                if (s == null || s == "")
                {
                    Console.WriteLine("Invalid input.");
                }
                else
                {
                    // manual bubble sort instead of using built-in sort
                    char[] c = s.ToCharArray();
                    for (int i = 0; i < c.Length; i++)
                    {
                        for (int j = 0; j < c.Length - 1; j++)
                        {
                            if (c[j] < c[j + 1])
                            {
                                char tmp = c[j];
                                c[j] = c[j + 1];
                                c[j + 1] = tmp;
                            }
                        }
                    }
                    res = ">>> Result: " + new string(c);
                    // duplicated output formatting
                    Console.WriteLine("============================");
                    Console.WriteLine(res);
                    Console.WriteLine("============================");
                }
            }
            else if (inp == "5")
            {
                r = false;
                Console.WriteLine("Goodbye!");
            }
            else
            {
                Console.WriteLine("Invalid option.");
            }
        }
    }
}
