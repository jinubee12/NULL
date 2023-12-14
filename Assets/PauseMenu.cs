using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

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

    // 메뉴 씬으로 전환하는 함수
    public void LoadMenu()
    {
        // Time.timeScale을 1로 설정하여 게임이 정상적으로 진행되도록 함
        Time.timeScale = 1f;

        // "MenuScene" 씬으로 전환
        SceneManager.LoadScene("MenuScene");
    }
}
