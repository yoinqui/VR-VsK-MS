using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DataGame
{
    public int NbAreaContainerKMS = 0;
    public int NbAreaContainerVR = 0;
    public int NbContaminatedPlayerByKMS = 0;
    public int NbContaminatedPlayerByVR = 0;

    public bool isKMSVictoryTeam;
    private readonly int ContaminationAreaCount = GameObject.FindGameObjectsWithTag("ContaminationArea").Length;

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
    public void UpdateNbContaminatedPlayer(GameObject player)
    {
        // IF IS A SCIENTIST OR VIRUS PLAYER -- SET VALUES
        if (player.GetComponent<IsScientistPlayer>() != null) { Inst.NbContaminatedPlayerByVR += 1; }
        else if (player.tag == "VRPlayer") { Inst.NbContaminatedPlayerByKMS += 1; }
        Debug.LogError("VRPlayer : " + player.tag == "VRPlayer");
        Inst.CheckEndGame();
    }

    private void CheckEndGame()
    {
        if (Inst.NbContaminatedPlayerByVR >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbContaminatedPlayerByKMS >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbAreaContainerKMS >= Inst.ContaminationAreaCount 
            || Inst.NbAreaContainerVR >= Inst.ContaminationAreaCount)
        {
            // DISPLAY ENDGAME PANEL
            Inst.isKMSVictoryTeam = Inst.NbContaminatedPlayerByKMS >= GameConfig.Inst.NbContaminatedPlayerToVictory
            || Inst.NbAreaContainerKMS >= Inst.ContaminationAreaCount;
            SceneManager.LoadScene("EndGameScene");
        }
    }
}
