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

    [Header("Level Up UI")]
    [SerializeField] private GameObject levelUpPanel; // Panel that will show the options
    [SerializeField] private Button[] upgradeButtons; // Array of buttons for upgrades
    [SerializeField] private TMP_Text[] buttonTexts; // Array of text components for buttons

    private int currentLevel = 1;
    private int currentXP = 0;
    private int xpToNextLevel;

    // Enum for upgrade types
    public enum UpgradeType
    {
        AddProjectile,
        IncreaseDamage,
        IncreaseSpeed
    }

    private void Start()
    {
        xpToNextLevel = baseXP;
        UpdateUI();
        levelUpPanel.SetActive(false); // Hide the level up panel initially
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
        levelUpPanel.SetActive(true);

        string[] possibleUpgrades = { "Add Projectile", "Increase Damage", "Increase Speed" };
        int[] upgradeIndexes = new int[3];

        for (int i = 0; i < upgradeIndexes.Length; i++)
        {
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, possibleUpgrades.Length);
            } while (System.Array.Exists(upgradeIndexes, element => element == randomIndex));

            upgradeIndexes[i] = randomIndex;
            buttonTexts[i].text = possibleUpgrades[randomIndex];
        }

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int upgradeIndex = upgradeIndexes[i];
            upgradeButtons[i].onClick.RemoveAllListeners();
            // Pass the upgrade type to ApplyUpgrade
            upgradeButtons[i].onClick.AddListener(() => ApplyUpgrade((UpgradeType)upgradeIndex));
        }
    }

    private void ApplyUpgrade(UpgradeType upgradeType)
    {
        if (levelUpPanel.activeSelf)
        {
            switch (upgradeType)
            {
                case UpgradeType.AddProjectile:
                    AddProjectileUpgrade();
                    break;
                case UpgradeType.IncreaseDamage:
                    IncreaseDamage();
                    break;
                case UpgradeType.IncreaseSpeed:
                    IncreaseSpeed();
                    break;
            }
            levelUpPanel.SetActive(false);
        }
    }

    private void AddProjectileUpgrade()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerAutoAttack autoProjectile = player.GetComponent<PlayerAutoAttack>();
            if (autoProjectile != null)
            {
                autoProjectile.AddProjectile();
                Debug.Log("Projectile Upgrade Selected! Projectiles per attack: " + autoProjectile.projectilesPerAttack);
            }
            else
            {
                Debug.LogWarning("AutoProjectile component not found on Player!");
            }
        }
    }

    private void IncreaseDamage()
    {
        AutoProjectile[] projectiles = FindObjectsOfType<AutoProjectile>();
        foreach (AutoProjectile projectile in projectiles)
        {
            projectile.BaseDamage = projectile.BaseDamage + 1;
            projectile.IncreaseDamage(1);
        }
        Debug.Log("Damage Increased for all active projectiles!");
    }

    private void IncreaseSpeed()
    {
        
     Debug.Log("Speed Increased!");

    }
}
