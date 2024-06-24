using UnityEngine;

public class HuntedCollisionDetector : MonoBehaviour
{
    public GameObject otherHunted;

    void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with a power-up
        if (collision.collider.CompareTag("PowerUp"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithPowerUp(collision.collider.gameObject, this.gameObject);
        }
        else if (collision.collider.CompareTag("Hunter"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithHunter();
        }
        else if (collision.collider.CompareTag("Hunter"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithHunter();
        }
        else if (this.gameObject.CompareTag("Hunted1") && collision.collider.CompareTag("Exit1"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithExit(this.gameObject, otherHunted);
        }
        else if (this.gameObject.CompareTag("Hunted2") && collision.collider.CompareTag("Exit2"))
        {
            // Notify the CollisionManager of the collision
            CollisionManager.Instance.OnHuntedCollisionWithExit(this.gameObject, otherHunted);
        }
        else if (collision.collider.CompareTag("Level1") || collision.collider.CompareTag("Level2") ||
                 collision.collider.CompareTag("Tutorial"))
        {
            CollisionManager.Instance.OnHunterCollisionWithLevelDoor(collision.collider.tag);
        }
    }
}