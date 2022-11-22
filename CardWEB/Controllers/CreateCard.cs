namespace Controllers;

using Cards;
using CardFactorys;
using JsonItems;
using System.Text;
using System.Text.Json;

public static class CardCreator {
    public static void CreateMonsterCard(HttpContext request) {
        string json = GetJSON(request.Request.Body);
        MonsterCardJsonItem? monsterJSON = JsonSerializer.Deserialize<MonsterCardJsonItem>(json);
        
        if (monsterJSON != null) {
            MonsterCard monster = new MonsterCard(monsterJSON);
            CardFactory factory = new CardFactory();
            factory.CreateCard<MonsterCard>(monster);
        }
    }

    public static void CreateEffectCard(HttpContext request) {
        string json = GetJSON(request.Request.Body);
        EffectCardJsonItem? effectJSON = JsonSerializer.Deserialize<EffectCardJsonItem>(json);

        if (effectJSON != null) {
            EffectCard monster = new EffectCard(effectJSON);
            CardFactory factory = new CardFactory();
            factory.CreateCard<EffectCard>(monster);
        }
    }

    public static string GetJSON(Stream s) {
        byte[] buffer = new byte[1<<11];

        s.ReadAsync(buffer,0,buffer.Length);
        
        string json = Encoding.UTF8.GetString(buffer, 0, buffer.Length);

        int i = -1;

        foreach (char? c in json) {
            i++;
            if (c == 0) {
                
                break;
            }
        }

        return json[0..i];
    }
}