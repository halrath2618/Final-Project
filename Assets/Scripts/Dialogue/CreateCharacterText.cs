using CHARACTERS;
using UnityEngine;

public class CreateCharacterText : MonoBehaviour
{
    [SerializeField] public Character Z;
    [SerializeField] public Character H;
    [SerializeField] public Character B;
    [SerializeField] public Character S;
    [SerializeField] public Character G;
    [SerializeField] public Character Z1;
    [SerializeField] public Character N;

    public static Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

    public void Start()
    {
        Z = CreateCharacter("Zino") as Character_Text;
        H = CreateCharacter("Halrath") as Character_Text;
        B = CreateCharacter("Blacky") as Character_Text;
        S = CreateCharacter("Scy") as Character_Text;
        G = CreateCharacter("Guard") as Character_Text;
        Z1 = CreateCharacter("Demon") as Character_Text;
        N = CreateCharacter(" ") as Character_Text;
    }
}
