using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SkillFactory
{
    public static ISkill CreateSkill(int skillId, int weaponId, GameObject sfx, List<AudioClip> audios) {
        switch (skillId) {
            case 0: return new SolarBeamSkill(weaponId, sfx, audios);
            case 1: return new NightAbyssSkill(weaponId, sfx, audios);
            case 2: return new PurifySkill(weaponId, sfx, audios);
            // case 3: return new PowerStrikeSkill(weaponId);
            // case 4: return new BurnRoomSkill(weaponId);
            // case 5: return new RockFallSkill(weaponId);
            // case 6: return new SuperStrikeSkill(weaponId);
            // case 7: return new RandomEffectSkill(weaponId);
            default: return new NoSkill();
        }
    }
}

public interface ISkill
{
    int SkillId { get; }
    int WeaponId { get; }
    int cooldown { get; }
    int maxCooldown { get; }
    bool CanExecute();
    void Execute();
    void TryReduceSkillTimer(TimerReducer trigger);
}

public class SolarBeamSkill : MonoBehaviour, ISkill
{

    public int SkillId => 0;
    public int WeaponId { get; private set; }
    public int cooldown { get; set; }
    public int maxCooldown { get; private set; }
    private TimerReducer _howReduceCooldown => TimerReducer.OnTileDiscover;
    private GameObject SkillSFX;
    private AudioClip audios;

    public SolarBeamSkill(int weaponId, GameObject SFXobj, List<AudioClip> audios, int cooldown = 0, int maxCooldown = 7)
    {
        WeaponId = weaponId;
        this.cooldown = cooldown;
        this.maxCooldown = maxCooldown;
        SkillSFX = SFXobj;
        this.audios = audios[0];
    }

    public bool CanExecute()
    {
        return cooldown <= 0;
    }

    public void Execute()
    {

        if (!CanExecute()) return;

        cooldown = maxCooldown;

        var boardManager = DungeonGameManager.Instance.boardManager;
        var allCells = boardManager.Board;
        var visibleEnemies = allCells.FindAll(c => c.Enemy != null && c.isRevelated);

        //Falta añadir particulas para señalar donde esta el enemigo
        AudioManager.SharedInstance.PlaySound(audios);
        CoroutineRunner.Instance.StartCoroutine(EachEnemyEffect(visibleEnemies));

    }

    private IEnumerator EachEnemyEffect(List<Cell> visibleEnemies)
    {

        var playerManager = DungeonGameManager.Instance.playerManager;

        foreach (var cell in visibleEnemies)
        {
            
            CombatResult skillCombat = CombatSystem.ProcessSkillCombat(playerManager.GetPlayer(), cell.Enemy, playerManager.AttackBoost, 0.75f);
            yield return new WaitForSeconds(0.15f);

            DungeonGameManager.Instance.HandleCombatResult(skillCombat, cell.Enemy);
            CoroutineRunner.Instance.StartCoroutine(ApplySkillEffect(cell));

        }

    }

    private IEnumerator ApplySkillEffect(Cell cell)
    {
        GameObject gridParent = GameObject.Find("SkillEffectGo");

        var SFXobj = Instantiate(SkillSFX, gridParent.transform.GetChild((9 * (cell.Pos.y - 2)) + cell.Pos.x));
        var SFXTransform = SFXobj.GetComponent<RectTransform>();
        var SFXImage = SFXobj.GetComponent<UnityEngine.UI.Image>();

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < 10; i++)
        {

            var originalSize = SFXTransform.sizeDelta;
            var originalColor = SFXImage.color;
            SFXTransform.sizeDelta = new Vector2(originalSize.x - 10, originalSize.y);

            SFXImage.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Clamp01(originalColor.a - 0.02f));

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(SFXobj);

    }

    public void TryReduceSkillTimer(TimerReducer trigger)
    {
        if (trigger == _howReduceCooldown)
        {
            this.cooldown--;
        }

    }
}

public class NightAbyssSkill : MonoBehaviour, ISkill  {

    public int SkillId => 1;
    public int WeaponId { get; private set; }
    public int cooldown {get; set;}
    public int maxCooldown { get; private set; }
    private TimerReducer _howReduceCooldown => TimerReducer.onKill;
    private GameObject SkillSFX;
    private List<AudioClip> audios;

    public NightAbyssSkill(int weaponId, GameObject SFXobj, List<AudioClip> audios, int cooldown = 0, int maxCooldown = 3)
    {
        WeaponId = weaponId;
        this.cooldown = cooldown;
        this.maxCooldown = maxCooldown;
        SkillSFX = SFXobj;
        this.audios = audios;
    }
    
    public bool CanExecute()
    {
        var boardManager = DungeonGameManager.Instance.boardManager;
        var allCells = boardManager.Board;
        var visibleEnemies = allCells.FindAll(c => c.Enemy != null && c.isRevelated);

        return visibleEnemies.Count != 0 || cooldown <= 0;
    }

    public void Execute()
    {

        if (!CanExecute()) return;

        cooldown = maxCooldown;
        
        var boardManager = DungeonGameManager.Instance.boardManager;
        var allCells = boardManager.Board;
        var visibleEnemies = allCells.FindAll(c => c.Enemy != null && c.isRevelated);

        //Falta desbloquear casillas por un tiempo o unas pocas aleatorias o de solo 1 enemigo

        CoroutineRunner.Instance.StartCoroutine(EachEnemyEffect(visibleEnemies));
        
    }

    private IEnumerator EachEnemyEffect(List<Cell> visibleEnemies)
    {

        var playerManager = DungeonGameManager.Instance.playerManager;

        for (int i = 0; i < 3; i++)
        {
            if (visibleEnemies.Count == 0)
            {
                yield break;
            }
            int rand = Random.Range(0, visibleEnemies.Count);
            var selectedCell = visibleEnemies[rand];

            CombatResult skillCombat = CombatSystem.ProcessSkillCombat(playerManager.GetPlayer(), selectedCell.Enemy, playerManager.AttackBoost);
            yield return ApplySkillEffect(selectedCell);

            DungeonGameManager.Instance.HandleCombatResult(skillCombat, selectedCell.Enemy);

            if (skillCombat.enemyDefeated)
            {
                visibleEnemies.Remove(selectedCell);
            }

        }            

    }

    private IEnumerator ApplySkillEffect(Cell cell)
    {
        GameObject gridParent = GameObject.Find("SkillEffectGo");

        var SFXobj = Instantiate(SkillSFX, gridParent.transform.GetChild((9 * (cell.Pos.y - 2)) + cell.Pos.x));
        var SFXTransform = SFXobj.GetComponent<RectTransform>();
        var SFXImage = SFXobj.GetComponent<UnityEngine.UI.Image>();

        AudioManager.SharedInstance.PlaySound(audios[Random.Range(0, audios.Count)]);
        yield return new WaitForSeconds(0.12f);

        for (int i = 0; i < 3; i++)
        {
            SFXobj.GetComponent<AbyssSlashAnim>().ExecuteAnim();


            yield return new WaitForSeconds(0.12f);
        }

        AudioManager.SharedInstance.PlaySound(audios[Random.Range(0, audios.Count)]);

        Destroy(SFXobj);
        
    }

    public void TryReduceSkillTimer(TimerReducer trigger)
    {
        if (trigger == _howReduceCooldown)
        {
            this.cooldown--;
        }
        
    }
}

public class PurifySkill : MonoBehaviour, ISkill  {

    public int SkillId => 2;
    public int WeaponId { get; private set; }
    public int cooldown {get; set;}
    public int maxCooldown { get; private set; }
    private TimerReducer _howReduceCooldown => TimerReducer.OnTileDiscover;
    private GameObject SkillSFX;
    private List<AudioClip> audios;

    public PurifySkill(int weaponId, GameObject SFXobj, List<AudioClip> audios, int cooldown = 0, int maxCooldown = 6)
    {
        WeaponId = weaponId;
        this.cooldown = cooldown;
        this.maxCooldown = maxCooldown;
        SkillSFX = SFXobj;
        this.audios = audios;
    }
    
    public bool CanExecute()
    {
        return cooldown <= 0;
    }

    public void Execute()
    {

        if (!CanExecute()) return;

        cooldown = maxCooldown;

        CoroutineRunner.Instance.StartCoroutine(PurifyEffect());
        
    }

    private IEnumerator PurifyEffect()
    {
        //Para limpiar effectos y curar
        var playerManager = DungeonGameManager.Instance.playerManager;

        yield return ApplySkillEffect();

        //Falta quitar in estado al player en caso de tener

        playerManager.Heal(Mathf.RoundToInt(playerManager.GetPlayer().MaxHealth / 10));

    }

    private IEnumerator ApplySkillEffect()
    {
        //Seleccionar todo el canva - Bien
        //Instanciar el objeto en el canva - Bien
        //Ponerle el audio, 1 o 2 piezas minimo - Bien
        //Difuminado lento
        //Destruir el objeto - Bien

        GameObject canvasGO = GameObject.Find("MainCanvas");

        var SFXobj = Instantiate(SkillSFX, canvasGO.transform);

        AudioManager.SharedInstance.PlaySound(audios[Random.Range(0, audios.Count)]);

        yield return new WaitForSeconds(0.5f);

        Destroy(SFXobj);
        
    }

    public void TryReduceSkillTimer(TimerReducer trigger)
    {
        if (trigger == _howReduceCooldown)
        {
            this.cooldown--;
        }
        
    }
}
public class NoSkill : ISkill
{
    public int SkillId => -1;
    public int WeaponId => -1;
    public int cooldown => -1;
    public int maxCooldown => -1;
    public bool CanExecute() => false;
    public void Execute() { }
    public void TryReduceSkillTimer(TimerReducer trigger) { }
}

public enum TimerReducer
{
    onKill,
    OnTileDiscover,
    onPickUpPotion
}