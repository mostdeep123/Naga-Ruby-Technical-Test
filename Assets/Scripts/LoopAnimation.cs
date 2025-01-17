using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopAnimation : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    public bool fadePingPongAnimation;

    // Start is called before the first frame update
    void Start()
    {
        if(fadePingPongAnimation) FadeIn();

        else UpFadeOutAnimation();
    }

     void FadeIn()
    {
        LeanTween.alphaCanvas(canvasGroup, 1.0f, 0.5f)
        .setIgnoreTimeScale(true)
        .setOnComplete(() =>
        {
            FadeOut(); 
        });
    }

    void FadeOut()
    {
        LeanTween.alphaCanvas(canvasGroup, 0.0f, 0.5f)
        .setIgnoreTimeScale(true)
        .setOnComplete(() =>
        {
            FadeIn();
        });
    }

    void UpFadeOutAnimation ()
    {
        LeanTween.moveY(this.gameObject, 10.0f, 5.0f)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => {

                Destroy(this.gameObject);
            });
        
        LeanTween.alphaCanvas(canvasGroup, 0f, 3.0f)
            .setIgnoreTimeScale(true);
    }
}
