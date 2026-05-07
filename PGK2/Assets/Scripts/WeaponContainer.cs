using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContainer : MonoBehaviour
{
    private SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    private void Start()
    {
        characterRenderer = GetComponentInParent<SpriteRenderer>();
        weaponRenderer = GetComponentInChildren<SpriteRenderer>(); // Possible bugs if many GameObjects with sprite in children
    }

    private void FixedUpdate()
    {
        // Follow mouse
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;

        // Flip sprite
        Vector2 scale = transform.localScale;
        if (direction.x < 0)
            scale = new Vector2(Mathf.Abs(scale.x),-Mathf.Abs(scale.y));
        else if (direction.x > 0)
            scale = new Vector2(Mathf.Abs(scale.x), Mathf.Abs(scale.y));
        transform.localScale = scale;

        // Render sprite behind weapon user
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        else
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder + 1;
    }
}
