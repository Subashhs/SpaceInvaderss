using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public string homePageSceneName = "HomePage"; // Replace with your scene name

    public void ChangeToHomePage()
    {
        SceneManager.LoadScene(homePageSceneName);
    }
}
