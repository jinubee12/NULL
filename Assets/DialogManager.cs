using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogManager : MonoBehaviour
{
    public GameObject gpt;
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
        DontDestroyOnLoad(gpt);
    }
}
