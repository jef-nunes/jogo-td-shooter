using UnityEngine;

public class Enemy02Health : MonoBehaviour
{
    public int currentGameLevel = 0;
    public float health = 400;
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.collider.CompareTag("PlayerBullet"))
        {
            health -= GlobalConstants.PLAYER_BULLET_DAMAGE;
            if(health<=-1)
            {
                GlobalVariables.IncMinionsKill(currentGameLevel);
                Destroy(gameObject);
            }
        }
    }
}
