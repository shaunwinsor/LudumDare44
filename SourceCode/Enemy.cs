using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health, attackPower;
    public Queue<Hero> heroInRange = new Queue<Hero>();
    public bool attack, isBoss;
    Health healthB;
    [SerializeField]
    GameObject healthBar;
    Room roomIn;

    void Start()
    {
        if (isBoss)
        {
            Init(null);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (heroInRange.Count > 0)
        {
            Animator anim = GetComponent<Animator>();
            if (anim)
            {
                anim.SetBool("Attack", true);
            }
            if (attack)
            {
                heroInRange.Peek().Hit(this);
            }
        }
        else
        {
            Animator anim = GetComponent<Animator>();
            if (anim)
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    public void Hit(Hero hero)
    {
        health -= hero.attackPower;
        healthB.ChangeHealth(health);
        if (health <= 0)
        {
            Die();
            hero.enemyInFront.Dequeue();
        }
    }

    void Die()
    {
        if (!isBoss)
        {
            GetComponent<Animator>().SetTrigger("Die");
            heroInRange.Clear();
            roomIn.RemoveEnemy(this.gameObject);
            Destroy(gameObject, 0.5f);
        }
        else
        {
            FindObjectOfType<GameController>().EndGame();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Hero>())
        {
            heroInRange.Enqueue(other.GetComponent<Hero>());
        }
    }

    public void Init(Room room)
    {
        heroInRange.Clear();
        healthB = new Health();
        healthB.Init(healthBar, health);
        if (room)
        {
            this.roomIn = room;
        }
    }
}
