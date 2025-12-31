using System;
using System.Text.RegularExpressions;

public class CommandDispatcher {
    public static string Dispatch(string jsonInput) {
        // 1. Manually extract the values using Regex
        string eventType = ExtractValue(jsonInput, "eventType");
        string location = ExtractValue(jsonInput, "location");

        if (string.IsNullOrEmpty(eventType) || eventType == "UNSUPPORTED EVENT") {
            throw new InvalidOperationException("Could not parse eventType from JSON or the event is UNSUPPORTED.");
        }

        // 2. Dispatch based on the extracted eventType
        return eventType switch {
            "FIRE" => $"Activate Sprinklers in {location}",
            "FLOOD" => $"Engage Water Pumps in {location}",
            "EARTHQUAKE" => $"Open Emergency Exits in {location}",
            _ => throw new InvalidOperationException($"Unknown command for event: {eventType}")
        };
    }

    // Helper method to extract JSON values without external libraries
    private static string ExtractValue(string json, string key) {
        string pattern = $"\"{key}\"\\s*:\\s*\"([^\"]+)\"";
        Match match = Regex.Match(json, pattern);
        return match.Success ? match.Groups[1].Value : null;
    }

    public static void Main() {
        string eventInput = "{\"eventType\": \"UNSUPPORTED EVENT\", \"location\": \"Lab-2\"}";

        try {
            Console.WriteLine($"Raw Input: {eventInput}");
            string action = Dispatch(eventInput);
            Console.WriteLine($"Resulting Action: {action}");
        } catch (Exception ex) {
            // This will now catch "UNSUPPORTED EVENT"
            Console.WriteLine($"Dispatcher Error: {ex.Message}");
        }
    }
}
