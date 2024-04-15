using UnityEngine;

public class FollowMouse : MonoBehaviour
{   
    [SerializeField] private bool flipSprite;
    private float initScale;

    void Start()
    {
        initScale =  transform.localScale.y;
    }

    void Update()
    {
        Vector3 pos = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

		float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, 0, angle)), 20f * Time.deltaTime);

        if (flipSprite) transform.localScale =  new Vector3(transform.localScale.x, angle >  90 || angle < -90 ? -initScale : initScale, transform.localScale.z);
    }
}
