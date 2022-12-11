namespace Players;

using Match;

public class VirtualPlayer : Player{
    public VirtualPlayer(int cardsInHand, int cardsInTable) : base(cardsInHand, cardsInTable){
        //do nothing
    }

    public override void BeginTurn(Match? match) {
        if (match != null) {
            base.BeginTurn(match);

            MakeMovement(match.Clone());

            EndTurn();
        }
    }

    public void MakeMovement(Match match){
        #region Stage #1: Invoke Monsters
            //todo
        #endregion

        #region Stage #2: Add powers to monsters
            //todo
        #endregion

        #region Stage #3: Attack
            //todo
        #endregion
    }
}