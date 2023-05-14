using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PusheableByDog : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    private float forceMultiplier = 10;
    [SerializeField][Tooltip("Punto de referencia de la máxima altura tolerada en la colisión con el perro, osea si el perro colisiona pero esta mas arriba que esto no mueve el obstaculo")]
    private Transform maxPushableHeightTransform;
    [SerializeField]
    private const string DOG_TAG = "Dog";
    #endregion
    private float maxPushableHeight
    {
        get
        {
            return maxPushableHeightTransform.position.y;
        }
    }
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(DOG_TAG))
        {
            if (collision.transform.position.y <= maxPushableHeight)
            {
                Vector2 forceDir = new Vector2(transform.position.x - collision.transform.position.x, 0);
                forceDir.Normalize();
                rb.AddForce(forceDir * forceMultiplier,ForceMode2D.Impulse);
            }
        }
    }
}
