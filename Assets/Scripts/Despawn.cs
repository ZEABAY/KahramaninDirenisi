using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float lifespan = 10f;

    void Start()
    {
        // Belirli bir s�re sonra mermiyi silmek i�in Invoke fonksiyonunu kullan
        Invoke("DestroyBullet", lifespan);
    }

    void DestroyBullet()
    {
        // Mermiyi yok et
        Destroy(gameObject);
    }
}
