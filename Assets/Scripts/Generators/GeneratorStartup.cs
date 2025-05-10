using Generators.Characters;
using UnityEngine;

public class GeneratorStartup : MonoBehaviour 
{
    public GeneratorTypes generatorType;
    public int genNumber;
    void Start()
    {
        switch (generatorType)
        {
            case GeneratorTypes.Character:
                for (int i = 0; i < genNumber; i++)
                {
                    CharacterGenerator characterGenerator = new CharacterGenerator();
                }
                break;
            
            case GeneratorTypes.Town:
                Debug.Log("Town Generator Not Implemented");
                break;
        }
        
    }
}

public enum GeneratorTypes
{
    Character,
    Town
}