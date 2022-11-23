using UnityEngine;
using Zork.Common;
using Newtonsoft.Json;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    private Game _game;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Player.LocationChanged += Player_LocationChanged;
        _game.Player.MovesChanged += Player_MovesChanged;
        _game.Player.ScoreChanged += Player_ScoreChanged;
        _game.Run(InputService, OutputService);
        LocationText.text = _game.Player.CurrentRoom.Name;
        ScoreText.text = "Score: " + _game.score;
        MovesText.text = "Moves: " + moves;
    }

    private void Player_LocationChanged(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    int moves = 0;

    private void Player_MovesChanged(object sender, int moves)
    {
        MovesText.text = $"Moves: {moves}";
    }

    private void Player_ScoreChanged(object sender, int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    void Start()
    {
        InputService.SetFocus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
        }

        if (_game.IsRunning == false)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }

            if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
