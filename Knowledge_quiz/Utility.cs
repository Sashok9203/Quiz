﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace KnowledgeQuiz
{
    public  static class Utility
    {
        public static void Swap<T>(ref T val1, ref T val2) 
        {
            if (val1?.Equals(val2) ?? false) return;
            (val2, val1) = (val1, val2);
        }

        public static IEnumerable<T> Shufflet<T>(IEnumerable<T> array)
        {
            var arr = array.ToArray();
            Random rnd = new Random();
            for (int i = 0; i < arr.Length; i++)
                Utility.Swap(ref arr[i], ref arr[rnd.Next(0, arr.Length)]);
            return arr;
        }

        public static string GetHash(string? str)
        {
            SHA256 sha256Hash = SHA256.Create();
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str ?? ""));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        public static bool HashCompare(string? str, string? hash) => hash?.CompareTo(GetHash(str ?? "")) == 0;
    }
}
