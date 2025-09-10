using UnityEngine;
using CHARACTERS;
using System.Collections;

public class CreateCharacterText : MonoBehaviour
{
    [SerializeField] public Character Z;
    [SerializeField] public Character H;
    [SerializeField] public Character B;
    [SerializeField] public Character S;
    [SerializeField] public Character G;
    [SerializeField] public Character Z1;

    public static Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

    public void Start()
    {
        Z = CreateCharacter("Zino");
        H = CreateCharacter("Halrath");
        B = CreateCharacter("Blacky");
        S = CreateCharacter("Scy");
        G = CreateCharacter("Guard");
        Z1 = CreateCharacter("Demon");
    }
}
