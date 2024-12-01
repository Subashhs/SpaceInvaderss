using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class TextClickChangeScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private string targetSceneName = "SpaceInvaders"; // Replace with your target scene name.

    void Start()
    {
        // Ensure this script is attached to the "SpaceInvaders" text GameObject.
        if (gameObject.name != "Text")
        {
            Debug.LogWarning("This script should be attached to the 'SpaceInvaders' text GameObject!");
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            Debug.Log("Loading Scene: " + targetSceneName);
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Target scene name is not set.");
        }
    }
}
