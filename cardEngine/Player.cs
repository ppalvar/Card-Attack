namespace Player;

using System;
using CardStores;

class Player{
    public bool IsPlaying{get;set;}
    private Deck? PlayerDeck{get;set;}

    public void Play(){
        //TODO
    }
    
    public void BeginTurn(){
        IsPlaying = true;
    }

    public void EndTurn(){
        IsPlaying = false;
    }
}