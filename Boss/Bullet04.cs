using System;
using UnityEngine;

public class Bullet04 : MonoBehaviour
{
    [SerializeField]
    private float lifeTime = 1.5f;
    private float timeElapsed = 0f;
    private Vector3 destination = new Vector3(0,0,0);
    public Vector2 velocity = new Vector2(2f, 2f); // Velocidade padrão da bala

    private float trajectoryOffsetX = 10f;
    private float trajectoryOffsetY = 10f;
    private bool useTrajectoryOffset = false;
    public void SetVelocity(float velX, float velY)
    {
        velocity = new Vector2(velX, velY);
    }
    public void SetOrigin(Vector2 origin)
    {
        transform.position = origin;
    }

    // Ativar ou desativar o deslocamento de trajetória
    public void SetTrajectoryOffset(bool value)
    {
        useTrajectoryOffset = value;
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
        if (useTrajectoryOffset)
        {
            // Aplica o offset uma vez, ajustando a direção permanentemente
            destination.x += trajectoryOffsetX;
            destination.y += trajectoryOffsetY;
        }
        // Calcula a direção da bala em direção ao destino
        Vector2 direction = (destination - transform.position).normalized;
        // Aplica a velocidade na direção ajustada
        transform.Translate(direction*velocity.magnitude*Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<PlayerCtrl>().TakeDamage(20);
            Disable();
        }
    }

    void Start()
    {
    }

    void Update()
    {
        Move(); // Move a bala em direção ao destino
        
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= lifeTime)
        {
            Disable();
        }
        else if (Vector2.Distance(transform.position, destination) < 0.1f) 
        {
            Disable();
        }
    }
}
