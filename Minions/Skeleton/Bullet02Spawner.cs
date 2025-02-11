using UnityEngine;

public class Bullet02Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float bulletSpeed = 5f;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private float cooldownTimer = 0f;
    [SerializeField]
    private bool isAttacking = false;
    private int oddEven=1;
    private void OddEvenCounter()
    {
        oddEven++;
        if(oddEven==2)
        {
            oddEven=1;
        }
    }
    private void Shot(Vector2 origin)
    {
        GameObject bullet = Bullet02Pool.SharedInstance.GetBullet();
        {
            if(bullet!=null)
            {
                bullet.GetComponent<Bullet02>().SetTrajectoryOffset(oddEven%2==0);
                bullet.GetComponent<Bullet02>().SetOrigin(origin);
                bullet.GetComponent<Bullet02>().SetDestination(player.transform.position);
                bullet.GetComponent<Bullet02>().SetVelocity(bulletSpeed, bulletSpeed);
                bullet.SetActive(true);
            }
        }
        OddEvenCounter();
    }

    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if(isAttacking)
        {
            if(cooldownTimer <= 0f)
            {
                Shot(transform.position);
                cooldownTimer += cooldown;
            }
            cooldownTimer -= Time.deltaTime;
        }
    }
}
