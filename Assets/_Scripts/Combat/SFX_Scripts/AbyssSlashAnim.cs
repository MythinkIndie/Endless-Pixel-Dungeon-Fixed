using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbyssSlashAnim : MonoBehaviour
{
    [SerializeField] List<Color> AnimColors;
    [SerializeField] List<Sprite> AnimSprites;
    [SerializeField] Image _img;
    private int State = -1;

    public void ExecuteAnim()
    {

        State++;
        _img.color = AnimColors[State];
        //_img.sprite = AnimSprites[State];

    }
}
