using UnityEngine;

public class DestroyAfterAnimation : MonoBehaviour
{
    [SerializeField] AudioClip _destroySFX;

    void Start()
    {
        if (_destroySFX != null)
        {
            AudioSource.PlayClipAtPoint(_destroySFX, transform.position);
        }
        else
        {
            Debug.LogWarning("SFX for destruction not found! (Optional)");
        }
        
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }

}
