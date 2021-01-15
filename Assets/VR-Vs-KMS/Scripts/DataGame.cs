using UnityEngine;
using UnityEngine.SceneManagement;
using vr_vs_kms;

[System.Serializable]
public class DataGame
{
    public int NbAreaContainerKMS = 0;
    public int NbAreaContainerVR = 0;
    public int NbContaminatedPlayerByKMS = 0;
    public int NbContaminatedPlayerByVR = 0;

    public bool isKMSVictoryTeam;

    private static DataGame inst;

    public static DataGame Inst
    {
        get
        {
            if (inst == null) inst = new DataGame();
            return inst;
        }
    }

    // Update AreaContamination taken by players
    public void UpdateNbAreaContainer(bool isKMS, bool swapSides)
    {
        if (isKMS) {
            Inst.NbAreaContainerKMS += 1;
            if (swapSides) {
                Inst.NbAreaContainerVR -= 1;
            }
        } else {
            Inst.NbAreaContainerVR += 1;
            if (swapSides)
            {
                Inst.NbAreaContainerKMS -= 1;
            }
        }
        Inst.CheckEndGame();
    }

    // Update of the number of infected players
    public void UpdateNbContaminatedPlayer(bool isKMS)
    {
        // IF IS A SCIENTIST OR VIRUS PLAYER -- SET VALUES
        if (isKMS) { Inst.NbContaminatedPlayerByVR += 1; }
        else { Inst.NbContaminatedPlayerByKMS += 1; }
        Inst.CheckEndGame();
    }

    private void CheckEndGame()
    {
        if (Inst.NbContaminatedPlayerByVR >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbContaminatedPlayerByKMS >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbAreaContainerKMS >= 5 || Inst.NbAreaContainerVR >= 5)
        {
            // DISPLAY ENDGAME PANEL
            Debug.LogError("Fin de Partie");
            Inst.isKMSVictoryTeam = Inst.NbContaminatedPlayerByKMS >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbAreaContainerKMS >= 5;            
            SceneManager.LoadScene("EndGameScene");
        }
    }
}
