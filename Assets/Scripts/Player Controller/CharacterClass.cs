// CharacterClass.cs
using UnityEngine;

public enum CharacterClass
{
    Brawler,
    Mage,
    SwordMaster,
    Starter,
    After_Meet_Halrath
}

// CharacterClassData.cs
[CreateAssetMenu(fileName = "NewClassData", menuName = "Character/Class Data")]
public class CharacterClassData : ScriptableObject
{
    public CharacterClass characterClass;
    public RuntimeAnimatorController animatorController;
    public Avatar avatar; // Optional for humanoid rigs
}