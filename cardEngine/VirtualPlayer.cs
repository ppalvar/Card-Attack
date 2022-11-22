namespace Players;

public class VirtualPlayer : Player{
    public VirtualPlayer(int cardsInHand, int cardsInTable) : base(cardsInHand, cardsInTable){
        //do nothing
    }

    public override void BeginTurn(){
        base.BeginTurn();

        MakeMovement();
    }

    public void MakeMovement(){
        //todo: put logic in here
    }
}