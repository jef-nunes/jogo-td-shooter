using UnityEngine;

public class Enemy02Move : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private bool allowMove = false;
    [SerializeField]
    private float moveSpeed = 5f;

    private float maxDistanceToPlayer = 5F;
    private float distanceToPlayer;
    public void SetAllowMove(bool value)
    {
        allowMove = true;
    }

    void Move()
    {
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distanceToPlayer>=maxDistanceToPlayer)
        {
        transform.position += (Vector3)(direction * moveSpeed) * Time.deltaTime;
        }
    }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    // Update is called once per frame
    void Update()
    {
        if(allowMove)
        {
            Move();
        }
    }
}
