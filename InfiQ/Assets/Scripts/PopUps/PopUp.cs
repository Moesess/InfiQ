using UnityEngine;

public class PopUp : MonoBehaviour
{
    [SerializeField] GameObject Panel;

    public void Close()
    {
        Destroy(Panel);
    }
}
