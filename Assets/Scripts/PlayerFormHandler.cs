using UnityEngine;
using Photon.Pun;
using System;
using UnityEngine.UIElements;

public class PlayerFormHandler : MonoBehaviourPunCallbacks
{
    public FormType CurrentForm;

    [SerializeField] private GameObject stoneModel;
    [SerializeField] private GameObject veilModel;
    [SerializeField] private GameObject bladeModel;

    private void Start()
    {
        Debug.developerConsoleEnabled = true;
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
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!photonView.IsMine) return;

        Debug.Log(CurrentForm);

        PlayerFormHandler other = collision.gameObject.GetComponent<PlayerFormHandler>();
        if (other == null) return;

        if (Beats(CurrentForm, other.CurrentForm) && !other.photonView.IsMine)
        {
            MutateOtherPlayer(other);
        }
    }

    private void MutateOtherPlayer(PlayerFormHandler otherPlayer)
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
}
