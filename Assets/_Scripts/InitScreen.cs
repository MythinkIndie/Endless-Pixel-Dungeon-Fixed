using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitScreen : MonoBehaviour {
    
    void Start() {
        
        UserData.SharedInstance.SceneToGo = 2;
        StartCoroutine(MyCoroutine());

    }

    IEnumerator MyCoroutine() {

        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene(1);

    }

}
