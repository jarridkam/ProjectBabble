using System.Collections.Generic;
using Unity.VisualScripting;

namespace Generators.Dialogue
{
    public class ConversationGenerator : IGenerator<Conversation>
    {
        public Conversation Generate()
        {
            GreetingGenerator greetingGenerator = new GreetingGenerator();
            DialogOptionGenerator dialogOptionGenerator = new DialogOptionGenerator();
            GoodbyeGenerator goodbyeGenerator = new GoodbyeGenerator();
            
            Conversation conversation = new Conversation();
            conversation.Greetings = new List<Greeting>(greetingGenerator.Generate());
            conversation.Options = new List<DialogueOption>(dialogOptionGenerator.Generate());
            conversation.Goodbyes = new List<Goodbye>(goodbyeGenerator.Generate());
            
            return conversation;
        }
    }
    
    public class GreetingGenerator : IGenerator<Greeting[]>
    {
        public Greeting[] Generate()
        {
            return new Greeting[0];
        }
    }

    public class DialogOptionGenerator : IGenerator<DialogueOption[]>
    {
        public DialogueOption[] Generate()
        {
            return new DialogueOption[0];
        }
    }

    public class ResponseGenerator : IGenerator<Response[]>
    {
        public Response[] Generate()
        {
            return new Response[0];
        }
    }
    
    public class GoodbyeGenerator : IGenerator<Goodbye[]>
    {
        public Goodbye[] Generate()
        {
            return new Goodbye[0];
        }
    }
}


