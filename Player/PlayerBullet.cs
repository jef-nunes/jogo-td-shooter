using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    private GameObject player;
    private float lifeTime = 1.5f;
    private float timeElapsed = 0f;
    private Vector3 destination = new Vector3(0,0,0);
    private Vector2 velocity = new Vector2(2f, 2f); // Velocidade padrão da bala
    private Vector2 adjustedDirection; // Direção ajustada com o offset, calculada uma vez

    public void SetVelocity(float velX, float velY)
    {
        velocity = new Vector2(velX, velY);
    }
    public void SetOrigin(Vector2 origin)
    {
        transform.position = origin;
    }

    // Define o destino com base na posição do jogador
    public void SetDestination(Vector3 dest)
    {
        destination = dest;
    }

    // Método para desativar a bala quando o tempo de vida acabar
    private void Disable()
    {
        timeElapsed = 0f;
        gameObject.SetActive(false);
    }

    //  private void Destroy()
    //  {
    //      Destroy(gameObject);
    //  }

    // Método para mover a bala
    private void Move()
    {
        // Calcula a direção da bala em direção ao destino (posição do jogador)
        Vector2 direction = destination - transform.position;
        adjustedDirection = direction.normalized;
        // Aplica a velocidade na direção ajustada
        transform.position += (Vector3)(adjustedDirection * velocity.magnitude) * Time.deltaTime;
    }


    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Enemy"))
        {
            Disable();
        }
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(),player.GetComponent<Collider2D>());
    }

    void Update()
    {
        Move(); // Move a bala em direção ao destino
        
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= lifeTime)
        {
            Disable();
        }
        else if(transform.position==destination)
        {
            Disable();
        }
    }
}
