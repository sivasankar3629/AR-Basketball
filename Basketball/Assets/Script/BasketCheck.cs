using UnityEngine;

public class BasketCheck : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Rigidbody>().linearVelocity.y < 0)
        {
            GameManager.Instance.AddScore(1);
        }
    }
}
