using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatMenuManager : MonoBehaviour
{
    private GameObject statPanel;
    private Text strengthText;
    private Text levelText;
    private Button strengthLevelUpButton;

    private int levelUpPoints;
    private MainStatType? chosenStatTemp;
    private bool menuOpen;

    // Start is called before the first frame update
    void Start()
    {
        menuOpen = false;
        statPanel = GameObject.Find("StatPanel");
        strengthText = GameObject.Find("StrengthValueText").GetComponent<Text>();
        levelText = GameObject.Find("LevelValueText").GetComponent<Text>();
        strengthLevelUpButton = GameObject.Find("LevelUpStrengthButton").GetComponent<Button>();
        strengthLevelUpButton.onClick.AddListener( delegate { 
            chosenStatTemp = MainStatType.Strength; 
        });
        chosenStatTemp = null;
        UpdateMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelUpPoints > 0) strengthLevelUpButton.gameObject.SetActive(true);
        else strengthLevelUpButton.gameObject.SetActive(false);
    }

    void UpdateMenu()
    {
        if (!menuOpen) CloseMenu();
    }

    public void Refresh(int level, Stats stats)
    {
        levelText.text = level.ToString();
        strengthText.text = stats.strength.ToString();
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
