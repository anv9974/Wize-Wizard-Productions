using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text moneyText;

    public TMP_Text timerText;
    public Image moneyBar;
    public GameObject wizardProfile;

    public float timer;
    private float originalTimer;
    private int money;
    private int multiplier;

    private float moneytime = 2f;
    private float moneytimer = 0f;

    Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        money = 0;
        multiplier = 1;

        originalTimer = timer;
        anim = wizardProfile.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        moneytimer += Time.deltaTime;
        if (moneytimer >= moneytime)
        {
            money += 1 * multiplier;
            moneytimer = 0f;
        }
        moneyText.text = money.ToString() + " - " + multiplier.ToString() + "x";
        timerText.text = "Timer: " + timer.ToString("F1");

        moneyBar.fillAmount = moneytimer / moneytime;

        timer -= Time.deltaTime;

        setWizardProfile();
    }

    void setWizardProfile()
    {

        if (timer <= originalTimer / 3)
        {
            anim.SetInteger("panicstate", 3);
        }
        else if (timer <= originalTimer / 3 * 2)
        {
            anim.SetInteger("panicstate", 2);
        }
        else
        {
            anim.SetInteger("panicstate", 1);
        }
    }
}
