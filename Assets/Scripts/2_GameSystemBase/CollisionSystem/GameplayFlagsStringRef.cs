using UtilsEditor;

namespace CollisionSystem
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