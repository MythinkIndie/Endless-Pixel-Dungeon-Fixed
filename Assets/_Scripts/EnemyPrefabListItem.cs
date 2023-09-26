using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyPrefabListItem : MonoBehaviour {

    [SerializeField] private TMPro.TMP_Text Attack;
    [SerializeField] private TMPro.TMP_Text Health;
    [SerializeField] private GameObject EnemyPrint;
    [SerializeField] private GameObject DefenseImage;
    [SerializeField] private GameObject StateOfEnemy;
    [SerializeField] private List<Sprite> SpriteStates;

    public int x, y;
    public void SetData(AttackDefenseView enemyData, int posx, int posy, int state = -1, bool hasArmor = false) {

        Attack.text = enemyData.Attack.ToString();
        Health.text = enemyData.Health.ToString();
        EnemyPrint.GetComponent<Image>().sprite = enemyData.EnemyPrint.GetComponent<Image>().sprite;
        x = posx;
        y = posy;

        if (enemyData.Health <= 0) {

            Destroy(this.gameObject);

        }

        if (state != -1) {

            StateOfEnemy.GetComponent<Image>().sprite = SpriteStates[state];
            StateOfEnemy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f); 
            
        } else {

            StateOfEnemy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0); 

        }

        DefenseImage.SetActive(hasArmor);

    }

}
