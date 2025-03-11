using UnityEngine;
using TMPro;
using System;

public class BossWordDisplay : MonoBehaviour
{
    public TMP_Text bossText; // Reference to the TextMeshProUGUI component
    public string[] words = { "the", "of", "is" }; // Array of words to choose from

    void Start()
    {
        // Seed the random number generator with the current time in milliseconds
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);

        // Call ShowRandomWord when the game starts
        ShowRandomWord();
    }

    void ShowRandomWord()
    {
        // Pick a random index from the words array
        int randomIndex = UnityEngine.Random.Range(0, words.Length);
        // Set the text of the boss rectangle to the selected word
        bossText.text = words[randomIndex];
    }
}
