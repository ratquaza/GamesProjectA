using UnityEngine;

public class FollowMouse : MonoBehaviour
{   
	[SerializeField] private SpriteRenderer heldSprite;
	
    void Update()
    {
        Vector3 pos = Input.mousePosition - Camera.main.WorldToScreenPoint (transform.position);

		float angle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

		heldSprite.flipY = transform.rotation.z < -.6 || transform.rotation.z > .6;
    }
}
