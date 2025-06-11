using UnityEngine;
using Photon.Pun;
using TMPro;

public class Player : MonoBehaviourPunCallbacks
{

    [SerializeField] private GameObject stoneModel;
    [SerializeField] private GameObject veilModel;
    [SerializeField] private GameObject bladeModel;

    private ScoreManager _scoreManager;
    private ExpManager _expManager;

    public FormType CurrentForm;

    public Rigidbody rb {  get; private set; }

    private PlayerInputSet inputSet;
    private PlayerStateMachine stateMachine;
    public PlayerIdleState idleState {  get; private set; }
    public PlayerMoveState moveState {  get; private set; }

    public Vector2 moveInput { get; private set; }

    public float moveSpeed = 10f;

    public string nickName;
    public TextMeshPro nickNameText;

    private void Awake()
    {
        _scoreManager = ScoreManager.Instance;
        _expManager = ExpManager.Instance;

        rb = GetComponent<Rigidbody>();
        inputSet = new PlayerInputSet();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "idle");
        moveState = new PlayerMoveState(this, stateMachine, "move");
    }

    private void OnEnable()
    {
        inputSet.Enable();

        inputSet.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputSet.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputSet.Disable();
    }

    private void Start()
    {
        if (!photonView.IsMine)
            return;

        stateMachine.Initialize(idleState);
    }

    private void Update()
    {  
        if(photonView.IsMine)
            stateMachine.UpdateActiveState();
    }

    public void SetVelocity(float xVelocity, float zVelocity)
    {
        if(photonView.IsMine)
            rb.linearVelocity = new Vector3(xVelocity, rb.linearVelocity.y, zVelocity);
    }

    [PunRPC]
    public void SetInitialForm(int form)
    {
        CurrentForm = (FormType)form;
        UpdateModel();
    }

    private void UpdateModel()
    {
        stoneModel.SetActive(CurrentForm == FormType.Stone);
        bladeModel.SetActive(CurrentForm == FormType.Blade);
        veilModel.SetActive(CurrentForm == FormType.Veil);
        
        nickNameText.gameObject.SetActive(photonView.IsMine);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!photonView.IsMine) return;

        Debug.Log(CurrentForm);

        Player other = collision.gameObject.GetComponent<Player>();
        if (other == null) return;

        if (Beats(CurrentForm, other.CurrentForm) && !other.photonView.IsMine)
        {
            _scoreManager.AddScoreToLocalPlayer(1);
            _expManager.AddEnergy(20);
            MutateOtherPlayer(other);
            
            other.photonView.RPC(nameof(LoseEnergy), other.photonView.Owner, 10);
        }
    }

    private void MutateOtherPlayer(Player otherPlayer)
    {
        int formIndex = (int)CurrentForm;
        otherPlayer.photonView.RPC(nameof(Mutate), RpcTarget.All, formIndex);
    }


    bool Beats(FormType mine, FormType theirs)
    {
        return (mine == FormType.Stone && theirs == FormType.Blade) ||
               (mine == FormType.Blade && theirs == FormType.Veil) ||
               (mine == FormType.Veil && theirs == FormType.Stone);
    }

    [PunRPC]
    private void Mutate(int newForm)
    {
        CurrentForm = (FormType)newForm;
        UpdateModel();
    }

    [PunRPC]
    private void SetNickname(string _name)
    {
        nickName = _name;
        nickNameText.text = nickName;
    }
    
    [PunRPC]
    private void LoseEnergy(int amount)
    {
        _expManager.SpendEnergy(amount);
    }
}
