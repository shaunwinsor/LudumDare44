using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public GameObject healthBar;
    int startHealth;
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        health = 0;
    }

    void ShowHealth()
    {
        float size = 0f;
        if (health > 0)
        {
            size = ((float)health) / ((float)startHealth);
        }
        healthBar.transform.localScale = new Vector3(size, 1f, 1f);
    }

    public void Init(GameObject healthBar, int startHealth)
    {
        this.healthBar = healthBar;
        this.startHealth = startHealth;
        ChangeHealth(startHealth);
    }

    public void ChangeHealth(int newHealth)
    {
        health = newHealth;
        ShowHealth();
    }
}
