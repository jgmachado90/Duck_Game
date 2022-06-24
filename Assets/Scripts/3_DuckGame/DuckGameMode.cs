using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;

public class DuckGameMode : GameMode
{
    [SerializeField] protected Grid2D grid;
    [SerializeField] CameraSnapFollow camera2D;
    public Grid2D Grid => grid;
    public CameraSnapFollow Camera2D => camera2D;

    private void Awake()
    {
        camera2D.SetGridSize(grid.GridSize);
        DisableAllRooms();
    }

    private void DisableAllRooms()
    {
        var allRooms = grid.ChildrenRooms;
        foreach (var item in allRooms)
        {
            item.Disable();
        }
    }
}
