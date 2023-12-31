using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager manager;
    public GameObject deathScreen;
    public GameObject pauseMenuUI;

    private void Awake()
    {
        manager = this;
    }

    void Update()
    {
        // ESC 키를 누르면 Pause 메뉴를 토글
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        // Pause 메뉴를 활성화 또는 비활성화
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);

        // 시간 흐름을 일시 정지 또는 재개
        Time.timeScale = (pauseMenuUI.activeSelf) ? 0f : 1f;
    }
    public void GameOver()
    {
        deathScreen.SetActive(true);
    }

    public void ReplayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ClearScene(string name)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }
}
