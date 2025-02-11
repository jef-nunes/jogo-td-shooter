using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy04Health : MonoBehaviour
{
    [SerializeField]
    private Slider hpBar;
    public TextMeshProUGUI hpPercentage;
    
    public float maxHealth = 2000;
    public float health = 2000;
    private void UIUpdater()
    {
        hpBar.value = health/maxHealth;
        hpPercentage.text = Math.Round((double)(health/maxHealth*100)).ToString()+"%";
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.CompareTag("PlayerBullet"))
        {
            health -= GlobalConstants.PLAYER_BULLET_DAMAGE;
            UIUpdater();
            if(health<=-1)
            {
                GlobalVariables.boss1Killed = true;
                Destroy(gameObject);
            }
        }
    }
}
