using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    
    public void ExitGame(){
        Application.Quit();
        Debug.Log("Quitting!");
    }

    public void NewGame(){
        Debug.Log("Loading Scene!");
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }

    public void Options(){
        Debug.Log("Options?");
        SceneManager.LoadScene("Options", LoadSceneMode.Single);
    }
}
