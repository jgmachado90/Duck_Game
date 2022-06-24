using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using Health;

public class Player : Entity
{
    [Header("Components")]
    [SerializeField] private PlayerMovement playerMovement;

    private void Start()
    {

    }

}