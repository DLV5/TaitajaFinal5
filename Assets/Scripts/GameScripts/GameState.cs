using System;

[Serializable]
public enum GameState
{
    WaitingPlayersToConnect,
    NewTurnStarted,
    Playing,
    Paused,
    FirstPlayerWin,
    SecondPlayerWin,
    GameEnded
}
