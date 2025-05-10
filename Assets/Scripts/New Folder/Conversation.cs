using System;
using System.Collections.Generic;

public class Conversation
{
    public List<Greeting> Greetings = new List<Greeting>();
    public List<DialogueOption> Options = new List<DialogueOption>();
    public List<Goodbye> Goodbyes = new List<Goodbye>();
}

public class DialogueOption 
{
    
} 

public class Greeting : SpeechBase
{
    
}

public class Response : SpeechBase
{
    
}

public class Goodbye : SpeechBase
{
    
}

public class SpeechBase
{
    public int priority;
    public string text;
}