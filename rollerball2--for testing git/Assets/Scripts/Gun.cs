using UnityEngine;
using UnityEngine.Events;

public class BillGun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform muzzle; // The tip of the gun (for ray origin or bullet spawn)
    private Transform player;

    [Header("Gun Settings")]
    public UnityEvent OnGunShoot;      // Hook this to Shoot() or bullet spawn in Inspector
    public float fireCooldown = 1.5f;  // Time between shots
    private float currentCooldown = 0f;

    [Header("Aiming")]
    public float aimRotationSpeed = 5f;

    void Update()
    {
        if (player == null) return;

        // Rotate to face player
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(dir.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * aimRotationSpeed);

        // Handle shooting
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Debug.Log(" Attempting to shoot...");
            OnGunShoot?.Invoke(); // <-- This must be wired up in Inspector
            currentCooldown = fireCooldown;
        }
    }
    void OnDrawGizmos()
    {
        if (muzzle != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(muzzle.position, muzzle.forward * 2f);
        }
    }

    public void SetTarget(Transform target)
    {
        player = target;
    }

    // Optional raycast-based shoot method
    public void Shoot()
    {
        if (player == null) return;
        if (muzzle == null) muzzle = transform; // fallback if not assigned

        Vector3 origin = muzzle.position;
        Vector3 direction = (player.position - origin).normalized;

        Ray ray = new Ray(origin, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            Debug.DrawLine(origin, hit.point, Color.red, 0.3f); //  Draw red line to hit
            Debug.Log($"Enemy hit: {hit.collider.name} at {hit.point}"); //  Log hit info

            if (hit.collider.TryGetComponent(out PlayerController p))
            {
                p.Health -= 10f;
                Debug.Log(" Hit Player! Dealt 10 damage.");
            }
        }
        else
        {
            Debug.DrawRay(origin, direction * 100f, Color.yellow, 0.3f); //  Draw yellow line if missed
            Debug.Log(" Missed shot.");
        }
    }

}
