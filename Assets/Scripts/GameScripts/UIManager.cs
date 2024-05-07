using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Controls all in game UI
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _announcerTextLine1;
    [SerializeField] private TMP_Text _announcerTextLine2;


    [SerializeField] private GameObject[] _winIndicatorGrids;
    [SerializeField] private GameObject _winIndicator;

    private GameObject _winScreen;
    [SerializeField] private TMP_Text _winText;

    private GameObject _pauseScreen;

    private int _roundNumber = 0;

    [field: SerializeField] public TMP_Text LevelTimer {  get; set; }

    public ObjectHUD[] HealthSliders { get; private set; }

    public static UIManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        _winScreen = GameObject.Find("WinScreen");
        _pauseScreen = GameObject.Find("PauseScreen");
    }

    private void Start()
    {
        HideScreen(_winScreen);
        HideScreen(_pauseScreen);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
        PlayerSpawner.OnBothPlayersConnected += () => StartCoroutine(ShowMessagesBeforeTurn());
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChangedEventHandelr;
        PlayerSpawner.OnBothPlayersConnected -= () => StartCoroutine(ShowMessagesBeforeTurn());
    }

    public void AddWinIndicator(int player)
    {
        GameObject go = Instantiate(_winIndicator, transform.position, Quaternion.identity);
        go.transform.SetParent(_winIndicatorGrids[player].transform);
    }

    private void OnGameStateChangedEventHandelr(GameState state)
    {
        switch (state)
        {
            case GameState.NewTurnStarted:
                StartCoroutine(ShowMessagesBeforeTurn());
                break;
            case GameState.Playing:
                HideScreen(_pauseScreen); 
                break;
            case GameState.Paused:
                ShowScreen(_pauseScreen);
                break;
            case GameState.FirstPlayerWin:
                AddOneWinToFirstPlayer();
                break;
            case GameState.SecondPlayerWin:
                AddOneWinToSecondPlayer();
                break;
            case GameState.GameEnded:
                ShowScreen(_winScreen);
                string winText = GameManager.Instance.FirstPlayerWins >= 2 ? "First " : "Second ";
                winText += "player wins this battle!";
                _winText.text = winText;
                break;
        }
    }

    private IEnumerator ShowMessagesBeforeTurn()
    {
        ShowScreen(_announcerTextLine1.gameObject);
        ShowScreen(_announcerTextLine2.gameObject);

        _announcerTextLine2.text = "";

        if(_roundNumber == 0)
        {
            _announcerTextLine1.text = "Round 1";

            //yield return new WaitForSeconds(2);

            //_announcerTextLine1.text = "Prepare to fight";

            //yield return new WaitForSeconds(1);

            //_announcerTextLine2.text = "3";

            //yield return new WaitForSeconds(1);

            //_announcerTextLine2.text = "2";

            //yield return new WaitForSeconds(1);

            //_announcerTextLine2.text = "1";

            yield return new WaitForSeconds(1);
        } else
        {
            _announcerTextLine1.text = "Round " + (_roundNumber + 1);

            //yield return new WaitForSeconds(2);

            //_announcerTextLine1.text = "Prepare to fight";

            //yield return new WaitForSeconds(1);

            //_announcerTextLine2.text = "3";

            //yield return new WaitForSeconds(1);

            //_announcerTextLine2.text = "2";

            //yield return new WaitForSeconds(1);

            //_announcerTextLine2.text = "1";
            yield return new WaitForSeconds(1);
        }


        GameManager.Instance.ChangeGameState(GameState.Playing);

        HideScreen(_announcerTextLine1.gameObject);
        HideScreen(_announcerTextLine2.gameObject);

        _roundNumber++;
    }

    private void AddOneWinToFirstPlayer()
    {
        GameObject indicator = Instantiate(_winIndicator, gameObject.transform.position, Quaternion.identity);
        indicator.transform.SetParent(_winIndicatorGrids[0].transform);
        indicator.transform.localScale = new Vector3(1, 1, 1);

        ShowScreen(_announcerTextLine1.gameObject);
        _announcerTextLine1.text = "Player one win this round!";

        StartCoroutine(StartNextTurnWithDelay());
    }
    
    private void AddOneWinToSecondPlayer()
    {
        GameObject indicator = Instantiate(_winIndicator, gameObject.transform.position, Quaternion.identity);
        indicator.transform.SetParent(_winIndicatorGrids[1].transform);
        indicator.transform.localScale = new Vector3(1, 1, 1);

        ShowScreen(_announcerTextLine1.gameObject);
        _announcerTextLine1.text = "Player two win this round!";

        StartCoroutine(StartNextTurnWithDelay());
    }

    private void ShowScreen(GameObject screen) => screen.SetActive(true);
    private void HideScreen(GameObject screen) => screen.SetActive(false);

    private IEnumerator StartNextTurnWithDelay()
    {
        yield return new WaitForSecondsRealtime(2);
        GameManager.Instance.ChangeGameState(GameState.NewTurnStarted);
    }
}
