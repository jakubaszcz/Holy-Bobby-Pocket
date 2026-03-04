using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("collectible"))
        {
            Collectible collectible = collision.gameObject.GetComponent<Collectible>();
            collectible.Collect();
        }
    }
}
