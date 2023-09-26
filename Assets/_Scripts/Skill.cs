using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour {
    
    private int skill;
    [SerializeField] private List<Sprite> skillsImageList;
    [SerializeField] private Image SkillImage;
    [SerializeField] private Image CooldownImageAlpha;
    private int cooldown;
    private int maxCooldownOfSkill;
    
    public void SetSkillData(int itemID, int imageSelector, int MaxCooldown) {

        SkillImage.sprite = skillsImageList[imageSelector];
        skill = itemID;
        cooldown = 0;
        maxCooldownOfSkill = MaxCooldown;
        UpdateIconCooldown();

    }

    public int GetCooldownFromSkill() {

        return cooldown;

    }

    public void SetCooldownFromSkill() {

        cooldown = maxCooldownOfSkill;
        UpdateIconCooldown();

    }

    public void SetCooldownSkillCountdown() {

        cooldown -= 1;
        UpdateIconCooldown();

    }

    private void UpdateIconCooldown() {

        CooldownImageAlpha.fillAmount = cooldown / maxCooldownOfSkill;

    }
    
}
