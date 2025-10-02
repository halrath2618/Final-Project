using TMPro;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;
    public float health = 100f;
    public float stamina = 100f;
    public float mana = 100f;

    public int characterClassNum = 0;

    public int HPPotionCount = 5;
    public int MPPotionCount = 0;

    public int storyProgress = 0;
    public int gold = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
       else
        {
            Destroy(gameObject);
        }
    }
    public void AddHPPotion(int num)
    {
        HPPotionCount = HPPotionCount + num;
    }
    public void AddMPPotion(int num)
    {
        MPPotionCount = MPPotionCount + num;
    }
    public void AddGold(int amount)
    {
        gold += amount;
    }

}
