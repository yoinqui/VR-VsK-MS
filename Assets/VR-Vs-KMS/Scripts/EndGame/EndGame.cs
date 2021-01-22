using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using vr_vs_kms;

public class EndGame : MonoBehaviour
{
    public Text EndGameTextPanel;
    public Image BackgroundImage;

    public Sprite VirusImg;
    public Sprite ScientistImg;

    private string title = "Victoire des ";

    public void Awake()
    {
        title += DataGame.Inst.isKMSVictoryTeam ? "scientifiques !" : "virus !";
        EndGameTextPanel.color = DataGame.Inst.isKMSVictoryTeam ? Color.green : Color.red;
        EndGameTextPanel.text = title.ToUpper();
        BackgroundImage.sprite = DataGame.Inst.isKMSVictoryTeam ? ScientistImg : VirusImg;
    }
}
