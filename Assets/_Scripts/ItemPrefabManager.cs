using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class ItemPrefabManager : MonoBehaviour {
    
    public GameObject iconObject;
    private MythicObject mythicObject;
    private InfoUI infoUI;

    public void SetData(MythicObject obj) {

        mythicObject = obj;
        iconObject.GetComponent<Image>().sprite = mythicObject.icon;
        infoUI = GameObject.Find("Usuario").GetComponent<InfoUI>();

    }

    public void SendObjectSelected() {

        infoUI.PrintObjectSelected(mythicObject);

    }

}
