using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerExperience : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider xpBar;
    [SerializeField] private TMP_Text xpText;
    [SerializeField] private TMP_Text levelText;

    [Header("XP Settings")]
    [SerializeField] private int baseXP = 100;
    [SerializeField] private float xpIncreaseFactor = 1.5f;

    private int currentLevel = 1;
    private int currentXP = 0;
    private int xpToNextLevel;

    private void Start()
    {
        xpToNextLevel = baseXP;
        UpdateUI();
    }

    public void AddXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }

        UpdateUI();
    }

    private void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;
        xpToNextLevel = Mathf.CeilToInt(xpToNextLevel * xpIncreaseFactor);

        Debug.Log($"Level Up! New Level: {currentLevel}");
        GrantReward(currentLevel);
    }


    private void UpdateUI()
    {
        xpBar.maxValue = xpToNextLevel;
        xpBar.value = currentXP;

        xpText.text = $"{currentXP} / {xpToNextLevel} XP";
        levelText.text = $"Level {currentLevel}";
    }

    private void GrantReward(int level)
    {
        // cadeau

    }


}
