using Photon.Pun;
using TMPro;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private GameObject playerHolder;

    private ScoreManager _scoreManager;

    public float refreshRate = 1.0f;
    public GameObject[] slots;
    public TextMeshProUGUI[] scoreTexts;
    public TextMeshProUGUI[] nameTexts;

    private void Awake()
    {
        _scoreManager = ScoreManager.Instance;
    }

    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 1f, refreshRate);
    }

    public void Refresh()
    {
        foreach (var slot in slots)
        {
            slot.SetActive(false);
        }

        var sortedPlayerList = (from player in PhotonNetwork.PlayerList orderby _scoreManager.GetPlayerScore(player) descending select player).ToList();

        for(int i = 0; i < sortedPlayerList.Count && i < slots.Length; i++)
        {
            var player = sortedPlayerList[i];

            slots[i].SetActive(true);
            
            if(player.NickName == "")
                player.NickName = "Unnamed";
            
            nameTexts[i].text = player.NickName;
            scoreTexts[i].text = _scoreManager.GetPlayerScore(player).ToString();
        }    
    }

    private void Update()
    {
        playerHolder.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
