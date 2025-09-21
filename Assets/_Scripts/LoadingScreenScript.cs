// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;

// public class LoadingScreenScript : MonoBehaviour {

//     [SerializeField] private Slider screenProgressBar;
//     [SerializeField] private GameObject loginBarGO;
//     [SerializeField] private GameObject usernameBarGO;
//     [SerializeField] private Button ConfirmUsernameBtn;
//     [SerializeField] private TMPro.TMP_InputField usernameInput;

//     void Start() {

//         loginBarGO.SetActive(false);
//         usernameBarGO.SetActive(false);

//         ConfirmUsernameBtn.onClick.AddListener(SetUpUsernameData);
//         StartCoroutine(MyStartCoroutine());

//     }

//     IEnumerator MyStartCoroutine() {

//         yield return new WaitUntil(() => PlayFabController.PFC.GetUserData());

//         if (UserData.SharedInstance.Username == "") {

//             usernameBarGO.SetActive(true);
//             yield break;

//         } else {

//             StartCoroutine(LoadNextScene());

//         }

//     }

//     void SetUpUsernameData() {

//         if (usernameInput.text.Trim() != "" && usernameInput.text.Trim().Length < 14) {

//             UserData.SharedInstance.Username = usernameInput.text.Trim();
//             PlayFabController.PFC.SetPlayfabUserData();
//             StartCoroutine(LoadNextScene());

//         }

//     }

//     IEnumerator LoadNextScene() {

//         loginBarGO.SetActive(true);
//         usernameBarGO.SetActive(false);

//         AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(UserData.SharedInstance.SceneToGo);

//         while (!asyncLoad.isDone) {

//             screenProgressBar.value = asyncLoad.progress * 100;
//             yield return null;

//         }

//     }

// }
