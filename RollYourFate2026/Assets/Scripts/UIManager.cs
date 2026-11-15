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
    private float multiplier;

    private float moneytime = 2f;
    private float moneytimer = 0f;

    private float MoneyTime => moneytime / multiplier;

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
        if (moneytimer >= MoneyTime)
        {
            money += 1;
            moneytimer -= MoneyTime;
        }
        moneyText.text = money.ToString() + " : " + multiplier.ToString() + "x";
        timerText.text = "Timer: " + timer.ToString("F1");

        moneyBar.fillAmount = moneytimer / MoneyTime;

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

    public bool spendMoney(int amount)
    {
        if (money - amount < 0)
        {
            return false;
        }
        money -= amount;
        return true;
    }

    public void increaseMultiplier(float amount)
    {
        multiplier += amount;
    }
}
