using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text score;
    private PlayerStats _ps;

    private void Start() 
    {
        _ps=Resources.Load<PlayerStats>("PlayerStats");
        score.text = "Last Room: "+_ps.roomNumber.ToString();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        _ps.roomNumber = 1;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
