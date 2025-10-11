using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyPrefabListItem : MonoBehaviour
{

    [SerializeField] private TMPro.TMP_Text Attack;
    [SerializeField] private TMPro.TMP_Text Health;
    [SerializeField] private GameObject EnemyPrint;
    [SerializeField] private GameObject DefenseImage;
    [SerializeField] private GameObject StateOfEnemy;
    [SerializeField] private List<Sprite> SpriteStates;

    public Action<Enemy> ShowEnemyData;
    private Enemy _enemy;

    public int x, y;
    public void SetData(Enemy enemyData, int state = -1)
    {
        _enemy = enemyData;
        Attack.text = enemyData.Attack.ToString();
        Health.text = enemyData.Health.ToString();
        EnemyPrint.GetComponent<Image>().sprite = _enemy.EnemySprite;
        x = _enemy.Position.x;
        y = _enemy.Position.y;

        if (enemyData.Health <= 0)
        {

            Destroy(this.gameObject);

        }

        if (state != -1)
        {

            StateOfEnemy.GetComponent<Image>().sprite = SpriteStates[state];
            StateOfEnemy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.8f);

        }
        else
        {

            StateOfEnemy.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0);

        }

        DefenseImage.SetActive(_enemy.HasArmor());

    }

    public void ShowEnemyDataUI()
    {
        ShowEnemyData?.Invoke(_enemy);
    }

}
