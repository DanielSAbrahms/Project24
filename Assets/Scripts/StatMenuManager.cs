using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatMenuManager : MonoBehaviour
{
    private GameObject statPanel;
    private Text levelText;
    private Text pointsRemainingText;
    private Text strengthText;
    private Text agilityText;
    private Text vitalityText;
    private Text damageText;
    private Text attackText;
    private Text defenseText;
    private Text maxHealthText;
    private Text maxStaminaText;
    private Button strengthLevelUpButton;
    private Button agilityLevelUpButton;
    private Button vitalityLevelUpButton;

    private int levelUpPoints;
    private MainStatType? chosenStatTemp;
    private bool menuOpen;

    // Start is called before the first frame update
    void Start()
    {
        menuOpen = false;
        statPanel = GameObject.Find("StatPanel");
        levelText = GameObject.Find("LevelValueText").GetComponent<Text>();
        pointsRemainingText = GameObject.Find("PointsRemainingText").GetComponent<Text>();
        strengthText = GameObject.Find("StrengthValueText").GetComponent<Text>();
        agilityText = GameObject.Find("AgilityValueText").GetComponent<Text>();
        vitalityText = GameObject.Find("VitalityValueText").GetComponent<Text>();
        damageText = GameObject.Find("DamageValueText").GetComponent<Text>();
        attackText = GameObject.Find("AttackValueText").GetComponent<Text>();
        defenseText = GameObject.Find("DefenseValueText").GetComponent<Text>();
        maxHealthText = GameObject.Find("MaxHealthValueText").GetComponent<Text>();
        maxStaminaText = GameObject.Find("MaxStaminaValueText").GetComponent<Text>();

        strengthLevelUpButton = GameObject.Find("LevelUpStrengthButton").GetComponent<Button>();
        strengthLevelUpButton.onClick.AddListener( delegate { 
            chosenStatTemp = MainStatType.Strength; 
        });
        agilityLevelUpButton = GameObject.Find("LevelUpAgilityButton").GetComponent<Button>();
        agilityLevelUpButton.onClick.AddListener(delegate {
            chosenStatTemp = MainStatType.Agility;
        });
        vitalityLevelUpButton = GameObject.Find("LevelUpVitalityButton").GetComponent<Button>();
        vitalityLevelUpButton.onClick.AddListener(delegate {
            chosenStatTemp = MainStatType.Vitality;
        });
        chosenStatTemp = null;
        UpdateMenu();
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    void UpdateMenu()
    {
        if (!menuOpen) CloseMenu();
    }

    public void Reset(int level, Stats stats, int maxHealth, int maxStamina, int? levelUpPointsValue = null)
    {
        levelText.text = level.ToString();
        strengthText.text = stats.strength.ToString();
        agilityText.text = stats.agility.ToString();
        vitalityText.text = stats.vitality.ToString();
        maxHealthText.text = maxHealth.ToString();
        maxStaminaText.text = maxStamina.ToString();
        damageText.text = stats.damageRange[0].ToString() + " - " + stats.damageRange[1].ToString();
        attackText.text = stats.attackRange[0].ToString() + " - " + stats.attackRange[1].ToString();
        defenseText.text = stats.defenseRange[0].ToString() + " - " + stats.defenseRange[1].ToString();
        if (!menuOpen) CloseMenu();

        // Optional Additional Points (pre-levelup)
        if (levelUpPointsValue != null)
        {
            pointsRemainingText.text = (levelUpPoints + (int)levelUpPointsValue).ToString();
        } else
        {
            pointsRemainingText.text = levelUpPoints.ToString();
        }
    }

    public IEnumerator WaitForLevelUpSelect(int newLevelUpPoints, System.Action<MainStatType> callbackOnFinish)
    {
        SetLevelUpButtonState(true);

        levelUpPoints += newLevelUpPoints;
        while (levelUpPoints > 0)
        {
            while (chosenStatTemp == null)
            {
                yield return null;
            }
            if (levelUpPoints > 0)
            {
                levelUpPoints--;
                callbackOnFinish((MainStatType)chosenStatTemp);
                chosenStatTemp = null;
            }
        }
        SetLevelUpButtonState(false);
    }

    public void OpenMenu(int level, Stats stats, int maxHealth, int maxStamina)
    {
        menuOpen = true;
        statPanel.gameObject.SetActive(true);
        Reset(level, stats, maxHealth, maxStamina);

        if (levelUpPoints > 0) { SetLevelUpButtonState(true); }
        else {  SetLevelUpButtonState(false);  }
    }

    public void CloseMenu()
    {
        menuOpen = false;
        statPanel.gameObject.SetActive(false);
    }

    public bool IsMenuOpen()
    {
        return menuOpen;
    }

    private void SetLevelUpButtonState(bool newState)
    {
        strengthLevelUpButton.gameObject.SetActive(newState);
        agilityLevelUpButton.gameObject.SetActive(newState);
        vitalityLevelUpButton.gameObject.SetActive(newState);
    }


}
