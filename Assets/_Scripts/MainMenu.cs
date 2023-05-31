using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour {

    [SerializeField] private UserData DatosPlayer;
    int _weaponEquiped;
    int _artifactEquiped;
    [SerializeField] private List<Button> botonesAOcultar;

    void Start() {

        _weaponEquiped = DatosPlayer.TakeWeaponEquiped().id;
        _artifactEquiped = DatosPlayer.TakeArtifactEquiped().id;

    }

    public void PlayGame() {
        
        SceneLoader(1);

    }

    public void SeeEquipment() {

        SceneLoader(2);

    }

    public void HelpScreen() {

        SceneLoader(3);

    }

    private void SceneLoader(int action) {

        //objectToMove.transform.DOMoveY(pantallaHeight.GetComponent<RectTransform>().rect.height * 1.5f, 1.3f, false);

        switch (action) {

            case 1:
                SceneManager.LoadScene("Game");
                break;
            case 2:
                SceneManager.LoadScene("Equipment");
                break;
            case 3:
                SceneManager.LoadScene("Help");
                break;

        }

    }

    public void ReturnTitleScreen() {

        SceneManager.LoadScene("Menu");

    }

}
