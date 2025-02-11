using UnityEngine;

public class Bullet04Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float bulletSpeed = 5f;
    [SerializeField]
    private float cooldown=0.3f;
    [SerializeField]
    private float cooldownTimer = 0f;
    [SerializeField]
    private bool isAttacking = true;
    private float spiralAngle = 0f;
    private int oddEven=1;
    private void OddEvenCounter()
    {
        oddEven++;
        if(oddEven==2)
        {
            oddEven=1;
        }
    }
    private void Shot()
    {
        GameObject bul1 = Bullet04Pool.SharedInstance.GetBullet();
        {
            if(bul1!=null)
            {
                float offsetBulDirX = (transform.position.x+Mathf.Sin(spiralAngle*Mathf.PI/180f))*100;
                float offsetBulDirY = (transform.position.y+Mathf.Cos(spiralAngle*Mathf.PI/180f))*100;
                if(player.transform.position.x<0)
                {
                    offsetBulDirX=offsetBulDirX*-1;
                }
                if(player.transform.position.y<0)
                {
                    offsetBulDirY=offsetBulDirY*-1;
                }
                Vector3 destination = player.transform.position;
                destination.x += offsetBulDirX;
                destination.y += offsetBulDirY;
                //Debug.Log("bul 04 dest: "+destination);
                //Debug.Log("player pos: "+player.transform.position);
                bul1.GetComponent<Bullet04>().SetTrajectoryOffset(oddEven%2==0);
                bul1.GetComponent<Bullet04>().SetOrigin(transform.position);
                bul1.GetComponent<Bullet04>().SetDestination(destination);
                bul1.GetComponent<Bullet04>().SetVelocity(bulletSpeed, bulletSpeed);
                bul1.SetActive(true);
                //Debug.Log(spiralAngle);
                spiralAngle+=25f;
                if(spiralAngle>=360f)
                {
                    spiralAngle=0f;
                }
            }
        }
        GameObject bul2 = Bullet04Pool2.SharedInstance.GetBullet();
        {
            if(bul2!=null)
            {
                float offsetBulDirX = (transform.position.x+Mathf.Cos(spiralAngle*Mathf.PI/180f))*100;
                float offsetBulDirY = (transform.position.y+Mathf.Sin(spiralAngle*Mathf.PI/180f))*100;
                if(player.transform.position.x>0)
                {
                    offsetBulDirX=offsetBulDirX*-1;
                }
                if(player.transform.position.y>0)
                {
                    offsetBulDirY=offsetBulDirY*-1;
                }
                Vector3 destination = player.transform.position;
                destination.x += offsetBulDirX;
                destination.y += offsetBulDirY;
                //Debug.Log("bul 04 (2) dest: "+destination);
                //Debug.Log("player pos: "+player.transform.position);
                bul2.GetComponent<Bullet04>().SetTrajectoryOffset(oddEven%2==0);
                bul2.GetComponent<Bullet04>().SetOrigin(transform.position);
                bul2.GetComponent<Bullet04>().SetDestination(destination);
                bul2.GetComponent<Bullet04>().SetVelocity(bulletSpeed, bulletSpeed);
                bul2.SetActive(true);
                //Debug.Log(spiralAngle);
                spiralAngle+=25f;
                if(spiralAngle>=360f)
                {
                    spiralAngle=0f;
                }
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
                Shot();
                cooldownTimer += cooldown;
            }
            cooldownTimer -= Time.deltaTime;
        }
    }
}
