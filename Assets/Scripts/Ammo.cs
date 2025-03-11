using UnityEngine;
using TMPro;
using System.Collections;
using System.Threading.Tasks;

public class Ammo : MonoBehaviour
{
    public bool enterCheck = false;
    private GameObject[] bullets = new GameObject[3]; // Store bullets in an array
    private TMP_Text textMeshProObject; // Reference to the TextMeshPro object
    public BossWordDisplay bossWordDisplay;
    private string response;
    void Start()
    {
        // Initialize bullet references (ensure these are assigned in the Inspector)
        bullets[0] = GameObject.Find("Bullet");
        bullets[1] = GameObject.Find("Bullet (1)");
        bullets[2] = GameObject.Find("Bullet (2)");

        // Deactivate all bullets at the start
        DeactivateBullets();

        // Initialize the TextMeshPro object (ensure this is assigned in the Inspector)
        textMeshProObject = GameObject.Find("Text (TMP)").GetComponent<TMP_Text>();

    }

    void Update()
    {
        if (enterCheck)
        {
            HandleEnterPress();
        }

        // Detect left mouse click on bullets
        if (Input.GetMouseButtonDown(0))
        {
            CheckBulletClick();
        }
    }

    private void DeactivateBullets()
    {
        foreach (GameObject bullet in bullets)
        {
            if (bullet != null) bullet.SetActive(false);
        }
    }

    private void HandleEnterPress()
    {
        Debug.Log("Ammo detected Enter");

        // Find the first available (inactive) bullet slot
        for (int i = 0; i < bullets.Length; i++)
        {
            if (bullets[i] != null && !bullets[i].activeSelf)
            {
                bullets[i].SetActive(true);
                SaveTextToBullet(bullets[i]);
                break; // Exit loop after activating a bullet
            }
        }

        enterCheck = false;
        Debug.Log("Ammo detected Enter and set enterCheck to false");
    }

    private void SaveTextToBullet(GameObject bullet)
    {
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null && textMeshProObject != null)
        {
            bulletComponent.bulletText = textMeshProObject.text;
            Debug.Log($"Saved text to bullet: {bulletComponent.bulletText}");
        }
        else
        {
            Debug.LogWarning("Bullet component or TextMeshPro object is not assigned.");
        }
    }

    private void CheckBulletClick()
    {
        // Check if the mouse click is over any bullet
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null)
        {
            GameObject clickedBullet = hit.collider.gameObject;

            for (int i = 0; i < bullets.Length; i++)
            {
                if (bullets[i] == clickedBullet)
                {
                    Bullet bulletComponent = clickedBullet.GetComponent<Bullet>();
                    if (bulletComponent != null)
                    {
                        StartCoroutine(SendBulletTextToAPI(bulletComponent.bulletText));
                    }
                    else
                    {
                        Debug.LogWarning($"Bullet {i + 1} does not have a Bullet component.");
                    }
                    DeactivateBullet(bullets[i]);
                    break; // Exit loop after deactivating the bullet
                }
            }
        }
    }

    private void DeactivateBullet(GameObject bullet)
    {
        if (bullet != null)
        {
            bullet.SetActive(false);
            textMeshProObject.text = string.Empty; // Clear associated text
        }
    }
    private IEnumerator SendBulletTextToAPI(string inputText)
    {
        Debug.Log("Sending bullet text to API: " + inputText);
        Task<string> task = HuggingFaceAPI.GetHuggingFaceResponse(inputText);

        // Wait for the API call to complete
        while (!task.IsCompleted)
        {
            yield return null;
        }

        if (task.IsFaulted)
        {
            Debug.LogError("API Error: " + task.Exception?.Message);
        }
        else
        {
            response = task.Result;
            Debug.Log("Hugging Face Response: " + response);
            // Optionally, further processing of the API response can be done here.
        }
        if (bossWordDisplay != null && IsResponseMatching(response, bossWordDisplay.words))
        {
            Debug.Log("The response contains a matching word from BossWordDisplay!");
        }
        else
        {
            Debug.Log("No matching words found in response.");
        }
    }
    private bool IsResponseMatching(string response, string[] words)
    {
        if (words == null || words.Length == 0)
        {
            return false; // No words to compare
        }

        foreach (string word in words)
        {
            if (response.Contains(word))
            {
                return true; // Match found
            }
        }
        return false; // No match found
    }
}
