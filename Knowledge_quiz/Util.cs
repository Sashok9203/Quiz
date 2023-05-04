
using System.Security.Cryptography;
using System.Text;


namespace KnowledgeQuiz
{
    public  static class Util
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
                Util.Swap(ref arr[i], ref arr[rnd.Next(0, arr.Length)]);
            return arr;
        }

        public static T Max<T>(params T[] values) where T : struct => values.Max();
    }
}
