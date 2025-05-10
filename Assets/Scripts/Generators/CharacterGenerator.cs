using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Generators.Characters
{
    public class CharacterGenerator 
    {
        public CharacterGenerator()
        {
            NameGenerator nameGenerator = new NameGenerator();
            PersonalityGenerator personalityGenerator = new PersonalityGenerator();
            
            GameObject newCharacterObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            newCharacterObject.AddComponent<Character>();
            
            var thisCharacter = newCharacterObject.GetComponent<Character>();
            thisCharacter.characterName = nameGenerator.Generate();
            
            newCharacterObject.name = thisCharacter.characterName;
            thisCharacter.personality = personalityGenerator.Generate();
        }
    }
    public class NameGenerator : IGenerator<string>
    {
        public string Generate() 
        {
            StringBuilder s = new StringBuilder();
            s.Append(NameGrabber(Names.FirstNames));
            s.Append(" ");
            s.Append(NameGrabber(Names.LastNames));
            return s.ToString();
        }

        private string NameGrabber(List<string> names)
        {
            return names[Random.Range(0, names.Count-1)];
        }
    }
}


public static class Names
{
    public static readonly List<string> FirstNames = new List<string>
    {
        "Ava", "Liam", "Maya", "Noah", "Elena",
        "Leo", "Zara", "Ezra", "Isla", "Kai",
        "Nina", "Arlo", "Luna", "Silas", "Juno"
    };

    public static readonly List<string> LastNames = new List<string>
    {
        "Rowan", "Wilder", "Ember", "Ashen", "Hollow",
        "Thorne", "Briar", "Vale", "Reed", "Vesper",
        "Frost", "Quill", "Mire", "Shade", "Cairn"
    };
}

public interface IGenerator<out T>
{
    T Generate();
}

