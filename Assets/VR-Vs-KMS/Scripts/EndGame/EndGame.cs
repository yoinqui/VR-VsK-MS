using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vr_vs_kms;

public class EndGame : MonoBehaviour
{
    public GameObject EndGamePanel;
    private string title = "Victoire des ";

    public void Awake()
    {
        title += DataGame.Inst.isKMSVictoryTeam ? "scientifiques !" : "virus !";
        EndGamePanel.GetComponentInChildren<Text>().color = DataGame.Inst.isKMSVictoryTeam ? Color.green : Color.red;
        EndGamePanel.GetComponentInChildren<Text>().text = title.ToUpper();
    }
}
