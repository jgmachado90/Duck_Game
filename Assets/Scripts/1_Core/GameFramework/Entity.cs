using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class Entity : MonoBehaviour
    {
        private LevelManager _ownerLevel;
        private bool _setOwnerLevelFirstTime;

        public LevelManager OwnerLevel
        {
            get
            {
                if (this == null) return null;
                if (_ownerLevel || _setOwnerLevelFirstTime) return _ownerLevel;
                _ownerLevel = SearchValidOwnerLevelInParent();
                _setOwnerLevelFirstTime = true;
                if (_ownerLevel != null) OnSetOwnerLevel();
                return _ownerLevel;
            }

            set
            {
                _setOwnerLevelFirstTime = true;
                _ownerLevel = value;
                OnSetOwnerLevel();
            }
        }

        private void OnSetOwnerLevel()
        {
            _ownerLevel.AddEntity(this);
            OnPostSetOwnerLevel();
        }

        protected virtual void OnPostSetOwnerLevel()
        {

        }

        public virtual void OnBeforeDestroyLevel()
        {

        }

        public virtual void OnDestroy()
        {
            if (_ownerLevel) _ownerLevel.RemoveEntity(this);
        }

        private LevelManager SearchValidOwnerLevelInParent()
        {
            if (this == null) return null;
            var result = gameObject.GetComponent<Entity>();
            var search = result._ownerLevel == null && transform.parent;
            return search ? SearchValidOwnerLevelInParent(transform.parent) : result._ownerLevel;
        }

        private LevelManager SearchValidOwnerLevelInParent(Transform target)
        {
            if (target == null) return null;
            var parentEntity = target.GetComponent<Entity>();
            if (parentEntity != null && parentEntity._ownerLevel != null) return parentEntity._ownerLevel;
            if (target.parent == null) return null;
            var resultLevelManager = SearchValidOwnerLevelInParent(target.parent);
            if (parentEntity && resultLevelManager) parentEntity.OwnerLevel = resultLevelManager;
            return resultLevelManager;
        }
    }
}
