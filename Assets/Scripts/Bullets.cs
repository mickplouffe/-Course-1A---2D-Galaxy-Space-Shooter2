using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 1;
    [SerializeField] bool isRotating;
    [SerializeField] AudioClip _amunitionSFX;

    void Start()
    {
        AudioSource.PlayClipAtPoint(_amunitionSFX, transform.position, 2);

        //Will destroy this gObj if not acheive other criteria in Destruction()
        Destroy(gameObject, 10);
    }

    void Update()
    {
        Movements();
        Destruction();
    }

    private void Movements()
    {
        transform.Translate(Vector3.up * _bulletSpeed * Time.deltaTime);
    }

    private void Destruction()
    {
        if (transform.position.y > 8)
        {
            Destroy(gameObject);
        }
    }

}
