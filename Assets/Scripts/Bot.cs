using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI; // For handling Unity UI elements

public class ChatBot : MonoBehaviour
{
    void Update()
    {
        // Trigger API call when the player presses the "E" key
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(CallHuggingFaceAPI("How to drink tea?"));
            Debug.Log("E is pressed");
        }
    }

    private IEnumerator CallHuggingFaceAPI(string inputText)
    {
        Task<string> task = HuggingFaceAPI.GetHuggingFaceResponse(inputText);

        // Wait for the async task to complete
        while (!task.IsCompleted) yield return null;

        if (task.IsFaulted)
        {
            Debug.LogError("API Error: " + task.Exception?.Message);
        }
        else
        {
            string response = task.Result;

            // Print to Console first
            Debug.Log("Hugging Face Response: " + response);

            // Count the occurrences of the word "the"
            int wordCount = CountWordOccurrences(response, "the");

            // Print the count to the console
            Debug.Log("The word 'the' appears " + wordCount + " times.");
        }
    }

    // Function to count occurrences of a word in a string
    private int CountWordOccurrences(string text, string word)
    {
        // Convert both the text and word to lowercase for case-insensitive comparison
        text = text.ToLower();
        word = word.ToLower();

        // Split the text into words and count the occurrences of the specified word
        string[] words = text.Split(' ', '\n', '\r');
        int count = 0;

        foreach (var w in words)
        {
            if (w == word)
            {
                count++;
            }
        }

        return count;
    }
}
