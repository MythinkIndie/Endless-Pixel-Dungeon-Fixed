using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class AttackDefenseView : MonoBehaviour {
    
    //Agarrará las variables de vida y daño y las actualizará en el Gameplay
    public int Attack;
    public int Health;
    public GameObject EnemyPrint;
    public SpriteRenderer Flash;

    public Vector2Int BoardPos = Vector2Int.zero;

    bool _hideOnDone = false;

    public void SetData(int attack, int health, Sprite enemy, bool flash = false) {

        StopAllCoroutines();
        Attack = attack;
        Health = health;
        EnemyPrint.GetComponent<Image>().sprite = enemy;
        _hideOnDone = health < 1;
        var col = Color.red;
        col.a = 0f;
        Flash.color = col;
        if (flash) {

            StartCoroutine(PlayFlashCoroutine());

        } else if (_hideOnDone) {

            BoardPos.x = -1;
            BoardPos.y = -1;
            gameObject.SetActive(false);

        }

    }

    IEnumerator PlayFlashCoroutine() {

        var col = Color.red;
        col.a = 0.5f;
        Flash.color = col;
        yield return new WaitForSeconds(0.1f);
        col.a = 0.0f;
        Flash.color = col;
        if (_hideOnDone) {

            BoardPos.x = -1;
            BoardPos.y = -1;
            gameObject.SetActive(false);

        }

    }

}
