using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InfoUI : MonoBehaviour {

    [Header("Datos")]
    [SerializeField] private UserData DatosPlayer;

    [Header("Texto para los datos")]
    [SerializeField] private TMPro.TMP_Text level;
    [SerializeField] private TMPro.TMP_Text hp;
    [SerializeField] private TMPro.TMP_Text attack;
    [SerializeField] private TMPro.TMP_Text gold;
    [SerializeField] private TMPro.TMP_Text deepest;

    [Header("Level Up Pop Up")]
    [SerializeField] private GameObject PopupNoLevelUp;
    [SerializeField] private GameObject PopupLevelUp;
    [SerializeField] private Button LevelUpButton;
    [SerializeField] private Button AcceptConfirm;
    [SerializeField] private Button CancelLvlUpButton;
    [SerializeField] private Button AcceptLvlUpButton;
    [SerializeField] private TMPro.TMP_Text levelUpText;
    [SerializeField] private TMPro.TMP_Text goldred;
    [SerializeField] private TMPro.TMP_Text goldyellow;
    [SerializeField] private TMPro.TMP_Text requieredgold1;
    [SerializeField] private TMPro.TMP_Text requieredgold2;

    //Variables para saber si sube de nivel
    private int nextLvlRequirements; 
    private bool haveNextLvlRequirements;
    private MythicObject itemLookingAt;

    //Cambiar esto por una busqueda de la lista de PlayerPrefs del primer objeto con el bool isWeapon activo y incativo
    [Header("HardCoded initial items")]
    [SerializeField] private List<MythicObject> ItemsDefault = new List<MythicObject>();

    [Header("Icono de lo que esta equipado")]
    [SerializeField] private GameObject EquipedWeaponIcon;
    [SerializeField] private GameObject EquipedArtifactIcon;
    private MythicObject WeaponEquipedInMemory;
    private MythicObject ArtifactEquipedInMemory;

    [Header("Cambiar Ranuras Equipamiento")]
    [SerializeField] private Button Ranura1;
    [SerializeField] private Button Ranura2;
    [SerializeField] private Button Ranura3;
    [SerializeField] private Sprite SpriteNoSelected;
    [SerializeField] private Sprite SpriteSelected;

    [Header("Objetos Miticos")]
    [SerializeField] private TMPro.TMP_Text NombreObjeto;
    [SerializeField] private TMPro.TMP_Text DescripcionObjeto;
    [SerializeField] private GameObject Equip;
    [SerializeField] private Button EquipButton;

    [SerializeField] private GameObject ItemPrefab;
    [SerializeField] private GameObject InventoryPrinter;
    [SerializeField] private Button WeaponInventoryButton;
    [SerializeField] private Button ArtifactsInventoryButton;
    [SerializeField] private Sprite InventoryNotSelected;
    [SerializeField] private Sprite InventorySelected;

    void Start() {
        
        UpdateUI();

        LevelUpButton.onClick.AddListener(LevelUp);
        AcceptConfirm.onClick.AddListener(ClosePopUp);
        CancelLvlUpButton.onClick.AddListener(ClosePopUp);
        AcceptLvlUpButton.onClick.AddListener(TrueLevelUp);

        Ranura1.onClick.AddListener(() => GetSelectedBuild(1));
        Ranura2.onClick.AddListener(() => GetSelectedBuild(2));
        Ranura3.onClick.AddListener(() => GetSelectedBuild(3));

        WeaponInventoryButton.onClick.AddListener(() => GetSelectedInventory(1));
        ArtifactsInventoryButton.onClick.AddListener(() => GetSelectedInventory(2));

        EquipButton.GetComponent<Button>().onClick.AddListener(() => EquipItem());

        WeaponEquipedInMemory = DatosPlayer.TakeWeaponEquiped();
        EquipedWeaponIcon.GetComponent<Image>().sprite = WeaponEquipedInMemory.icon;

        ArtifactEquipedInMemory = DatosPlayer.TakeArtifactEquiped();
        EquipedArtifactIcon.GetComponent<Image>().sprite = ArtifactEquipedInMemory.icon;

        GetSelectedBuild(DatosPlayer.Build);

    }

    void UpdateUI() {

        hp.text = DatosPlayer.HP.ToString();
        level.text = DatosPlayer.Level.ToString();
        attack.text = DatosPlayer.Attack.ToString();
        gold.text = DatosPlayer.Gold.ToString();
        deepest.text = DatosPlayer.Deepest.ToString();

        nextLvlRequirements = (DatosPlayer.Level * 20) + (DatosPlayer.HP / 2);
        haveNextLvlRequirements = TextLevelUp();

        goldred.text = gold.text;
        goldyellow.text = gold.text;

        requieredgold1.text = nextLvlRequirements.ToString();
        requieredgold2.text = nextLvlRequirements.ToString();

        ClearInventoryItems();
        PrintMythicObjects();

    }

    private void ClearInventoryItems() {

        for (var i = InventoryPrinter.transform.childCount - 1; i >= 0; i--) {
            Object.Destroy(InventoryPrinter.transform.GetChild(i).gameObject);
        }

    }

    public bool TextLevelUp() {

        if (DatosPlayer.Gold >= nextLvlRequirements) {

            levelUpText.color = Color.green; 
            return true;

        } else {

            levelUpText.color = Color.red; 
            return false;

        }

    }

    private void LevelUp() {

        if (haveNextLvlRequirements) {

            PopupLevelUp.SetActive(true);

        } else {

            PopupNoLevelUp.SetActive(true);

        }

    }

    private void TrueLevelUp() {

        ++DatosPlayer.Level;
        DatosPlayer.HP += (DatosPlayer.HP / 10);
        
        if (DatosPlayer.Level < 10) {

            ++DatosPlayer.Attack;

        } else {

            DatosPlayer.Attack += (DatosPlayer.Level/5);

        }

        DatosPlayer.Gold -= nextLvlRequirements;
        
        UpdateUI();
        ClosePopUp();

    }

    private void ClosePopUp() {

        PopupLevelUp.SetActive(false);
        PopupNoLevelUp.SetActive(false);

    }

    private void GetSelectedBuild(int selected) {

        Ranura1.GetComponent<Image>().sprite = SpriteNoSelected;
        Ranura2.GetComponent<Image>().sprite = SpriteNoSelected;
        Ranura3.GetComponent<Image>().sprite = SpriteNoSelected;

        Ranura1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().color = Color.blue;
        Ranura2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().color = Color.blue;
        Ranura3.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().color = Color.blue;

        if (selected == 1) {

            Ranura1.GetComponent<Image>().sprite = SpriteSelected;
            Ranura1.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().color = Color.green;
            DatosPlayer.Build = 1;

        } else if (selected == 2) {

            Ranura2.GetComponent<Image>().sprite = SpriteSelected;
            Ranura2.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().color = Color.green;
            DatosPlayer.Build = 2;

        } else {

            Ranura3.GetComponent<Image>().sprite = SpriteSelected;
            Ranura3.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().color = Color.green;
            DatosPlayer.Build = 3;

        }

        GetSelectedInventory(3);
        ForcedEquipItem();

    }

    //Provando
    public void GetSelectedInventory(int selected) {
        
        if (selected == 1 && WeaponInventoryButton.GetComponent<Image>().sprite != InventorySelected) {

            WeaponInventoryButton.GetComponent<Image>().sprite = InventorySelected;
            ArtifactsInventoryButton.GetComponent<Image>().sprite = InventoryNotSelected;
            ClearInventoryItems();
            PrintMythicObjects();
            PrintObjectSelected(ItemsDefault[1]);

        } else if (selected == 2 && ArtifactsInventoryButton.GetComponent<Image>().sprite != InventorySelected) {

            ArtifactsInventoryButton.GetComponent<Image>().sprite = InventorySelected;
            WeaponInventoryButton.GetComponent<Image>().sprite = InventoryNotSelected;
            ClearInventoryItems();
            PrintMythicObjects();
            PrintObjectSelected(ItemsDefault[0]);

        } else if (selected == 3) {

            WeaponInventoryButton.GetComponent<Image>().sprite = InventorySelected;
            ArtifactsInventoryButton.GetComponent<Image>().sprite = InventoryNotSelected;
            ClearInventoryItems();
            PrintMythicObjects();
            PrintObjectSelected(ItemsDefault[1]);

        }

    }

    private void PrintMythicObjects() {

        if (WeaponInventoryButton.GetComponent<Image>().sprite == InventorySelected) {

            foreach (MythicObject item in DatosPlayer.Inventory) {

                if (item.isWeapon) {

                    PutItemInInventory(item);

                }

            }

        } else {

            foreach (MythicObject item in DatosPlayer.Inventory) {

                if (!item.isWeapon) {

                    PutItemInInventory(item);

                }

            }

        }

    }

    private void PutItemInInventory(MythicObject item) {

        var itemPrefabComplete = Instantiate(ItemPrefab, new Vector3(0,0,0), Quaternion.identity);
        itemPrefabComplete.GetComponent<ItemPrefabManager>().SetData(item);
        itemPrefabComplete.transform.SetParent(InventoryPrinter.transform);
        itemPrefabComplete.transform.localScale = new Vector3(1f,1f,1f);

    }

    public void PrintObjectSelected(MythicObject item) {

        itemLookingAt = item;

        StopAllCoroutines();
        StartCoroutine(PrintItemTextDescription());
        
        if (itemLookingAt.isWeapon) {

            if (DatosPlayer.WeaponEquiped == itemLookingAt.id) {

                Equip.SetActive(false);

            } else {

                Equip.SetActive(true);

            }

        } else {

            if (DatosPlayer.ArtifactEquiped == itemLookingAt.id) {

                Equip.SetActive(false);

            } else {

                Equip.SetActive(true);

            }

        }

    }

    IEnumerator PrintItemTextDescription() {

        NombreObjeto.text = itemLookingAt.nameObject;
        DescripcionObjeto.text = "";

        foreach(var character in itemLookingAt.description) {

            DescripcionObjeto.text += character;
            yield return new WaitForSeconds(1/DatosPlayer.TextSpeed);

        }

        yield return new WaitForSeconds(0.1f);

    }

    public void EquipItem() {

        Equip.SetActive(false);

        if (itemLookingAt.isWeapon) {

            DatosPlayer.WeaponEquiped = itemLookingAt.id;
            WeaponEquipedInMemory = DatosPlayer.TakeWeaponEquiped();
            EquipedWeaponIcon.GetComponent<Image>().sprite = WeaponEquipedInMemory.icon;

        } else {

            DatosPlayer.ArtifactEquiped = itemLookingAt.id;
            ArtifactEquipedInMemory = DatosPlayer.TakeArtifactEquiped();
            EquipedArtifactIcon.GetComponent<Image>().sprite = ArtifactEquipedInMemory.icon;

        }

    }

    public void ForcedEquipItem() {

        WeaponEquipedInMemory = DatosPlayer.TakeWeaponEquiped();
        EquipedWeaponIcon.GetComponent<Image>().sprite = WeaponEquipedInMemory.icon;

        ArtifactEquipedInMemory = DatosPlayer.TakeArtifactEquiped();
        EquipedArtifactIcon.GetComponent<Image>().sprite = ArtifactEquipedInMemory.icon;

        if (WeaponEquipedInMemory.id == 0) {

            Equip.SetActive(false);

        } else {

            Equip.SetActive(true);

        }

    }

}
