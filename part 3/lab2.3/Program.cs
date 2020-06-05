﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2._3
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Input the first value:");
            float numb1 = float.Parse(Console.ReadLine());
            Console.WriteLine("Input the second value:");
            float numb2 = float.Parse(Console.ReadLine());
            CalculateResult(numb1, numb2);
            Console.ReadKey();
        }

        /// <summary>
        /// Calculate result
        /// </summary>
        /// <param name="numb1"></param>
        /// <param name="numb2"></param>
        static void CalculateResult(float numb1, float numb2)
        {
            int bias = 127;

            bool is_adding = numb1 * numb2 >= 0;
            if (Math.Abs(numb2) > Math.Abs(numb1))
            {
                float temp;
                temp = numb1;
                numb1 = numb2;
                numb2 = temp;
            }
            Console.WriteLine();
            Console.WriteLine("{0} + {1} ", numb1, numb2);
            Console.WriteLine();
            int a_sign_bit = numb1 < 0 ? 1 : 0,
                b_sign_bit = numb2 < 0 ? 1 : 0;
            numb1 = Math.Abs(numb1);
            numb2 = Math.Abs(numb2);

            int a_int_bits = (int)numb1,
                b_int_bits = (int)numb2;
            numb1 -= a_int_bits;
            numb2 -= b_int_bits;

            FloatingBits(numb1, out int a_float_bits);
            FloatingBits(numb2, out int b_float_bits);
            int exp_a = NormalizeIncomingByte(a_int_bits, ref a_float_bits),
                exp_b = NormalizeIncomingByte(b_int_bits, ref b_float_bits);

            byte exponent_a = (byte)(exp_a + bias),
                exponent_b = (byte)(exp_b + bias);

            string a_float_bits_string = ConvertToBinnaryString(a_sign_bit, exponent_a, a_float_bits);
            Console.WriteLine("First value in normalize view:SIGN||EXPONENT||MANTISSA\n{0}", a_float_bits_string);
            Console.WriteLine("Second value in normalize view:SIGN||EXPONENT||MANTISSA\n{0}", ConvertToBinnaryString(b_sign_bit, exponent_b, b_float_bits));
            Console.WriteLine();
            b_float_bits >>= exp_a - exp_b;
            string b_float_bits_string = ConvertToBinnaryString(b_sign_bit, exponent_b, b_float_bits);
            Console.WriteLine("Shift left second value on {0}:\n{1}", exp_a - exp_b, b_float_bits_string);
            Console.WriteLine("Adding first value to second value:\n {0}\n+\n {1}", a_float_bits_string, b_float_bits_string);

            int result = is_adding ? a_float_bits + b_float_bits : a_float_bits - b_float_bits;
            NormilizeResultingVal(ref result, ref exp_a, is_adding);
            exponent_a = (byte)(exp_a + bias);
            Console.WriteLine();
            Console.WriteLine("Result\n {1}\nIn decimal: {0}",
                ConvertToDecimal(exp_a, result, a_sign_bit), ConvertToBinnaryString(a_sign_bit, exponent_a, result));
        }

        /// <summary>
        /// Calculate 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="float_bits"></param>
        static void FloatingBits(float value, out int float_bits)
        {
            const int amount_of_mantisa_bits = 23;
            int i = 0;

            float_bits = 0;
            while (value != 0 && i < 22)
            {
                value *= 2;
                if (value >= 1)
                {
                    float_bits |= 1;
                    value -= 1;
                }
                float_bits <<= 1;
                ++i;
            }
            float_bits <<= amount_of_mantisa_bits - i - 1;
        }

        /// <summary>
        /// Normalize incoming value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="float_bits"></param>
        /// <returns></returns>
        static int NormalizeIncomingByte(int value, ref int float_bits)
        {
            int exp = 0;
            int hidden_one = 1 << 23;

            if (value > 0)
            {
                while (value > 1)
                {
                    ++exp;
                    float_bits >>= 1;
                    float_bits |= (value & 1) << 22;
                    value >>= 1;
                }
                float_bits |= hidden_one;
            }

            return exp;
        }

        /// <summary>
        /// Normalize resulting value
        /// </summary>
        /// <param name="result"></param>
        /// <param name="exp"></param>
        /// <param name="is_adding"></param>
        static void NormilizeResultingVal(ref int result, ref int exp, bool is_adding)
        {
            int hidden_one = 1 << 23;

            if ((result & hidden_one) == hidden_one)
            {
                ++exp;
                result >>= 1;
                return;
            }

            if (is_adding)
                do
                {
                    ++exp;
                    result >>= 1;
                } while ((result & hidden_one) != hidden_one);
            else
                do
                {
                    --exp;
                    result <<= 1;
                } while ((result & hidden_one) != hidden_one);
        }

        /// <summary>
        /// Binary Result to string
        /// </summary>
        /// <param name="sign_bit"></param>
        /// <param name="exponent"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        static string ConvertToBinnaryString(int sign_bit, byte exponent, int result)
        {
            string result_string = sign_bit + "||";
            for (int i = 7; i >= 0; --i)
                result_string += exponent >> i & 1;
            result_string += "||";
            for (int i = 22; i >= 0; --i)
                result_string += result >> i & 1;

            return result_string;
        }

        /// <summary>
        /// Convert int value to decimal To Decimal
        /// </summary>
        /// <param name="exp_a"></param>
        /// <param name="mantissa"></param>
        /// <param name="sign_bit"></param>
        /// <returns></returns>
        static float ConvertToDecimal(int exp_a, int mantissa, int sign_bit)
        {
            float result = 0,
                multiplier = (float)Math.Pow(2, exp_a);


            for (int i = 23; i >= 0; --i, multiplier /= 2)
                result += multiplier * (mantissa >> i & 1);
            if (sign_bit == 1)
                result = -result;
            return result;
        }
    }
}