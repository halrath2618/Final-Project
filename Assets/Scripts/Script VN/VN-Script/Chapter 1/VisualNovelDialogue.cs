using CHARACTERS;
using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VisualNovelDialogue : MonoBehaviour
{
    public Button choice1;
    public Button choice2;
    public TMP_Text text1;
    public TMP_Text text2;

    public SoundtrackController soundtrack;

    public GameObject choicePanel;
    public RectTransform _choicePanel;
    public static TMP_FontAsset tempFont;
    private float storyProcess;
    private double mainHealthPoint = 100f;
    public GameObject alertPanel2;
    public Button okButton;
    public GameObject choicePanel2;
    public RectTransform _choicePanel2;
    public GameObject sorcFight;
    public GameObject knightFight;
    public GameObject assassinFight;
    public GameObject priestFight;
    public GameObject wolfRed;
    public GameObject wolfGreen;
    public GameObject wolfBlue;
    public RectTransform _wolfRed;
    public RectTransform _wolfGreen;
    public RectTransform _wolfBlue;
    public GameObject monsterHP;
    public GameObject blood;
    public GameObject run;
    public GameObject vortex;
    public GameObject fighting;

    private bool click = false;

    public TMP_Text health;

    public TMP_Text noti;

    [SerializeField] private Character_Sprite Z;
    [SerializeField] private Character_Sprite H;
    [SerializeField] private Character_Sprite B;
    [SerializeField] private Character_Sprite S;
    [SerializeField] private Character N;
    [SerializeField] private Character_Sprite D;
    [SerializeField] private Character RANDOM;
    private Sprite normal, happy, sad, cry, confused, shock;



    private int loadsavefile = 5;
    private int halrathHeartPoint = 5;
    private int scyHeartPoint = 5;
    private int blackyHeartPoint = 5;
    private string path;
    private bool knight = false;
    private bool assassin = false;
    private bool priest = false;
    private bool sorcerer = false;
    private bool stun = false;
    private bool invi = false;
    private bool amp = false;
    private bool dUp = false;
    private bool isHeal = false;
    private bool isActive = false;
    private double _atk, _def, _int, dmg;
    private int HP;
    private double wolfRHP = 100, wolfRDef = 10, wolfRAtk = 5;
    private double wolfGHP = 100, wolfGDef = 25, wolfGAtk = 5;
    private double wolfBHP = 100, wolfBDef = 19, wolfBAtk = 5;
    public TMP_Text wolfHP;
    private float esc = 0;
    private bool isDefend = false;


    private static Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);



    public CanvasShaking cv_Shaking;
    public CharacterShaking char_Shaking;
    public BackgroundChangeSystem bg;

    public void ChoicePanelAnimation()
    {
        choicePanel.SetActive(true);
        _choicePanel.DOMoveX(_choicePanel.position.x, 1).From(6000);
    }
    public void ChoicePanel2Animation()
    {
        choicePanel2.SetActive(true);
        _choicePanel2.DOMoveX(_choicePanel2.position.x, 1).From(6000);
    }
    public void EnemyAppear1()
    {
        wolfHP.text = wolfRHP.ToString();
        monsterHP.SetActive(true);
        wolfRed.SetActive(true);
        _wolfRed.DOMoveX(_wolfRed.position.x, 1).From(6000);
    }
    public void EnemyAppear2()
    {
        wolfHP.text = wolfGHP.ToString();
        monsterHP.SetActive(true);
        wolfGreen.SetActive(true);
        _wolfGreen.DOMoveX(_wolfGreen.position.x, 1).From(6000);
    }
    public void EnemyAppear3()
    {
        wolfHP.text = wolfBHP.ToString();
        monsterHP.SetActive(true);
        wolfBlue.SetActive(true);
        _wolfBlue.DOMoveX(_wolfBlue.position.x, 1).From(6000);
    }
    public void EnemyHideAll()
    {
        wolfRed.SetActive(false);
        wolfBlue.SetActive(false);
        wolfGreen.SetActive(false);
        monsterHP.SetActive(false);
    }
    public void Fighting()
    {
        fighting.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGame();
        Debug.Log("Story point: " + storyProcess);
        Z = CreateCharacter("Zino") as Character_Sprite;
        H = CreateCharacter("Halrath") as Character_Sprite;
        B = CreateCharacter("Blacky") as Character_Sprite;
        S = CreateCharacter("Scy") as Character_Sprite;
        N = CreateCharacter("  ") as Character_Text;
        D = CreateCharacter("Demon") as Character_Sprite;
        RANDOM = CreateCharacter("Guard") as Character_Text;
        happy = Z.GetSprite("Zhappy");
        sad = Z.GetSprite("Zsad");
        cry = Z.GetSprite("Zcry");
        confused = Z.GetSprite("Zconfused");
        shock = Z.GetSprite("Zshock");
        normal = Z.GetSprite("Zino");

        health.text = mainHealthPoint.ToString();

        StartCoroutine(Chap());
    }
    public void Choice1()
    {
        choicePanel.SetActive(false);
        storyProcess++;
        Debug.Log("S = " + storyProcess);
        StartCoroutine(Chap());
        SaveGame();
    }
    public void Choice2()
    {
        choicePanel.SetActive(false);
        storyProcess += 2;
        StartCoroutine(Chap());
        SaveGame();
    }

    public void Clicked()
    {
        alertPanel2.SetActive(false);
        storyProcess += 0.25f;
        StartCoroutine(Chap());
    }

    public void Knight()
    {
        choicePanel2.SetActive(false);
        storyProcess += 2;
        knight = true;
        StartCoroutine(Chap());
    }
    public void Sorcerer()
    {
        choicePanel2.SetActive(false);
        storyProcess += 2;
        sorcerer = true;
        StartCoroutine(Chap());
    }
    public void Priest()
    {
        choicePanel2.SetActive(false);
        storyProcess += 2;
        priest = true;
        StartCoroutine(Chap());
    }
    public void Assassin()
    {
        choicePanel2.SetActive(false);
        storyProcess += 2;
        assassin = true;
        StartCoroutine(Chap());
    }

    public void ShieldBash()
    {
        knightFight.SetActive(false);
        dmg = UnityEngine.Random.Range(10, 25);
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void AoEStun()
    {
        priestFight.SetActive(false);
        sorcFight.SetActive(false);
        knightFight.SetActive(false);
        stun = true;
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void DefenseUp()
    {
        knightFight.SetActive(false);
        _def = _def * 1.5f;
        dUp = true;
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void EnhancePower()
    {
        sorcFight.SetActive(false);
        amp = true;
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void Invisible()
    {
        assassinFight.SetActive(false);
        invi = true;
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void SCE()
    {
        assassinFight.SetActive(false);
        if (invi)
        {
            dmg = Math.Round(UnityEngine.Random.Range(10, 19) * 1.5f, 0);
            invi = false;
        }
        else
            dmg = UnityEngine.Random.Range(10, 19);
        storyProcess++;
        soundtrack.PlaySFXByName("SwordSlash");
        StartCoroutine(Chap());
    }
    public void DemonStrike()
    {
        assassinFight.SetActive(false);
        if (invi)
        {
            dmg = Math.Round(UnityEngine.Random.Range(1, 5) * 10 * 1.5f, 0);
            invi = false;
        }
        else
            dmg = UnityEngine.Random.Range(1, 7) * 10;
        storyProcess++;
        soundtrack.PlaySFXByName("SwordSlash");
        StartCoroutine(Chap());
    }
    public void MercyBlessing()
    {
        //if (mainhealth == 100)
        //{
        //  mercy.Setactive(false);
        //}
        priestFight.SetActive(false);
        HP = 30;
        mainHealthPoint = mainHealthPoint + HP;
        isHeal = true;
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void SolarFlare()
    {
        priestFight.SetActive(false);
        dmg = UnityEngine.Random.Range(10, 25);
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void LightningStrike()
    {

        sorcFight.SetActive(false);
        if (amp)
        {
            dmg = Math.Round(UnityEngine.Random.Range(15, 25) * 1.5f, 0);
            amp = false;
        }

        else
            dmg = UnityEngine.Random.Range(15, 25);
        storyProcess++;
        StartCoroutine(Chap());
    }
    public void Run()
    {
        run.SetActive(false);
        storyProcess = storyProcess + 0.2f;
        StartCoroutine(Chap());
    }
    public void Fight()
    {
        fighting.SetActive(false);
        soundtrack.PlaySFXByName("SwordSlash");
        dmg = UnityEngine.Random.Range(20, 30);
        storyProcess += 0.15f;
        StartCoroutine(Chap());
    }
    public void Defend()
    {
        fighting.SetActive(false);
        isDefend = true;
        storyProcess += 0.15f;
        StartCoroutine(Chap());
    }
    IEnumerator Chap()
    {

        switch (storyProcess)
        {
            case 0:
                {

                    bg.ChangeBackground("CG-CS1");

                    _atk = 1;
                    _def = 1;
                    _int = 1;
                    if (soundtrack != null)
                        soundtrack.PlayTrackByName("ForestNight");
                    yield return N.Say("Ở một vùng đất nọ. Zino, một nhà thám hiểm trẻ tuổi, đang khám phá một khu rừng bí ẩn.{c}Trên tay cậu là một chiếc đèn pin nhỏ, soi rọi từng gốc cây, bụi cỏ.{c}Màn đêm bao trùm khiến khu rừng càng trở nên âm u.{c}Vô tình, cậu phát hiện ra một ngôi đền bị bỏ hoang trong khu rừng.{a} Không một chút chần chừ, cậu tiến thẳng vào trong ngôi đền.");
                    yield return N.Say("Bên trong ngôi đền tối tăm, phủ đầy mạng nhện và rêu xanh do bỏ hoang lâu năm. Sâu bên trong căn phòng của ngôi đền, có một vật phát ra một ánh sáng yếu ớt.{c}Zino tìm thấy 1 cái rương, bên trong chứa cổ vật phát sáng.{c}Thứ ánh sáng yếu ớt gần như có sự sống, ánh sáng vươn dài ra như muốn dẫn Zino đến địa điểm nào đó.{c}Không nghĩ ngợi gì thêm, Zino đi theo luồng ánh sáng yếu ớt đấy ra khu vực phía sau của ngôi đền.");
                    bg.ChangeBackground("Forest-dark");
                    Z.Show();
                    Z.SetSprite(confused);
                    yield return Z.Say("Khu rừng này thật kỳ lạ…{a}im lặng một cách đáng sợ,{a} như thể có điều gì đó đang rình rập vậy.");
                    Z.Hide();
                    yield return N.Say("Sau một lúc tìm kiếm, Zino cảm thấy mệt mỏi và tựa lưng vào một gốc cây để nghỉ ngơi.{c}Bất ngờ, một luồng sáng xanh rực rỡ xuất hiện từ xa, chiếu sáng cả một góc rừng.");
                    bg.ChangeBackground("Portal");
                    vortex.SetActive(true);
                    if (soundtrack != null)
                        soundtrack.PlayTrackByName("InDanger");
                    cv_Shaking.Shake();
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Gì thế kia? Một thứ ánh sáng…kỳ lạ.{c}Nó phát ra từ đâu vậy?");
                    Z.Hide();
                    yield return N.Say("Bị cuốn hút bởi ánh sáng, Zino tiến lại gần và phát hiện ra một cánh cổng phát sáng kỳ ảo.{c}Trước khi kịp hiểu chuyện gì, một lực hút mạnh mẽ kéo cậu vào cánh cổng.");



                    cv_Shaking.Shake();
                    cv_Shaking.Shake();

                    cv_Shaking.Shake();
                    cv_Shaking.Shake();
                    cv_Shaking.Shake();
                    yield return new WaitForSeconds(2);
                    vortex.SetActive(false);
                    bg.ChangeBackground("Black");
                    soundtrack.PlayTrackByName("ForestNight");

                    yield return N.Say("Phần 1: Lạc vào thế giới mới");


                    bg.ChangeBackground("Forest-dark");
                    mainHealthPoint -= 50;
                    health.text = mainHealthPoint.ToString();
                    if (soundtrack != null)
                    {
                        soundtrack.PlayTrackByName("HeartBeat");
                        soundtrack.PlaySFXByName("Fall");
                    }
                    blood.SetActive(true);
                    yield return N.Say("Zino tỉnh dậy, cơ thể rã rời. Xung quanh cậu là một khu rừng u tối, những cây cổ thụ kỳ dị che khuất bầu trời, tạo nên không gian ma quái.");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Mình…{a}mình đang ở đâu?{c}Đây không phải khu rừng mình vừa bước vào!");
                    cv_Shaking.Shake();
                    yield return Z.Say("BALO MÌNH ĐÂU ???");
                    Z.Hide();
                    yield return N.Say("Zino loạng choạng tìm đồ đạc của bản thân.{c}Nó đã biến mất không còn dấu tích.....đau lòng, Zino đứng dậy và bước tiếp");
                    Z.Show();
                    Z.SetSprite(sad);
                    yield return Z.Say("Đau quá......mình phải tìm đường ra khỏi khu rừng này.");
                    Z.Hide();
                    yield return N.Say("Cảm thấy đói, khát và hoang mang, Zino loạng choạng đi tìm lối thoát.{c}Bỗng nhiên có một thứ gì đó lướt ngang Zino.");

                    D.SetPosition(new Vector2(-2, 0));
                    D.Show();
                    D.MoveToPosition(new Vector2(3, 0), 9, false);
                    yield return new WaitForSeconds(2);
                    D.Hide();
                    cv_Shaking.Shake();
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("CÁI GÌ VẬY !!!!!!!!!");
                    yield return N.Say("Hốt hoảng, Zino nhìn xung quanh và cố gắng xác định thứ gì vừa lướt qua.{c}Cố gắng lấy lại bình tĩnh.........");
                    yield return Z.Say("Chắc mình hoang tưởng do mệt và đói thôi.....{a}chắc là vậy nhỉ.....");
                    Z.Hide();
                    soundtrack.PlayTrackByName("ForestNight");
                    blood.SetActive(false);
                    yield return N.Say("Khu rừng tối tăm một cách đầy bí ẩn, những tiếng rít và tiếng gào thét xung quanh làm cho không khí càng thêm đáng sợ...{c}Zino trong trạng thái vừa hoang mang, vừa phải cần thận khi đi trong khu rừng. Mất hết đồ đạc chuyên dụng, Zino đành phải dựa vào bản năng để vượt qua.{c}Dù vậy, cũng không thể phủ nhận rằng khu rừng có một sức hút cực kỳ mạnh mẽ bởi sự huyền bí và ma mị.");
                    cv_Shaking.Shake();
                    EnemyAppear1();
                    soundtrack.PlayTrackByName("InDanger");
                    yield return N.Say("Bất thình lình, một con quái thú nhảy ra chặn đường Zino....{c}Zino giật mình và ngã ra sau");
                    soundtrack.PlaySFXByName("Fall");
                    Z.Show();
                    Z.SetSprite(shock);
                    blood.SetActive(true);
                    yield return Z.Say("AAAAAAAAAAAAAAAAAAAAAAAAAAAA!!!!!");
                    yield return N.Say("Zino quay đầu bỏ chạy thục mạng sâu vào trong khu rừng...");
                    EnemyHideAll();
                    run.SetActive(true);
                    break;
                }
            case 0.2f:
                {
                    while (storyProcess == 0.2f)
                    {
                        if (esc == 5)
                        {
                            yield return N.Say("Zino bằng cách nào đó đã cắt đuôi được con quái thú và trốn sâu vào khu rừng....");
                            storyProcess += 0.05f;
                            StartCoroutine(Chap());
                        }
                        else
                        {
                            EnemyAppear1();
                            cv_Shaking.Shake();
                            mainHealthPoint--;
                            health.text = mainHealthPoint.ToString();
                            esc++;
                            storyProcess = storyProcess - 0.2f;
                            yield return N.Say("Quái thú đã nhảy ra chặn đầu Zino. Zino mất 1 máu.");
                            soundtrack.PlaySFXByName("MonsterSlash");
                            EnemyHideAll();
                            run.SetActive(true);
                        }
                    }
                    break;
                }
            case 0.25f:
                {
                    bg.ChangeBackground("Cave");
                    soundtrack.PlayTrackByName("Calm");
                    yield return N.Say("Sau khi di chuyển một đoạn, Zino tìm thấy một cửa hang.{c}Hang động rất rộng rãi và cho lại một cảm giác yên bình hơn bên ngoài.{a}Zino quyết định vào trong hang để tạm trú ẩn. Sau một lúc,{a} Zino tìm thấy một ngôi nhà nhỏ nằm sâu trong hang động, ánh lửa từ bên trong hắt ra.{c}Zino thở phào, đầy hy vọng");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Có người ở đây!{c}Có lẽ họ sẽ giúp được mình.");
                    soundtrack.PlaySFXByName("Knock");
                    yield return N.Say("Zino gõ cửa.{c}*knock* *knock* *knock*{c}Nhưng không ai trả lời.{c}Zino cố gắng gọi");
                    Z.SetSprite(normal);
                    yield return Z.Say("Có ai không.....{c}Tôi bị lạc vào khu rừng này...{a}làm ơn giúp tôi với.........");
                    Z.SetSprite(sad);
                    soundtrack.PlayTrackByName("HeartBeat");

                    yield return N.Say("Nhưng vẫn không ai trả lời...");
                    soundtrack.PlaySFXByName("Knock");
                    yield return N.Say("Cậu cố gắng vừa gọi vừa gõ cửa, nhưng do kiệt sức, Zino ngã quỵ và ngất trước cửa.");
                    soundtrack.PlaySFXByName("Fall");
                    Z.Hide();
                    blood.SetActive(false);
                    yield return new WaitForSeconds(2);

                    yield return N.Say("Nghe tiếng động, Halrath - một chiến binh cao lớn với vẻ ngoài oai phong - mở cửa và thấy Zino nằm bất động. Ông cau mày, đôi mắt sắc bén như cân nhắc điều gì đó.");
                    soundtrack.PlaySFXByName("ClosingDoor");
                    H.Show();
                    yield return H.Say("Một kẻ lạ mặt...{a} từ thế giới khác.{c}Sao hắn lại đến đây?");
                    yield return N.Say("Liệu số phận của Zino sẽ ra sao......");
                    yield return new WaitForSeconds(1);
                    H.Hide();
                    bg.ChangeBackground("Black");

                    yield return N.Say("Phần 2: Gặp Halrath - Chiến binh dũng cảm");


                    bg.ChangeBackground("Cave");
                    soundtrack.PlayTrackByName("House");
                    yield return N.Say("Halrath bế Zino vào nhà và giúp cậu hồi sức.");
                    bg.ChangeBackground("Black");
                    noti.text = "Halrath chữa vết thương cho Zino. Máu hồi tối đa";
                    mainHealthPoint = 100;
                    health.text = mainHealthPoint.ToString();
                    yield return new WaitForSeconds(5);
                    noti.text = "";
                    bg.ChangeBackground("Bedroom");

                    yield return N.Say("Một lúc sau, Zino từ từ tỉnh dậy,{a} thấy mình đang nằm trên giường trong căn nhà ấm áp.");
                    Z.Show();
                    Z.SetSprite(normal);
                    soundtrack.PlaySFXByName("Sniff");
                    yield return Z.Say("Mùi gì thơm quá.....");
                    Z.Hide();
                    yield return N.Say("Halrath từ ngoài bước vào phòng, trên tay cầm theo dĩa thức ăn mới nóng hổi.{c}Zino hoảng hốt hét toáng lên làm Halrath giật mình....");
                    Z.SetSprite(shock);
                    cv_Shaking.Shake();
                    yield return Z.Say("AAAAAAAAAAAAAAAAAAAAAAAAAA!!!!!!");
                    cv_Shaking.Shake();
                    yield return Z.Say("Ông là ai? Sao tôi lại ở đây?");
                    Z.Hide();
                    H.Show();
                    yield return N.Say("Halrath bình tĩnh đáp....");
                    yield return H.Say("Bình tĩnh nào.{c}Ta là Halrath, người bảo vệ của khu rừng này. Còn cậu là ai? Tại sao lại tới vùng đất này?");

                    yield return N.Say("Zino tỏ ra lo lắng đáp....");
                    Z.Show();
                    Z.SetSprite(sad);
                    yield return Z.Say("Tôi là Zino…{a} một nhà thám hiểm. Tôi tìm thấy một cánh cổng phát sáng và…{a} sau đó tôi tỉnh dậy đã thấy trong khu rừng này.{c}Mất hết đồ đạc, giờ tôi không biết làm sao để quay trở về nhà.");
                    Z.Hide();

                    yield return N.Say("Halrath quan sát Zino với ánh mắt thận trọng, rồi gật đầu chậm rãi nói....");
                    yield return H.Say("Vậy là cậu đã vô tình bước qua ''Cánh cổng'' đó à?");
                    Z.Show();
                    Z.SetSprite(sad);
                    yield return Z.Say("Tôi không biết nữa....khi nhìn thấy cánh cổng phát sáng....{a} thì điều tiếp theo tỉnh dậy tôi đã ở đây rồi.");
                    Z.Hide();
                    yield return N.Say("Halrath nhìn Zino một lúc lâu rồi lên tiếng....");
                    yield return H.Say("Tôi có thể giúp cậu.{c}NHƯNG!!!");
                    soundtrack.PlayTrackByName("HeartBeat");
                    yield return N.Say("Halrath mặt Bỗng nhiên nghiêm lại, tỏ vẻ nghiêm trọng nói tiếp...");

                    yield return H.Say("Để có thể trở về, cậu phải có đủ lòng dũng cảm và chắc chắn rằng cậu sẽ không bỏ cuộc giữa chừng.{c}Bởi vì, theo truyền thuyết, chìa khóa chỉ được trao cho người có đủ can đảm. Cậu có làm được không?");
                    H.Hide();

                    alertPanel2.SetActive(true);

                    break;
                }
            case 0.5f:
                {
                    Z.Show();

                    Z.SetSprite(shock);
                    yield return Z.Say("Mình nên làm gì đây?");
                    ChoicePanelAnimation();
                    text1.text = "Thế giới này quá đáng sợ… tôi không biết liệu mình có đủ sức đối mặt không…";
                    text2.text = "Không, tôi không thể bỏ cuộc. Dù khó khăn đến đâu, tôi nhất định phải trở về!";
                    storyProcess += 0.5f;
                    Debug.Log("S = " + storyProcess);
                    break;
                }
            case 2:
                {
                    bg.ChangeBackground("Bedroom");

                    yield return N.Say("Zino cảm thấy tuyệt vọng, cúi đầu, giọng yếu ớt đáp...");
                    soundtrack.PlayTrackByName("FinalDecision");
                    Z.Show();
                    Z.SetSprite(sad);
                    yield return Z.Say("Thế giới này quá đáng sợ… tôi không biết liệu mình có đủ sức đối mặt không…{c}Hay tôi sẽ chết ngoài đấy trong lúc tìm kiếm.{c}Tôi không biết nữa.....");
                    yield return N.Say("Cảm thấy bản thân quá yêu đuối, tự cảm thấy xấu hổ khi gọi bản thân là một nhà thám hiểm nhưng lại rụt rè trước một thử thách mới.{c}Zino bật khóc...{c}Halrath trầm ấm nói...");
                    H.Show();
                    yield return H.Say("Nếu cậu không thể đối diện với thử thách, hãy ở lại đây. Ta sẽ bảo vệ cậu, nhưng cậu sẽ mãi mãi sống trong thế giới này.");
                    yield return N.Say("Zino cảm thấy sợ hãi và quyết định ở lại cùng Halrath, từ bỏ ý định trở về nhà.{c}GAME OVER....");

                    SceneManager.LoadScene("Main Menu");

                    break;
                }
            case 3:
                {
                    bg.ChangeBackground("Bedroom");
                    soundtrack.PlayTrackByName("Calm");
                    yield return N.Say("Halrath có thể nhận thấy rõ sự mừng rỡ cũng như hạnh phúc khi nghe Halrath có thể giúp Zino quay trở về. Zino mạnh mẽ đáp, giọng nói đầy nghị lực..");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Không, tôi không thể bỏ cuộc. Dù khó khăn đến đâu, tôi nhất định phải trở về!");
                    Z.Hide();
                    H.Show();
                    yield return H.Say("Rất tốt. Nếu cậu thực sự quyết tâm, hãy đi đến thành phố Azzaband và tìm gặp Scy - một pháp sư quyền năng. Ông ta có thể giúp cậu tìm hiểu thêm về thế giới này cũng như những thứ có thể bảo vệ bản thân.");
                    yield return N.Say("Halrath vừa mỉm cười vừa nói....{c}Đồng thời, Halrath nhận ra có gì đó bất thường toát ra từ cơ thể của Zino. Halrath nói...");
                    yield return H.Say("Cậu nên cẩn thận trong quá trình khám phá vùng đất. Có một thứ gì đó đã thoát ra khỏi cậu ngay khi cậu đặt chân đến đây.{c}Nếu có gì bất thường cứ quay về gặp ta tại đây.");
                    yield return N.Say("Nói xong, Halrath đưa cho Zino một tấm bản đồ khu rừng và đường dẫn đến thành phố Azzaband cùng với giấy giới thiệu đến Scy. Đồng thời đưa cậu một số vũ khí có thể tự vệ trên đường ra khỏi khu rừng.{c}Zino cảm ơn rối rít rồi tạm biệt Halrath...");
                    H.Hide();
                    bg.ChangeBackground("Cave");

                    yield return N.Say("Zino một lần nữa kiểm tra lại tất cả các vật dụng mà Halrath đã đưa. Bao gồm: một túi nước, một ít lương khô và trái cây, một thanh đoản kiếm, một hộp cứu thương.{c}Sau khi kiểm tra xong, Zino cất đồ cẩn thận và bắt đầu lên đường.");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Halrath thật là tốt bụng, lần sau đến thăm chắc chắn là sẽ mang theo quà để cảm ơn{c}............");
                    soundtrack.PlayTrackByName("Suprised");
                    Z.SetSprite(shock);
                    yield return Z.Say("Ủa mà khoan......{c}Mình không biết gì về tiền tệ của thế giới này hết..........{a}quên mất không hỏi thêm thông tin rồi.....làm sao giờ đây.");
                    soundtrack.PlayTrackByName("Calm");
                    ChoicePanelAnimation();
                    text1.text = "Quay lại hỏi thêm thông tin";
                    text2.text = "Có thể Scy sẽ giúp được";
                    Debug.Log("S = " + storyProcess);
                    break;
                }
            case 4:
                {
                    bg.ChangeBackground("Cave");

                    yield return Z.Say("Cũng chưa đi xa lắm, quay lại hỏi thêm thông tin từ Halrath thôi.....{a}có hơi phiền thật.");
                    soundtrack.PlayTrackByName("Calm");
                    Z.Hide();
                    yield return N.Say("Zino quay ngược trở về căn nhà trong hang động để tìm Halrath một lần nữa....");
                    Z.Show();
                    Z.SetSprite(happy);
                    soundtrack.PlaySFXByName("Knock");
                    yield return N.Say("*knock* *knock* *knock*");
                    yield return Z.Say("Ngài Halrath ơi!! Tôi có chuyện cần hỏi thêm ạ...Xin lỗi vì đã làm phiền.");
                    H.Show();
                    soundtrack.PlaySFXByName("ClosingDoor");
                    yield return N.Say("Halrath từ trong nhà đi ra, dáng người của Halrath cao to, lực lưỡng.{c}Zino hơi hoảng vì lúc Halrath ngồi trong phòng thân hình của ông ta không lớn đến vậy. Giọng Halrath trầm ấm đáp..");
                    yield return H.Say("Có việc gì nữa sao?");
                    Z.SetSprite(shock);
                    yield return new WaitForSeconds(3);
                    yield return H.Say("Sao vậy ???");
                    Z.SetSprite(happy);
                    yield return Z.Say("À....cũng không có gì. Tôi mới đến đây lần đầu tiền nên khi đến thành phố chắc chắn sẽ phải cần lệ phí hay thứ gì đó tương tự.{c}Ngài biết đó....tôi không có thứ gì như thế cả, ngài có thể giúp tôi được không.{c}Tôi có thể làm việc để được trả công!!! Làm gì cũng được ạ.");
                    Z.Hide();
                    yield return N.Say("Halrath trầm ngâm một lúc rồi đáp...");
                    yield return H.Say("bất kỳ điều gì cũng được sao ??");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Vân-");
                    Z.SetSprite(shock);
                    yield return N.Say("Bỗng nhiên một dòng suy nghĩ cắt ngang khi Zino nhớ lại những bộ phim kinh dị đã xem ở thế giới cũ....Zino rụt rè đáp lại..");
                    Z.SetSprite(happy);
                    yield return Z.Say("Vâng, trong khả năng của tôi thì tôi sẽ làm ạ.");
                    Z.Hide();
                    yield return N.Say("Halrath bật cười khi thấy thái độ sợ hãi kèm theo ngượng ngùng khi trả lời của Zino và đáp...");
                    yield return H.Say("Haha, nào đừng nghĩ xấu về tôi như vậy chứ, tôi vừa cứu cậu đấy.");
                    yield return N.Say("Zino ngượng ngùng nói...");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Xin lỗi, tại.......à thôi đừng bận tâm. Thế ngài có công việc gì cho tôi nào?");
                    yield return H.Say("Eh...đừng gọi tôi là ngài nữa...tôi không lớn tuổi tới mức đó đâu.{c}Tôi chắc chỉ lớn cậu chừng hai đến ba tuổi thôi và gọi tôi là Halrath được rồi.");
                    yield return N.Say("Halrath vẫn nói với giọng trầm và ấm...làm người nghe cảm thấy thoải mái và dễ chịu.{c}Zino ngạc nhiên...");
                    yield return N.Say("Zino vui vẻ trả lời.");
                    yield return Z.Say("Thật vậy sao ?{c}Thế phải gọi là anh rồi, anh Halrath.{c}Thế anh có việc gì để em làm không? Em sẽ làm để có tiền công vào thành phố.");

                    yield return H.Say("Được thôi, sẵn đây cũng có một số việc cần làm.");
                    yield return N.Say("Halrath lấy ra một cái túi đeo lưng cỡ lớn và đánh dấu nơi cần đến trên bản đồ đưa cho Zino.");
                    yield return H.Say("Bây giờ cậu đến khu vực nơi tôi đánh dấu để thu thập gỗ và một số trái cây dại nhé. Nhớ cẩn thận các khu vực nguy hiểm.");
                    yield return Z.Say("Được rồi, cảm ơn nhé, bây giờ em sẽ lên đường ngay.");
                    H.Hide();
                    Z.Hide();
                    bg.ChangeBackground("Forest");
                    yield return N.Say("Zino cẩn thận nhìn qua bản đồ,{a} với kinh nghiệm thám hiểm của mình,{a} Zino hoàn toàn dễ dàng vượt qua các khu vực nguy hiểm và đến được khu vực lấy gỗ như Halrath yêu cầu và hoàn thành nhiệm vụ trong thời gian ngắn.");

                    yield return N.Say("Trên đường trở về, Zino vô tình bắt gặp con quái thú lần trước.{c}Không đợi bị tấn công, Zino rút thanh kiếm mà Halrath đã đưa và bắt đầu chiến đấu.");
                    yield return N.Say(" ");
                    EnemyAppear1();
                    soundtrack.PlayTrackByName("InDanger");
                    storyProcess += 0.1f;
                    StartCoroutine(Chap());
                    break;
                }
            case 4.1f:
                {
                    fighting.SetActive(true);
                    break;
                }
            case 4.25f:
                {
                    while (wolfRHP > 0)
                    {
                        double wolfdmg;
                        double dmgdeal = dmg - wolfRDef;
                        if (dmgdeal > 0)
                        {
                            if (dmgdeal > wolfRHP)
                            {
                                wolfRHP = 0;
                                wolfHP.text = wolfRHP.ToString();
                                char_Shaking.Shake();
                                yield return N.Say("Zino sử dụng kỹ năng gây " + dmgdeal + " sát thương");
                            }
                            else if (dmgdeal < wolfRHP)
                            {
                                wolfRHP = wolfRHP - dmgdeal;
                                wolfHP.text = wolfRHP.ToString();
                                char_Shaking.Shake();
                                yield return N.Say("Zino sử dụng kỹ năng gây " + dmgdeal + " sát thương");
                            }

                        }
                        else if (dmgdeal < 0)
                        {
                            if (isDefend)
                                yield return N.Say("Zino thủ thế chuẩn bị chặn đòn đánh tiếp theo.");
                            else
                                yield return N.Say("Zino đã đánh hụt và không gây ra sát thương.");
                        }

                        if (wolfRHP <= 0)
                        {

                            dmg = 0;
                            storyProcess += 0.25f;
                            StartCoroutine(Chap());
                            break;
                        }
                        //RNG Monster dmg and return Zino dmg to 0

                        wolfdmg = Math.Round(UnityEngine.Random.Range(1, 10) - _def, 0);


                        dmg = 0;
                        if (isDefend)
                        {
                            soundtrack.PlaySFXByName("MonsterSlash");
                            yield return N.Say("Zino đã chặn được đòn đánh của quái vật.");
                            isDefend = false;
                            storyProcess -= 0.15f;
                            StartCoroutine(Chap());
                        }
                        else
                        {
                            //check monster dmg <= 0
                            if (wolfdmg <= 0)
                            {
                                soundtrack.PlaySFXByName("MonsterSlash");
                                yield return N.Say("Zino đã kịp thời né đòn đánh của quái vật sói.");
                                storyProcess -= 0.15f;
                                StartCoroutine(Chap());
                            }
                            //check if monster is dealing dmg
                            else if (wolfdmg > 0)
                            {
                                if (mainHealthPoint <= 0)
                                {
                                    soundtrack.PlaySFXByName("MonsterSlash");
                                    yield return N.Say("Quái vật sói đã hạ gục Zino. GAME  OVER ~");
                                    SceneManager.LoadScene("Main Menu");
                                }
                                else
                                {
                                    soundtrack.PlaySFXByName("MonsterSlash");
                                    blood.SetActive(true);
                                    cv_Shaking.Shake();
                                    yield return N.Say("Quái vật sói tấn công Zino gây ra " + wolfdmg + " sát thương");

                                    blood.SetActive(false);
                                    mainHealthPoint = mainHealthPoint - wolfdmg;
                                    health.text = mainHealthPoint.ToString();
                                    storyProcess -= 0.15f;
                                    StartCoroutine(Chap());
                                }
                            }
                        }
                        //check Zino health
                        break;
                    }
                    break;
                }
            case 4.5f:
                {
                    EnemyHideAll();
                    soundtrack.PlayTrackByName("Calm");
                    yield return N.Say("Zino tung đòn đánh cuối cùng, đánh bật con quái thú về phía sau. Con quái thú chợt bỏ đi vào sâu trong khu rừng.{c}Zino thở phào nhẹ nhõm như vừa thoát được kiếp nạn không thể tránh khỏi.");
                    bg.ChangeBackground("Cave");
                    yield return N.Say("Zino sau đó quay trở về nhà Halrath.{c}Halrath ngạc nhiên nói.");

                    H.Show();
                    yield return H.Say("Đây là lần đầu tiên tôi thấy có người có thể thực hiện việc này nhanh như vậy đấy. Cậu thật là giỏi.{c}Như đã hứa, đây là thù lao của cậu");
                    yield return N.Say("Halrath đưa cho Zino một túi tiền chứa khá nhiều vàng bên trong.");
                    yield return H.Say("Nhiêu đây sẽ đủ cho cậu có thể sống trong thành phố khoảng 3 ngày. Còn lại cậu sẽ phải tự lo nhé. Cậu có thể quay về đây bất cứ lúc nào nếu cậu muốn hoặc gặp khó khăn.");
                    yield return N.Say("Nói xong, Halrath đưa cho Zino hai chìa khóa, một là của phòng ngủ, một là cửa chính.");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Thật sao, anh không sợ rằng tôi sẽ làm gì đó sai trái hay sao?");
                    yield return N.Say("Halrath cười mỉm...");
                    yield return H.Say("Bởi vì, cậu sẽ không thể. Cậu vẫn chưa nhận ra sao?");
                    yield return N.Say("Zino ngẩn ra vì không hiểu...");
                    Z.SetSprite(confused);
                    yield return Z.Say("Không{a}......{a}hiểu lắm.....");
                    yield return H.Say("Được rồi,{a} muốn biết thì cậu lại gần đây....");
                    yield return N.Say("Zino tiến lại gần Halrath theo yêu cầu.{c}Khi vừa đến nơi, Halrath nhéo má Zino một cái thật đau.");
                    Z.SetSprite(shock);
                    cv_Shaking.Shake();
                    yield return N.Say("Zino ré lên vì đau và khụy xuống sàn.");
                    cv_Shaking.Shake();
                    soundtrack.PlaySFXByName("Fall");
                    soundtrack.PlayTrackByName("SillyMoment");
                    yield return Z.Say("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA!!! ĐAUUUUUU");
                    Z.Hide();
                    yield return N.Say("Zino chợt nhận ra một thứ{a}.........{a}cậu không thể nổi giận.....");
                    yield return H.Say("Đúng vậy, cái tôi đang nói tới, chính là mặt tối của cậu bao gồm giận dữ, điên loạn, tội đồ đã biến mất.");
                    yield return N.Say("Zino vẫn còn đang rướm nước mắt vì đau.....");
                    Z.SetSprite(cry);
                    yield return Z.Say("Nhưng{a}........{a}ow...ow...ow..đauuuuu..");
                    yield return N.Say("Halrath cười phá lên.....");
                    yield return H.Say("Haha, xin lỗi nhé, nhưng phải làm thế thì cậu sẽ nhận ra nhanh hơn là tôi ngồi giải thích.");
                    yield return Z.Say("Nhưng tại sao chứ......?");
                    yield return H.Say("Có khả năng là khi cậu bị hút bởi cánh cổng, nó đã vô tình làm cho 2 linh hồn của cậu bị tách rời ra.{c}Tôi nhận ra điều này khi nhìn thấy các hào quang xung quanh cậu mất đi một thứ mà mọi người thường có.");
                    Z.SetSprite(shock);
                    soundtrack.PlayTrackByName("HeartBeat");
                    yield return Z.Say("Vậy.....cái thứ mà đã lướt ngang mặt em tối qua là.......{a}ôi không....");
                    yield return H.Say("Vậy là đã rõ, cậu nên cẩn thận vẫn hơn.{c}Hơn nữa, hãy tìm Scy như đã nói, cậu ta cũng là một trong những người như cậu.{c}Bị hút sang thế giới bên này.");
                    Z.SetSprite(confused);
                    yield return Z.Say("Thật sao, vậy sao cậu ta không quay trở về?");
                    Z.Hide();
                    yield return N.Say("Halrath trầm ngâm một lúc rồi nói tiếp.");
                    yield return H.Say("Có lẽ, cậu nên hỏi Scy vẫn hơn.{c}Cũng đã trễ rồi, cậu ở lại đây một đêm rồi sáng mai lên đường sớm.");
                    yield return N.Say("Zino gật đầu đồng ý ở lại và cả hai đều đi ngủ ngay sau đó. Kết thúc một ngày bận rộn.");
                    H.Hide();
                    bg.ChangeBackground("Black");
                    noti.text = "Zino ăn no ngủ kỹ, Máu hồi tối đa";
                    mainHealthPoint = mainHealthPoint + (100 - mainHealthPoint);
                    health.text = mainHealthPoint.ToString();
                    yield return new WaitForSeconds(3);
                    noti.text = "";
                    yield return N.Say("Ngày hôm sau.........");
                    yield return new WaitForSeconds(5);
                    bg.ChangeBackground("Bedroom");
                    soundtrack.PlayTrackByName("House");
                    yield return N.Say("Zino tỉnh dậy vào khoảng sáng sớm.{c}Ánh nắng len lỏi qua các tán lá cây soi sáng ô cửa sổ.{c}Zino đứng bên bệ cửa sổ thầm nghĩ một lúc.....{a}Halrath tỉnh dậy thấy Zino trầm ngâm....");
                    H.Show();
                    yield return H.Say("Tại sao sâu bên trong hang động nhưng cửa sổ vẫn thấy bầu trời bên ngoài? - tôi nói có đúng dòng suy nghĩ của cậu không ?");

                    yield return N.Say("Zino chợt giật mình khi nghe Halrath nói, cậu đáp...");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Ra là anh à, làm em giật mình.");
                    Z.SetSprite(confused);
                    yield return Z.Say("Đúng vậy, em đang thắc mắc tại sao?");
                    yield return H.Say("Đó là sản phẩm của Scy đấy, quà cảm ơn vì đã cứu anh ta. Căn phòng cũng sáng sủa hơn khi có ánh nắng mặt trời đúng không?");
                    Z.Hide();
                    yield return N.Say("Đúng như vậy, căn phòng sáng hẳn khi những tia nắng sớm rọi vào phòng cho ta một cảm giác ấm áp một cách tự nhiên không còn cảm giác lạnh lẽo của hang động nữa.");
                    bg.ChangeBackground("Cave");
                    soundtrack.PlayTrackByName("Calm");
                    yield return H.Say("Thôi được rồi, cũng không còn sớm nữa.{a} Cậu nên bắt đầu khởi hành cho kịp thời gian.");
                    yield return Z.Say("Thật cảm ơn anh trong thời gian qua.{c}Em sẽ ghi nhớ và đền đáp công ơn này.");
                    Z.Hide();
                    yield return N.Say("Nói xong, Zino vác túi đồ trên lưng và lên đường ra khỏi hang động.");
                    H.Hide();
                    bg.ChangeBackground("Black");
                    storyProcess += 1.5f;
                    halrathHeartPoint += 1;
                    noti.text = "Halrath đã tin tưởng Zino nhiều hơn.";
                    yield return new WaitForSeconds(2);
                    noti.text = "";
                    StartCoroutine(Chap());
                    break;
                }
            case 5:
                {
                    bg.ChangeBackground("Cave");
                    yield return Z.Say("Thôi thì lên đường luôn, có thể Scy sẽ biết thêm.");
                    soundtrack.PlayTrackByName("Calm");
                    yield return N.Say("Nói xong, Zino lên đường đến thành phố Azzaband.");
                    halrathHeartPoint -= 1;
                    noti.text = "Halrath đã quên đưa Zino một thứ gì đó quan trọng......";
                    yield return new WaitForSeconds(2);
                    noti.text = "";
                    bg.ChangeBackground("Forest");
                    yield return N.Say("Trên đường ra khỏi khu rừng, Zino vô tình bắt gặp con quái thú lần trước.{c}Không đợi bị tấn công, Zino rút thanh kiếm mà Halrath đã đưa và bắt đầu chiến đấu.");
                    soundtrack.PlayTrackByName("InDanger");
                    EnemyAppear1();
                    storyProcess += 0.1f;
                    StartCoroutine(Chap());
                    break;
                }
            case 5.1f:
                {
                    fighting.SetActive(true);
                    break;
                }
            case 5.25f:
                {
                    while (wolfRHP > 0)
                    {
                        double dmgdeal = dmg - wolfRDef;
                        if (dmgdeal > 0)
                        {
                            if (dmgdeal > wolfRHP)
                            {
                                wolfRHP = 0;
                                wolfHP.text = wolfRHP.ToString();
                                char_Shaking.Shake();
                                yield return N.Say("Zino sử dụng kỹ năng gây " + dmgdeal + " sát thương");
                            }
                            else if (dmgdeal < wolfRHP)
                            {
                                wolfRHP = wolfRHP - dmgdeal;
                                wolfHP.text = wolfRHP.ToString();
                                char_Shaking.Shake();
                                yield return N.Say("Zino sử dụng kỹ năng gây " + dmgdeal + " sát thương");
                            }

                        }
                        else if (dmgdeal < 0)
                        {

                            if (isDefend)
                                yield return N.Say("Zino thủ thế chuẩn bị chặn đòn đánh tiếp theo.");
                            else
                                yield return N.Say("Zino đã đánh hụt và không gây ra sát thương.");
                        }

                        if (wolfRHP <= 0)
                        {
                            dmg = 0;
                            EnemyHideAll();
                            soundtrack.PlayTrackByName("Calm");
                            yield return N.Say("Zino tung đòn đánh cuối cùng, đánh bật con quái thú về phía sau. Con quái thú chợt bỏ đi vào sâu trong khu rừng.{c}Zino thở phào nhẹ nhõm như vừa thoát được kiếp nạn không thể tránh khỏi.");
                            storyProcess += 0.75f;
                            StartCoroutine(Chap());
                            break;
                        }
                        //RNG Monster dmg and return Zino dmg to 0
                        double wolfdmg = Math.Round(UnityEngine.Random.Range(1, 10) - _def, 0);
                        dmg = 0;
                        if (isDefend)
                        {
                            soundtrack.PlaySFXByName("MonsterSlash");
                            yield return N.Say("Zino đã chặn được đòn đánh của quái vật.");
                            isDefend = false;
                            storyProcess -= 0.15f;
                            StartCoroutine(Chap());
                        }
                        else
                        {
                            //check monster dmg <= 0
                            if (wolfdmg <= 0)
                            {
                                soundtrack.PlaySFXByName("MonsterSlash");
                                yield return N.Say("Zino đã kịp thời né đòn đánh của quái vật sói.");
                                storyProcess -= 0.15f;
                                StartCoroutine(Chap());
                            }
                            //check if monster is dealing dmg
                            else if (wolfdmg > 0)
                            {
                                if (mainHealthPoint <= 0)
                                {
                                    soundtrack.PlaySFXByName("MonsterSlash");
                                    yield return N.Say("Quái vật sói đã hạ gục Zino. GAME  OVER ~");
                                    SceneManager.LoadScene("Main Menu");
                                }
                                else
                                {
                                    soundtrack.PlaySFXByName("MonsterSlash");
                                    blood.SetActive(true);
                                    cv_Shaking.Shake();
                                    yield return N.Say("Quái vật sói tấn công Zino gây ra " + wolfdmg + "sát thương");

                                    blood.SetActive(false);
                                    mainHealthPoint = mainHealthPoint - wolfdmg;
                                    health.text = mainHealthPoint.ToString();
                                    storyProcess -= 0.15f;
                                    StartCoroutine(Chap());
                                }
                            }
                        }
                        break;
                    }
                    break;
                }
            case 6:
                {
                    bg.ChangeBackground("Black");
                    yield return N.Say("Phần 3: Azzaband - Thành phố phép thuật");
                    yield return new WaitForSeconds(5);
                    bg.ChangeBackground("Forest");
                    soundtrack.PlayTrackByName("Bird");
                    yield return N.Say("Zino đã ra đến bìa rừng theo bản đồ mà Halrath đưa cho.");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Halrath thật chu đáo, đánh dấu hết tất cả những nguy hiểm, những nơi có thể thu thập trái cây có thể ăn và nước uống.");
                    Z.Hide();
                    yield return N.Say("Zino dễ dàng băng qua khu rừng mà không phải gặp nguy hiểm.{c}Khi đến gần bìa rừng, cây bắt đầu thưa thớt hơn và bầu trời bắt đầu hiện rõ ra trước mắt.{c}Mặt trời rọi thẳng xuống con đường mòn làm cho thực vật nơi đây càng thêm sinh động. Tiếng chim kêu rộn ràng chào đón một ngày mới.{c}");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Khu rừng nơi đây thật là đẹp, tuy thực vật nhìn không khác gì thế giới cũ nhưng cảm giác sinh động vẫn hơn phần nào.");
                    Z.Hide();
                    bg.ChangeBackground("Gate");
                    soundtrack.PlayTrackByName("TownSquare");
                    yield return N.Say("Cuối cùng, Zino cũng đã ra khỏi khu rừng an toàn.{c}Trước mắt cậu là một tòa thành lớn. Các bờ tường kéo dài gần như vô tận......{c}Zino ngỡ ngàng trước sự hoành tráng của tòa thành, cậu đứng lặng trong phút chốc.");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Tòa thành này thật to lớn hơn những gì mình thấy trong các sách vở đã từng học....{a}thật tráng lệ.{c}Được rồi, tiến vào trong thôi.");
                    Z.Hide();
                    yield return N.Say("Khi Zino vừa đến cổng, một lính gác đã chặn Zino lại....");
                    yield return RANDOM.Say("Dừng lại, cậu là người từ nơi khác đến đúng không. Vui lòng xuất trình giấy tờ!");
                    yield return N.Say("Zino lấy ra giấy giới thiệu mà Halrath đã đưa và bình tĩnh đáp...");
                    Z.Show();
                    Z.SetSprite(normal);
                    yield return Z.Say("Xin chào, tôi là Zino, đúng như cậu nói, tôi là người từ ... nơi khác đến đây. Đây là giấy giới thiệu từ ngài Halrath.");
                    yield return N.Say("Cậu lính xem qua giấy giới thiệu sau đó nhìn sơ qua người Zino. Cậu gật đầu sau đó đưa Zino một mẫu giấy, trên đó ghi những ký tự khó hiểu như thể không muốn cho người khác biết nội dung vậy. Người lính nói.");
                    yield return RANDOM.Say("Được rồi, cậu có thể vào trong. Cứ đi thẳng vào Town Square sau đó rẽ trái sẽ có một cửa hiệu sách cũ. Vào trong, cậu sẽ thấy một cái chuông nhỏ để trên bàn tính tiền. Cầm lên và bấm lần lượt hai lần - sáu lần - một lần - tám lần. Nhớ kỹ nhé.");
                    yield return Z.Say("Vâng, cảm ơn cậu.");
                    bg.ChangeBackground("Town");
                    yield return N.Say("Nói xong, Zino tiến vào trong thành phố.{c}Thành phố Azzaband thật tráng lệ, những tòa nhà tráng lệ với nhiều họa tiết đặc biệt Zino chưa thấy bao giờ.{c}Choáng ngộp bởi sự hoành tráng của thành phố, Zino không biết mình đang đứng giữa Town Square tự bao giờ.{c}Người dân xung quanh nhìn Zino một cách khó hiểu.{c}Zino nhận thấy quần áo của mình nó quá khác biệt so với người dân thành phố.{a}Zino chạy nhanh đến cửa hàng sách cũ như người lính gác đã nói.{c}Zino bước vào trong cửa tiệm.");
                    bg.ChangeBackground("bookstore");
                    yield return Z.Say("Là đây sao......");
                    yield return N.Say("Zino cẩn thận bước vào trong và tìm đến bàn tính tiền.{c}Tìm thấy cái chuông như lời nói, Zino bắt đầu bấm.....");
                    ChoicePanelAnimation();
                    text1.text = "2-6-1-8";
                    text2.text = "1-6-2-8";
                    break;
                }
            case 7:
                {
                    bg.ChangeBackground("lab");
                    soundtrack.PlaySFXByName("Bellring");
                    yield return new WaitForSeconds(5);
                    soundtrack.PlayTrackByName("HeartBeat");
                    soundtrack.PlaySFXByName("ThingOpen");
                    yield return N.Say("*Ầm* *Ầm* *Ầm*{c}Chiếc bàn gỗ từ từ di chuyển sang 1 bên và để lại một lối đi bên dưới.{c}Zino bước theo bậc cầu thang dẫn đến 1 tầng hầm của cửa hàng.{c}Cậu gõ cửa.");
                    soundtrack.PlaySFXByName("Knock");
                    yield return N.Say("*knock* *knock* *knock*{c}Scy bước đến mở cửa.");
                    S.Show();
                    yield return S.Say("Xin chào, tôi chờ cậu hơi bị lâu rồi đó chàng trai trẻ.");
                    yield return N.Say("Một chàng trai trẻ tuổi chạc tuổi với Zino, tay đang cầm quyển sách dày cộm, tay còn lại đang giữ một quả cầu lửa. Zino giật bắn mình.");
                    soundtrack.PlayTrackByName("SillyMoment");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Eikkkkk!!!!");
                    cv_Shaking.Shake();
                    Z.Hide();
                    yield return N.Say("Scy cười phá lên sau khi thấy Zino giật mình. Cậu ta nhẹ nhàng đáp.");
                    yield return S.Say("Đừng sợ...tôi không làm hại cậu đâu Zino.{c}Cậu là anh chàng mà Halrath đã nhắc tới đúng không ?");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Đúng vậy, Halrath nói rằng cậu có thể giúp được tôi.{c}Tuy nhiên, tôi có hơi thắc mắc.....");
                    Z.Hide();
                    yield return N.Say("Zino vẫn chưa hết bàng hoàng.{c}Scy không để cho Zino nói hết câu, cậu liền đáp..");
                    yield return S.Say("Tại sao tôi lại chọn ở lại thế giới này, tại sao tôi lại biết tên và mặt cậu, tại sao........ muôn vàn câu hỏi tại sao đang ở trong đầu cậu.{c}Tôi sẽ trả lời cho cậu từng câu một, nhưng không phải bây giờ.");
                    yield return N.Say("Scy nhìn qua Zino rồi nói tiếp...");
                    yield return S.Say("Trước mắt để tôi dẫn cậu đi mua quần áo trước đã. Cậu có đủ vàng không??");
                    if (halrathHeartPoint < 5)
                    {
                        scyHeartPoint = scyHeartPoint - 1;
                        noti.text = "Scy tỏ ra thất vọng....";
                        yield return new WaitForSeconds(1);
                        noti.text = "";
                        yield return N.Say("Zino chợt nhận ra mình đã quên mất rằng cậu không có tiền tệ của thế giới này....");
                        Z.Show();
                        Z.SetSprite(sad);
                        yield return Z.Say("Tôi hiện tại không có vàng.....Halrath chỉ đưa tôi những vật dụng cần thiết thôi.");
                        yield return S.Say("hmmmm, tôi có thể cho cậu mượn một ít.{c}Đổi lại, cậu sẽ giúp tôi một số việc nhé.");
                        Z.SetSprite(happy);
                        yield return Z.Say("Đồng ý !");
                        yield return N.Say("Zino mừng rỡ đáp...");
                        S.Hide();
                        Z.Hide();
                        Debug.Log("Scy = " + scyHeartPoint);
                    }
                    else if (halrathHeartPoint > 5)
                    {
                        scyHeartPoint++;
                        noti.text = "Scy cảm thấy có niềm tin hơn ở Zino....";
                        yield return new WaitForSeconds(1);
                        noti.text = "";
                        yield return N.Say("Zino lôi ra một túi tiền vàng do Halrath trả công cho cậu..Scy đáp");
                        yield return S.Say("Chà..Halrath vẫn hào phóng như ngày nào nhỉ...Tôi không biết anh ấy làm gì để có số vàng khổng lồ như vậy...{c}Tuy nhiên thì..số tiền này sẽ dư để cậu có thể mua 2 đến 3 bộ trang phục theo ý của cậu.{c}Nên nhớ rằng cậu còn các chi phí khác trong lúc ở thành phố nên là đừng tiêu phí nhé.");
                        yield return N.Say("Zino hơi bối rối do còn nhiều thứ phải lo và những thứ đó tốn ...... {a}tiền. Zino nghĩ...");
                        Z.SetSprite(shock);
                        yield return Z.Say("Phải nhanh chóng tìm việc làm thêm thôi....");
                        S.Hide();
                        Z.Hide();
                        Debug.Log("Scy = " + scyHeartPoint);
                    }
                    bg.ChangeBackground("Town");
                    soundtrack.PlayTrackByName("TownSquare");
                    yield return N.Say("Sau đó, cả hai ra ngoài và đến một tiệm quần áo gần đó.{c}Zino loay hoay mãi không biết nên chọn như thế nào.{c}Ở cuối cửa hàng, một bộ trang phục lọt vào mắt của Zino, một bộ trang phục theo kiểu thoải mái, không quá gò bó và không cầu kỳ.{c}Zino liền thay đồ và cả hai ra quầy tính tiền.{c}Cả hai quay về lại cửa hàng của Scy....");
                    S.Show();
                    yield return S.Say("Giờ thì cậu có thể thoải mái đi trong thành phố mà không bị ai nghi ngờ.{c}Giờ thì vào việc chính thôi.{c}Đi theo tôi....");
                    bg.ChangeBackground("bookstore");
                    soundtrack.PlayTrackByName("Calm");
                    yield return N.Say("Zino đi theo Scy xuống tầng hầm ban đầu..{c}Bên dưới là một căn phòng rộng lớn, sức chứa có khả năng lên đến hơn vài chục người.{c}Căn phòng chứa đầy sách vở, bảng đen với đầy rẫy thứ công thức lạ kỳ, những chiếc bàn để những lọ thuốc, lọ thí nghiệm.{c}Có vẻ như Scy hoàn toàn chăm chú vào tìm hiểu và nghiên cứu về thế giới này.");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Thật không ngờ, bên dưới một cửa tiệm sách nhỏ lại là một tầng hầm thật rộng lớn.{c}Những thứ này là do cậu nghiên cứu hết à....");
                    Z.Hide();
                    yield return N.Say("Scy gật đầu..");
                    yield return S.Say("Cũng không hẳn là một mình tôi.{a}Trong đó có sự giúp đỡ của Halrath nữa, không có anh ta chắc tôi cũng không tìm được gì nhiều.");
                    yield return N.Say("Scy lôi ra một quyển sách cũ dày cộm và đặt nó lên bàn...cậu từ từ giải thích với Zino các thứ cơ bản của thế giới mới");
                    soundtrack.PlayTrackByName("HeartBeat");
                    yield return S.Say("Để có thể trở về, cậu cần có tổng cộng bốn mảnh ghép của chìa khóa kích hoạt cánh cổng mà cậu đã bước qua.{c}Bốn mảnh này được phân bổ ở 4 nơi mà người ở thế giới này chưa bao giờ dám bước đến.{c}Một là hầm ngục sâu bên trong *Khu rừng quỷ dị* nơi mà Halrath đang canh giữ.{c}Hai là hầm ngục nằm bên sườn dốc của *Vực Đá Không Đáy* ở phía tây thành phố.{c}Ba là hầm ngục bên trong *Tàn Tích Cổ Đại* ở phía Nam thành phố.{c}Cuối cùng là cánh cổng xuyên không cũng là đích đến của cậu khi có đủ mảnh ghép nằm sâu bên trong hang *Núi Lửa Hủy Diệt*.{c}Cậu sẽ cần tôi ở khu tàn tích vì nó sẽ cần sức mạnh của pháp sư để mở cánh cổng.{c}Cậu chỉ có MỘT cơ hội duy nhất để lấy mảnh ghép. Nếu thất bại, mảnh ghép sẽ biến mất và chỉ xuất hiện lại khi có một người như cậu bị lạc vào đây.");
                    yield return N.Say("Nghe xong, Zino thoáng qua một chút nghi ngờ.....");
                    Z.Show();
                    Z.SetSprite(confused);
                    yield return Z.Say("*Mảnh ghép chỉ xuất hiện khi có người lạc vào cánh cổng đó sao.....{a}không lẽ nào.....*");
                    yield return Z.Say("Thế cậu đã thu thập được bao nhiêu mảnh ghép rồi{a}.......{a}cậu có thành công không?");
                    yield return S.Say("Tôi đã thất bại trong việc tìm kiếm mảnh ghép cuối cùng. Hậu quả là kẹt lại ở thế giới này.{c}Nhưng cậu đừng lo, tôi sẽ giúp cậu, dù gì tôi đã quen với cuộc sống ở đây rồi...");
                    Z.Hide();
                    yield return N.Say("Scy thoáng qua một nét buồn bả như thể luôn muốn quay trở về thế giới cũ. Zino nảy sinh nghi ngờ....");
                    S.Hide();
                    storyProcess++;
                    ChoicePanelAnimation();
                    text1.text = "Tin tưởng Scy hỗ trợ mình trong việc tìm kiếm các mảnh ghép.";
                    text2.text = "Không nhận sự trợ giúp của Scy.";
                    break;
                }
            case 8:
                {
                    bg.ChangeBackground("bookstore");
                    soundtrack.PlayTrackByName("HeartBeat");
                    while (storyProcess == 8)
                    {
                        soundtrack.PlaySFXByName("Bellring");
                        yield return N.Say("Không có gì xảy ra cả......thử lại xem");
                        noti.text = "Có người đang nổi giận.....";
                        yield return new WaitForSeconds(1);
                        noti.text = "";
                        storyProcess = storyProcess - 2;
                        scyHeartPoint = scyHeartPoint - 1;
                        ChoicePanelAnimation();
                        text1.text = "2-6-1-8";
                        text2.text = "1-6-2-8";
                        if (storyProcess == 7)
                        {
                            break;
                        }
                        else if (scyHeartPoint == 0)
                        {
                            choicePanel.SetActive(false);
                            soundtrack.PlayTrackByName("Danger");
                            yield return S.Say("Là đứa nào phá cái chuông vậy!!!!!!!!");
                            yield return N.Say("Giọng nói như hồn ma vang vọng khắp cửa hàng.{c}Sau đó, Zino đã phải nhận một quả cầu lửa khổng lồ vào ngực bắn Zino ra khỏi cửa tiệm và gục tại chổ...");
                            noti.text = "Bạn đã bị Scy tấn công: -5 máu";
                            mainHealthPoint -= 5;
                            health.text = mainHealthPoint.ToString();
                            yield return new WaitForSeconds(5);
                            noti.text = "";

                            Debug.Log("Halrath Heart Point: " + halrathHeartPoint);
                            Debug.Log("Scy Heart Point" + scyHeartPoint);
                            SceneManager.LoadScene("Main Menu");

                            break;
                        }


                    }
                    break;
                }
            case 9:
                {
                    scyHeartPoint++;
                    halrathHeartPoint++;
                    bg.ChangeBackground("bookstore");
                    noti.text = "Scy tin tưởng Zino hơn !";
                    yield return new WaitForSeconds(5);
                    noti.text = "";
                    soundtrack.PlayTrackByName("Calm");
                    yield return N.Say("Zino gật đầu đồng ý để Scy giúp đỡ. Scy cười một cách hồn nhiên và tất bật chạy đi làm một số thứ.{c}Cậu ta nói...");
                    S.Show();
                    yield return S.Say("Được rồi, trước hết cậu hãy học thêm về cách thức hoạt động của thế giới này trước đã.{c}Sẽ có rất nhiều việc cho cậu làm lắm đây và sẽ tốn kha khá thời gian, cậu có chịu được không.");
                    yield return N.Say("Scy hỏi Zino với một nụ cười đầy sự tích cực, Zino đáp...");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("tôi muốn trở về, đó là lý do tôi ở đây. Mong cậu hãy chỉ cho tôi thấy những thứ tôi cần làm..");
                    Z.Hide();
                    noti.text = "4 năm sau.....";
                    yield return new WaitForSeconds(10);
                    noti.text = "";
                    yield return N.Say("Bốn năm đã trôi qua, Zino luôn ở bên cạnh Scy để học hỏi thêm đồng thời phụ giúp Scy trong việc ở cửa hàng sách phía trên.{c}Zino thu thập được rất nhiều kiến thức về thế giới mới cũng như cách hoạt động của nó, lịch sử hình thành.{c}Tuy nhiên, trong đầu cậu chỉ có một mục đích duy nhất{c}Trở về !...{c}Hàng ngày, Halrath đều đến thăm, hỏi han đồng thời cũng phụ giúp một số công việc ở đây.{c}Halrath hỗ trợ Scy trong việc nghiên cứu và phát triển thêm các phép thuật mới.");
                    S.Show();
                    yield return S.Say("Cũng đã 4 năm rồi nhỉ, cậu đã học được những gì rồi?");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Nhờ có cậu mà tôi học được cũng kha khá rồi.");
                    yield return S.Say("Vậy à,{a} đã đến lúc cậu phải đưa ra quyết định cho bản thân cậu rồi đấy.");
                    Z.SetSprite(confused);
                    yield return Z.Say("Là sao? Tôi không hiểu....");
                    yield return S.Say("Tính đến thời điểm này,{a} cậu đã học được một số kỹ năng cơ bản để có thể chiến đấu rồi. Để có thể chinh phục được 4 hầm ngục, cậu cần có kỹ năng đặc biệt để có thể đối phó với quái vật trong đó.");
                    Z.Hide();
                    yield return N.Say("Scy bắt đầu giải thích...");
                    yield return S.Say("Hiện tại, có 4 loại kỹ năng tương ứng với 4 nghề nghiệp mà cậu phải học qua. Nhưng tính tới hiện tại, thì mỗi người chỉ có được 1 nghề nghiệp duy nhất và chưa từng có ngoại lệ.{c}Đầu tiên là Hiệp sĩ, với khả năng phòng thủ tuyệt đối.{c} Thứ 2 là Pháp sư, sử dụng ma lực để thi triển phép thuật.{c}Thứ 3 là Sát thủ, với lượng sát thương cao bù vào khả năng phòng thủ cực kỳ kém.{c}Thứ 4 là Linh mục, với khả năng hồi phục mạnh mẽ, sức phòng thủ tương đối và khả năng tấn công kém.{c}Cậu hãy suy nghĩ thật kỹ và chọn cho mình một nghề nghiệp mà cậu cho rằng hợp với cậu nhất.");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Rắc rối vậy......{c}Thế tôi phải làm gì để học được những nghề nghiệp đó đây.");
                    Z.Hide();
                    yield return N.Say("Scy lặng một lúc rồi nói....");
                    yield return S.Say("Trước hết, cậu phải biết mình muốn làm gì đã..");
                    S.Hide();
                    Z.Show();
                    Z.SetSprite(confused);
                    yield return Z.Say("Mình nên chọn gì đây");
                    ChoicePanel2Animation();
                    break;
                }
            case 10:
                {
                    bg.ChangeBackground("bookstore");
                    scyHeartPoint--;
                    halrathHeartPoint--;
                    Z.Hide();
                    S.Hide();
                    soundtrack.PlayTrackByName("HeartBeat");
                    yield return N.Say("Zino quyết định từ chối sự giúp đỡ từ Scy. Cậu ta không tin tưởng Scy ngay tại thời điểm này và quyết định tự mình sẽ tìm đường trở về.{c}Zino cảm ơn Scy vì những thông tin về mảnh ghép và quay trở ra ngoài.{c}Scy không ngăn cản, cậu ta biết vì sao Zino lại từ chối.");
                    bg.ChangeBackground("Tavern");
                    soundtrack.PlayTrackByName("FinalDecision");
                    yield return N.Say("Vài năm sau, Scy gặp lại Zino trong một quán rượu.{c}Cậu ngồi xuống kế bên Zino.");
                    S.Show();
                    yield return S.Say("Làm một cốc bia không?");
                    Z.Show();
                    Z.SetSprite(sad);
                    yield return Z.Say("Cậu còn nhận người làm thêm không?");
                    yield return S.Say("Được thôi...");
                    Z.Hide();
                    S.Hide();
                    yield return N.Say("Zino đã thất bại trong việc tìm kiếm mảnh ghép thứ 3 khi không đủ ma lực để có thể mở cánh cổng. GAME OVER");
                    SaveGame();
                    SceneManager.LoadScene("Main Menu");

                    break;
                }
            case 11:
                {
                    bg.ChangeBackground("bookstore");
                    if (knight)
                    {
                        _atk = 3;
                        _def = 9;
                        _int = 4;
                        mainHealthPoint = 100;
                        health.text = Convert.ToString(mainHealthPoint);
                        soundtrack.PlayTrackByName("Calm");
                        S.Show();
                        yield return S.Say("Oh...lựa chọn tốt đấy");
                        yield return N.Say("Zino đã quyết định sẽ chọn Chiến binh.{c}Scy lấy ra một vật phẩm có vẻ như là một cổ vật huyền thoại.{c}Zino đã từng thấy nó trong 1 cuốn sách nói về cổ vật có thể ban tặng những kỹ năng dựa theo ý chí của người dùng.");
                        noti.text = "Đã học được kỹ năng -Defensive Up-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -AoE stun-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -Shield Bash-";
                        yield return new WaitForSeconds(2);
                        noti.text = "";
                        yield return S.Say("Được rồi, cậu hãy bước đến trước tủ gương và đưa tay ra trước mặt và làm động tác như cậu đang cầm nắm 1 thứ vũ khí.");
                        yield return N.Say("Zino bước đến trước cửa tủ gương mà Scy nhắc đến.{c}Cậu đưa tay lên và nắm lại thì Bỗng nhiên hình ảnh trong gương xuất hiện một thứ vũ khí theo nghề nghiệp cậu lựa chọn.");
                        noti.text = "Đã nhận được -Kiếm ngắn và khiên-";
                        yield return new WaitForSeconds(5);
                        noti.text = "";
                    }
                    else if (assassin)
                    {
                        _atk = 9;
                        _def = 1;
                        _int = 3;
                        mainHealthPoint = 100;
                        health.text = Convert.ToString(mainHealthPoint);
                        S.Show();
                        yield return S.Say("Oh...lựa chọn tốt đấy");
                        soundtrack.PlayTrackByName("Calm");
                        yield return N.Say("Zino đã quyết định sẽ chọn sát thủ.{c}Scy lấy ra một vật phẩm có vẻ như là một cổ vật huyền thoại.{c}Zino đã từng thấy nó trong 1 cuốn sách nói về cổ vật có thể ban tặng những kỹ năng dựa theo ý chí của người dùng.");
                        noti.text = "Đã học được kỹ năng -Invisible-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -Slient Cutting Edge-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -Demon Strike-";
                        yield return new WaitForSeconds(2);
                        noti.text = "";
                        yield return S.Say("Được rồi, cậu hãy bước đến trước tủ gương và đưa tay ra trước mặt và làm động tác như cậu đang cầm nắm 1 thứ vũ khí.");
                        yield return N.Say("Zino bước đến trước cửa tủ gương mà Scy nhắc đến.{c}Cậu đưa tay lên và nắm lại thì Bỗng nhiên hình ảnh trong gương xuất hiện một thứ vũ khí theo nghề nghiệp cậu lựa chọn.");
                        noti.text = "Đã nhận được -Lưỡi hái 2 đầu-";
                        yield return new WaitForSeconds(3);
                        noti.text = "";
                    }
                    else if (priest)
                    {
                        _atk = 3;
                        _def = 5;
                        _int = 9;
                        mainHealthPoint = 100;
                        health.text = Convert.ToString(mainHealthPoint);
                        S.Show();
                        yield return S.Say("Oh...lựa chọn tốt đấy");
                        soundtrack.PlayTrackByName("Calm");
                        yield return N.Say("Zino đã quyết định sẽ chọn Linh mục.{c}Scy lấy ra một vật phẩm có vẻ như là một cổ vật huyền thoại.{c}Zino đã từng thấy nó trong 1 cuốn sách nói về cổ vật có thể ban tặng những kỹ năng dựa theo ý chí của người dùng.");
                        noti.text = "Đã học được kỹ năng -Mercy Blessing-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -AoE stun-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -Solar Flare-";
                        yield return new WaitForSeconds(2);
                        noti.text = "";
                        yield return S.Say("Được rồi, cậu hãy bước đến trước tủ gương và đưa tay ra trước mặt và làm động tác như cậu đang cầm nắm 1 thứ vũ khí.");
                        yield return N.Say("Zino bước đến trước cửa tủ gương mà Scy nhắc đến.{c}Cậu đưa tay lên và nắm lại thì Bỗng nhiên hình ảnh trong gương xuất hiện một thứ vũ khí theo nghề nghiệp cậu lựa chọn.");
                        noti.text = "Đã nhận được -Trượng gỗ dài-";
                        yield return new WaitForSeconds(3);
                        noti.text = "";
                    }
                    else if (sorcerer)
                    {
                        _atk = 7;
                        _def = 4;
                        _int = 6;
                        mainHealthPoint = 100;
                        health.text = Convert.ToString(mainHealthPoint);
                        S.Show();
                        yield return S.Say("Oh...lựa chọn tốt đấy");
                        soundtrack.PlayTrackByName("Calm");
                        yield return N.Say("Zino đã quyết định sẽ chọn Pháp sư.{c}Scy lấy ra một vật phẩm có vẻ như là một cổ vật huyền thoại.{c}Zino đã từng thấy nó trong 1 cuốn sách nói về cổ vật có thể ban tặng những kỹ năng dựa theo ý chí của người dùng.");
                        noti.text = "Đã học được kỹ năng -Enhance Power-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -AoE stun-";
                        yield return new WaitForSeconds(2);
                        noti.text = "Đã học được kỹ năng -Lightning Strike-";
                        yield return new WaitForSeconds(2);
                        noti.text = "";
                        yield return S.Say("Được rồi, cậu hãy bước đến trước tủ gương và đưa tay ra trước mặt và làm động tác như cậu đang cầm nắm 1 thứ vũ khí.");
                        yield return N.Say("Zino bước đến trước cửa tủ gương mà Scy nhắc đến.{c}Cậu đưa tay lên và nắm lại thì Bỗng nhiên hình ảnh trong gương xuất hiện một thứ vũ khí theo nghề nghiệp cậu lựa chọn.");
                        noti.text = "Đã nhận được -Đũa phép gỗ-";
                        yield return new WaitForSeconds(3);
                        noti.text = "";
                    }
                    Z.Show();
                    Z.SetSprite(shock);
                    cv_Shaking.Shake();
                    yield return Z.Say("Eikkkkkk!!!!!!!!");
                    Z.Hide();
                    yield return N.Say("Zino cảm thấy có gì đó khác lạ chảy xuyên suốt trong cơ thể, cậu có thể cảm nhận được sức mạnh đang tăng dần.");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Chuyện gì vừa xảy ra vậy !!!!");
                    yield return S.Say("Viên đá vừa rồi đã cho cậu những kỹ năng và trang bị cần thiết cho nghề nghiệp và cậu chọn.{c}Việc bây giờ cậu nên làm là bắt đầu tập luyện những kỹ năng mới.{c}Hãy tìm đến Halrath, anh ta có thể giúp cậu tập luyện nhé.");
                    Z.SetSprite(happy);
                    yield return Z.Say("Thật sự rất cảm ơn cậu thời gian vừa qua, tôi sẽ cố gắng luyện tập và quay lại sớm nhất có thể.");
                    Z.Hide();
                    yield return N.Say("Zino mừng rỡ, vội vã gói gém đồ đạc chào Scy và lên đường ngay lập tức");
                    yield return S.Say("Hy vọng lần này mọi việc sẽ tốt hơn...........");
                    scyHeartPoint++;

                    S.Hide();
                    bg.ChangeBackground("Gate");
                    soundtrack.PlayTrackByName("TownSquare");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Phải nhanh chóng tìm đến Halrath mới được....");
                    Z.Hide();
                    bg.ChangeBackground("Forest");
                    yield return N.Say("Zino rảo bước nhanh chóng đến khu rừng....{c}Bất ngờ, cậu bị chặn lại bởi một con quái thú...");
                    EnemyAppear1();
                    wolfRHP = 100;
                    wolfHP.text = Convert.ToString(wolfRHP);
                    soundtrack.PlayTrackByName("InDanger");
                    Z.Show();
                    Z.SetSprite(shock);
                    yield return Z.Say("Thôi tiêu rồi, vội quá mình chưa kịp hỏi phải làm sao để sử dụng kỹ năng.{c}Đành liều vậy.");
                    yield return Z.Say("");

                    storyProcess++;
                    StartCoroutine(Chap());
                    break;
                }
            case 12:
                {
                    if (knight)
                    {
                        knightFight.SetActive(true);
                        isActive = true;
                    }
                    else if (assassin)
                    {
                        assassinFight.SetActive(true);
                        isActive = true;
                    }
                    else if (priest)
                    {
                        priestFight.SetActive(true);
                        isActive = true;
                    }
                    else if (sorcerer)
                    {
                        sorcFight.SetActive(true);
                        isActive = true;
                    }
                    break;
                }
            case 13:
                {
                    while (wolfRHP > 0)
                    {
                        double dmgdeal = dmg - wolfRDef;
                        if (dmgdeal <= 0)
                        {
                            if (stun)
                            {
                                yield return N.Say("Zino dùng kỹ năng gây choáng quái vật.");

                            }
                            else if (dUp)
                            {
                                yield return N.Say("Zino tập trung sức mạnh nâng cao điểm phòng thủ, điểm phòng thủ tăng 50% trong 1 lượt.");
                            }
                            else if (invi)
                            {
                                yield return N.Say("Zino sử dụng kỹ năng để tàng hình, đòn đánh tiếp theo tăng 50% sức mạnh");
                            }
                            else if (amp)
                            {
                                yield return N.Say("Zino tích tụ năng lượng vào bản thân, cường hóa đòn đánh tiếp theo lên 50% sát thương");
                            }
                            else if (isHeal)
                            {
                                yield return N.Say("Zino hồi " + HP + " máu cho bản thân");
                                HP = 0;
                                isHeal = false;
                            }
                            else
                                yield return N.Say("Zino đã đánh hụt, không gây ra sát thương");
                        }
                        else if (dmgdeal > 0)
                        {
                            if (dmgdeal > wolfRHP)
                            {
                                wolfRHP = 0;
                                wolfHP.text = wolfRHP.ToString();
                                char_Shaking.Shake();
                                yield return N.Say("Zino sử dụng kỹ năng gây " + dmgdeal + " sát thương");
                            }
                            else if (dmgdeal < wolfRHP)
                            {
                                wolfRHP = wolfRHP - dmgdeal;
                                wolfHP.text = wolfRHP.ToString();
                                char_Shaking.Shake();
                                yield return N.Say("Zino sử dụng kỹ năng gây " + dmgdeal + " sát thương");
                            }

                        }
                        if (wolfRHP <= 0)
                        {
                            dmg = 0;
                            storyProcess++;
                            StartCoroutine(Chap());
                            break;
                        }
                        if (invi || stun)
                        {
                            stun = false;
                            yield return N.Say("Quái vật không thể tấn công Zino.");
                            storyProcess--;
                            StartCoroutine(Chap());
                        }
                        else
                        {
                            //RNG Monster dmg and return Zino dmg to 0
                            double wolfdmg = Math.Round(UnityEngine.Random.Range(1, 21) - _def, 0);
                            dmg = 0;
                            if (dUp)
                            {
                                dUp = false;
                                _def = _def / 1.5f;
                                Debug.Log("Defense Up = " + dUp);
                            }
                            //check monster dmg <= 0
                            if (wolfdmg <= 0)
                            {
                                soundtrack.PlaySFXByName("MonsterSlash");
                                yield return N.Say("Zino đã kịp thời né đòn đánh của quái vật sói.");
                                storyProcess--;
                                StartCoroutine(Chap());
                            }
                            //check if Zino is invi

                            //check if monster is dealing dmg
                            else if (wolfdmg > 0)
                            {

                                soundtrack.PlaySFXByName("MonsterSlash");
                                blood.SetActive(true);
                                cv_Shaking.Shake();
                                yield return N.Say("Quái vật sói tấn công Zino gây ra " + wolfdmg + " sát thương");
                                mainHealthPoint = mainHealthPoint - wolfdmg;
                                health.text = mainHealthPoint.ToString();
                                //check Zino health
                                if (mainHealthPoint <= 0)
                                {
                                    soundtrack.PlaySFXByName("MonsterSlash");
                                    yield return N.Say("Quái vật sói đã hạ gục Zino. GAME  OVER ~");
                                    SceneManager.LoadScene("Main Menu");
                                }

                                else
                                {
                                    soundtrack.PlaySFXByName("MonsterSlash");
                                    blood.SetActive(false);
                                    storyProcess--;
                                    StartCoroutine(Chap());
                                }
                            }
                        }


                        break;
                    }
                    break;
                }
            case 14:
                {
                    monsterHP.SetActive(false);
                    EnemyHideAll();
                    yield return N.Say("Quái vật đã bị đánh bại");
                    Z.Hide();
                    soundtrack.PlayTrackByName("FinalDecision");
                    Z.Show();
                    Z.SetSprite(happy);
                    yield return Z.Say("Game đến đây là tạm hết và đang trong quá trình hoàn thiện, cảm ơn các bạn đã tham gia. Mọi đóng góp ý kiến hoặc phát hiện bug, bạn hãy để lại thông tin qua email: halrath2618@gmail.com kèm hình ảnh để bọn mình sửa chữa nhanh chóng nhé.");
                    SceneManager.LoadScene("Main Menu");
                    break;
                }
        }



    }
    public void SaveGame()
    {
        PlayerData data = new PlayerData(storyProcess);
        SaveSystem.SaveGame(data);
    }
    public void LoadGame()
    {
        PlayerData data = SaveSystem.LoadGame();
        if (data != null)
        {
            storyProcess = data.storyProcess;
            StartCoroutine(Chap());
        }
        // Update is called once per frame
    }
}
