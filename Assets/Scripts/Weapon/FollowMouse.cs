using UnityEngine;

public class FollowMouse : MonoBehaviour
{   
    void Update()
    {
        Vector3 pos = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

		float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
