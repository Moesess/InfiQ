using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Dialog;

    public void Close()
    {
        CanvasGroup panelGroup = Panel.GetComponent<CanvasGroup>();
        LeanTween.scale(Dialog, Vector3.zero, 0.5f).setEaseInElastic().setOnComplete(
            () => LeanTween.alphaCanvas(panelGroup, 0f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(() => Destroy(Panel))
        );
        
    }

    private void Start()
    {
        CanvasGroup panelGroup = Panel.GetComponent<CanvasGroup>();
        panelGroup.alpha = 0;
        Dialog.transform.localScale = Vector3.zero;

        LeanTween.alphaCanvas(panelGroup, 1f, 0.5f).setEase(LeanTweenType.easeOutSine).setOnComplete(
            () => LeanTween.scale(Dialog, Vector3.one, 0.5f).setEaseOutElastic()
        );
    }
}
