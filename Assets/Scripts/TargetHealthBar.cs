using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TargetHealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI nameText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.targetCharacterData != null)
        {
            slider.gameObject.SetActive(true);
            slider.value = PlayerController.instance.targetCharacterData.currentStat.hp / (float)PlayerController.instance.targetCharacterData.defaultStat.hp;
            nameText.text = PlayerController.instance.targetCharacterData.characterName;
        }
        else
        {
            slider.gameObject.SetActive(false);
        }
    }
}
