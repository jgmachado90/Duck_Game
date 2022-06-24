using System;
using System.Collections.Generic;
using UtilsEditor;

namespace CollisionSystem
{
    [Serializable]
    public class GameplayFlags {
        [StringRef(typeof(GuidRefSource))]
        public List<string> flags;

        public bool ContainsFlag(string flag) {
            if (flags.Count <= 0) return true;
            return flags.Contains(flag);
        }
        
        public bool ContainsOneFlag(GameplayFlags gameplayFlags) {
            if (flags.Count <= 0) return true;
            var otherFlags = gameplayFlags.flags;
            for (var i = 0; i < otherFlags.Count; i++) {
                var item = otherFlags[i];
                if (ContainsFlag(item)) {
                    return true;
                }
            }
            return false;
        }
    }

    [Serializable]
    public class DamageFlag{
        [GuidRefName(nameof(flag),false)]
        public string name; 
        [StringRef(typeof(GuidRefSource))]
        public string flag;
        public float damageMultiplier = 1;
    }
    
    [Serializable]
    public class DamageMultiplierFlags {
        public List<DamageFlag> flags = new List<DamageFlag>();

        public float GetDamageMultiplier(string flag) {
            DamageFlag first = null;
            foreach (var damageFlag in flags) {
                if (damageFlag.flag == flag) {
                    first = damageFlag;
                    break;
                }
            }

            if (first != null) {
                return first.damageMultiplier;
            }
            return 1;
        }
        
        public float GetDamageMultiplier(List<string> flags) {
            var otherFlags = flags;
            for(var i = 0; i < otherFlags.Count; i++) {
                var item = otherFlags[i];
                var value = GetDamageMultiplier(item);
                if(value != 1) {
                    return value;
                }
            }
            return 1;
        }
    }
}
