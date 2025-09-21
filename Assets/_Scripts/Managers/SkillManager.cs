using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [Header("Skill UI")]
    public GameObject skillPrefab;
    public Transform skillParent;
    
    private List<ISkill> activeSkills = new List<ISkill>();
    private int currentWeaponId;

    public void Initialize(int weaponId) {

        currentWeaponId = weaponId;
        SetupWeaponSkills();
        
    }
    
    private void SetupWeaponSkills()
    {
        ClearSkills();
        
        var skillId = new NoSkill();

        if (skillId.SkillId != -1) {

            CreateSkill(skillId.SkillId);
                
        }

    }
    
    private void CreateSkill(int skillId)
    {
        var skillObj = Instantiate(skillPrefab, skillParent);
        var skillComponent = skillObj.GetComponent<Skill>();
        
        skillComponent.SetSkillData(currentWeaponId, skillId);
        
        var skillButton = skillObj.GetComponent<UnityEngine.UI.Button>();
        skillButton.onClick.AddListener(() => ExecuteSkill(skillId));
        
        // Create skill behavior
        ISkill skill = SkillFactory.CreateSkill(skillId, currentWeaponId);
        activeSkills.Add(skill);
    }
    
    public void ExecuteSkill(int skillId)
    {
        var skill = activeSkills.Find(s => s.SkillId == skillId);
        if (skill != null && skill.CanExecute())
        {
            skill.Execute();
        }
    }
    
    private void ClearSkills()
    {
        activeSkills.Clear();
        
        foreach (Transform child in skillParent)
        {
            Destroy(child.gameObject);
        }
    }
}