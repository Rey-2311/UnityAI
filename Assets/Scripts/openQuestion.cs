using UnityEngine;
using TMPro;

public class QuestionClickHandler : MonoBehaviour
{
    public TMP_Text textMeshProObject;   // Assign in Inspector
    public Ammo ammoComponent;           // Assign in Inspector
    private bool isEditing = false;

    void Start()
    {
        // Ensure the TextMeshPro object is hidden at the start
        if (textMeshProObject != null)
        {
            textMeshProObject.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("TextMeshPro object is not assigned.");
        }
    }

    void OnMouseDown()
    {
        // Toggle editing mode when the cube is clicked
        isEditing = !isEditing;
        if (isEditing)
        {
            // Activate the TextMeshPro object for editing
            textMeshProObject.gameObject.SetActive(true);
            // Optionally, set the text to an empty string or placeholder
            textMeshProObject.text = "";
        }
        else
        {
            // Deactivate the TextMeshPro object after editing
            textMeshProObject.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (isEditing)
        {
            // Capture keyboard input to edit the text
            foreach (char c in Input.inputString)
            {
                if (c == '\b' && textMeshProObject.text.Length > 0)
                {
                    // Handle backspace
                    textMeshProObject.text = textMeshProObject.text.Substring(0, textMeshProObject.text.Length - 1);
                }
                else if (c == '\n' || c == '\r')
                {
                    // Handle Enter key (submit the text)
                    isEditing = false;
                    textMeshProObject.gameObject.SetActive(false);

                    // Set enterCheck to true in the Ammo component
                    if (ammoComponent != null)
                    {
                        ammoComponent.enterCheck = true;
                    }
                    else
                    {
                        Debug.LogWarning("Ammo component is not assigned.");
                    }
                }
                else
                {
                    // Append the character to the text
                    textMeshProObject.text += c;
                }
            }
        }
    }
}
