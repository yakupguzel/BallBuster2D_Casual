using UnityEngine;

public class Box : MonoBehaviour
{
    [SerializeField] GameManager gameManager;


    public void PlayExplosionEffect()
    {
        gameManager.BoxExplosionEffect(transform.position);
        gameObject.SetActive(false);
    }
}
