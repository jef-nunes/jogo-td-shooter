using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerCtrl : MonoBehaviour
{
    /* [Usado para fins de teste] */
    #if UNITY_EDITOR
    public bool testing = true;
    #else
    public bool testing = false;
    #endif

    /* [Constantes de animação] */
    private const string IDLE_DOWN = "hero_idle_down";
    private const string IDLE_UP = "hero_idle_up";
    private const string IDLE_RIGHT = "hero_idle_right";
    private const string IDLE_LEFT = "hero_idle_left";

    private const string MOVE_DOWN = "hero_move_down";
    private const string MOVE_UP = "hero_move_up";
    private const string MOVE_RIGHT = "hero_move_right";
    private const string MOVE_LEFT = "hero_move_left";

    /* [Interface de usuário] */
    [SerializeField]
    private Slider hpBar;
    public TextMeshProUGUI hpPercentage;

    /* [Disparo de projéteis] */
    private float bulletSpeed = 5f;
    private float bulletRange = 5f;  

    /* [Atributos de personagem] */
    // Nível máximo
    private const int maxLevel = 50;
    // Nível atual o qual será corretamente atualizado no método ReloadStats()
    private int level = 1;

    private const float BASE_MAX_HEALTH = 800;
    private const float BASE_HEALTH_REGEN_RATE = 5f;
    
    // Vida
    private float finalHealthRegenRate;
    private float finalMaxHealth;
    private const float BASE_SPEED = 5f;
    private float currentHealth;
    private float finalSpeed;

    // Indicadores de movimentos atualizado a cada Update()
    private float bulletSpeedX;
    private float bulletSpeedY;
    private float moveHorizontal;
    private float moveVertical;
    private string lastAnim=IDLE_DOWN;

    /* [Componentes do GameObject] */
    private Animator animator;
    private Rigidbody2D rig;
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.CompareTag("Enemy"))
        {
            // Ignora a colisão entre o objeto com a tag "Player" e o objeto com a tag "Enemy"
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), col.collider);
        }
    }

    // Função para recarregar os atributos com base no nível
    private void ReloadStats()
    {
        level = PlayerPrefs.GetInt("HeroLevel", 1);
        finalMaxHealth = BASE_MAX_HEALTH + (level * 20);
        finalHealthRegenRate = BASE_HEALTH_REGEN_RATE + (level/2);
        currentHealth = finalMaxHealth;
        finalSpeed = BASE_SPEED + (level / 20f);
    }

    private void UIUpdater()
    {
        hpBar.value = currentHealth/finalMaxHealth;
        hpPercentage.text = Math.Round((double)(currentHealth/finalMaxHealth*100)).ToString()+"%";
    }
    //private void TestTakeDamage()
    //{
    //    currentHealth -= 100;
    //    //Debug.Log("Jogador recebeu dano");
    //    //Debug.Log("Vida atual: "+currentHealth);
    //    UIUpdater();
    //}

    public void TakeDamage(float dmg)
    {
        Debug.Log("Jogador recebeu dano");
        currentHealth -= dmg;
        UIUpdater();
    }

    private void RegenHealth()
    {
        if(currentHealth<finalMaxHealth)
        {
            currentHealth += finalHealthRegenRate;
            ////Debug.Log("+1 HP");
            UIUpdater();
        }
    }

    private void ResetLevel()
    {
        level = 1;
        PlayerPrefs.SetInt("HeroLevel",level);
        //Debug.Log("Nível do jogador resetado");
        ReloadStats();
    }

    // Função para o aumento de nível
    //private void LevelUp()
    //{
    //    if (level < maxLevel)
    //    {
    //        level++;
    //        PlayerPrefs.SetInt("HeroLevel", level);
    //        PlayerPrefs.Save();
    //        ReloadStats();
    //
    //        //Debug.Log("Jogador subiu para o nível "+level);
    //    }
    //}

    private bool IsIdle()
    {
        return moveHorizontal==0 && moveVertical==0;
    }

    // Calcular as coordenadas de destino do projétil do jogador
    private Vector2 CalculateBulletDestination()
    {
        // Armazenar as coordenadas
        float destinationX = transform.position.x;
        float destinationY = transform.position.y;
        // O jogador esta se movendo
        if(!IsIdle())
        {
            // Calcular eixo X de destino do projétil
            if(moveHorizontal==1)
            {
                destinationX = transform.position.x + bulletRange;
            }
            else if(moveHorizontal==-1)
            {
                destinationX = transform.position.x - bulletRange;
            }

            // Calcular eixo Y de destino do projétil
            if(moveVertical==1)
            {
                destinationY = transform.position.y + bulletRange;
            }
            else if(moveVertical==-1)
            {
                destinationY = transform.position.y - bulletRange;
            }
        }
        // Se o jogador estiver parado, recalcular
        else
        {
            // Utilizar como base a ultima animação
            // para identificar para qual direção o
            // jogador está voltado
            switch (lastAnim)
            {
                case IDLE_UP:
                destinationY = transform.position.y + bulletRange;
                break;
                case IDLE_DOWN:
                destinationY = transform.position.y - bulletRange;
                break;
                case IDLE_LEFT:
                destinationX = transform.position.x - bulletRange;
                break;
                case IDLE_RIGHT:
                destinationX = transform.position.x + bulletRange;
                break;
                default:
                break;
            }
        }
        // Criar e retornar um vetor 2D
        return new Vector2(destinationX, destinationY);
    }
    private void SetBulletSpeedOnIdle()
    {
        //Debug.Log("Setting bullet speed");
        //Debug.Log("Last anim: "+lastAnim);
        switch (lastAnim)
        {
            case IDLE_UP:
                bulletSpeedX = 0;
                bulletSpeedY = bulletSpeed;
            break;
            case IDLE_DOWN:
                bulletSpeedX = 0;
                bulletSpeedY = bulletSpeed*-1;
                //Debug.Log("Bullet speed X, Y was set:");
                //Debug.Log(bulletSpeedX);
                //Debug.Log(bulletSpeedY);
            break;
            case IDLE_LEFT:
                bulletSpeedX = bulletSpeed*-1;
                bulletSpeedY = 0;
            break;
            case IDLE_RIGHT:
                bulletSpeedX = bulletSpeed;
                bulletSpeedY = 0;
            break;
            default:
            break;
        }
    }
    private void Attack()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = PlayerBulletPool.Instance.GetBullet();
            {
                if(bullet!=null)
                {
                    bullet.GetComponent<PlayerBullet>().SetOrigin(transform.position);
                    Vector2 destination = CalculateBulletDestination();
                    bullet.GetComponent<PlayerBullet>().SetDestination(destination);
                    if(!IsIdle())
                    {
                        bulletSpeedX = bulletSpeed*moveHorizontal;
                        bulletSpeedY = bulletSpeed*moveVertical;
                    }
                    else
                    {
                        SetBulletSpeedOnIdle();
                    }
                    //Debug.Log("Player bullet");
                    //Debug.Log("destination: "+destination);
                    //Debug.Log("speed X: "+bulletSpeedX);
                    //Debug.Log("speed Y: "+bulletSpeedY);
                    bullet.GetComponent<PlayerBullet>().SetVelocity(bulletSpeedX, bulletSpeedY);
                    bullet.SetActive(true);
                }
            }
        }
    }

    // Animação
    private void Animate()
    {
        // Animações de repouso
        if (moveHorizontal == 0 && moveVertical == 0)
        {
                        switch (lastAnim)
            {
                case MOVE_UP: // Caso tenha se movido para cima na última vez
                    if (lastAnim != IDLE_UP)
                    {
                        lastAnim = IDLE_UP;
                        animator.Play(IDLE_UP);
                    }
                    break;

                case MOVE_DOWN: // Caso tenha se movido para baixo na última vez
                    if (lastAnim != IDLE_DOWN)
                    {
                        lastAnim = IDLE_DOWN;
                        animator.Play(IDLE_DOWN);
                    }
                    break;

                case MOVE_RIGHT: // Caso tenha se movido para a direita na última vez
                    if (lastAnim != IDLE_RIGHT)
                    {
                        lastAnim = IDLE_RIGHT;
                        animator.Play(IDLE_RIGHT);
                    }
                    break;

                case MOVE_LEFT: // Caso tenha se movido para a esquerda na última vez
                    if (lastAnim != IDLE_LEFT)
                    {
                        lastAnim = IDLE_LEFT;
                        animator.Play(IDLE_LEFT);
                    }
                    break;

                default:
                    // Se não houve movimento anterior, você pode colocar uma animação padrão ou nenhuma animação
                    break;
            }
        }
        // Animação de movimento
        else
        {
            // Animações na vertical são aplicadas apenas quando não há movimentação na horizontal
            if (moveHorizontal == 0)
            {
                if (moveVertical == 1 && !(lastAnim==MOVE_UP))
                {
                    // Animar para cima
                    animator.Play(MOVE_UP);
                    lastAnim=MOVE_UP;
                }
                else if (moveVertical == -1 && !(lastAnim==MOVE_DOWN))
                {
                    // Animar para baixo
                    animator.Play(MOVE_DOWN);
                    lastAnim=MOVE_DOWN;
                }
            }
            else
            {
                if (moveHorizontal == 1 && !(lastAnim==MOVE_RIGHT))
                {
                    // Animar para a direita
                    animator.Play(MOVE_RIGHT);
                    lastAnim=MOVE_RIGHT;
                }
                else if (moveHorizontal == -1 && !(lastAnim==MOVE_LEFT))
                {
                    // Animar para a esquerda
                    animator.Play(MOVE_LEFT);
                    lastAnim=MOVE_LEFT;
                }
            }
        }
    }


    // Movimentação
    void Move()
    {
        // Vetores da movimentação
        Vector2 movement = new Vector2(moveHorizontal, moveVertical).normalized;
        // Aplicar movimento ao Rigidbody2D
        rig.linearVelocity = movement * finalSpeed;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        if (rig == null)
        {
            //Debug.Log("Erro - Rigidbody não encontrado");
        }
        ReloadStats();
        InvokeRepeating("RegenHealth", 1f, 1f);
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");
        Move();
        Animate();
        Attack();
        if (testing)
        {
            //if(Input.GetKeyDown(KeyCode.U))
            //{
            //  LevelUp();
            //}
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                ResetLevel();
            }
        }
    }
}
