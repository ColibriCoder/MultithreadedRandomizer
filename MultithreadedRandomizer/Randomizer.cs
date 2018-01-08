using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultithreadedRandomizer
{
    class Randomizer
    {
        private const string allowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";

        private static readonly Random _global = new Random();
        [ThreadStatic] private static Random _local;

        public Randomizer()
        {
            if (_local == null)
            {
                int seed;
                lock (_global)
                {
                    seed = _global.Next();
                }
                _local = new Random(seed);
            }
        }

        public int GetRandomInt(int min, int max)
        {
            return _local.Next(min, max);
        }

        public string generateRandomString(int min, int max)
        {
            int strLenght = GetRandomInt(min, max + 1);
            char[] chars = new char[strLenght];
            for (int i = 0; i < strLenght; i++)
                chars[i] = allowedChars[_local.Next(0, allowedChars.Length)];

            return new string(chars);
        }
    }
}
