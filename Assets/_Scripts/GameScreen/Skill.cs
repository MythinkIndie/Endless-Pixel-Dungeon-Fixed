using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skill : MonoBehaviour {
    
    public int skill;
    [SerializeField] private List<Sprite> skillsImageList;
    [SerializeField] private Image SkillImage;
    [SerializeField] private Image CooldownImageAlpha;
    [SerializeField] private List<int> MaxCooldownList;
    private Button InternButtonSkill;
    private int cooldown;
    private int maxCooldownOfSkill;
    private bool isSkillSelected;

    //El cooldown estará aqui ya preconfigurado
    public void SetSkillData(int itemID, int imageSelector) {

        InternButtonSkill = this.GetComponent<Button>();
        SkillImage.sprite = skillsImageList[imageSelector];
        skill = itemID;
        cooldown = 0;
        maxCooldownOfSkill = MaxCooldownList[itemID];
        isSkillSelected = false;
        //InternButtonSkill.onClick.AddListener();
        UpdateIconCooldown();

    }

    public bool IsSkillSelected {

        get {
            return this.isSkillSelected;
        }

        set {
            this.isSkillSelected = value;
        }

    }

    public int GetCooldownFromSkill() {

        return cooldown;

    }

    public void SetMaxCooldownFromSkill() {

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
