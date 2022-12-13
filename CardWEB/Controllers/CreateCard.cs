namespace Controllers;

using Cards;
using CardFactorys;
using CardJsonItems;
using System.Text;
using System.Text.Json;

/// <summary>
/// Static class with methods for managing new cards creation
/// </summary>
public static class CardCreator
{
    /// <summary>
    /// Creates a new instance of MonsterCard and saves it to the disk
    /// </summary>
    /// <param name="request">An HttpContext with all the needed data in the body</param>
    public static void CreateMonsterCard(HttpContext request)
    {
        string json = GetJSON(request.Request.Body);
        MonsterCardJsonItem? monsterJSON = JsonSerializer.Deserialize<MonsterCardJsonItem>(json);

        if (monsterJSON != null)
        {
            MonsterCard monster = new MonsterCard(monsterJSON);
            CardFactory factory = new CardFactory();
            factory.CreateCard<MonsterCard>(monster);
        }
    }

    /// <summary>
    /// Creates a new instance of EffectCard and saves it to the disk
    /// </summary>
    /// <param name="request">An HttpContext with all the needed data in the body</param>
    public static void CreateEffectCard(HttpContext request)
    {
        string json = GetJSON(request.Request.Body);
        EffectCardJsonItem? effectJSON = JsonSerializer.Deserialize<EffectCardJsonItem>(json);

        if (effectJSON != null)
        {
            EffectCard monster = new EffectCard(effectJSON);
            CardFactory factory = new CardFactory();
            factory.CreateCard<EffectCard>(monster);
        }
    }

    /// <summary>
    /// Reads a json from a Stream and casts it to a string
    /// </summary>
    /// <param name="s">A stream with the json</param>
    /// <returns>The json as a string</returns>
    public static string GetJSON(Stream s)
    {
        byte[] buffer = new byte[1 << 11];

        s.ReadAsync(buffer, 0, buffer.Length);

        string json = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

        int i = -1;

        foreach (char? c in json)
        {
            i++;
            if (c == 0)
            {

                break;
            }
        }

        return json[0..i];
    }
}