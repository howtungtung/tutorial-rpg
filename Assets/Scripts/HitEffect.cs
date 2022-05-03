using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class HitEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        Destroy(gameObject, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
