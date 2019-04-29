using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    public int moveSpeed, attackPower, health;
    Transform target;
    public Sprite[] headSprites, bodySprites, weaponSprites;
    SpriteRenderer head, body, weapon;
    public int inRoom = -1;
    int playerWorth = 5;
    public Queue<Enemy> enemyInFront = new Queue<Enemy>();
    public bool attacking;
    Health healthB;
    [SerializeField]
    GameObject healthBar;

    // Start is called before the first frame update
    void Start()
    {
        head = transform.Find("Head").GetComponent<SpriteRenderer>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        weapon = transform.Find("Right Arm/Weapon").GetComponent<SpriteRenderer>();
        healthB = new Health();
        healthB.Init(healthBar, health);
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            return;
        }
        if (transform.position.x >= target.position.x)
        {
            FindObjectOfType<RoomManager>().MoveToNext(this);
        }
        if (enemyInFront.Count <= 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, 0);
            GetComponent<Animator>().SetBool("Moving", true);
        }
        else
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Animator>().SetBool("Moving", false);
            if (attacking)
            {
                if (enemyInFront.Peek() == null)
                {
                    enemyInFront.Dequeue();
                }
                else
                {
                    enemyInFront.Peek().Hit(this);
                }
            }
        }
        
    }

    public void Init(int attackPower, int health)
    {
        head = transform.Find("Head").GetComponent<SpriteRenderer>();
        body = transform.Find("Body").GetComponent<SpriteRenderer>();
        weapon = transform.Find("Right Arm/Weapon").GetComponent<SpriteRenderer>();

        int val = Random.Range(0, headSprites.Length);
        head.sprite = headSprites[val];

        val = Random.Range(0, bodySprites.Length);
        body.sprite = bodySprites[val];

        weapon.sprite = weaponSprites[attackPower];
        this.health = health;
        this.attackPower = attackPower;

        enemyInFront.Clear();
        FindObjectOfType<RoomManager>().MoveToNext(this);
        playerWorth *= health;
    }

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void Hit(Enemy enemy)
    {
        health -= enemy.attackPower;
        healthB.ChangeHealth(health);
        if (health <= 0)
        {
            enemy.heroInRange.Dequeue();
            Die();
        }
    }

    void Die()
    {
        GetComponent<Animator>().SetTrigger("Die");
        Destroy(gameObject, 0.5f);
        FindObjectOfType<GameController>().CollectCoins(playerWorth);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>())
        {
            enemyInFront.Enqueue(other.GetComponent<Enemy>());
        }
    }
}
