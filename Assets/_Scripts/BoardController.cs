using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using DG.Tweening;

public class BoardController : MonoBehaviour {

    [SerializeField] private GameObject canvaAnimation;
    [SerializeField] private GameObject imageAnimation;
    [SerializeField] private GameObject canvaGame;
    [SerializeField] private GameObject gridGO;
    [SerializeField] private GameObject enemyListLayer;

    [SerializeField] private UserData DatosPlayer;
    [SerializeField] private TMPro.TMP_Text PLayerAttackText;
    [SerializeField] private TMPro.TMP_Text PLayerHealthText;
    [SerializeField] private TMPro.TMP_Text PLayerMaxHealthText;
    [SerializeField] private TMPro.TMP_Text PLayerGoldText;
    [SerializeField] private TMPro.TMP_Text PLayerDepthText;

    [SerializeField] private GameObject GameOverPopup;
    [SerializeField] private GameObject NewPersonalRecord;
    [SerializeField] private GameObject ChestFound;

    private int boardSizeCol = 9;
    private int boardSizeFil = 7;

    [SerializeField] private Tilemap Items;
    [SerializeField] private Tilemap Top;
    [SerializeField] private Tilemap SuperTop;

    //Casilla bloqueada
    [SerializeField] private Tile BlockedTile;
    //Casilla libre
    [SerializeField] private Tile NormalTile;
    //Casilla libre
    [SerializeField] private Tile NormalTileBroked1;
    //Casilla libre
    [SerializeField] private Tile NormalTileBroked2;
    //Casilla libre
    [SerializeField] private Tile NormalTileBroked3;
    //Muro en el interior del mapa
    [SerializeField] private Tile WallTile;
    //Piso que se puede tocar
    [SerializeField] private Tile FloorTile;

    //Escalera bloqueada
    [SerializeField] private Tile StairKeyTile;
    //Ecalera que se empieza
    [SerializeField] private Tile StairTile;
    //Escalera bloqueada
    [SerializeField] private Tile BloquedStairTile;
    //Escalera bloqueada
    [SerializeField] private Tile EntranceTile;
    //Enemigo
    [SerializeField] private Tile EnemyTile;
    [SerializeField] private List<Sprite> EnemySprites;

    [SerializeField] private Tile PotionTile;
    [SerializeField] private Tile BigPotionTile;
    [SerializeField] private Tile SwordTile;
    [SerializeField] private Tile CoinTile;
    [SerializeField] private Tile ChestTile;

    //EnemyPrefab
    [SerializeField] public AttackDefenseView EnemyUIPrefab;
    [SerializeField] public EnemyPrefabListItem enemyPrefabListItem;

    //Button Exit
    [SerializeField] private Button exitMain;
    [SerializeField] private Button exitCancel;
    [SerializeField] private GameObject exitMainGameObject;
    [SerializeField] private GameObject exitPanel;
    bool _panelPopup;

    [SerializeField] private GameObject BuffHealth;
    [SerializeField] private GameObject BuffAtk;
    [SerializeField] private GameObject BuffCoins;

    [SerializeField] private Image bloodScreen;
    [SerializeField] private Image bloodScreenBounds;

    [SerializeField] private GameObject particlesPrefab;
    [SerializeField] private GameObject particlesContainer;
    [SerializeField] private GameObject InventoryItemsBox;

    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private GameObject skillLayer;

    Camera _camara;
    List<Cell> _board = new List<Cell>();
    List<AttackDefenseView> _enemyUi = new List<AttackDefenseView>();
    List<ItemType> _inventory = new List<ItemType>();
    int _weaponEquiped = 0;
    int _artifactEquiped = 0;
    int _floorsDeep = 0;
    int _playerHealth = 0;
    int _maxPlayerHealth = 0;
    int _playerAttack = 0;
    int _playerLuck = 0;
    float _maxBloodScreen = 0;
    bool isDoingCoroutine = false;
    bool isBreathing = true;
    private bool _WasGenerated = false;

    void Awake() {

        _camara = Camera.main;

    }

    void Start() {

        exitMain.onClick.AddListener(() => OpenExitConfirmation());
        exitCancel.onClick.AddListener(() => CancelExitConfirmation());

        _weaponEquiped = DatosPlayer.TakeWeaponEquiped().id;
        _artifactEquiped = DatosPlayer.TakeArtifactEquiped().id;
        _panelPopup = false;
        _playerAttack = DatosPlayer.Attack;
        _playerHealth = DatosPlayer.HP;
        _maxPlayerHealth = DatosPlayer.HP;
        PLayerMaxHealthText.text = "/ " + _maxPlayerHealth.ToString();
        _floorsDeep = 0;
        InventoryItemsBox.transform.GetChild(0).GetComponent<Image>().color = new Color(0.537255f, 0.537255f, 0.537255f, 0.313725f);
        InventoryItemsBox.transform.GetChild(1).GetComponent<Image>().color = new Color(0.537255f, 0.537255f, 0.537255f, 0.313725f);
        WeaponSkillsSetUpData();
        GenerateRandomMap();
        StartCoroutine(AnimationGameStart());

    }

    IEnumerator AnimationGameStart() {

        canvaGame.SetActive(false);
        gridGO.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        gridGO.SetActive(true);
        gridGO.transform.DOMove(new Vector3(4.42f, -20f, 0f), 0f);
        imageAnimation.transform.DOMoveY(canvaAnimation.GetComponent<RectTransform>().rect.height * 1.5f, 1.5f, false).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(0.5f);

        gridGO.transform.DOMove(new Vector3(4.42f, 4.5f, 0f), 1f).SetEase(Ease.OutQuad);

        yield return new WaitForSeconds(1f);

        canvaAnimation.SetActive(false);
        canvaGame.SetActive(true);

        //canvaGame.GetComponent<CanvasGroup>().DOFade(0f, 0f);
        //canvaGame.GetComponent<CanvasGroup>().DOFade(1f, 0.3f);
        //Hacer que la mazmorra caiga

    }

    void Update() {

        Vector3? touchedPos = GetTouchedPos();

        if (!touchedPos.HasValue) {

            return;

        }

        var pos = _camara.ScreenToWorldPoint(touchedPos.Value);
        //Debug.Log(pos.x + " " + pos.y);
        bool inBounds = pos.x > -0.5f && pos.x < boardSizeCol && pos.y > 1.5f && pos.y < boardSizeFil + 2;
        
        if (!inBounds) {

            return;

        }

        CellTouched(Mathf.FloorToInt(pos.x + 0.5f), Mathf.FloorToInt(pos.y + 0.5f));

    }

    void LateUpdate() {

        if (!isDoingCoroutine) {

            isDoingCoroutine = true;

            if (isBreathing) {

                StartCoroutine(GoToAlpha());

            } else {

                StartCoroutine(GoToAlpha());

            }

        }

    }

    IEnumerator GoToAlpha() {

        Color blood = bloodScreen.color;
        Color bounds = bloodScreenBounds.color;

        if (isBreathing) {

            blood.a = _maxBloodScreen - 0.01f;
            bounds.a = (_maxBloodScreen - 0.01f) * 1.8f;
            bloodScreen.color = blood;
            bloodScreenBounds.color = bounds;
            yield return new WaitForSeconds(0.8f);

        } else {

            blood.a += _maxBloodScreen;
            bounds.a += _maxBloodScreen * 1.8f;
            bloodScreen.color = blood;
            bloodScreenBounds.color = bounds;
            yield return new WaitForSeconds(0.8f);

        }

        isBreathing = bloodScreen.color.a>=_maxBloodScreen?true:false;
        //Debug.Log(isBreathing);

        isDoingCoroutine = false;
        yield return new WaitForSeconds(0.2f);

    }

    private void GenerateRandomMap() {

        _panelPopup = false;
        List<Cell> data = new List<Cell>();
        var oldExit = _board.Find(c => c.Item == ItemType.Exit);
        for (int y = 0; y < boardSizeFil; ++y) {

            for (int x = 0; x < boardSizeCol; ++x) {

                var cell = new Cell(x, y + 2);
                data.Add(cell);

                if (oldExit != null && oldExit.Pos.x == x && oldExit.Pos.y == y + 2) {

                    cell.Type = CellType.Entrance;

                }

            }

        }

        if (oldExit == null) {

            data[0].Type = CellType.Entrance;

        }

        GenerateRandomWalls(data, Random.Range(0, 5));
        GenerateRandomItem(data, ItemType.Exit, 1);
        GenerateRandomItem(data, ItemType.Key, 1);

        if (Random.Range(0, 100) > (97 - _floorsDeep - (_playerLuck * 0.2)) && !_WasGenerated) {

            GenerateRandomItem(data, ItemType.Chest, 1);
            _WasGenerated = true;

        }

        if (Random.Range(0, 100) > 80 - (_playerLuck * 2)) {

            GenerateRandomItem(data, ItemType.BigPotion, 1);
            GenerateRandomItem(data, ItemType.Potion, 1);

        } else {

            GenerateRandomItem(data, ItemType.Potion, 2);

        }

        GenerateRandomItem(data, ItemType.Coin, Random.Range(1, 4));

        if (Random.Range(0, 100) > 100 - (_playerLuck * 2)) {

            GenerateRandomItem(data, ItemType.Sword, 2);

        } else {

            GenerateRandomItem(data, ItemType.Sword, 1);

        }

        GenerateRandomEnemies(data, Random.Range(3, 8), 1, 3, 14, 22);
        Reload(data);

    }

    void GenerateRandomWalls(List<Cell> data, int amount) {

        for (int i = 0; i < amount; ++i) {

            var aviable = data.FindAll(c => c.Type == CellType.Empty && c.Item == ItemType.None);
            aviable[Random.Range(0, aviable.Count)].Type = CellType.Wall;

        }

    }

    void GenerateRandomItem(List<Cell> data, ItemType type, int amount) {

        for (int i = 0; i < amount; ++i) {

            var aviable = data.FindAll(c => c.Type == CellType.Empty && c.Item == ItemType.None);
            aviable[Random.Range(0, aviable.Count)].Item = type;

        }

    }

    void GenerateRandomEnemies(List<Cell> data, int amount, int attackMin, int attackMax, int healthMin, int healthMax) {

        //Comprobar dificultad
        attackMin += (int)(_floorsDeep * 1.5);
        attackMax += (int)(_floorsDeep * 1.5);
        healthMin += (int)(_floorsDeep * 2.5);
        healthMax += (int)(_floorsDeep * 3);

        for (int i = 0; i < amount; ++i) {

            //Poner longitud real
            int TypeOfEnemy = Random.Range(0, 33);

            while (TypeOfEnemy == 8 || TypeOfEnemy == 9 || TypeOfEnemy == 15 || TypeOfEnemy == 32) {

                TypeOfEnemy = Random.Range(0, 33);

            }

            var aviable = data.FindAll(c => c.Type == CellType.Empty && c.Item != ItemType.Exit);
            //Debug.Log(TypeOfEnemy);
            aviable[Random.Range(0, aviable.Count)].Enemy = new Enemy(Random.Range(attackMin, attackMax), Random.Range(healthMin, healthMax), EnemySprites[TypeOfEnemy], TypeOfEnemy);

        }

    }

    void Reload(List<Cell> levelData) {

        ++_floorsDeep;
        if (DatosPlayer.Deepest < _floorsDeep) {

            DatosPlayer.Deepest = _floorsDeep;

        }

        _inventory.Clear();
        InventoryItemsBox.transform.GetChild(0).GetComponent<Image>().color = new Color(0.537255f, 0.537255f, 0.537255f, 0.313725f);
        _board.Clear();
        _board.AddRange(levelData);
        foreach (var c in levelData) {

            SetCellView(c);

        }

        UpdateUI();
        foreach (var eui in _enemyUi) {

            eui.gameObject.SetActive(false);
            eui.BoardPos = new Vector2Int(-1, -1);

        }

        foreach (Transform child in enemyListLayer.transform) {

            Destroy(child.gameObject);
                
        }

    }

    void UpdateUI() {

        PLayerAttackText.text = _playerAttack.ToString();
        PLayerHealthText.text = _playerHealth.ToString();
        PLayerGoldText.text = DatosPlayer.Gold.ToString();
        PLayerDepthText.text = _floorsDeep.ToString();

    }

    void SetCellView(Cell cell) {

        switch(cell.Type) {

            case CellType.Empty:

                if (cell.isRevelated) {

                    Top.SetTile(Top.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), null);
                    

                } else if (cell.Locks > 0) {

                    SuperTop.SetTile(SuperTop.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), BlockedTile);

                } else {

                    int tileRandom = Random.Range(0, 100);
                    int quaternionRandom = Random.Range(0, 4);
                    var tile = NormalTile;
                    Quaternion quaternion = Quaternion.Euler(0, 0, quaternionRandom * 90);

                    if (tileRandom > 30) {

                        tile = NormalTile;

                    } else if (tileRandom > 20) {

                        tile = NormalTileBroked1;

                    } else if (tileRandom > 10) {

                        tile = NormalTileBroked2;

                    } else {

                        tile = NormalTileBroked3;

                    }

                    tile.transform = Matrix4x4.TRS(Vector3.zero, quaternion, Vector3.one);
                    Top.SetTile(Top.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), tile);
                    SuperTop.SetTile(SuperTop.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), null);

                }
                break;

            case CellType.Entrance:

                Top.SetTile(Top.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), EntranceTile);
                break;

            case CellType.Wall:

                SuperTop.SetTile(SuperTop.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), null);
                Top.SetTile(Top.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), WallTile);
                break;

        }

        if (cell.Enemy != null) {

            Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), EnemyTile);

        }

        switch(cell.Item) {

            case ItemType.Key:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), StairKeyTile);
                break;
            case ItemType.Potion:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), PotionTile);
                break;
            case ItemType.Exit:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), _inventory.Contains(ItemType.Key)?StairTile:BloquedStairTile);
                break;
            case ItemType.Sword:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), SwordTile);
                break;
            case ItemType.Coin:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), CoinTile);
                break;
            case ItemType.BigPotion:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), BigPotionTile);
                break;
            case ItemType.Chest:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), ChestTile);
                break;
            case ItemType.None:
                Items.SetTile(Items.WorldToCell(new Vector3(cell.Pos.x, cell.Pos.y)), null);
                break;

        }

    }

    Vector3? GetTouchedPos() {

        if (Input.GetMouseButtonDown(0)) {

            return Input.mousePosition;

        }
        return null;

    }

    bool CanTouchCell(Cell cell) {

        if (_playerHealth < 1 || cell.Locks > 0 || cell.Type == CellType.Wall || _panelPopup) return false;
        var neighbours = GetNeighbours(cell.Pos.x, cell.Pos.y);
        var adjacents = neighbours.FindAll(c => c.Pos.x == cell.Pos.x || c.Pos.y == cell.Pos.y);
        var available = adjacents.FindAll(c => c.isRevelated || c.Type == CellType.Entrance);
        return available.Count > 0;

    }

    void CellTouched(int x, int y) {

        var cell = _board.Find(c => c.Pos.x == x && c.Pos.y == y);
        if (cell == null || !CanTouchCell(cell)) return;
        if (!cell.isRevelated) {

            cell.isRevelated = true;
            
            if (cell.Enemy != null) {

                var neighbours = GetNeighbours(x, y);
                foreach (var n in neighbours) {

                    if (!n.isRevelated && n.Type != CellType.Wall) {

                        n.Locks++;
                        SetCellView(n);

                    }

                }

                var ui = GetEnemyUI(x, y);
                ui.SetData(cell.Enemy.Attack, cell.Enemy.Health, cell.Enemy.EnemySprite, false);
                var enemyElement = Instantiate(enemyPrefabListItem, enemyListLayer.transform);
                enemyElement.SetData(ui, x, y, -1, haveArmorShield(cell));

            }

            SetCellView(cell);

            /*var alrededor = GetNeighbours(x, y);
            var adjacents = alrededor.FindAll(c => c.Pos.x == cell.Pos.x || c.Pos.y == cell.Pos.y);
            var availableToPrint = adjacents.FindAll(c => !c.isRevelated && c.Type != CellType.Entrance && c.Type != CellType.Wall && c.Locks == 0);

            if (availableToPrint.Count > 0) {

                foreach (var p in availableToPrint) {

                    particlesContainer = Instantiate(particlesPrefab, new Vector3(p.Pos.x, p.Pos.y, 0), new Quaternion(180, 0, 0, 1));

                }

            }*/

            

        } else if (cell.Enemy != null) {

            if (cell.Enemy.States.Where(s => s == StateOfCharacter.Freezed).Count() >= 1 && !cell.Enemy.WasFreezed) {

                cell.Enemy.WasFreezed = true;

            } else {

                int hitForPlayer = DamageForPlayer(cell);

                if (_weaponEquiped == 19) {

                    hitForPlayer = Mathf.Clamp(hitForPlayer - 2, 0, 9999);

                }

                if (_playerHealth - hitForPlayer < 0) {

                    BuffHealth.GetComponent<BuffBenefits>().SetData(-_playerHealth);

                } else {

                    BuffHealth.GetComponent<BuffBenefits>().SetData(-hitForPlayer);

                }

                _playerHealth = Mathf.Max(0, _playerHealth - hitForPlayer);

            }
            cell.Enemy.Health = Mathf.Max(0, cell.Enemy.Health - DamageForEnemy(cell));
            UpdateUI();
            var ui = GetEnemyUI(x, y);
            ui.SetData(cell.Enemy.Attack, cell.Enemy.Health, cell.Enemy.EnemySprite, true);

            foreach (Transform child in enemyListLayer.transform) {

                if (child.gameObject.GetComponent<EnemyPrefabListItem>().x == x && child.gameObject.GetComponent<EnemyPrefabListItem>().y == y){

                    child.gameObject.GetComponent<EnemyPrefabListItem>().SetData(ui, x, y, (cell.Enemy.States.Count == 0?-1:(int)cell.Enemy.States.First()), haveArmorShield(cell));
                
                }
                
            }

            if (_playerHealth <= 0) {

                GameOverPopup.SetActive(true);
                exitMainGameObject.SetActive(false);
                return;

            } else if (cell.Enemy.Health <= 0) {

                cell.Enemy.Health = 0;
                cell.Enemy = null;
                SetCellView(cell);

                var neighbours = GetNeighbours(x, y);
                foreach (var n in neighbours) {

                    if (!n.isRevelated) {

                        n.Locks = Mathf.Max(0, n.Locks - 1);
                        SetCellView(n);

                    }

                }

            }

            StopAllCoroutines();
            _maxBloodScreen = Mathf.Abs(((float)_playerHealth / (float)_maxPlayerHealth) - 1f)/20;
            Color color = bloodScreen.color;
            color.a = _maxBloodScreen;
            bloodScreen.color = color;
            color.a = _maxBloodScreen * 1.8f;
            bloodScreenBounds.color = color;
            isDoingCoroutine = false;

        } else {

            PickupItem(cell);

        }
        
    }

    List<Cell> GetNeighbours(int x, int y) {

        List<Vector2Int> positions = new List<Vector2Int>();

        //Adyacentes
        if (x > 0) {

            positions.Add(new Vector2Int(x - 1, y));

        }

        if (y > 2) {

            positions.Add(new Vector2Int(x, y - 1));

        }

        if (x < boardSizeCol - 1) {

            positions.Add(new Vector2Int(x + 1, y));

        }

        if (y < boardSizeFil + 1) {

            positions.Add(new Vector2Int(x, y + 1));

        }

        //Diagonales

        if (x > 0 && y > 2) {

            positions.Add(new Vector2Int(x - 1, y - 1));

        }

        if (x < boardSizeCol - 1 && y < boardSizeFil + 1) {

            positions.Add(new Vector2Int(x + 1, y + 1));

        }

        if (x < boardSizeCol - 1 && y > 2) {

            positions.Add(new Vector2Int(x + 1, y - 1));

        }

        if (x > 0 && y < boardSizeFil + 1) {

            positions.Add(new Vector2Int(x - 1, y + 1));

        }

        return _board.FindAll(c => positions.Contains(c.Pos));

    }

    AttackDefenseView GetEnemyUI(int x, int y) {

        var retVal = _enemyUi.Find(e => e.BoardPos.x == x && e.BoardPos.y == y);

        if (retVal != null) {

            retVal.transform.position = new Vector3(x - 0.1f, y, 0);
            return retVal;

        }

        retVal = _enemyUi.Find(e => e.BoardPos.x == -1 && e.BoardPos.y == -1);

        if (retVal != null) {

            retVal.gameObject.SetActive(true);
            retVal.transform.position = new Vector3(x - 0.1f, y, 0);
            retVal.BoardPos = new Vector2Int(x, y);
            return retVal;

        }

        //Comprobar
        retVal = Instantiate(EnemyUIPrefab, new Vector3(x - 0.1f, y, 0f), Quaternion.identity).GetComponent<AttackDefenseView>();
        retVal.BoardPos = new Vector2Int(x, y);
        _enemyUi.Add(retVal);
        return retVal;

    }

    void PickupItem(Cell cell) {

        switch (cell.Item) {

            case ItemType.Key:

                cell.Item = ItemType.None;
                _inventory.Add(ItemType.Key);
                SetCellView(cell);
                InventoryItemsBox.transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                var exit = _board.Find(c => c.Item == ItemType.Exit);
                if (exit != null) {

                    Items.SetTile(Items.WorldToCell(new Vector3(exit.Pos.x, exit.Pos.y, 0f)), StairTile);

                }
                break;

            case ItemType.Exit:
                if (_inventory.Contains(ItemType.Key)) {

                    GenerateRandomMap();

                }
                break;

            case ItemType.Potion:
            //Poner limitante
                if (_playerHealth != _maxPlayerHealth) {
                    
                    int cure = 7 + Random.Range(0 + _playerLuck, 5 + (int)(_playerLuck * 1.2));

                    if (_playerHealth + cure > _maxPlayerHealth) {

                        BuffHealth.GetComponent<BuffBenefits>().SetData(_maxPlayerHealth - _playerHealth);
                        _playerHealth = _maxPlayerHealth;

                    } else {

                        _playerHealth += cure;
                        BuffHealth.GetComponent<BuffBenefits>().SetData(cure);

                    }

                    UpdateUI();
                    cell.Item = ItemType.None;
                    SetCellView(cell);

                    StopAllCoroutines();
                    _maxBloodScreen = Mathf.Abs(((float)_playerHealth / (float)_maxPlayerHealth) - 1f)/20;
                    Color color = bloodScreen.color;
                    color.a = _maxBloodScreen;
                    bloodScreen.color = color;
                    color.a = _maxBloodScreen * 1.8f;
                    bloodScreenBounds.color = color;
                    isDoingCoroutine = false;
                }
                break;
            
            case ItemType.BigPotion:
            //Poner limitante
                if (_playerHealth != _maxPlayerHealth) {

                    int cure = 16 + Mathf.FloorToInt(Random.Range(0 + _playerLuck, 8 + _playerLuck));

                    if (_playerHealth + cure > _maxPlayerHealth) {

                        BuffHealth.GetComponent<BuffBenefits>().SetData(_maxPlayerHealth - _playerHealth);
                        _playerHealth = _maxPlayerHealth;

                    } else {

                        _playerHealth += cure;
                        BuffHealth.GetComponent<BuffBenefits>().SetData(cure);

                    }

                    UpdateUI();
                    cell.Item = ItemType.None;
                    SetCellView(cell);

                    StopAllCoroutines();
                    _maxBloodScreen = Mathf.Abs(((float)_playerHealth / (float)_maxPlayerHealth) - 1f)/20;
                    Color color = bloodScreen.color;
                    color.a = _maxBloodScreen;
                    bloodScreen.color = color;
                    color.a = _maxBloodScreen * 1.8f;
                    bloodScreenBounds.color = color;
                    isDoingCoroutine = false;

                }
                break;

            case ItemType.Sword:
            //Poner limitante
                int addAtk = Random.Range(0, 80 + _playerLuck)>70?2:1;
                _playerAttack += addAtk;
                BuffAtk.GetComponent<BuffBenefits>().SetData(addAtk);
                UpdateUI();
                cell.Item = ItemType.None;
                SetCellView(cell);
                break;

            case ItemType.Coin:
            //Poner limitante
                int gold = Random.Range(1, 3) + _floorsDeep + Random.Range(0, (int)(_playerLuck * 0.2));

                if (DatosPlayer.Gold + gold > 9999) {

                    DatosPlayer.Gold = 9999;

                } else {

                    DatosPlayer.Gold += gold;
                    BuffCoins.GetComponent<BuffBenefits>().SetData(gold);

                }
                
                UpdateUI();
                cell.Item = ItemType.None;
                SetCellView(cell);
                break; 
            
            case ItemType.Chest:

                if (DatosPlayer.CanGetItemFromGame()) {

                    DatosPlayer.GetItemFromChest();

                } else {

                    DatosPlayer.Gold += 100;
                    BuffCoins.GetComponent<BuffBenefits>().SetData(100);


                } 
                UpdateUI();
                cell.Item = ItemType.None;
                SetCellView(cell);
                InventoryItemsBox.transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
                break; 


        }

    }

    private void OpenExitConfirmation() {

        exitPanel.SetActive(true);
        exitMainGameObject.SetActive(false);
        _panelPopup = true;

    }

    private void CancelExitConfirmation() {

        exitPanel.SetActive(false);
        exitMainGameObject.SetActive(true);
        _panelPopup = false;

    }

    //Skills Scripting

    private void WeaponSkillsSetUpData() {

        int[] skillsToPrint = new int[2];

        switch(_weaponEquiped) {

            case 5:
            //Rayo Solar
                skillsToPrint[0] = 0;
                skillsToPrint[1] = -1;
                break;
            case 9:
            //3 golpes en cadena night abyss
                skillsToPrint[0] = 1;
                skillsToPrint[1] = -1;
                break;
            case 10:
            //Remueve nerfeos
                skillsToPrint[0] = 2;
                skillsToPrint[1] = -1;
                break;
            case 12:
            //Golpe más potente, si mata al instante reset
                skillsToPrint[0] = 3;
                skillsToPrint[1] = -1;
                break;
            case 14:
            //Quema la habitacion entera
                skillsToPrint[0] = 4;
                skillsToPrint[1] = -1;
                break;
            case 15:
            //Rompe el techo y tira piedras
                skillsToPrint[0] = 5;
                skillsToPrint[1] = -1;
                break;
            case 19:
            //Super golpe
                skillsToPrint[0] = 6;
                skillsToPrint[1] = -1;
                break;
            case 17:
            //Revisar ya que sera el más random y puede dar problemas
            //Imagenes de ejemplo solo
                skillsToPrint[0] = 7;
                skillsToPrint[1] = 8;
                break;
            default:
                skillsToPrint[0] = -1;
                skillsToPrint[1] = -1;
                break;

        }

        if (skillsToPrint[0] != -1) {

            var newPrefabSkill = Instantiate(skillPrefab, skillLayer.transform);
            newPrefabSkill.GetComponent<Skill>().SetSkillData(_weaponEquiped, skillsToPrint[0], 4);

        }

        if (skillsToPrint[1] != -1) {

            var newPrefabSkill = Instantiate(skillPrefab, skillLayer.transform);
            newPrefabSkill.GetComponent<Skill>().SetSkillData(_weaponEquiped, skillsToPrint[1], 4);

        }

    }

    int breakArmor = 0;
    bool lastHitKillDemon = false;

    private int DamageForEnemy(Cell cell) {

        //Debug.Log(DamageWithWeapon(cell));
        Debug.Log(DamageForStatus(cell));
        Debug.Log(cell.Enemy.TypeOfEnemy);
        //Debug.Log(breakArmor);

        int DamageValue = Mathf.FloorToInt(Mathf.FloorToInt(_playerAttack * DamageWithWeapon(cell)) + DamageForStatus(cell)) - (haveArmorShield(cell)?Mathf.Clamp(Mathf.FloorToInt(cell.Enemy.Health * 0.1f) - breakArmor, 0, 9999):0);

        //Curses Bane pasive
        if (DamageValue >= cell.Enemy.Health && cell.Enemy.TypeOfEnemy == Specie.Undead && _weaponEquiped == 6) {

            int cure = Mathf.FloorToInt(_maxPlayerHealth * 0.1f);

            if (_playerHealth + cure > _maxPlayerHealth) {

                BuffHealth.GetComponent<BuffBenefits>().SetData(_maxPlayerHealth - _playerHealth);
                _playerHealth = _maxPlayerHealth;

            } else {

                _playerHealth += cure;
                BuffHealth.GetComponent<BuffBenefits>().SetData(cure);

            }

        }

        if (DamageValue >= cell.Enemy.Health && cell.Enemy.TypeOfEnemy == Specie.Demon && _weaponEquiped == 11) {

            lastHitKillDemon = true;

        }

        return DamageValue;

    }

    private int DamageForPlayer(Cell cell) {

        int cure = 0;

        if (0 >= cell.Enemy.Health && cell.Enemy.TypeOfEnemy == Specie.Undead && _weaponEquiped == 6) {

            cure = Mathf.FloorToInt(_maxPlayerHealth * 0.1f);

        }

        //Acabar de hacer
        if (_weaponEquiped == 16) {

            return Mathf.FloorToInt(cell.Enemy.Attack * 1.5f) - cure;

        } else if (_weaponEquiped == 19) {

            return Mathf.Clamp(cell.Enemy.Attack - 2, 1, 9999) - cure;

        } else {

            return cell.Enemy.Attack - cure;

        }

    }

    private float DamageWithWeapon(Cell cell) {

        //TODO
        // Duelista
        // Rose Katana
        // Creator Sword
        // Pochita
        // Royal Edge

        switch (_weaponEquiped) {

            case 1:
                
                cell.Enemy.TryToAddState(StateOfCharacter.Fired);
                break;
            
            case 2:

                cell.Enemy.TryToAddState(StateOfCharacter.Freezed);
                break;

            case 3:

                cell.Enemy.TryToAddState(StateOfCharacter.Poisoned);
                break;

            case 6:

                if (cell.Enemy.TypeOfEnemy == Specie.Undead) {

                    return 1.5f;

                }

                break;
            
            case 7:

                if (cell.Enemy.States.Count == 0) {

                    cell.Enemy.TryToAddState((StateOfCharacter)Random.Range(0, 5));

                }                

                break;

            case 8:

                cell.Enemy.TryToAddState(StateOfCharacter.Blessing);

                if (cell.Enemy.TypeOfEnemy == Specie.Demon) {

                    return 1.4f;

                }

                break;

            case 10:

                return 1.25f;

            case 11:

                breakArmor = Random.Range(0, 3);

                if (lastHitKillDemon) {

                    lastHitKillDemon = false;
                    return 2f;

                }
                
                break;

            case 12:

                // Si tiene la habilidad activada, el golpe hace el 1.2f
                // Si no mata el enemigo con la habilidad activa, hay que ponerlo en cooldown
                //return 1.2f;
                break;

            case 13:

                cell.Enemy.TryToAddState(StateOfCharacter.Fired);
                cell.Enemy.TryToAddState(StateOfCharacter.Freezed);

                break;

            case 15:

                if (cell.Enemy.TypeOfEnemy == Specie.Beast) {

                    return 1.4f;

                }

                break;

            case 16:

                return 1.75f;

            case 17:

                return 1.1f;

            case 18:

                cell.Enemy.TryToAddState(StateOfCharacter.Bleeding);
                break;

            case 19:

                //return 1.5f;
                break;

        }

        return 1f;

    }

    private int DamageForStatus(Cell cell) {

        int acomulatetStateDamage = 0;

        if (cell.Enemy.States.Count(state => state == StateOfCharacter.Fired) != 0) {

            acomulatetStateDamage += 2;

        }

        if (cell.Enemy.States.Count(state => state == StateOfCharacter.Freezed) != 0) {

            acomulatetStateDamage += 1;

        }

        if (cell.Enemy.States.Count(state => state == StateOfCharacter.Poisoned) != 0) {

            acomulatetStateDamage += 2;

        }

        if (cell.Enemy.States.Count(state => state == StateOfCharacter.Blessing) != 0) {

            acomulatetStateDamage += 3;
            
        }

        if (cell.Enemy.States.Count(state => state == StateOfCharacter.Cursed) != 0) {

            acomulatetStateDamage += 2;

        }

        if (cell.Enemy.States.Count(state => state == StateOfCharacter.Bleeding) != 0) {

            acomulatetStateDamage += 2;

        }

        return acomulatetStateDamage;

    }


    bool haveArmorShield(Cell cell) {

        if ((cell.Enemy.SpecificEnemy >= 8) && (cell.Enemy.SpecificEnemy <= 10) || cell.Enemy.SpecificEnemy == 30 || cell.Enemy.SpecificEnemy == 32) {

            return true;

        } else {

            return false;

        }

    }

    

}
