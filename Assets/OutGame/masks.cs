using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class masks : MonoBehaviour
{
    private Animator _animator;

    public AudioSource audioSource;
    public AudioClip audioClip;
    // Start is called before the first frame update
    void Start()
    {
        _animator = transform.GetComponent<Animator>();
        audioSource.clip = audioClip;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f))
        {
            Destroy(gameObject);
        }
        
    }
}
