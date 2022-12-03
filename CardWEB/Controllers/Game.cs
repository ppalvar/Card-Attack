namespace Controllers;

using Match;
using Powers;
using Cards;
using Models;
using System.Text.Json;

public static class Game {
    private static Match? match{get;set;}

    public static string NewGame(string type, HttpContext request) {
        if (type == "human") {
            match = new Match(MatchTypes.HumanVSHuman, 5, 5);
        }
        else if (type == "ai"){
            match = new Match(MatchTypes.ComputerVSHuman, 5, 5);
        }

        string json = CardCreator.GetJSON(request.Request.Body);

        CardNames? names = JsonSerializer.Deserialize<CardNames>(json);

        if (match != null) {
            if (names != null) {
                match.SetDeck(names.playerA, true);
                match.SetDeck(names.playerB, false);
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

                response.Hand = new CardResponse[match.player.Hand.Length];
                for (int i = 0; i < match.player.Hand.Length; i++) {
                    Card? m = match.player.Hand[i];

                    if (m != null) {
                        response.Hand[i] = new CardResponse(m, i);
                    }
                }

                response.TableA = new CardResponse[match.player.Table.Length];
                response.TableB = new CardResponse[match.player.Table.Length];
                
                for (int i = 0; i < match.player.Table.Length; i++) {
                    MonsterCard? p = (MonsterCard?) match.player.Table[i];
                    MonsterCard? e = (MonsterCard?) match.enemy.Table[i];

                    if (p != null) {
                        response.TableA[i] = new CardResponse(p, i);
                    }
                    if (e != null) {
                        response.TableB[i] = new CardResponse(e, i);
                    }
                }

                response.Hand = response.Hand.Where((val) => val != null).ToArray();

                return JsonSerializer.Serialize(response);
            }
            catch (Exception e){
                Console.WriteLine(e.Message);
            }
        }

        return JsonSerializer.Serialize(response);
    }

    public static string DropCard(int index, HttpContext httpContext) {
        if (match == null) throw new Exception("there is no active match yet");

        bool flag = match.player.DropCard(index);
        bool endGame = match.Winner() != null;

        return JsonSerializer.Serialize(new ActionResponse(flag, false, endGame));
    }

    public static string EquipPower(int cardIndex, int targetIndex, HttpContext context) {
        if (match == null) throw new Exception("there is no active match yet");

        bool flag = false;

        try {
            match.Play(cardIndex, match.player.Table[targetIndex]);
            flag = true;
        }
        catch {
            // do nothing
        }

        bool endGame = match.Winner() != null;

        return JsonSerializer.Serialize(new ActionResponse(flag, false, endGame));
    }

    public static string UsePower(int cardindex, int targetIndex, string powerName, HttpContext context) {
        if (match == null) throw new Exception("there is no active match yet");

        ActionResponse response = new ActionResponse(false, false, false);

        if (cardindex <= match.player.Table.Length && targetIndex <= match.enemy.Table.Length) {
            MonsterCard? monster = (MonsterCard?) match.player.Table[cardindex];

            if (monster != null) {
                int power = -1;
                for (int i = 0; i < monster.Powers.Length; i++) {
                    Power? p = monster.Powers[i];

                    if (p != null && p.Name == powerName) {
                        power = i;
                    }
                }

                if (power != -1) {
                    MonsterCard? target = (MonsterCard?) match.enemy.Table[targetIndex];
                    if (target != null) {
                        try {
                            response.turnEnds = match.UsePower(cardindex, power, target);

                            response.canMove = true;
                        }
                        catch (Exception e) {
                            Console.WriteLine(e.Message);
                        }
                    }
                }
            }
        }

        response.gameEnds = match.Winner() != null;

        response.Hand = new CardResponse[match.player.Hand.Length];
        for (int i = 0; i < match.player.Hand.Length; i++) {
            Card? m = match.player.Hand[i];

            if (m != null) {
                response.Hand[i] = new CardResponse(m, i);
            }
        }

        response.TableA = new CardResponse[match.player.Table.Length];
        response.TableB = new CardResponse[match.player.Table.Length];
        
        for (int i = 0; i < match.player.Table.Length; i++) {
            MonsterCard? p = (MonsterCard?) match.player.Table[i];
            MonsterCard? e = (MonsterCard?) match.enemy.Table[i];

            if (p != null) {
                response.TableA[i] = new CardResponse(p, i);
            }
            if (e != null) {
                response.TableB[i] = new CardResponse(e, i);
            }
        }

        response.Hand = response.Hand.Where((val) => val != null).ToArray();

        return JsonSerializer.Serialize(response);
    }
}