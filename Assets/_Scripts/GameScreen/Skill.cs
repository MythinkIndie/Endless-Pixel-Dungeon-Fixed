using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour {
    
    private ISkill skill;
    [SerializeField] private List<Sprite> skillsImageList;
    [SerializeField] private Image SkillImage;
    [SerializeField] private Image CooldownImageAlpha;
    public Button InternButtonSkill;
    private int cooldown;
    private int maxCooldownOfSkill;

    //El cooldown estará aqui ya preconfigurado
    public void SetSkillData(ISkill skill) {
        
        SkillImage.sprite = skillsImageList[skill.SkillId];
        this.skill = skill;
        cooldown = skill.cooldown;
        maxCooldownOfSkill = skill.maxCooldown;
        UpdateIconCooldown();

    }

    public int GetCooldownFromSkill() {

        return cooldown;

    }

    public void SetMaxCooldownFromSkill() {

        cooldown = maxCooldownOfSkill;
        UpdateIconCooldown();

    }

    public void SetCooldownSkillCountdown(int newCooldown) {

        cooldown = newCooldown;
        UpdateIconCooldown();

    }

    private void UpdateIconCooldown() {

        CooldownImageAlpha.fillAmount = (float)cooldown / maxCooldownOfSkill;

    }
    
}
