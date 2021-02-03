using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuToggle : MonoBehaviour
{
    public GameObject HelpMenu;
    public GameObject MainMenu;

    public void HelpBUtton()
    {
        HelpMenu.SetActive(!HelpMenu.activeSelf);
        MainMenu.SetActive(!MainMenu.activeSelf);
    }

    public void playGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
