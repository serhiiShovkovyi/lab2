using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2._1
{
    class Program
    {
        static void Main(string[] args)
        {
            int mult1 = 0, mult2 = 0;
            Console.Write("Enter the first number in base 10: ");
            mult1 = int.Parse((Console.ReadLine()));
            Console.Write("Enter the second number in base 10: ");
            mult2 = int.Parse((Console.ReadLine()));
            Console.WriteLine();

            BoothAlgorithmFunction(mult1, mult2);
        }

        /// <summary>
        /// Change chars
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        static void SwapChars<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        /// <summary>
        /// Algorithm function
        /// </summary>
        /// <param name="numb1"></param>
        /// <param name="numb2"></param>
        /// <returns></returns>
        static int BoothAlgorithmFunction(int numb1, int numb2)
        {
            int m = numb1, r = numb2;

            List<int> a = new List<int>();
            List<int> s = new List<int>();
            List<int> p = new List<int>();

            GetTwoPointKbyteNum(m, r, out a, out p, out s);

            int x = a.Count;
            int y = p.Count;

            AddBitsToListWhileCount(a, y + 1);
            AddBitsToListWhileCount(s, y + 1);
            ComplementForListForPoint(p, x);

            Console.WriteLine($"a: \t\t{TranslateBitsToString(a)}");
            Console.WriteLine($"s: \t\t{TranslateBitsToString(s)}");
            Console.WriteLine($"p: \t\t{TranslateBitsToString(p)}");
            Console.WriteLine();

            for (int i = 0; i < y; i++)
            {
                if (p[p.Count - 1] == 1 && p[p.Count - 2] == 0)
                {
                    AddBinaryToList(p, a);
                    Console.WriteLine($"{i+1}. ADD (p + a) \t{TranslateBitsToString(p)}");
                }
                else if (p[p.Count - 1] == 0 && p[p.Count - 2] == 1)
                {
                    AddBinaryToList(p, s);
                    Console.WriteLine($"{i+1}. SUB (p + s) \t{TranslateBitsToString(p)}");
                }
                else
                {
                    Console.WriteLine($"{i+1}. NOP");
                }
                RightShift(p);
                Console.WriteLine($"right shift\t{TranslateBitsToString(p)}");
            }
            p.RemoveAt(p.Count - 1);
            Console.WriteLine($"Result \t\t{TranslateBitsToString(p)}");
            int result = GetNumbFromTwoComplementNumber(p);
            Console.WriteLine($"\nResult in base 10: {result}");
            return result;
        }

        /// <summary>
        /// Add result to the list
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        private static void ComplementForListForPoint(List<int> p, int x)
        {
            List<int> helper = new List<int>() { 0 };
            while (x > 0)
            {
                helper.AddRange(p);
                p.Clear();
                p.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
                x--;
            }
            p.Add(0);
        }

        /// <summary>
        /// Get number from 2 bin chars
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        static int GetNumbFromTwoComplementNumber(List<int> p)
        {
            int result = -p[0] * (int)Math.Pow(2, p.Count - 1);
            for (int i = p.Count - 1; i > 0; i--)
            {
                result += p[i] * (int)Math.Pow(2, p.Count - i - 1);
            }
            return result;
        }

        /// <summary>
        /// change bits to string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        static string TranslateBitsToString(List<int> list)
        {
            string result = "";

            for (int i = 0; i < list.Count; i++)
            {
                if (i % 8 == 0 && i != 0)
                    result += " ";
                result += list[i];
            }
            return result;
        }

        /// <summary>
        /// Add binary value to the list
        /// </summary>
        /// <param name="p"></param>
        /// <param name="aOrs"></param>
        static void AddBinaryToList(List<int> p, List<int> aOrs)
        {
            int mind = 0;
            for (int i = p.Count - 1; i >= 0; i--)
            {
                if ((p[i] == 0 && aOrs[i] == 1 && mind == 0) || (p[i] == 1 && aOrs[i] == 0 && mind == 0) || (p[i] == 0 && aOrs[i] == 0 && mind == 1))
                {
                    p[i] = 1;
                    mind = 0;
                }
                else if ((p[i] == 0 && aOrs[i] == 1 && mind == 1) || (p[i] == 1 && aOrs[i] == 0 && mind == 1) || (p[i] == 1 && aOrs[i] == 1 && mind == 0))
                {
                    p[i] = 0;
                    mind = 1;
                }
                else if (p[i] == 0 && aOrs[i] == 0 && mind == 0)
                {
                    p[i] = 0;
                    mind = 0;
                }
                else
                {
                    p[i] = 1;
                    mind = 1;
                }
            }
        }

        /// <summary>
        /// Shift to the right
        /// </summary>
        /// <param name="p"></param>
        static void RightShift(List<int> p)
        {
            for (int i = p.Count - 1; i > 0; i--)
            {
                p[i] = p[i - 1];
            }
        }

        /// <summary>
        /// Add bit to the list
        /// </summary>
        /// <param name="bits"></param>
        /// <param name="count"></param>
        static void AddBitsToListWhileCount(List<int> bits, int count)
        {
            while (count > 0)
            {
                bits.Add(0);
                count--;
            }
        }

        /// <summary>
        /// Get bits from number
        /// </summary>
        /// <param name="numb"></param>
        /// <returns></returns>
        static List<int> GetBits(int numb)
        {
            List<int> bits = new List<int>();
            if (numb > 0)
            {
                while (numb != 0)
                {
                    bits.Add(numb % 2);
                    numb = numb / 2;
                }
                bits.Reverse();
            }
            else
            {
                bits = GetBits(-numb);
                Reverse(bits);
                AddByteToTheList(bits);
                List<int> addOne = new List<int>() { 1 };
                addOne.AddRange(bits);
                return addOne;
            }
            return bits;
        }

        /// <summary>
        /// Get 2 pivot bit number
        /// </summary>
        /// <param name="numb1"></param>
        /// <param name="numb2"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <param name="invertFirst"></param>
        static void GetTwoPointKbyteNum(int numb1, int numb2, out List<int> first, out List<int> second, out List<int> invertFirst)
        {
            first = GetBits(numb1);
            second = GetBits(numb2);
            invertFirst = GetBits(-numb1);

            int maxCount = second.Count;
            if (invertFirst.Count > first.Count && invertFirst.Count > second.Count)
            {
                maxCount = invertFirst.Count;
            }
            else if (first.Count > second.Count)
            {
                maxCount = first.Count;
            }
            int lenght = (int)Math.Log(maxCount, 2) + 1;
            lenght = (int)Math.Pow(2, lenght);

            int elem1 = (numb1 < 0) ? 1 : 0;
            List<int> helper = new List<int>() { elem1 };
            int countOfElem = first.Count;
            for (int i = 0; i < lenght - countOfElem; i++)
            {
                helper.AddRange(first);
                first.Clear();
                first.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
            }
            int elem2 = (numb2 < 0) ? 1 : 0;
            helper = new List<int>() { elem2 };
            countOfElem = second.Count;
            for (int i = 0; i < lenght - countOfElem; i++)
            {
                helper.AddRange(second);
                second.Clear();
                second.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
            }
            int elem3 = ((-numb1) < 0) ? 1 : 0;
            countOfElem = invertFirst.Count;
            helper = new List<int>() { elem3 };
            for (int i = 0; i < lenght - countOfElem; i++)
            {
                helper.AddRange(invertFirst);
                invertFirst.Clear();
                invertFirst.AddRange(helper);
                helper.RemoveRange(1, helper.Count - 1);
            }
        }

        /// <summary>
        /// reversing
        /// </summary>
        /// <param name="bits"></param>
        static void Reverse(List<int> bits)
        {
            for (int i = 0; i < bits.Count; i++)
            {
                if (bits[i] == 0)
                {
                    bits[i] = 1;
                }
                else
                {
                    bits[i] = 0;
                }
            }
        }

        /// <summary>
        /// add new bit
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        static List<int> AddByteToTheList(List<int> bits)
        {
            int mind = 1;
            for (int i = bits.Count - 1; i >= 0; i--)
            {
                if (i == 0 && bits[i] == 1 && mind == 1)
                {
                    List<int> newBits = new List<int>();
                    newBits.AddRange(new List<int>() { 1, 1 });
                    newBits.AddRange(bits);
                    return newBits;
                }
                else if (bits[i] == 0 && mind == 1)
                {
                    bits[i] = 1;
                    mind = 0;
                }
                else if (bits[i] == 1 && mind == 1)
                {
                    bits[i] = 0;
                }
            }
            return bits;
        }

    }
}
