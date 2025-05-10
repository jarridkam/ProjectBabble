using System;
using Random = UnityEngine.Random;

namespace Generators
{
    public class PersonalityGenerator :IGenerator<Personality>
    {
        public PersonalityGenerator() { }

        public Personality Generate()
        {
            Personality personality = new Personality(GrabRandomMood());
            int secondaryMoodNum = Random.Range(1, 3);

            for (int i = 0; i < secondaryMoodNum; i++)
            {
                Mood randomMood = GrabRandomMood();
                if (randomMood != personality.primaryMood)
                {
                    personality.secondaryMoods.Add(randomMood);
                }
            }
            return personality;
        }

        public Mood GrabRandomMood()
        {
            return GetRandomEnumValue<Mood>();
            
            static T GetRandomEnumValue<T>() where T : Enum
            {
                Array values = Enum.GetValues(typeof(T));
                return (T)values.GetValue(UnityEngine.Random.Range(0, values.Length));
            }
        }
        

    }
}