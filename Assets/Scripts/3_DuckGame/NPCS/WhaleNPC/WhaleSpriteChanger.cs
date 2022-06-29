using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleSpriteChanger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;
    private int currentIndex;
    public float delayToChange;
    public float randomToChange;
    void Start()
    {
        StartCoroutine(SpriteChangerCoroutine());
    }

    public IEnumerator SpriteChangerCoroutine()
    {
        spriteRenderer.sprite = sprites[0];
        currentIndex = 0;
        while (true)
        {
            float delay = CalculateDelay();
            yield return new WaitForSeconds(delay);
            spriteRenderer.sprite = GetNextSprite();
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.sprite = GetNextSprite();
        }
    }

    private Sprite GetNextSprite()
    {
        if(sprites.Count == currentIndex + 1)
        {
            currentIndex = 0;
            return sprites[0];
        }
        currentIndex++;
        return sprites[currentIndex];
    }

    private float CalculateDelay()
    {
        return delayToChange + Random.Range(0, randomToChange);

    }
}
