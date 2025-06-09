using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    public static ScoreManager Instance { get; private set; }

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
    }
    private void Start()
    {
        UpdateScoreUI(GetPlayerScore(PhotonNetwork.LocalPlayer));
    }

    public void SetPlayerScore(int score)
    {
        Hashtable props = new Hashtable
        {
            {
                "score", score
            }
        };

        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }

    public int GetPlayerScore(Photon.Realtime.Player player)
    {
        if (player.CustomProperties.TryGetValue("score", out object value)) 
            return (int)value;
        return 0;
    }

    public void AddScoreToLocalPlayer(int amount)
    {
        int currentScore = GetPlayerScore(PhotonNetwork.LocalPlayer);
        SetPlayerScore(currentScore + amount);
    }

    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player player, Hashtable changedProps)
    {
        if (!changedProps.ContainsKey("score")) return;
        int newScore = (int)changedProps["score"];
        UpdateScoreUI(newScore);
    }

    private void UpdateScoreUI(int score)
    {
        _scoreText.text = "Score:" + score;
    }
}
