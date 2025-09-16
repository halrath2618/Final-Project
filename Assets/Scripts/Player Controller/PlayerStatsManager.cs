using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager instance;
    public float health;
    public float stamina;
    public float mana;

    public int HPPotionCount = 5;
    public int MPPotionCount = 5;

    public int storyProgress = 0;
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
}
