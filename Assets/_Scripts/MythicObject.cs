using UnityEngine;

[CreateAssetMenu(fileName = "MythicObject", menuName = "MythicObject/New Mythic Object")]
public class MythicObject : ScriptableObject {

    public int id;
    public string nameObject;
    [TextArea] public string description;
    public Sprite icon;
    public bool isWeapon;
    
}
