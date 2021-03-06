using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UseAbility : MonoBehaviour, IPointerClickHandler
{
    public GameObject ability;

    public DungeonMasterController controller;

    public Slots slots;

    public int size;

    public int amount;

    public Text text;

    public float startingCooldownTime;

    public float cooldownTime;

    public float cooldown;

    public int charges;

    // Start is called before the first frame update
    public void Start()
    {
        var data = ability.GetComponent<EnemyGeneric>();
        startingCooldownTime = data.cooldown;
        cooldownTime = data.cooldown;
        cooldown = cooldownTime;
        size = data.slotSize;
        text = GetComponentInChildren<Text>();
        text.resizeTextForBestFit = true;
        text.text = ability.name + " X" + amount + "\nCooldown: " + cooldown.ToString("0.00");

        if(controller.charges.ContainsKey(ability))
        {
            text.text = ability.name + " X" + amount + "\nCharges: " + charges + "\nCooldown: " + cooldown.ToString("0.00");
        }
    }
    public void Update()
    {
        cooldown -= Time.deltaTime;
        text.text = ability.name + " X" + amount + "\nCooldown: " + cooldown.ToString("0.00"); ;

        if(cooldown <= 0)
        {
            text.text = ability.name + " X" + amount + "\nReady";
        }

        if(controller.charges.ContainsKey(ability))
        {
            charges = controller.charges[ability];

            text.text = ability.name + " X" + amount + "\nCharges: " + charges + "\nCooldown: " + cooldown.ToString("0.00");

            if (cooldown <= 0 && charges > 0)
            {
                text.text = ability.name + " X" + amount + "\nCharges: " + charges + "\nReady";
            }

            else if (charges == 0)
            {
                text.text = ability.name + " X" + amount + "\nNo Charges";
            }
        }
    }

    public void SetCurrentAbility()
    {
        controller.SetCurrentSelection(ability, this);
        // cooldown = cooldownTime;     
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(amount == 1)
            {
                slots.available += size;
                Destroy(gameObject);
            }
            
            else
            {
                slots.available += size;
                amount -= 1;
                var prevCooldown = cooldownTime;
                cooldownTime = startingCooldownTime / amount;

                if(cooldown < cooldownTime)
                {
                    cooldown = cooldownTime - prevCooldown + cooldown;
                }
            }
        }
    }
}
