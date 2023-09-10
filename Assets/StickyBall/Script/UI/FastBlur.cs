using UnityEngine;

[ExecuteAlways]
public class FastBlur : MonoBehaviour
{
    [SerializeField, Range(0.0f, 30.0f)] private float _blurSize = 2;
    [SerializeField] private Material _material = null;
    [SerializeField] Shader _shader;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            if (_shader == null)
            {
                Graphics.Blit(source, destination);
                return;
            }
            else
            {
                _material = new Material(_shader);
            }
        }
        _material.SetFloat("_BlurSize", _blurSize);

        var temp = RenderTexture.GetTemporary(Screen.width / 4, Screen.height / 4, 0, source.format);
        var temp1 = RenderTexture.GetTemporary(Screen.width / 8, Screen.height / 8, 0, source.format);

        Graphics.Blit(source, temp, _material);

        Graphics.Blit(temp, temp1, _material);

        Graphics.Blit(temp1, destination, _material);

        RenderTexture.ReleaseTemporary(temp);
        RenderTexture.ReleaseTemporary(temp1);
    }
}