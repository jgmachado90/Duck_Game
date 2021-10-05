using UnityEngine;

namespace GameFramework
{
    public class Entity : MonoBehaviour
    {
        private LevelManager _ownerLevel;

        public LevelManager OwnerLevel
        {
            get
            {
                if (!_ownerLevel && transform.parent)
                {
                    var parent = transform.parent.GetComponentInParent<Entity>();
                    if (parent)
                    {
                        _ownerLevel = parent.OwnerLevel;
                        OnSetOwnerLevel();
                    }
                }

                return _ownerLevel;
            }

            set
            {
                _ownerLevel = value;
                OnSetOwnerLevel();
            }
        }

        private void OnSetOwnerLevel()
        {
            _ownerLevel.Entities.Add(this);
            OnPostSetOwnerLevel();
        }

        public virtual void OnPostSetOwnerLevel()
        {

        }

        public virtual void OnDestroy()
        {
            if (_ownerLevel)
                _ownerLevel.Entities.Remove(this);
        }
    }
}