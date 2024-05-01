using UnityEngine;
using UnityEngine.UI;

public class LoadingUI : MonoBehaviour
{
    [SerializeField] protected Image skullImage;
    [SerializeField] protected Image backgroundImage;

    bool isShowing = false;

    void Update()
    {
        if (!isShowing) return;
        skullImage.transform.Rotate(Vector3.forward, 30 * Time.deltaTime);
    }

    public void SetVisible(bool value)
    {
        isShowing = value;
        skullImage.color = value ? Color.white : new Color(1,1,1,0);
        backgroundImage.color = value ? Color.black : new Color(1,1,1,0);
    }
}
