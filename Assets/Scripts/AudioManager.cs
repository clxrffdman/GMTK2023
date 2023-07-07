using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;



public class AudioManager : MonoBehaviour
{
    [SerializeField] private EventReference testSFX;
    // Start is called before the first frame update
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot(testSFX, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
