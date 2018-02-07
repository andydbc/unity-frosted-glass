using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CommandBufferBlur : MonoBehaviour
{
    [SerializeField]
    Shader _Shader;

    Material _Material = null;

    Camera _Camera = null;
    CommandBuffer _CommandBuffer = null;

    public void Cleanup()
    {
        if (!Initialized)
            return;

        _Camera.RemoveCommandBuffer(CameraEvent.AfterSkybox, _CommandBuffer);
        _CommandBuffer = null;
        Object.DestroyImmediate(_Material);
    }

    public void OnEnable()
    {
        Cleanup();
        Initialize();
    }

    public void OnDisable()
    {
        Cleanup();
    }

    public bool Initialized
    {
        get { return _CommandBuffer != null; }
    }

    void Initialize()
    {
        if (Initialized)
            return;

        if (!_Material)
        {
            _Material = new Material(_Shader);
            _Material.hideFlags = HideFlags.HideAndDontSave;
        }

        _Camera = GetComponent<Camera>();

        _CommandBuffer = new CommandBuffer();
        _CommandBuffer.name = "Blur screen";

        int numIterations = 4;

        Vector2[] sizes = {
            new Vector2(Screen.width, Screen.height),
            new Vector2(Screen.width / 2, Screen.height / 2),
            new Vector2(Screen.width / 4, Screen.height / 4),
            new Vector2(Screen.width / 8, Screen.height / 8),
        };

        for (int i = 0; i < numIterations; ++i)
        {
            int screenCopyID = Shader.PropertyToID("_ScreenCopyTexture");
            _CommandBuffer.GetTemporaryRT(screenCopyID, -1, -1, 0, FilterMode.Bilinear);
            _CommandBuffer.Blit(BuiltinRenderTextureType.CurrentActive, screenCopyID);

            int blurredID = Shader.PropertyToID("_Grab" + i + "_Temp1");
            int blurredID2 = Shader.PropertyToID("_Grab" + i + "_Temp2");
            _CommandBuffer.GetTemporaryRT(blurredID, (int)sizes[i].x, (int)sizes[i].y, 0, FilterMode.Bilinear);
            _CommandBuffer.GetTemporaryRT(blurredID2, (int)sizes[i].x, (int)sizes[i].y, 0, FilterMode.Bilinear);

            _CommandBuffer.Blit(screenCopyID, blurredID);
            _CommandBuffer.ReleaseTemporaryRT(screenCopyID);

            _CommandBuffer.SetGlobalVector("offsets", new Vector4(2.0f / sizes[i].x, 0, 0, 0));
            _CommandBuffer.Blit(blurredID, blurredID2, _Material);
            _CommandBuffer.SetGlobalVector("offsets", new Vector4(0, 2.0f / sizes[i].y, 0, 0));
            _CommandBuffer.Blit(blurredID2, blurredID, _Material);

            _CommandBuffer.SetGlobalTexture("_GrabBlurTexture_" + i, blurredID);
        }

        _Camera.AddCommandBuffer(CameraEvent.AfterSkybox, _CommandBuffer);
    }

    void OnPreRender()
    {
        Initialize();
    }
}
