using UnityEngine;

public class Enemy04Move : MonoBehaviour
{
    //[SerializeField]
    //private GameObject player;
    private float moveSpeed = 5f;
    private bool moveLeft = false;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Obstacle"))
        {
            Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),collision.collider);
        }
    }
    void Move()
    {
        // Se atingir a borda esquerda, moveLeft será falso
        if(transform.position.x<-10)
        {
            moveLeft=false;
        }

        // Se moveLeft for verdadeiro mover-se para a esquerda
        if(moveLeft)
        {
            if(transform.position.x >= -10)
            {
                float newXPos = transform.position.x-(moveSpeed*Time.deltaTime);
                Vector3 updatedPosVector = transform.position;
                updatedPosVector.x = newXPos;
                transform.position = updatedPosVector;
            }
        }
        else
        {
            if(transform.position.x<=9)
            {
                float newXPos = transform.position.x+(moveSpeed*Time.deltaTime);
                Vector3 updatedPosVector = transform.position;
                updatedPosVector.x = newXPos;
                transform.position = updatedPosVector;
            }
            // Se atingir a borda direita, moveLeft será verdadeiro
            else if(transform.position.x>=9)
            {
                moveLeft = true;
            }
        }
    }

    void Start()
    {
       // InvokeRepeating("Move",0.1f,0.1f);
    }
    void Update()
    {
        Move();
    }
}
