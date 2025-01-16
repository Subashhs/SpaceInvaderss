using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextHoverColorChange : UIBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text textComponent;
    [SerializeField] private Color hoverColor = Color.green; // Color when the mouse hovers.
    private Color originalColor; // Store the original text color.

    void Start()
    {
        textComponent = GetComponent<Text>();

        if (textComponent != null)
        {
            originalColor = textComponent.color; // Save the original color.
        }
        else
        {
            Debug.LogWarning("Text component not found on this GameObject!");
        }
    }

    // Change text color when the mouse enters.
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textComponent != null)
        {
            textComponent.color = hoverColor;
        }
    }

    // Revert to the original color when the mouse exits.
    public void OnPointerExit(PointerEventData eventData)
    {
        if (textComponent != null)
        {
            textComponent.color = originalColor;
        }
    }
}
