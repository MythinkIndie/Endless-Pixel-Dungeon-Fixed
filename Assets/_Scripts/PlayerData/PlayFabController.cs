// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using PlayFab;
// using PlayFab.ClientModels;

// public class PlayFabController : MonoBehaviour {

//     public static PlayFabController PFC;

//     void OnEnable() {

//         if (PlayFabController.PFC == null) {

//             PlayFabController.PFC = this;

//         } else {

//             Destroy(this.gameObject);

//         }

//         DontDestroyOnLoad(this.gameObject);

//     }

//     void Start() {

//         #if UNITY_ANDROID
//             var requestAndroid = new LoginWithAndroidDeviceIDRequest {AndroidDeviceId = ReturnMobileId(), CreateAccount = true};
//             PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginSuccess, OnLoginFailure);
//         #endif
//         #if UNITY_IOS
//             var requestIOS = new LoginWithIOSDeviceIDRequest {IOSDeviceId = ReturnMobileId(), CreateAccount = true};
//             PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginSuccess, OnLoginFailure);
//         #endif

//     }

//     public string ReturnMobileId() {

//         string deviceId = SystemInfo.deviceUniqueIdentifier;
//         return deviceId;

//     }

//     private void OnLoginSuccess(LoginResult result) {

//         Debug.Log("Congratulations, you made your first successful API call!");
//         UserData.SharedInstance.ID = result.PlayFabId;
//         GetUserData();

//     }

//     private void OnLoginFailure(PlayFabError error) {

//         Debug.LogWarning("Something went wrong with your first API call.  :(");
//         Debug.LogError("Here's some debug information:");
//         Debug.LogError(error.GenerateErrorReport());

//     }

//     public bool GetUserData() {

//         PlayFabClientAPI.GetUserData(new GetUserDataRequest() {
//             PlayFabId = UserData.SharedInstance.ID,
//             Keys = null
//         }, result => {

//             if (result.Data == null || !result.Data.ContainsKey("NewGame")) {

//                 SetBasicUserData();
//                 GetUserData();

//             } else {

//                 UserData.SharedInstance.NewGame = int.Parse(result.Data["NewGame"].Value);
//                 UserData.SharedInstance.CharSprite = int.Parse(result.Data["CharSprite"].Value);
//                 UserData.SharedInstance.Build = int.Parse(result.Data["Build"].Value);
//                 UserData.SharedInstance.WeaponEquiped = int.Parse(result.Data["WeaponEquiped"].Value);
//                 UserData.SharedInstance.ArtifactEquiped = int.Parse(result.Data["ArtifactEquiped"].Value);
//                 UserData.SharedInstance.Level = int.Parse(result.Data["Level"].Value);
//                 UserData.SharedInstance.Attack = int.Parse(result.Data["Attack"].Value);
//                 UserData.SharedInstance.HP = int.Parse(result.Data["HP"].Value);
//                 UserData.SharedInstance.Gold = int.Parse(result.Data["Gold"].Value);
//                 UserData.SharedInstance.Deepest = int.Parse(result.Data["Deepest"].Value);
//                 UserData.SharedInstance.Username = result.Data["Username"].Value;
                
//             }

//         }, (error) => {
//             Debug.Log("Got error retrieving user data:");
//             Debug.Log(error.GenerateErrorReport());
//         });

//         return true;

//     }

//     void SetBasicUserData() {

//         PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
//             Data = new Dictionary<string, string>() {
//                 {"NewGame", "0"},
//                 {"CharSprite", UserData.SharedInstance.CharSprite.ToString()},
//                 {"Build", "1"},
//                 {"WeaponEquiped", "0"},
//                 {"ArtifactEquiped", "0"},
//                 {"Username", ""},
//                 {"Level", "1"},
//                 {"Attack", "5"},
//                 {"HP", "30"},
//                 {"Gold", "0"},
//                 {"Deepest", "0"}
//             }
//         },
//         result => Debug.Log("Successfully updated user data"),
//         error => {
//             Debug.Log("Got error setting user data Ancestor to Arthur");
//             Debug.Log(error.GenerateErrorReport());
//         });
        
//     }

//     public void SetPlayfabUserData() {

//         PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest() {
//             Data = new Dictionary<string, string>() {
//                 {"NewGame", UserData.SharedInstance.NewGame.ToString()},
//                 {"Build", UserData.SharedInstance.Build.ToString()},
//                 {"WeaponEquiped", UserData.SharedInstance.WeaponEquiped.ToString()},
//                 {"ArtifactEquiped", UserData.SharedInstance.ArtifactEquiped.ToString()},
//                 {"Username", UserData.SharedInstance.Username},
//                 {"Level", UserData.SharedInstance.Level.ToString()},
//                 {"Attack", UserData.SharedInstance.Attack.ToString()},
//                 {"HP", UserData.SharedInstance.HP.ToString()},
//                 {"Gold", UserData.SharedInstance.Gold.ToString()},
//                 {"Deepest", UserData.SharedInstance.Deepest.ToString()}
//             }
//         },
//         result => Debug.Log("Successfully updated user data"),
//         error => {
//             Debug.Log("Got error setting user data Ancestor to Arthur");
//             Debug.Log(error.GenerateErrorReport());
//         });
        
//     }

// }
