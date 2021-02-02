using System;
using System.Collections.Generic;
using System.Text;

namespace Engine
{
    public static class Rng
    {
        private static Random random = new Random();

        public static float Value => (float)random.NextDouble();

        public static int Range(int min, int max)
        {
            return random.Next(min, max);
        }
        public static double Range(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
        public static float Range(float minimum, float maximum)
        {
            return (float)Range(minimum, maximum);
        }
    }
}
