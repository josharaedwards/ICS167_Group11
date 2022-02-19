//Joshara Edwards (2022)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitInfoDisplay : MonoBehaviour
{
    public Image unitImageObject;
    public TextMeshProUGUI unitNameText;
    public Slider healthBar;
    public TextMeshProUGUI healthBarValue;

    public GameObject buttonContainer;
    public Button abilityButtonPrototype;

    public StatsTracker unitStats;

    private void OnEnable()
    {
        Init();
        UpdateAbilities();
    }

    public void Init()
    {
        if(unitStats)
        {
            unitNameText.text = unitStats.unit.type;
            unitImageObject.sprite = unitStats.unit.fullSprite;

            stats stats_ = unitStats.GetStats();
            healthBar.maxValue = stats_.maxHP;
            healthBar.value = stats_.hp;

            healthBarValue.text = stats_.hp.ToString() + " / " + stats_.maxHP.ToString();
        }
        else
        {
            unitNameText.text = "Debug";
        }
    }

    public void UpdateAbilities()
    {
        if (!unitStats)
            return;

        int numOfAbilities = unitStats.abilities.Length;

        foreach (Transform child in buttonContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        for(int i = 0; i < numOfAbilities; ++i)
        {
            CreateAbilityButton(unitStats.abilities[i], unitStats);
        }
    }

    private void CreateAbilityButton(Ability ability, StatsTracker unit)
    {
        Button abilityButton = Instantiate(abilityButtonPrototype, buttonContainer.transform);
        abilityButton.GetComponent<AbilityButtonBroadcast>().Init(ability, unit);
    }
}
