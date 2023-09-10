using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeObjectsDithering : MonoBehaviour
{
    const float ALPHA = 0.15f;
    const float PLAIN_ALPHA = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sticky"))
        {
            var collisionRenderer = new Renderer();
            if (other.gameObject.TryGetComponent<Renderer>(out collisionRenderer))
            {
                var collisionMat = collisionRenderer.material;
                if (collisionMat.HasProperty("_Alpha"))
                {
                    collisionMat.SetFloat("_Alpha", ALPHA);
                }
            }

        }
    }


    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Sticky"))
        {

            var collisionRenderer = new Renderer();
            if (other.gameObject.TryGetComponent<Renderer>(out collisionRenderer))
            {
                var collisionMat = collisionRenderer.material;
                if (collisionMat.HasProperty("_Alpha"))
                {
                    collisionMat.SetFloat("_Alpha", PLAIN_ALPHA);
                }
            }
        }
    }
}
