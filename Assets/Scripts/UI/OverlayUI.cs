using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Image heartImage;
    [SerializeField] private Image heartbeatImage;
    [SerializeField] private float lowHealthPercent = .4f;

    private bool isLowHealth = false;
    private float heartbeatAnim = 0f;
    private Vector3 heartbeatBaseScale;

    // Start is called before the first frame update
    void Start()
    {
        heartbeatImage.color = new Color(1, 1, 1, 0);
        heartbeatBaseScale = heartbeatImage.transform.localScale;

        player.onHealthChange += (hp) => {
            heartImage.fillAmount = hp/(float) player.MaxHealth();
            if (!isLowHealth && heartImage.fillAmount <= lowHealthPercent)
            {
                isLowHealth = true;
                heartbeatAnim = 0f;
            }
            else if (isLowHealth && heartImage.fillAmount > lowHealthPercent)
            {
                isLowHealth = false;
                heartbeatAnim = 0f;
            }
        };
    }

    void Update()
    {
        if (isLowHealth)
        {
            heartbeatImage.color = new Color(1, 1, 1, Mathf.Lerp(.5f, 0, heartbeatAnim/2f));
            heartbeatImage.transform.localScale = heartbeatBaseScale * Mathf.Lerp(1, 1.5f, heartbeatAnim/2f);
            heartbeatAnim += Time.deltaTime;
            if (heartbeatAnim >= 3f) heartbeatAnim = 0;
        }
    }
}
