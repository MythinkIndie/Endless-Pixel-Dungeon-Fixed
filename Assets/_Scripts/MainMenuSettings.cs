using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainMenuSettings : MonoBehaviour {

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider fbxSlider;
    [SerializeField] private Slider textSpeedSlider;
    [SerializeField] private UserData DatosUser;

    [SerializeField] private Button ExitSettings;
    [SerializeField] private Button PutSettings;
    [SerializeField] private GameObject SettingGlobalCanva;
    [SerializeField] private RectTransform BackgroundSettings;
    [SerializeField] private RectTransform AllSettingOptions;
    
    void Start() {

        SettingGlobalCanva.SetActive(false);
        BackgroundSettings.transform.localPosition = new Vector3(0, -2000, 0);
        AllSettingOptions.transform.localPosition = new Vector3(0, -2000, 0);

        volumeSlider.value = DatosUser.MusicVolume;
        fbxSlider.value = DatosUser.FbxVolume;
        textSpeedSlider.value = DatosUser.TextSpeed;

        volumeSlider.onValueChanged.AddListener((v) => ChangeUserData(1));
        fbxSlider.onValueChanged.AddListener((v) => ChangeUserData(2));
        textSpeedSlider.onValueChanged.AddListener((v) => ChangeUserData(3));

        PutSettings.onClick.AddListener(AnimationUpSettings);
        ExitSettings.onClick.AddListener(AnimationDownSettings);
        
    }

    private void ChangeUserData(int sliderNum) {

        switch (sliderNum) {

            case 1:
                DatosUser.MusicVolume = volumeSlider.value;
                break;
            case 2:
                DatosUser.FbxVolume = fbxSlider.value;
                break;
            case 3:
                DatosUser.TextSpeed = (int)Mathf.Round(textSpeedSlider.value);
                break;

        }

    }

    private void AnimationUpSettings() {

        SettingGlobalCanva.SetActive(true);
        BackgroundSettings.DOAnchorPos(new Vector2(0,0), 1.2f, false).SetEase(Ease.OutElastic);
        AllSettingOptions.DOAnchorPos(new Vector2(0,0), 1.2f, false).SetEase(Ease.OutElastic);

    }

    private void AnimationDownSettings() {

        BackgroundSettings.DOAnchorPos(new Vector2(0,-2500), 0.6f, false).SetEase(Ease.InQuad);
        AllSettingOptions.DOAnchorPos(new Vector2(0,-2500), 0.6f, false).SetEase(Ease.InQuad);
        StartCoroutine(SettingDown());

    }

    IEnumerator SettingDown() {

        yield return new WaitForSeconds(0.6f);

        SettingGlobalCanva.SetActive(false);

    }

}
