using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuffBenefits : MonoBehaviour {
    
    public TMPro.TMP_Text BenefitText;

    void Start() {

        BenefitText.color = new Color(0f, 0f, 0f, 0f);

    }

    public void SetData(int valueBenefit) {

        StopAllCoroutines();

        if (valueBenefit < 0) {

            BenefitText.text = "-" + Mathf.Abs(valueBenefit).ToString();
            Color colorred = new Color(0.9137256f, 0.03529412f, 0f, 1f);
            BenefitText.color = colorred;

        } else {

            BenefitText.text = "+" + valueBenefit.ToString();
            Color colorgreen = new Color(0.1834448f, 1f, 0.1650943f, 1f);
            BenefitText.color = colorgreen;

        }

        StartCoroutine(PlayBenefitCoroutine());

    }

    IEnumerator PlayBenefitCoroutine() {
        
        yield return new WaitForSeconds(1.5f);

        while (BenefitText.color.a >= 0f) {

            Color alpha = BenefitText.color;
            alpha.a -= 0.08f;
            BenefitText.color = alpha;

            yield return new WaitForSeconds(0.1f);

        }

    }

}
