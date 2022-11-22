namespace Controllers;

using Match;
using Cards;
using Models;
using System.Text.Json;

public static class Game {
    private static Match? match{get;set;}

    public static string NewGame(string type, HttpContext request) {
        if (type == "human") {
            match = new Match(MatchTypes.HumanVSHuman, 5, 5, 0, 1);
        }
        else if (type == "ai"){
            match = new Match(MatchTypes.ComputerVSHuman, 5, 5);
        }

        string json = CardCreator.GetJSON(request.Request.Body);

        CardNames? names = JsonSerializer.Deserialize<CardNames>(json);

        if (match != null) {
            if (names != null) {
                string[] playerA = new string[names.playerA.Length];
                for (int i = 0; i < names.playerA.Length; i++) {
                    playerA[i] = names.playerA[i];
                }

                string[] playerB = new string[names.playerB.Length];
                for (int i = 0; i < names.playerB.Length; i++) {
                    playerB[i] = names.playerB[i];
                }

                match.SetDeck(playerA, true);
                match.SetDeck(playerB, false);
            }

            match.SetDeck(true);
            match.SetDeck(false);

            match.BeginGame();

            List <CardResponse> responseBody = new List<CardResponse>();

            int x = 0;
            foreach (Cards.Card? c in match.player.Hand) {
                if (c != null) {
                    CardResponse r = new CardResponse(c, x);
                    responseBody.Add(r);
                }
                x++;
            }

            string jsonResponse = JsonSerializer.Serialize(responseBody);
            
            return jsonResponse;
        }

        throw new Exception("something happenned");
    }

    public static string NewTurn(bool auto, HttpContext request) {
        if (match == null) throw new Exception("there is no match yet");
        
        if (!auto)match.EndTurn();

        List <CardResponse> responseBody = new List <CardResponse>();

        int i = 0;
        foreach (Cards.Card? c in match.player.Hand) {
            if (c != null) {
                CardResponse r = new CardResponse(c, i);
                responseBody.Add(r);
            }
            i++;
        }

        string jsonResponse = JsonSerializer.Serialize(responseBody);
        
        return jsonResponse;
    }

    public static string Play(HttpContext request) {
        if (match == null) throw new Exception("there is no active match yet");

        string _json = CardCreator.GetJSON(request.Request.Body);
        InvokeRequest? action = JsonSerializer.Deserialize<InvokeRequest>(_json);

        ActionResponse response = new ActionResponse(false, false, false);
        if (action != null && match.player.Hand[action.CardIndex] is MonsterCard monster) { 
            if (monster != null) {
                int turnCount = match.TurnCounter;
                match.Play(action.CardIndex, null);

                response.canMove = true;
            }
        }

        return JsonSerializer.Serialize(response);
    }

    public static string Attack(int card, int target, HttpContext request) {
        if (match == null) throw new Exception("there is no active match yet");

        MonsterCard? _target = match.enemy.Table[target];
        ActionResponse response = new ActionResponse(false, false, false);

        if (_target != null && match.player.Table[card] != null) {
            try {
                response = new ActionResponse(true, match.Attack(card, _target), match.Winner() != null);

                return JsonSerializer.Serialize(response);
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
            }
        }

        return JsonSerializer.Serialize(response);
    }
}