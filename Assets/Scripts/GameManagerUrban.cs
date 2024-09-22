using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManagerUrban : MonoBehaviour
{
    public float starttime = 120.0f;
    public TextMeshProUGUI time;
    float timeelapsed;
    int min;
    int sec;
    public Button pause;
    public bool isActive;
    public bool paused;
    public Button restart;
    public TextMeshProUGUI levelcomp;
    public TextMeshProUGUI levelfail;
    // Start is called before the first frame update
    void Start()
    {
        levelcomp.gameObject.SetActive(false);
        restart.gameObject.SetActive(false);
        levelfail.gameObject.SetActive(false);
        paused = false;
        isActive = true;
        timeelapsed = starttime;
        min = Mathf.FloorToInt(timeelapsed / 60);
        sec = Mathf.FloorToInt(timeelapsed % 60);
        time.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            timeelapsed -= Time.deltaTime;
            if (timeelapsed > 0)
            {
                CDown(timeelapsed);
            }
            else
            {
                isActive = false;
                GameOver();
            }
        }
    }

    public void CDown(float timeelapsed)
    {
        min = Mathf.FloorToInt(timeelapsed / 60);
        sec = Mathf.FloorToInt(timeelapsed % 60);
        time.text = string.Format("{0:00}:{1:00}", min, sec);
    }

    public void GameOver()
    {
        time.text = string.Format("{0:00}:{1:00}", 0, 0);
        if (isActive)
        {
            levelcomp.gameObject.SetActive(true);
        }
        else
        {
            restart.gameObject.SetActive(true);
            levelfail.gameObject.SetActive(true);
        }
    }

    public void Pause()
    {
        if(paused)
        {
            paused = !paused;
            Time.timeScale = 1;
            restart.gameObject.SetActive(false);
        }
        else
        {
            paused = !paused;
            Time.timeScale = 0;
            restart.gameObject.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}