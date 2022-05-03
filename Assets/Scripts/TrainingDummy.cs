using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    private CharacterData characterData;

    // Start is called before the first frame update
    void Start()
    {
        characterData = GetComponent<CharacterData>();
        characterData.Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
