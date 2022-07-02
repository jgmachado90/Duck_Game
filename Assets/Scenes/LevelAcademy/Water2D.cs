using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water2D : MonoBehaviour
{
    const float springconstant = 0.02f;
    const float damping = 0.04f;
    const float spread = 0.05f;
    const float z = -1f;

    float[] xpositions;
    float[] ypositions;
    float[] velocities;
    float[] accelerations;

    LineRenderer Body;
    GameObject[] meshobjects;
    Mesh[] meshes;
    GameObject[] colliders;

    float baseheight;
    float left;
    float bottom;


    public GameObject splash;
    public Material mat;
    public GameObject watermesh;


}
