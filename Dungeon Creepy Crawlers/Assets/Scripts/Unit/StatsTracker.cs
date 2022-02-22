// Dien Nguyen

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct stats
{
    public int hp;
    public int maxHP;

    public int str;
    public int def;
    public int res;

    public int movement;
}

public class StatsTracker : MonoBehaviour
{
    public Unit unit;

    public string type;

    public Sprite miniSprite;
    public Sprite selectedSprite;
    public Sprite fullSprite;

    public Ability[] abilities;

    private stats unitStats;

    private PlayerManager playerManager;

    private AbilityHandler abilityHandler;

    private SpriteRenderer spriteRenderer;

    private Movement movementInst;

    public delegate void OnSetupHealthUI(int health, Transform parent);
    public static event OnSetupHealthUI updateHealthUI;

    public delegate void OnDamageUpdate(int damage, Transform parent);
    public static event OnDamageUpdate updateDamageUI;

    void Awake()
    {
        UpdateUnit();
    }

    void Start()
    {
        if (gameObject.tag == "Player")
        {
            playerManager = PlayerManager.GetInstance();
            playerManager.Add(this);
        }

        updateHealthUI(unitStats.hp, gameObject.transform);

        abilityHandler = GetComponent<AbilityHandler>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        movementInst = GetComponent<Movement>();

        movementInst.SetMovementSpeed(unitStats.movement);
        spriteRenderer.sprite = miniSprite;

    }

    public void DamageCalc(int dmg, Ability.AbilType abilType)
    {
        int dmgCalc = dmg;

        if (abilType == Ability.AbilType.Phys)
        {
            dmgCalc += unitStats.def;

            unitStats.hp += dmgCalc;
        }
        else if (abilType == Ability.AbilType.Mag) {
            dmgCalc += unitStats.res;

            unitStats.hp += dmgCalc;
        }
        else
        {
            unitStats.hp += dmgCalc;
        }

        if (unitStats.hp < 0)
        {
            unitStats.hp = 0;
        }

        if (unitStats.hp > unitStats.maxHP) {
            unitStats.hp = unitStats.maxHP;
        }


        updateDamageUI(dmgCalc, gameObject.transform);

        if (unitStats.hp == 0)
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        GameManager.GetInstance().Unsubscribe(GetComponent<TurnEventHandler>()); // Will probably move this away from TurnEventHandler
        if (playerManager != null)
        {
            playerManager.Remove(this);
        } 
        gameObject.SetActive(false);
    }

    public void SelectAbility(Ability ability) // Will be called by an ability button
    {
        if(ability)
        {
            abilityHandler.Select(ability);
        }
        else
        {
            Debug.Log("Missing Ability in Stats Tracker");
        }
    }

    public void DeselectAbility()
    {
        abilityHandler.Deselect();
    }

    private void UpdateUnit()
    {
        type = unit.type;
        miniSprite = unit.miniSprite;
        selectedSprite = unit.selectedSprite;
        fullSprite = unit.fullSprite;
        unitStats.hp = unit.hp;
        unitStats.maxHP = unitStats.hp;
        unitStats.str = unit.str;
        unitStats.def = unit.def;
        unitStats.res = unit.res;
        unitStats.movement = unit.movement;
        abilities = unit.abilities;
    }

    public Ability[] GetInitAbilities()
    {
        return unit.abilities;
    }

    public stats GetStats()
    {
        return unitStats;
    }

    public AbilityHandler GetAbilityHandler()
    {
        return abilityHandler;
    }
}
