using Pixel;

namespace GameFramework
{
    public class GameplayFlagsStringRef : StringRefSource
    {
        public override void Populate(StringRefList list)
        {
#if UNITY_EDITOR
            list.AddEmpty();
            list.AddSeparator();

            foreach (var item in StringList.Load("Flags"))
            {
                list.Add(item);
            }
#endif
        }
    }
}

// public override string GetComparisonValue(string value)
    // {
    //     return value.Trim().ToLowerInvariant();
    // }