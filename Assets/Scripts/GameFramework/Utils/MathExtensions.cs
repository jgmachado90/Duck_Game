namespace GameFramework
{
    public static class MathExtensions
    {
        public static float SignWithZero(float value)
        {
            return value < 0 ? -1 : (value > 0 ? 1 : 0);
        }
    }
}