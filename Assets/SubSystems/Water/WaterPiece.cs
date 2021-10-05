using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPiece : MonoBehaviour
{
    [SerializeField] private Transform waterSprite;

    [Range(0f, 1f)][SerializeField] private float speed;
    [Range(0f, 5f)] [SerializeField] private float height;
    [Range(0f, 5f)] [SerializeField] private float width;
    [SerializeField] private bool randonDirection;
    [SerializeField] private bool y;
    [SerializeField] private bool x;

    float startX;
    float startY;
    float tY;
    float tX;
    int dirY = 1;
    int dirX = 1;


    private void Start()
    {
        if (waterSprite == null)
            waterSprite = transform.GetChild(0);

        startY = waterSprite.position.y;
        startX = waterSprite.position.x;

        if (randonDirection)
        {
            tY = Random.Range(-1, 1);
            if(Random.Range(-10,10) < 0)
            {
                dirY = -1;
            }
            else
            {
                dirY = 1;
            }

            tY = Random.Range(-1, 1);
            if (Random.Range(-10, 10) < 0)
            {
                dirX = -1;
            }
            else
            {
                dirX = 1;
            }

        }
    }

    private void FixedUpdate()
    {
        if (tY >= Random.Range(0.9f,1f))
            dirY = -1;
        if (tY <= Random.Range(-0.9f, -1f))
            dirY = 1;

        tY += dirY * speed;

        if (tX >= Random.Range(0.9f, 1f))
            dirX = -1;
        if (tX <= Random.Range(-0.9f, -1f))
            dirX = 1;
        
        tX += dirX * speed;


        float y = startY + (Mathf.Sin(tY) * height);
        float x = startX + (Mathf.Cos(tX) * width);

        waterSprite.position = new Vector3(x, y, transform.position.z);
    }
}
