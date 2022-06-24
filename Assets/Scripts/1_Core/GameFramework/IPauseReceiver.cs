using System.Collections.Generic;
using System;

namespace GameFramework
{
    public interface IPauseReceiver
    {
        public void OnPaused();
        public void OnUnpaused();
    }

    [Serializable]
    public class PauseEntitiesRegister
    {
        public List<Entity> list;

        public void RegisterPause(Entity entity)
        {
            if (entity is IPauseReceiver pauseReceiver)
            {
                list.Add(entity);
                if (entity.IsPaused())
                {
                    pauseReceiver.OnPaused();
                }
            }
        }

        public void UnregisterPause(Entity entity)
        {
            if (entity != null && entity is IPauseReceiver)
            {
                list.Remove(entity);
            }
        }

        public void Pause()
        {
            foreach (var entity in list)
            {
                if (entity != null && entity is IPauseReceiver pauseReceiver)
                {
                    pauseReceiver.OnPaused();
                }
            }
        }

        public void Unpause()
        {
            foreach (var entity in list)
            {
                if (entity != null && entity is IPauseReceiver pauseReceiver)
                {
                    pauseReceiver.OnUnpaused();
                }
            }
        }
    }
}