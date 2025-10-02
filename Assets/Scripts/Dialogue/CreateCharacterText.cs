using CHARACTERS;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class CreateCharacterText : MonoBehaviour
{
    public static CreateCharacterText instance;
    public Character Z;
    public Character H;
    public Character B;
    public Character S;
    public Character G;
    public Character Z1;
    public Character N;
    public MainMenuActive mainMenuActive;

    public static Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

    public void Awake()
    {
        
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    private void Start()
    {
        Z = CreateCharacter("Zino") as Character_Text;
        H = CreateCharacter("Halrath") as Character_Text;
        B = CreateCharacter("Blacky") as Character_Text;
        S = CreateCharacter("Scy") as Character_Text;
        G = CreateCharacter("Guard") as Character_Text;
        Z1 = CreateCharacter("Demon") as Character_Text;
        N = CreateCharacter(" ") as Character_Text;
    }
    private void Update()
    {
        mainMenuActive = FindAnyObjectByType<MainMenuActive>();
        if (mainMenuActive != null)
        {
            if (mainMenuActive.isActive)
            {
                Destroy(gameObject);
            }
        }
    }
}
