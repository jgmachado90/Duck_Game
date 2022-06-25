using System.Collections;
using System.Collections.Generic;
using UnityEngine.Playables;
using GameFramework;

public class TimelinePlayer : Entity, ISubsystem<LevelManager>
{
    public LevelManager OwnerManager { get; set; }

    public List<PlayableDirector> timelines;

    public void PlayTimeline(int index)
    {
        timelines[index].Play();
    }
}
