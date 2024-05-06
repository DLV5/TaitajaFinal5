using System;

[Serializable]
public enum GameState
{
    NewTurnStarted,
    Playing,
    Paused,
    FirstPlayerWin,
    SecondPlayerWin
}
