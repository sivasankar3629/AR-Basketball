using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClickButton : MonoBehaviour
{
    public void OnClickPlay()
    {
        SceneManager.LoadScene("BasketBall");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnClickMainMenu()
    {
        SceneManager.LoadScene("HomePage");
    }
}
