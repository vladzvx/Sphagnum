using System.Security.Cryptography;
using System.Text;

namespace Sphagnum.Common.Utils
{
    internal static class HashCalculator
    {
        private readonly static SHA256 _hash = SHA256.Create();
        public static byte[] Calc(string text)
        {
            return _hash.ComputeHash(Encoding.UTF8.GetBytes(text));
        }
    }
}
