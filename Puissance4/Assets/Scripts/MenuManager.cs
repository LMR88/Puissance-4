using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void PlayLocal()
    {
        SceneManager.LoadScene(1);
    }

    public void JoueurvsIa()
    {
        SceneManager.LoadScene(2);
    }

    public void JoueurvsIaAmeliorer()
    {
        SceneManager.LoadScene(3);
    }

    public void IavsIa()
    {
        SceneManager.LoadScene(4);
    }
    public void QuitButton()
    {
        Application.Quit();
    }
}
