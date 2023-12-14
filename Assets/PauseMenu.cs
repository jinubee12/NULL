using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Update()
    {
        // ESC Ű�� ������ Pause �޴��� ���
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    void TogglePauseMenu()
    {
        // Pause �޴��� Ȱ��ȭ �Ǵ� ��Ȱ��ȭ
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);

        // �ð� �帧�� �Ͻ� ���� �Ǵ� �簳
        Time.timeScale = (pauseMenuUI.activeSelf) ? 0f : 1f;
    }

    // �޴� ������ ��ȯ�ϴ� �Լ�
    public void LoadMenu()
    {
        // Time.timeScale�� 1�� �����Ͽ� ������ ���������� ����ǵ��� ��
        Time.timeScale = 1f;

        // "MenuScene" ������ ��ȯ
        SceneManager.LoadScene("MenuScene");
    }
}
