using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatMenuManager : MonoBehaviour
{
    private GameObject statPanel;
    private Text levelText;
    private Text strengthText;
    private Text agilityText;
    private Text vitalityText;
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
        strengthText = GameObject.Find("StrengthValueText").GetComponent<Text>();
        agilityText = GameObject.Find("AgilityValueText").GetComponent<Text>();
        vitalityText = GameObject.Find("VitalityValueText").GetComponent<Text>();

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
        if (levelUpPoints > 0)
        {
            strengthLevelUpButton.gameObject.SetActive(true);
            agilityLevelUpButton.gameObject.SetActive(true);
            vitalityLevelUpButton.gameObject.SetActive(true);
        }
        else
        {
            strengthLevelUpButton.gameObject.SetActive(false);
            agilityLevelUpButton.gameObject.SetActive(false);
            vitalityLevelUpButton.gameObject.SetActive(false);
        }
    }

    void UpdateMenu()
    {
        if (!menuOpen) CloseMenu();
    }

    public void Refresh(int level, Stats stats)
    {
        levelText.text = level.ToString();
        strengthText.text = stats.strength.ToString();
        agilityText.text = stats.agility.ToString();
        vitalityText.text = stats.vitality.ToString();
        if (!menuOpen) CloseMenu();
    }

    public IEnumerator WaitForLevelUpSelect(int newLevelUpPoints, System.Action<MainStatType> callbackOnFinish)
    {
        levelUpPoints = newLevelUpPoints;
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
    }

    public void OpenMenu(int level, Stats stats)
    {
        menuOpen = true;
        statPanel.gameObject.SetActive(true);
        Refresh(level, stats);
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


}
