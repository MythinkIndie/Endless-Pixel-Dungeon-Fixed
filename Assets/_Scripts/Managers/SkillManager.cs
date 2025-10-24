using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("Skill UI")]
    public GameObject skillPrefab;
    public Transform skillParent;

    private List<ISkill> activeSkills = new List<ISkill>();
    private int currentWeaponId;
    [SerializeField] private List<int> WeaponsWithSkill = new List<int>();
    private GameObject skillUI;

    [SerializeField] private List<AudioGroup> SkillsAudios;
    [SerializeField] private List<GameObject> SkillsSFX;

    public void Initialize(int weaponId)
    {

        currentWeaponId = weaponId;
        SetupWeaponSkills();

    }

    private void SetupWeaponSkills()
    {
        ClearSkills();

        int id = WeaponsWithSkill.FindIndex(i => i == currentWeaponId);

        if (id != -1)
        {

            CreateSkill(id);

        }

    }

    private void CreateSkill(int skillId)
    {

        var skillObj = Instantiate(skillPrefab, skillParent);
        skillUI = skillObj;
        var skillComponent = skillUI.GetComponent<Skill>();

        ISkill skill = SkillFactory.CreateSkill(skillId, currentWeaponId, SkillsSFX[skillId], SkillsAudios[skillId].clips);
        skillComponent.SetSkillData(skill);

        var skillButton = skillUI.GetComponent<Button>();

        skillButton.onClick.AddListener(() => ExecuteSkill(skillId));

        activeSkills.Add(skill);
    }

    public void ExecuteSkill(int skillId)
    {
        var skill = activeSkills.Find(s => s.SkillId == skillId);
        var skillComponent = skillUI.GetComponent<Skill>();

        if (skill != null && skill.CanExecute())
        {
            skill.Execute();
            skillComponent.SetMaxCooldownFromSkill();

        }
    }

    public void TryReduceSkillTimer(TimerReducer trigger)
    {

        foreach (var skill in activeSkills)
        {
            if (skill.cooldown > 0)
            {

                skill.TryReduceSkillTimer(trigger);
                skillUI.GetComponent<Skill>().SetCooldownSkillCountdown(skill.cooldown);

            }
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