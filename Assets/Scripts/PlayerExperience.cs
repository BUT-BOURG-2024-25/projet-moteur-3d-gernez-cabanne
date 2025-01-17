using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    [SerializeField] private GameObject levelUpPanel;
    [SerializeField] private Button[] upgradeButtons;
    [SerializeField] private TMP_Text[] buttonTexts;

    private int currentLevel = 1;
    private int currentXP = 0;
    private int xpToNextLevel;

    public enum UpgradeType
    {
        AddProjectile,
        IncreaseDamage,
        IncreaseHealth,
        Explo,
        Gosth
    }

    private Dictionary<UpgradeType, string> upgradeDescriptions = new Dictionary<UpgradeType, string>
    {
        { UpgradeType.AddProjectile, "More projectile" },
        { UpgradeType.IncreaseDamage, "Increase damage" },
        { UpgradeType.IncreaseHealth, "Increase health" },
        { UpgradeType.Gosth, "Add Ghost Shield" },
        { UpgradeType.Explo, "Better Explosion" }
    };

    private void Start()
    {
        xpToNextLevel = baseXP;
        UpdateUI();
        levelUpPanel.SetActive(false);
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
        ShowLevelUpOptions();
    }

    private void UpdateUI()
    {
        xpBar.maxValue = xpToNextLevel;
        xpBar.value = currentXP;

        xpText.text = $"{currentXP} / {xpToNextLevel} XP";
        levelText.text = $"Level {currentLevel}";
    }

    private void ShowLevelUpOptions()
    {
        Time.timeScale = 0f;

        levelUpPanel.SetActive(true);

        List<UpgradeType> upgradePool = new List<UpgradeType>((UpgradeType[])System.Enum.GetValues(typeof(UpgradeType)));
        List<UpgradeType> selectedUpgrades = new List<UpgradeType>();

        while (selectedUpgrades.Count < 3 && upgradePool.Count > 0)
        {
            int index = Random.Range(0, upgradePool.Count);
            selectedUpgrades.Add(upgradePool[index]);
            upgradePool.RemoveAt(index);
        }

        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < selectedUpgrades.Count)
            {
                UpgradeType upgrade = selectedUpgrades[i];
                buttonTexts[i].text = upgradeDescriptions[upgrade];
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(upgrade));
                upgradeButtons[i].gameObject.SetActive(true);
            }
            else
            {
                upgradeButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void SelectUpgrade(UpgradeType upgrade)
    {
        switch (upgrade)
        {
            case UpgradeType.AddProjectile:
                AddProjectileUpgrade();
                break;
            case UpgradeType.IncreaseDamage:
                IncreaseDamage();
                break;
            case UpgradeType.IncreaseHealth:
                IncreaseHealth();
                break;
            case UpgradeType.Gosth:
                Gosth();
                break;
            case UpgradeType.Explo:
                Explo();
                break;
        }

        levelUpPanel.SetActive(false);
        Time.timeScale = 1f;
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
            }
        }
    }

    private void IncreaseDamage()
    {
        AutoProjectile.GlobalDamageBonus += 1;
        Debug.Log("Damage increased!");
    }

    private void IncreaseHealth()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.IncreaseMaxHealth(1);
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on Player!");
            }
        }
    }

    private void Explo()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            ExplosionAbility explosionAbility = player.GetComponent<ExplosionAbility>();
            if (explosionAbility != null)
            {
                if (!explosionAbility.IsActive())
                {
                    explosionAbility.ActivateExplosion();
                    Debug.Log("Explosion ability activated!");
                }
                else
                {
                    explosionAbility.IncreaseExplosionDamageRadius(1);
                    Debug.Log("Explosion damage increased!");
                }
            }
        }
    }


    private void Gosth()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            OrbitalsManager orbitalsManager = player.GetComponent<OrbitalsManager>();
            if (orbitalsManager != null)
            {
                orbitalsManager.AddOrbital();
                Debug.Log("Orbital added!");
            }
            else
            {
                Debug.LogWarning("OrbitalsManager component not found on Player!");
            }
        }
    }

}
