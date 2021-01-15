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

    public void UpdateNbContaminatedPlayer(GameObject obj)
    {
        // TODO -- IF IS A SCIENTIST OR VIRUS -- SET VALUES
        Inst.CheckEndGame();
    }

    private void CheckEndGame()
    {
        if (Inst.NbContaminatedPlayerByVR >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbContaminatedPlayerByKMS >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbAreaContainerKMS >= 5 || Inst.NbAreaContainerVR >= 5)
        {
            // TODO -- DISPLAY ENDGAME PANEL
            Debug.LogError("Fin de Partie");
            Inst.isKMSVictoryTeam = Inst.NbContaminatedPlayerByKMS >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbAreaContainerKMS >= 5;            
            SceneManager.LoadScene("EndGameScene");
        }
    }
}
