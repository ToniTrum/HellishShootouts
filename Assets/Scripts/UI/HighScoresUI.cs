using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class HighScoresUI : MonoBehaviour
{
    public Text scoresText;

    void Start()
    {
        DisplayHighScores();
    }

    void DisplayHighScores()
    {
        // Это заглушка. Позже здесь будет логика загрузки рекордов из файла.
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("ТАБЛИЦА РЕКОРДОВ");
        sb.AppendLine("--------------------");
        sb.AppendLine("1. PlayerOne - 10000");
        sb.AppendLine("2. GamerPro - 8500");
        sb.AppendLine("3. NoobSlayer - 7000");
        sb.AppendLine("4. UnityDev - 5000");
        sb.AppendLine("5. Gemini - 1000");

        if (scoresText != null)
        {
            scoresText.text = sb.ToString();
        }
        else
        {
            Debug.LogError("Не назначен текстовый компонент для отображения рекордов в скрипте HighScoresUI!");
        }
    }
} 