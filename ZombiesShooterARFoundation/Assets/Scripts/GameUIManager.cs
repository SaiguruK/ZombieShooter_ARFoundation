using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    public Animator bloodAnimController;
    public Text ActiveCountTxt;
    public Text EliminatedCountTxt;

    public Text KillCountUI;

    int activeCount, eliminatedCount;

    private void OnEnable()
    {
        Zombie.IsAttacking += ShowBlood;
        Zombie.NotAttacking += HideBlood;

        Zombie.IsActive += OnZombieCreated;
        Zombie.IsEliminated += OnZombieDestroyed;
    }

    public void UpdateKillCount()
    {
        KillCountUI.text = "Kill Count: " + eliminatedCount;
    }

    void ShowBlood()
    {
        bloodAnimController.SetTrigger("showblood");
    }

    void HideBlood()
    {
        bloodAnimController.SetTrigger("hideblood");
    }

    void OnZombieCreated()
    {
        activeCount++;
        ActiveCountTxt.text = "Active: " + activeCount;
    }

    void OnZombieDestroyed()
    {
        activeCount--;
        eliminatedCount++;
        ActiveCountTxt.text = "Active: " + activeCount;
        EliminatedCountTxt.text = "Eliminated: " + eliminatedCount;
    }

    private void OnDisable()
    {
        Zombie.IsAttacking -= ShowBlood;
        Zombie.NotAttacking -= HideBlood;

        Zombie.IsActive -= OnZombieCreated;
        Zombie.IsEliminated -= OnZombieDestroyed;
    }
}
