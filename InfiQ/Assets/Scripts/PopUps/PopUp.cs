using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject Dialog;

    public void Close()
    {
        CanvasGroup panelGroup = Panel.GetComponent<CanvasGroup>();
        LeanTween.scale(Dialog, Vector3.zero, 0.35f).setEaseInCubic().setIgnoreTimeScale(true).setOnComplete(
            () => LeanTween.alphaCanvas(panelGroup, 0f, 0.2f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true).setOnComplete(() => Destroy(Panel))
        );
        
    }

    private void Start()
    {
        CanvasGroup panelGroup = Panel.GetComponent<CanvasGroup>();
        panelGroup.alpha = 0;
        Dialog.transform.localScale = Vector3.zero;

        LeanTween.alphaCanvas(panelGroup, 1f, 0.2f).setEase(LeanTweenType.easeOutSine).setIgnoreTimeScale(true).setOnComplete(
            () => LeanTween.scale(Dialog, Vector3.one, 0.3f).setEaseOutCubic().setIgnoreTimeScale(true)
        );
    }
}
