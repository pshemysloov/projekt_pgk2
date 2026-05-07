using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer hpContainer;
    [SerializeField]
    private SpriteRenderer hpBar;
    public EnemyAI parent;

    [SerializeField]
    private Vector3 position, scale;
    [SerializeField]
    private Quaternion rotation;


    private void Start()
    {
        hpContainer.transform.localPosition = position;
        hpContainer.transform.rotation = rotation;
        hpContainer.transform.localScale = scale;
    }
    public void onHealthChange(Component sender, object data)
    {
        if (sender == parent) 
        {
            float completionRatio = (float)data * 16; //*16 for current sprite "hpbar"
            hpBar.transform.localScale = new Vector3(completionRatio, 16, 1);
        } 
    }
}
