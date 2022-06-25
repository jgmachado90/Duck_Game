using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class PlayTimelineOnEnable : Entity
{
    public int timelineIndex;
    TimelinePlayer timelinePlayer;

    private void Start()
    {
        timelinePlayer = this.GetLevelManagerSubsystem<TimelinePlayer>();
        timelinePlayer.PlayTimeline(timelineIndex);
    }

}
