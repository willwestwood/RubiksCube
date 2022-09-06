using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RubiksCube
{
    static public class MessageHandler
    {
        static public String GetCubeState(Cube cube)
        {
            return JsonSerializer.Serialize(cube);
        }

        static public void InterpretCommand(String message)
        {
            try
            {
                var doc = JsonSerializer.Deserialize<JsonDocument>(message);

                Console.WriteLine(doc.RootElement.GetString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
