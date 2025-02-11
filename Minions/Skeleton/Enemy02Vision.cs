using UnityEngine;

public class Enemy02Vision : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D detectionCollider;

    [SerializeField]
    private Bullet02Spawner bulletSpawner;
    private Enemy02Move enemyMove;

    void Start()
    {
        enemyMove = gameObject.GetComponent<Enemy02Move>();
        // Encontrar o jogador na cena
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player com a tag 'Player' não encontrado na cena!");
        }

        // Se o Collider de detecção não for atribuído, tenta buscar o componente automaticamente
        if (detectionCollider == null)
        {
            detectionCollider = GetComponent<CircleCollider2D>();
            if (detectionCollider == null)
            {
                Debug.LogError("CircleCollider2D de detecção não encontrado!");
            }
        }
    }

    // Método chamado quando o jogador entra na área de detecção do inimigo
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (bulletSpawner != null)
            {
                bulletSpawner.SetIsAttacking(true); // Ativa o ataque
                enemyMove.SetAllowMove(true);
            }
        }
    }

    // Método chamado quando o jogador sai da área de detecção do inimigo
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (bulletSpawner != null)
            {
                bulletSpawner.SetIsAttacking(false); // Desativa o ataque
                enemyMove.SetAllowMove(false);
            }
        }
    }
}
