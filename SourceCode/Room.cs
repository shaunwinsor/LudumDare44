using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Room : MonoBehaviour
{
    public Sprite[] roomSprites;
    static int maximumCapacity = 3;
    int enemyCapacity;
    List<GameObject> enemiesInRoom = new List<GameObject>();
    public SpriteRenderer roomS;
    [SerializeField]
    Transform[] spawnPoints;
    [SerializeField]
    TextMeshProUGUI capacity;
    public Button upgradeButton;

    void Start()
    {
        enemyCapacity = 1;
        ShowCapacity();
    }

    bool AddEnemy(GameObject enemyToAdd, int position)
    {
        GameController gc = FindObjectOfType<GameController>();
        if (enemiesInRoom.Count >= enemyCapacity)
        {
            StartCoroutine(FindObjectOfType<UIManager>().ShowErrorMessage("Room capacity has already been reached!"));
            return false;
        }

        for(int i=0; i<gc.enemies.Length; i++)
        {
            if (enemyToAdd == gc.enemies[i])
            {
                if (!gc.SpendMoney(gc.enemiesCost[i]))
                {
                    StartCoroutine(FindObjectOfType<UIManager>().ShowErrorMessage("Not enough money!"));
                    return false;
                }
            }
        }

        GameObject e = Instantiate(enemyToAdd, spawnPoints[position].position, Quaternion.identity);
        e.transform.name = "Enemy";
        e.transform.parent = spawnPoints[position];
        e.GetComponent<Enemy>().Init(this);
        enemiesInRoom.Add(e);
        ShowCapacity();
        return true;
    }

    public void UpgradeRoom()
    {
        GameController gm = FindObjectOfType<GameController>();
        if (!gm || !gm.SpendMoney(gm.roomUpgradeCost))
        {
            StartCoroutine(FindObjectOfType<UIManager>().ShowErrorMessage("Not enough money!"));
            return;
        }
        enemyCapacity++;
        roomS.sprite = roomSprites[enemyCapacity - 1];
        if (enemyCapacity >= maximumCapacity)
        {
            upgradeButton.interactable = false;
        }
        ShowCapacity();
    }

    void ShowCapacity()
    {
        capacity.text = enemiesInRoom.Count.ToString() + "/" + enemyCapacity.ToString();
    }

    public void RemoveEnemy(GameObject go)
    {
        foreach (GameObject enemy in enemiesInRoom)
        {
            if (enemy == go)
            {
                enemiesInRoom.Remove(enemy);
                break;
            }
        }
        ShowCapacity();
    }

    public void DropEnemy(int pos)
    {
        UIManager ui = FindObjectOfType<UIManager>();
        if (!ui || !ui.enemy)
        {
            return;
        }
        AddEnemy(FindObjectOfType<GameController>().enemies[FindObjectOfType<UIManager>().DropEnemy()], pos);
    }
}