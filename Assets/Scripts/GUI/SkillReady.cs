using UnityEngine;

public class SkillReady : MonoBehaviour
{
    public GameObject skill_1;
    public GameObject skill_2;
    public GameObject skill_3;
    public GameObject skill_4;
    public GameObject skill_5;
    [SerializeField] private SkillCoolDownManager skillCoolDownManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        skillCoolDownManager = FindAnyObjectByType<SkillCoolDownManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (skillCoolDownManager.skill1_isReady)
        {
            skill_1.SetActive(true);
        }
        else
        {
            skill_1.SetActive(false);
        }
        if (skillCoolDownManager.skill2_isReady)
        {
            skill_2.SetActive(true);
        }
        else
        {
            skill_2.SetActive(false);
        }
        if (skillCoolDownManager.auraReady)
        {
            skill_3.SetActive(true);
        }
        else
        {
            skill_3.SetActive(false);
        }
        if (skillCoolDownManager.manacdReady)
        {
            skill_4.SetActive(true);
        }
        else
        {
            skill_4.SetActive(false);
        }
        if (skillCoolDownManager.hpcdReady)
        {
            skill_5.SetActive(true);
        }
        else
        {
            skill_5.SetActive(false);
        }
    }
}
