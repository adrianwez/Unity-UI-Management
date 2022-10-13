using UnityEngine;
/*
    a basic fade effect to be called from any other component
*/
public class UI_CameraFade : MonoBehaviour
{
    // making it a singleton
    public static UI_CameraFade _instance;
    [Header("Transition Color")]
    [SerializeField] private Color _fadeColor = Color.black;
    [Header("Transition Curve")]
    [SerializeField] private AnimationCurve Curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, 0.5f, -1.5f, -1.5f), new Keyframe(1, 0));
    
    private float _fadeVelocity = 1f;                   // how quick it fades
    private float _alpha = 0f;                          // transparency
    private Texture2D _texture;                         // texture used to fade the screen
    private int _direction = 0;                         // -1 fade out / 1 fade in 
    private float _time = 0f;                           // transition time

    private void Awake() => _instance = this;
    private void Start()
    {
        _texture = new Texture2D(1, 1);
        _texture.SetPixel(0, 0, new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _alpha));
        _texture.Apply();
    }

    [Tooltip("Fade in the screen given the speed")]
    public void FadeIn(float fadeSpeed = 1)
    {
        _alpha = .0f;
        _time = 1f;
        _direction = -1;
        _fadeVelocity = fadeSpeed;
    }
    [Tooltip("Fade out the screen given the speed")]
    public void FadeOut(float fadeSpeed = 1)
    {
        _alpha = 1.0f;
        _time = 0f;
        _direction = 1;
        _fadeVelocity = fadeSpeed; 
    }
    [Tooltip("Quick transition with fade in and out")]
    public System.Collections.IEnumerator Transition(float _timeBetweenTransition = 2)
    {
        yield return null;
        // fade in
        _alpha = .0f;
        _time = 0;
        _direction = -1;
        

        yield return new WaitForSeconds(_timeBetweenTransition);
        
        // fade out
        _alpha = 1.0f;
        _time = 1;
        _direction = 1;
        yield return null;
    }
    private void OnGUI()
    {
        if (_alpha > 0f) GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);
        if (_direction != 0f)
        {
            _time += _direction * Time.deltaTime * _fadeVelocity;
            _alpha = Curve.Evaluate(_time);
            _texture.SetPixel(0, 0, new Color(_fadeColor.r, _fadeColor.g, _fadeColor.b, _alpha));
            _texture.Apply();
        }
        if (_alpha <= .0f || _alpha >= 1.0f) _direction = 0;
    }
}
