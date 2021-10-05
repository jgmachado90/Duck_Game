using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKillable
{
    void Hit();
    void Death();

    int Health { get; set; }
    bool Dead { get; set; }
}
