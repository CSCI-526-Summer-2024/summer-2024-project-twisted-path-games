using UnityEngine;

public class HunterCollisionDetector : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a power-up
        if (collision.collider.CompareTag("PowerUp"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithPowerUp(collision.collider.gameObject);
        }
        else if (collision.collider.CompareTag("Hunter"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithHunter(collision.collider.gameObject);
        }
        else if (collision.collider.CompareTag("Exit"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithExit(collision.collider.gameObject);
        }
    }
}