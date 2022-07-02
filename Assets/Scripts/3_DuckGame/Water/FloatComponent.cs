using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatComponent : MonoBehaviour
{
    [SerializeField] Transform content;
    [SerializeField] Rigidbody2D rb;
    public bool floating;
    public Water myWater;

    private void Update()
    {
        if (floating && myWater != null)
        {
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            myWater.Float(transform.position.x, content);
        }
    }
}
