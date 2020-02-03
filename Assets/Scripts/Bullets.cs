using UnityEngine;

public class Bullets : MonoBehaviour
{
    [SerializeField] float _timeToDie = 20;
    public float _bulletSpeed = 1;
    //[SerializeField] bool isRotating;
    [SerializeField] AudioClip _amunitionSFX;

    void Start()
    {
        AudioSource.PlayClipAtPoint(_amunitionSFX, transform.position, 2);

        //Will destroy this gObj if not acheive other criteria in Destruction()
        Destroy(gameObject, _timeToDie);
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
        if (transform.position.y > 12 || transform.position.y < -12 || transform.position.x > 16 || transform.position.x < -16)
        {
            Destroy(gameObject);
        }
    }

}
