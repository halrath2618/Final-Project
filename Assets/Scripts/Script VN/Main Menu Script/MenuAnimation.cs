using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MenuAnimation : MonoBehaviour
{
    [SerializeField] private Transform _play;

    // Start is called before the first frame update
    void Start()
    {
        PlayOpenAnimation();
    }
    private void PlayOpenAnimation() => _play.DOMoveX(_play.position.x, 2).From(6000);

    // Update is called once per frame
    void Update()
    {
        
    }
}
