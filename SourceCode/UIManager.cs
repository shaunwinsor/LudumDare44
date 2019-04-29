using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    RoomManager roomManager;
    [SerializeField]
    Button nextRoundButton;
    public GameObject enemy, errorPanel, startPanel;
    int enemyChosen;
    public TextMeshProUGUI errorText, pausedText;
    public Image pauseImg;
    [SerializeField]
    Sprite pause, resume;
    bool paused;

    // Start is called before the first frame update
    void Start()
    {
        roomManager = FindObjectOfType<RoomManager>();
        Resume();
        startPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z += 0.35f;
            enemy.transform.position = pos;
            if (Input.GetMouseButtonUp(0))
            {
                Destroy(enemy);
            }
        }
    }

    public void AddRoom()
    {
        roomManager.AddRoom();
    }

    public void EndRound()
    {
        nextRoundButton.interactable = true;
    }

    public void StartRound()
    {
        nextRoundButton.interactable = false;
        FindObjectOfType<GameController>().RunRound();
    }

    public void PickUpEnemy(int enemyNum)
    {
        enemyChosen = enemyNum;
        enemy = GameObject.Instantiate(FindObjectOfType<GameController>().enemyIcons[enemyNum], Input.mousePosition, Quaternion.identity);
    }

    public int DropEnemy()
    {
        Destroy(enemy);
        return enemyChosen;
    }

    public IEnumerator ShowErrorMessage(string message)
    {
        errorPanel.SetActive(true);
        errorText.text = message;
        yield return new WaitForSeconds(2f);
        errorPanel.SetActive(false);
    }

    public void Toggle()
    {
        if (!paused)
        {
            Pause();
        }
        else if (paused)
        {
            Resume();
        }
    }

    void Pause()
    {
        paused = true;
        Time.timeScale = 0;
        pauseImg.sprite = resume;
        pausedText.gameObject.SetActive(true);
    }

    void Resume()
    {
        paused = false;
        Time.timeScale = 1;
        pauseImg.sprite = pause;
        pausedText.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        FindObjectOfType<GameController>().StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
