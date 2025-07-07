using UnityEngine;

public class Projectiledamage : MonoBehaviour
{

    public float damage;
    public bool deflected=false;
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Hit: " + other.name);

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player hit!");

            if (other.TryGetComponent(out PlayerController player))
            {
                player.Health -= damage;
            }
        }
        else if (other.CompareTag("OuterDeflectLayer"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.linearVelocity = -rb.linearVelocity;
            deflected = true;
            Debug.Log("deflected");
            
        }
        else if(other.CompareTag("Enemy") && deflected)
        {
            if (other.TryGetComponent(out Bill_AI bill))
            {
                bill.TakeDamage(damage); // Directly call the method
                Debug.Log("BILL HIT");
            }
        }
    }

}
