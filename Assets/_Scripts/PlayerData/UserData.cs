using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class UserData : MonoBehaviour {

    public static UserData SharedInstance;
    
    int _newGame = -1;
    int _charSprite = -1;
    int _level = 0;
    int _attack = 0;
    int _hp = 0;
    int _gold = -1;
    int _deepest = -1;
    int _textSpeed = -1;
    float _musicVolume = -1f;
    float _fbxVolume = -1f;

    int _buildSelected = 1;
    int _weaponEquiped = 0;
    int _artifactEquiped = 0;

    string _username = "";
    int _sceneToGo = -1;

    [SerializeField] private List<MythicObject> InitialItems = new List<MythicObject>();
    [SerializeField] private List<MythicObject> ToGetInGameItems = new List<MythicObject>();
    [SerializeField] private List<MythicObject> ArchivementsItems = new List<MythicObject>();
    List<MythicObject> _inventory;

    public int NewGame {

        get {

            if (_newGame < 0) {

                if (!PlayerPrefs.HasKey("USER_NEWGAME")) {

                    NewGame = 0;

                }

                _newGame = PlayerPrefs.GetInt("USER_NEWGAME");

            }

            return _newGame;

        } set {

            _newGame = 1;
            PlayerPrefs.SetInt("USER_NEWGAME", _newGame);

        }

    }

    public string Username {

        get {

            if (_username == "") {

                if (!PlayerPrefs.HasKey("USER_USERNAME")) {

                    Username = "";

                }

                _username = PlayerPrefs.GetString("USER_USERNAME");

            }

            return _username;

        } set {

            _username = value;
            PlayerPrefs.SetString("USER_USERNAME", _username);

        }

    }

    public int CharSprite {

        get {

            if (_charSprite < 0) {

                if (!PlayerPrefs.HasKey("USER_CHAR")) {

                    CharSprite = Random.Range(0, 9);

                }

                _charSprite = PlayerPrefs.GetInt("USER_CHAR");

            }

            return _charSprite;

        } set {

            _charSprite = value;
            PlayerPrefs.SetInt("USER_CHAR", _charSprite);

        }

    }

    public int Level {

        get {

            if (_level < 1) {

                if (!PlayerPrefs.HasKey("USER_LEVEL")) {

                    Level = 1;

                }

                _level = PlayerPrefs.GetInt("USER_LEVEL");

            }

            return _level;

        } set {

            _level = value;
            PlayerPrefs.SetInt("USER_LEVEL", _level);

        }

    }

    public int Attack {

        get {

            if (_attack < 1) {

                if (!PlayerPrefs.HasKey("USER_ATTACK")) {

                    Attack = 5;

                }

                _attack = PlayerPrefs.GetInt("USER_ATTACK");

            }

            return _attack;

        } set {

            _attack = value;
            PlayerPrefs.SetInt("USER_ATTACK", _attack);

        }

    }

    public int HP {

        get {

            if (_hp < 1) {

                if (!PlayerPrefs.HasKey("USER_HP")) {

                    HP = 30;

                }

                _hp = PlayerPrefs.GetInt("USER_HP");

            }

            return _hp;

        } set {

            _hp = value;
            PlayerPrefs.SetInt("USER_HP", _hp);

        }

    }

    public int Gold {

        get {

            if (_gold < 0) {

                if (!PlayerPrefs.HasKey("USER_GOLD")) {

                    Gold = 0;

                }

                _gold = PlayerPrefs.GetInt("USER_GOLD");

            }

            return _gold;

        } set {

            _gold = value;
            PlayerPrefs.SetInt("USER_GOLD", _gold);

        }

    }

    public int Deepest {

        get {

            if (_deepest < 0) {

                if (!PlayerPrefs.HasKey("USER_DEEPEST")) {

                    Deepest = 0;

                }

                _deepest = PlayerPrefs.GetInt("USER_DEEPEST");

            }

            return _deepest;

        } set {

            _deepest = value;
            PlayerPrefs.SetInt("USER_DEEPEST", _deepest);

        }

    }

    public int TextSpeed {

        get {

            if (_textSpeed < 1) {

                if (!PlayerPrefs.HasKey("USER_TEXT_SPEED")) {

                    TextSpeed = 100;

                }

                _textSpeed = PlayerPrefs.GetInt("USER_TEXT_SPEED");

            }

            return _textSpeed;

        } set {

            _textSpeed = value;
            PlayerPrefs.SetInt("USER_TEXT_SPEED", _textSpeed);

        }

    }

    public int SceneToGo {

        get {

            if (_sceneToGo <= -1) {

                SceneToGo = 1;

            }

            return _sceneToGo;

        } set {

            _sceneToGo = value;

        }

    }

    public float MusicVolume {

        get {

            if (_musicVolume < 0) {

                if (!PlayerPrefs.HasKey("USER_MUSIC")) {

                    MusicVolume = 100f;

                }

                _musicVolume = PlayerPrefs.GetFloat("USER_MUSIC");

            }

            return _musicVolume;

        } set {

            _musicVolume = value;
            PlayerPrefs.SetFloat("USER_MUSIC", _musicVolume);

        }

    }

    public float FbxVolume {

        get {

            if (_fbxVolume < 0) {

                if (!PlayerPrefs.HasKey("USER_FBX")) {

                    FbxVolume = 100f;

                }

                _fbxVolume = PlayerPrefs.GetFloat("USER_FBX");

            }

            return _fbxVolume;

        } set {

            _fbxVolume = value;
            PlayerPrefs.SetFloat("USER_FBX", _fbxVolume);

        }

    }

    public int Build {

        get {

            if (_buildSelected < 0) {

                if (!PlayerPrefs.HasKey("USER_BUILD")) {

                    Build = 1;

                }

                _buildSelected = PlayerPrefs.GetInt("USER_BUILD");

            }

            return _buildSelected;

        } set {

            _buildSelected = value;
            PlayerPrefs.SetInt("USER_BUILD", _buildSelected);

        }

    }

    public int WeaponEquiped {

        get {

            if (_weaponEquiped < 0) {

                if (!PlayerPrefs.HasKey("USER_WEAPON_1")) {

                    WeaponEquiped = 0;
                    PlayerPrefs.SetInt("USER_WEAPON_2", 0);
                    PlayerPrefs.SetInt("USER_WEAPON_3", 0);

                }

                _weaponEquiped = PlayerPrefs.GetInt("USER_WEAPON_1");

            }

            if (Build == 1) {

                _weaponEquiped = PlayerPrefs.GetInt("USER_WEAPON_1");

            } else if (Build == 2) {

                _weaponEquiped = PlayerPrefs.GetInt("USER_WEAPON_2");

            } else {

                _weaponEquiped = PlayerPrefs.GetInt("USER_WEAPON_3");

            }

            return _weaponEquiped;

        } set {

            _weaponEquiped = value;

            if (Build == 1) {

                PlayerPrefs.SetInt("USER_WEAPON_1", _weaponEquiped);

            } else if (Build == 2) {

                PlayerPrefs.SetInt("USER_WEAPON_2", _weaponEquiped);

            } else {

                PlayerPrefs.SetInt("USER_WEAPON_3", _weaponEquiped);

            }
            

        }

    }

    public int ArtifactEquiped {

        get {

            if (_artifactEquiped < 0) {

                if (!PlayerPrefs.HasKey("USER_ARTIFACT_1")) {

                    ArtifactEquiped = 0;
                    PlayerPrefs.SetInt("USER_ARTIFACT_2", 0);
                    PlayerPrefs.SetInt("USER_ARTIFACT_3", 0);

                }

                _artifactEquiped = PlayerPrefs.GetInt("USER_ARTIFACT_1");

            }

            if (Build == 1) {

                _artifactEquiped = PlayerPrefs.GetInt("USER_ARTIFACT_1");

            } else if (Build == 2) {

                _artifactEquiped = PlayerPrefs.GetInt("USER_ARTIFACT_2");

            } else {

                _artifactEquiped = PlayerPrefs.GetInt("USER_ARTIFACT_3");

            }

            return _artifactEquiped;

        } set {

            _artifactEquiped = value;

            if (Build == 1) {

                PlayerPrefs.SetInt("USER_ARTIFACT_1", _artifactEquiped);

            } else if (Build == 2) {

                PlayerPrefs.SetInt("USER_ARTIFACT_2", _artifactEquiped);

            } else {

                PlayerPrefs.SetInt("USER_ARTIFACT_3", _artifactEquiped);

            }
            

        }

    }

    public List<MythicObject> Inventory {

        get {

            if (!PlayerPrefs.HasKey("USER_INVENTORY")) {

                _inventory = new List<MythicObject>();

                foreach (MythicObject item in InitialItems) {

                    AddToInventary(item);

                }

            }

            return PlayerPrefsExtra.GetList<MythicObject>("USER_INVENTORY", new List<MythicObject>());

        } set {

            PlayerPrefsExtra.SetList("USER_INVENTORY", _inventory);

        }

    }

    private void OnEnable() {

        if (UserData.SharedInstance == null) {

            UserData.SharedInstance = this;

        } else {

            Destroy(this.gameObject);

        }

        DontDestroyOnLoad(this.gameObject);

        _inventory = null;
        _weaponEquiped = -1;
        _artifactEquiped = -1;
        _buildSelected = -1;
        _level = -1;
        _username = "";
        _charSprite = -1;
        _attack = -1;
        _hp = -1;
        _gold = -1;
        _deepest = -1;
        _sceneToGo = -1;
        _textSpeed = -1;
        _musicVolume = -1f;
        _fbxVolume = -1f;

    }

    void Start() {

        _inventory = Inventory;
        _weaponEquiped = WeaponEquiped;
        _artifactEquiped = ArtifactEquiped;
        _buildSelected = Build;
        _level = Level;
        _username = Username;
        _charSprite = CharSprite;
        _attack = Attack;
        _hp = HP;
        _gold = Gold;
        _deepest = Deepest;
        _sceneToGo = SceneToGo;
        _textSpeed = TextSpeed;
        _musicVolume = MusicVolume;
        _fbxVolume = FbxVolume;

    }

    public void AddToInventary(MythicObject item) {

        _inventory.Add(item);
        PlayerPrefsExtra.SetList("USER_INVENTORY", _inventory);

    }

    public MythicObject TakeWeaponEquiped() {

        foreach (MythicObject item in Inventory) {

            if (item.isWeapon && item.id == WeaponEquiped) {

                return item;

            }

        }

        return null;

    }

    public MythicObject TakeArtifactEquiped() {

        foreach (MythicObject item in Inventory) {

            if (!item.isWeapon && item.id == ArtifactEquiped) {

                return item;

            }

        }

        return null;

    }

    public bool CanGetItemFromGame() {

        List<MythicObject> NotInInventory = ToGetInGameItems.Except(Inventory).ToList();

        if (NotInInventory.Count > 0) {

            return true;

        }

        return false;

    }

    public void GetItemFromChest() {

        List<MythicObject> NotInInventory = ToGetInGameItems.Except(Inventory).ToList();

        Debug.Log(NotInInventory.Count);

        int random = Random.Range(0, NotInInventory.Count);

        Debug.Log("Random: " + random);
        Debug.Log(NotInInventory[random]);

        _inventory = Inventory;
        _inventory.Add(NotInInventory[random]);
        PlayerPrefsExtra.SetList("USER_INVENTORY", _inventory);

    }

}
