using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InfoHelp : MonoBehaviour {

    [SerializeField] private Button NextInfo;
    [SerializeField] private Button PrevInfo;
    [SerializeField] private GameObject Panel1;
    [SerializeField] private GameObject Panel2;

    void Start() {

        NextInfo.onClick.AddListener(SecondPanel);
        PrevInfo.onClick.AddListener(FirstPanel);

    }

    private void SecondPanel() {

        Panel1.SetActive(false);
        Panel2.SetActive(true);

    }

    private void FirstPanel() {

        Panel1.SetActive(true);
        Panel2.SetActive(false);

    }

}
