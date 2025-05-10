using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine.UI;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using TestModels;
using TMPro;
using UnityEngine.Rendering;


public class LanguageTester : MonoBehaviour
{
    private PhraseRoot allPhrases;

    public PhraseCategory GreetingContainer = new PhraseCategory();
    public string testCharacterName;
    
    public TextMeshProUGUI testText;
    public TextMeshProUGUI testName;

    void Start()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "Test.json");

        if (!File.Exists(path))
        {
            Debug.LogError($"JSON file not found: {path}");
            return;
        }

        string json = File.ReadAllText(path);
        allPhrases = JsonConvert.DeserializeObject<PhraseRoot>(json);

        if (allPhrases.TryGetValue("Greetings", out var greetings))
        {
            GreetingContainer = greetings;
            foreach (var kv in greetings)
            {
                var key = kv.Key;
                var phrase = kv.Value;

                Debug.Log($"Phrase: {key} | Mood: {string.Join(", ", phrase.mood)} | Tone: {phrase.tone}");

                if (phrase.specifications?.TryGetValue("Time", out var time) == true)
                {
                    Debug.Log($"Time: {time}");
                }

            }
        }
    }

#if UNITY_EDITOR
    [Button("Generate Greeting", ButtonSizes.Medium)]
    [EnableIf("@UnityEngine.Application.isPlaying")]
    private void GenerateGreeting()
    {
        Character currentDialogCharacter = null;
        List<Phrase> compatiblePhrases = new List<Phrase>();

        Debug.Log("Generating greeting");

        // Find character by name
        foreach (GameObject gObj in FindObjectsByType<GameObject>(FindObjectsSortMode.None))
        {
            if (gObj.name == testCharacterName)
            {
                currentDialogCharacter = gObj.GetComponent<Character>();
                break; // stop once we find it
            }
        }

        if (currentDialogCharacter == null)
        {
            Debug.LogWarning("No character found with the name: " + testCharacterName);
            return;
        }

        // Match phrases based on mood
        string currentMood = currentDialogCharacter.personality.currentMood.ToString();

        foreach (var kvp in GreetingContainer)
        {
            var phrase = kvp.Value;

            if (phrase.mood.Contains("All") || phrase.mood.Contains(currentMood))
            {
                compatiblePhrases.Add(phrase);
                Debug.Log("Matching greeting: " + kvp.Key);
            }
        }

        // Set UI
        testName.text = currentDialogCharacter.characterName;

        if (compatiblePhrases.Count > 0)
        {
            var selectedPhrase = compatiblePhrases[Random.Range(0, compatiblePhrases.Count)];
            testText.text = GreetingContainer.FirstOrDefault(p => p.Value == selectedPhrase).Key ?? "<missing phrase>";
        }
        else
        {
            testText.text = "[No matching greetings]";
            Debug.LogWarning("No compatible greetings found for mood: " + currentMood);
        }
    }

#endif
    
}


namespace TestModels
{
    using System.Collections.Generic;

    public class Phrase
    {
        public List<string> mood { get; set; }
        public string tone { get; set; }

#nullable enable
        
        public string? dialect { get; set; } 
        public Dictionary<string, string>? specifications { get; set; }  
    }

    public class PhraseCategory : Dictionary<string, Phrase> { }
    public class PhraseRoot : Dictionary<string, PhraseCategory> { }

}

