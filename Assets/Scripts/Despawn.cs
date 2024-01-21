using UnityEngine;

public class Despawn : MonoBehaviour
{
    public float lifespan = 10f;

    void Start()
    {
        // Belirli bir süre sonra mermiyi silmek için Invoke fonksiyonunu kullan
        Invoke("DestroyBullet", lifespan);
    }

    void DestroyBullet()
    {
        // Mermiyi yok et
        Destroy(gameObject);
    }
}
