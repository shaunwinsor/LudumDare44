using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField]
    GameObject hero;
    bool spawnHero;

    public GameObject[] enemies, enemyIcons;
    public int[] enemiesCost;

    public GameObject goblin, demon, deadPanel;

    int coins, roundNum, spawnedHeroes;
    public int roomCost, roomUpgradeCost;
    RoomManager roomManager;
    [SerializeField]
    float spawnTime;
    [SerializeField]
    TextMeshProUGUI coinsText, roundText, lostText;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        coins = 250;
        goblin.SetActive(false);
        demon.SetActive(false);
        deadPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        coinsText.text = coins.ToString();
        if (spawnedHeroes <= 0)
        {
            if (!GameObject.Find("Hero"))
            {
                EndRound();
            }
            return;
        }
        if (spawnHero)
        {
            StartCoroutine(SpawnHero());
        }
    }

    void AddHero()
    {
        GameObject h = Instantiate(hero, roomManager.startPoint.position, Quaternion.identity);
        h.GetComponent<Hero>().Init(1, 3);
        h.transform.name = "Hero";
        spawnedHeroes--;
    }

    public bool SpendMoney(int coins)
    {
        if (this.coins < coins)
        {
            return false;
        }
        this.coins -= coins;
        return true;
    }

    public void CollectCoins(int coins)
    {
        this.coins += coins;
    }

    public void EndGame()
    {
        //End the game
        StopAllCoroutines();
        roomManager.ClearRooms();
        deadPanel.SetActive(true);
        lostText.text = roundNum.ToString();
    }

    IEnumerator SpawnHero()
    {
        spawnHero = false;
        AddHero();
        float intervalTime = 5f / ((float)roundNum/2);
        yield return new WaitForSeconds(intervalTime);
        spawnHero = true;
    }

    void EndRound()
    {
        FindObjectOfType<UIManager>().EndRound();
        if (roundNum == 4)
        {
            goblin.SetActive(true);
        }
        if (roundNum == 9)
        {
            demon.SetActive(true);
        }
    }

    public void RunRound()
    {
        roundNum++;
        spawnedHeroes = 5*roundNum;
        roundText.text = roundNum.ToString();
    }

    public void AddRoom()
    {
        roomManager.AddRoom();
    }

    public void StartGame()
    {
        if (roomManager.Init())
        {
            spawnHero = true;
        }
    }
}
