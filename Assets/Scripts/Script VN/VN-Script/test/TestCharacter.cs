using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHARACTERS;
using DIALOGUE;
using TMPro;

namespace TESTING
{
    public class TestCharacter : MonoBehaviour
    {
        
        public TMP_FontAsset tempFont;
        private Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);

        public CanvasShaking shaking;
        // Start is called before the first frame update
        void Start()
        {
            
            //Character halrath = CharacterManager.instance.CreateCharacter("Halrath");
            //Character halrath2 = CharacterManager.instance.CreateCharacter("Halrath");
            //Character pan = CharacterManager.instance.CreateCharacter("Pan");
            StartCoroutine(Test());
        }

        IEnumerator Test()
        {

            
            shaking.Shake();

            Character mys = CreateCharacter("?????") as Character_Text;

            yield return mys.Say("Ouch!!!!! My head hurt.....");



            Character_Sprite Halrath = CreateCharacter("Halrath") as Character_Sprite;

            

            Halrath.SetPosition(new Vector2(0.7f, 0));
            yield return Halrath.Show();

            Halrath.Animate("Walking Left", true);
            yield return new WaitForSeconds(1);

            yield return Halrath.Say("Are you alright??? Are you hurt???");
            yield return Halrath.Say("What your name?{a} Where are you from?");

            Character_Sprite Zino = CreateCharacter("Zino") as Character_Sprite;

            Zino.SetPosition(new Vector2(0,0));

            yield return new WaitForSeconds(1);
            yield return Zino.Show();
            shaking.Shake();

            yield return Zino.Say("Eikkkkk!! Monster!!!!!!!!!!");

            yield return Zino.MoveToPosition(new Vector2(-1,0),2, false);

            yield return null;





        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}