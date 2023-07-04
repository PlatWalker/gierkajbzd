namespace jbzd.Common.Extensions
{
    public static class IntExtensionScript
    {
        /// <summary>
        /// remaping range of values of type int to new float type range value
        /// </summary>
        public static float Remap(this int from, float fromMin, float fromMax, float toMin, float toMax)
        {
            float fromAbs = (float)from - fromMin;
            float fromMaxAbs = fromMax - fromMin;

            float normal = fromAbs / fromMaxAbs;

            float toMaxAbs = toMax - toMin;
            float toAbs = toMaxAbs * normal;

            float to = toAbs + toMin;

            return to;
        }

    }
}
