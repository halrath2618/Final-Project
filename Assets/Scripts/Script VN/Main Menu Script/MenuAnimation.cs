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
    private void PlayOpenAnimation() => _play.DOLocalMoveX(_play.localPosition.x + 1000, 1f)
        .SetEase(Ease.OutBack)
        .OnComplete(() => _play.DOLocalMoveX(_play.localPosition.x - 965, 1f).SetEase(Ease.InBack));

    // Update is called once per frame
    void Update()
    {
        
    }
}
