using System.Text.RegularExpressions;

namespace lab8;

enum State_L2
{
    Start,
    HashRead,
    FirstPart,
    AtRead,
    SecondPart,
    Accept,
    Error
}

enum State_L3
{
    S0,
    S1,
    S2,
    S3,
    S4,
    S5,
    S6,
    ACCEPT,
    ERROR
}

static public class Tasks
{
    public static void Level1()
    {
        string filePath = "./words.txt";
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не знайдено!");
            return;
        }
        
        string pattern = @"^#[0-9A-F]+@#[0-9A-F]+$";

        string[] lines = File.ReadAllLines(filePath);
        
       

        Console.WriteLine("Слова, що відповідають шаблону:");

        foreach (string line in lines)
        {
            if (Regex.IsMatch(line.Trim(), pattern))
            {
                Console.WriteLine(line);
            }
        }
    }

    public static bool IsValid(string input)
    {
        State_L2 state = State_L2.Start;
        int i = 0;

        while (i < input.Length)
        {
            char c = input[i];

            switch (state)
            {
                case State_L2.Start:
                    if (c == '#')
                    {
                        state = State_L2.HashRead;
                    }
                    else
                    {
                        state = State_L2.Error;
                    }

                    break;

                case State_L2.HashRead:
                    if (c == 'd' || (c >= 'a' && c <= 'f'))
                    {
                        state = State_L2.FirstPart;
                    }
                    else if (c == '@') // Порожня перша частина
                    {
                        state = State_L2.AtRead;
                    }
                    else
                    {
                        state = State_L2.Error;
                    }

                    break;

                case State_L2.FirstPart:
                    if (c == 'd' || (c >= 'a' && c <= 'f'))
                    {
                        // залишаємося в FirstPart
                    }
                    else if (c == '@')
                    {
                        state = State_L2.AtRead;
                    }
                    else
                    {
                        state = State_L2.Error;
                    }

                    break;

                case State_L2.AtRead:
                    if (char.IsDigit(c) || (c >= 'a' && c <= 'f'))
                    {
                        state = State_L2.SecondPart;
                    }
                    else
                    {
                        state = State_L2.Error;
                    }

                    break;

                case State_L2.SecondPart:
                    if (char.IsDigit(c) || (c >= 'a' && c <= 'f'))
                    {
                        // залишаємося в SecondPart
                    }
                    else
                    {
                        state = State_L2.Error;
                    }

                    break;

                default:
                    state = State_L2.Error;
                    break;
            }

            i++;
        }

        // Приймаємо, якщо завершили в стані SecondPart або AtRead (порожня друга частина)
        return state == State_L2.SecondPart || state == State_L2.AtRead;
    }

    public static void Level2()
    {
        Console.WriteLine("Введіть рядок:");
        string input = Console.ReadLine();

        if (Tasks.IsValid(input))
        {
            Console.WriteLine("Рядок правильний");
        }
        else
        {
            Console.WriteLine("Рядок неправильний");
        }
    }

    static bool IsValidWord(string word)
    {
        State_L3 state = State_L3.S0;

        for (int i = 0; i < word.Length; i++)
        {
            char c = word[i];

            switch (state)
            {
                case State_L3.S0:
                    if (c == '#') state = State_L3.S1;
                    else state = State_L3.ERROR;
                    break;

                case State_L3.S1:
                    if (c == 'd') state = State_L3.S2;
                    else if (c >= 'a' && c <= 'f') state = State_L3.S3;
                    else if (c == '@') state = State_L3.S4;
                    else state = State_L3.ERROR;
                    break;

                case State_L3.S2:
                    if (c == 'd') state = State_L3.S2;
                    else if (c == '@') state = State_L3.S4;
                    else state = State_L3.ERROR;
                    break;

                case State_L3.S3:
                    if (c >= 'a' && c <= 'f') state = State_L3.S3;
                    else if (c == '@') state = State_L3.S4;
                    else state = State_L3.ERROR;
                    break;

                case State_L3.S4:
                    if (char.IsDigit(c)) state = State_L3.S5;
                    else if (c >= 'a' && c <= 'f') state = State_L3.S6;
                    else state = State_L3.ERROR;
                    break;

                case State_L3.S5:
                    if (char.IsDigit(c)) state = State_L3.S5;
                    else state = State_L3.ERROR;
                    break;

                case State_L3.S6:
                    if (c >= 'a' && c <= 'f') state = State_L3.S6;
                    else state = State_L3.ERROR;
                    break;

                default:
                    state = State_L3.ERROR;
                    break;
            }

            if (state == State_L3.ERROR)
                return false;
        }

        // Приймаємо, якщо завершилися у S4 (порожня друга частина), S5 або S6
        return state == State_L3.S4 || state == State_L3.S5 || state == State_L3.S6;
    }

    public static void Level3()
    {
        string filePath = "input.txt";
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Файл не знайдено!");
            return;
        }

        string text = File.ReadAllText(filePath);

        // Розбиття на слова за роздільниками: пробіл, крапка, крапка з комою
        string[] words = Regex.Split(text, @"[\s;.]+");

        Console.WriteLine("Перевірка слів з файлу:");

        foreach (var word in words)
        {
            if (string.IsNullOrWhiteSpace(word)) continue;

            bool isValid = IsValidWord(word);

            Console.WriteLine($"{word} -> {(isValid ? "Правильне" : "Неправильне")}");
        }
    }
}