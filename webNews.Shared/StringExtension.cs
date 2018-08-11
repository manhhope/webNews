using ServiceStack;

namespace ZP.AnPos.Shared
{
    public static class StringExtension
    {
        public static T GetFromJson<T>(this string t)
        {
            return t.FromJson<T>();
        }
    }
}