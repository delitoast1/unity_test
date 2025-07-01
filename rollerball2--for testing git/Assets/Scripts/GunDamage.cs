using UnityEngine;

public class GunDamage : MonoBehaviour
{
    public float Damage = 10f;
    public float BulletRange = 50f;
    public GameObject muzzleObject; // the origin of the shot (e.g., gun barrel)

    void Start()
    {
        if (muzzleObject == null)
        {
            muzzleObject = this.gameObject; // fallback
        }
    }

    public void Shoot()
    {
        Vector3 origin = muzzleObject.transform.position;
        Vector3 direction = muzzleObject.transform.forward;

        Ray gunRay = new Ray(origin, direction);

        if (Physics.Raycast(gunRay, out RaycastHit hitInfo, BulletRange))
        {
            Debug.DrawLine(origin, hitInfo.point, Color.red, 0.2f); // Line to hit point (for hit)

            if (hitInfo.collider.TryGetComponent(out PlayerController player))
            {
                player.Health -= Damage;
                Debug.Log($"Hit Player! Dealt {Damage} damage.");
            }
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * BulletRange, Color.yellow, 0.2f); // Line to max range (miss)
        }
    }

}
