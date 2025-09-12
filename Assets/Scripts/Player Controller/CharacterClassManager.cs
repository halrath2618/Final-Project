using UnityEngine;

public class CharacterClassManager : MonoBehaviour
{
    [Header("Class References")]
    [SerializeField] private CharacterClassData[] classDatas;

    [Header("Components")]
    [SerializeField] private Animator characterAnimator;
    //[SerializeField] private Transform equipmentSocket;

    private CharacterClass currentClass;
    private GameObject currentEquipment;


    public void SwitchClass(CharacterClass newClass)
    {
        if (currentClass == newClass) return;

        // Find class data
        CharacterClassData classData = GetClassData(newClass);
        if (classData == null)
        {
            Debug.LogError($"No data found for class: {newClass}");
            return;
        }

        // Switch animator controller
        characterAnimator.runtimeAnimatorController = classData.animatorController;

        // Set avatar if using humanoid rig
        if (classData.avatar != null)
        {
            characterAnimator.avatar = classData.avatar;
        }

        // Update state
        currentClass = newClass;
        Debug.Log($"Class changed to: {newClass}");
    }

    private CharacterClassData GetClassData(CharacterClass targetClass)
    {
        foreach (CharacterClassData data in classDatas)
        {
            if (data.characterClass == targetClass)
            {
                return data;
            }
        }
        return null;
    }

    //private void SwapEquipment(GameObject newEquipmentPrefab)
    //{
    //    // Remove current equipment
    //    if (currentEquipment != null)
    //    {
    //        Destroy(currentEquipment);
    //    }

    //    // Instantiate new equipment
    //    if (newEquipmentPrefab != null)
    //    {
    //        currentEquipment = Instantiate(
    //            newEquipmentPrefab,
    //            equipmentSocket.position,
    //            equipmentSocket.rotation,
    //            equipmentSocket
    //        );
    //    }
    //}

    // Example method for UI buttons
    //public void SetWarriorClass() => SwitchClass(CharacterClass.Brawler);
    //public void SetMageClass() => SwitchClass(CharacterClass.Mage);
    //public void SetArcherClass() => SwitchClass(CharacterClass.SwordMaster);
}