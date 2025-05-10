using System;
using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    public string characterName;
    public Personality personality;
}

public enum Mood
{
    Happy,
    Proud,
    Playful,
    Sad,
    Angry,
    Guilty,
    Scared,
    Calm,
    Tired,
    Excited,
    Friendly
}

[Serializable]
public struct Personality
{
    public Personality(Mood primaryMood)
    {
        this.primaryMood = primaryMood;
        this.secondaryMoods = new List<Mood>();
        this.currentMood = primaryMood;
    }
    public Mood primaryMood;
    public List<Mood> secondaryMoods;
    public Mood currentMood;
}




