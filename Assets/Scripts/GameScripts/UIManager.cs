using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _announcerTextLine1;
    [SerializeField] private TMP_Text _announcerTextLine2;


    [SerializeField] private GameObject[] _winIndicatorGrids;
    [SerializeField] private GameObject _winIndicator;

    private GameObject _loseScreen;
    private GameObject _winScreen;
    private GameObject _pauseScreen;

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

        _loseScreen = GameObject.Find("LoseScreen");
        _winScreen = GameObject.Find("WinScreen");
        _pauseScreen = GameObject.Find("PauseScreen");
    }

    private void Start()
    {
        HideScreen(_loseScreen);
        HideScreen(_winScreen);
        HideScreen(_pauseScreen);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnGameStateChanged += OnGameStateChangedEventHandelr;
    }
    
    private void OnDisable()
    {
        GameManager.Instance.OnGameStateChanged -= OnGameStateChangedEventHandelr;
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
                ShowScreen(_winScreen);
                break;
            case GameState.SecondPlayerWin:
                ShowScreen(_loseScreen);
                break;
        }
    }

    private IEnumerator ShowMessagesBeforeTurn()
    {
        ShowScreen(_announcerTextLine1.gameObject);
        ShowScreen(_announcerTextLine2.gameObject);

        _announcerTextLine2.text = "";

        _announcerTextLine1.text = "Round 1";

        yield return new WaitForSeconds(2);

        _announcerTextLine1.text = "Prepare to fight";

        yield return new WaitForSeconds(1);

        _announcerTextLine2.text = "3";

        yield return new WaitForSeconds(1);

        _announcerTextLine2.text = "2";

        yield return new WaitForSeconds(1);

        _announcerTextLine2.text = "1";

        yield return new WaitForSeconds(1);

        GameManager.Instance.ChangeGameState(GameState.Playing);

        HideScreen(_announcerTextLine1.gameObject);
        HideScreen(_announcerTextLine2.gameObject);
    }

    private void ShowScreen(GameObject screen) => screen.SetActive(true);
    private void HideScreen(GameObject screen) => screen.SetActive(false);
}
