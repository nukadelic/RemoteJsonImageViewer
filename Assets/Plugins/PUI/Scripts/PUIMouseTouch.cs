using UnityEngine;

[DisallowMultipleComponent]
public class PUIMouseTouch : MonoBehaviour
{
    public static int GlobalDepth = 0;
    [HideInInspector] public int depth = 0;

    RectTransform rect;

    public event System.Action OnClick;
    public event System.Action OnDown;
    public event System.Action OnUp;

    [Tooltip("If set to true, events will be dispatched when mouse/touch point is outside of the rect transform area")]
    public bool inverseArea = false;

    void Awake( )
    {
        rect = GetComponent<RectTransform>( );
    }

    void MouseDown( ) 
    {
        OnDown?.Invoke( );
    }

    /// <summary>
    /// Can be mouse up for a click or a mouse up for a cancel operation 
    /// </summary>
    void MouseUp() 
    {
        OnUp?.Invoke( );
    }

    /// <summary>
    /// Mouse Was pressed down and release within the rect trasform area 
    /// </summary>
    void MouseClick() 
    {
        OnClick?.Invoke( );
    }

    public bool IsMouseDown => _isDown;

    bool _isDown = false;
    bool _touchBegan = false;

    bool InArea( Vector2 p )
    {
        bool inside = RectTransformUtility.RectangleContainsScreenPoint( rect, p );

        return inverseArea ? ! inside : inside ;
    }

    public Vector2 GetPoint()
    {
        if(Input.touchCount > 0) 
            return Input.touches[ 0 ].position;
        return Input.mousePosition;
    }

    void Update( )
    {
        if( ! Application.isPlaying ) return;

        if( GlobalDepth > depth ) return;

        if(Input.GetMouseButtonDown( 0 ))
        {
            if( InArea( Input.mousePosition ) )
            if( this.SetIfNotEqual( ref _isDown, true ) )
            {
                MouseDown();
            }
        }
        else if( Input.GetMouseButtonUp( 0 ) )
        {
            if(this.SetIfNotEqual( ref _isDown, false ))
            {
                MouseUp( );

                if( InArea( Input.mousePosition ) )
                {
                    MouseClick( );
                }
            }
        }
        if( Input.touchCount > 0 )
        {
            var touch = Input.touches[ 0 ];

            if(touch.phase == TouchPhase.Began
            && InArea( touch.position )
            && this.SetIfNotEqual( ref _isDown, true ))
            {
                _touchBegan = true;
                MouseDown();
            }
            // in bounds end of touch 
            if( touch.phase == TouchPhase.Ended 
            &&  this.SetIfNotEqual( ref _isDown, false ) )
            {
                _touchBegan = false;
                MouseUp( );

                if( InArea( touch.position ) )
                {
                    MouseClick( );
                }
            }
        }
        else if( _touchBegan && this.SetIfNotEqual( ref _isDown, false ) )
        {
            _touchBegan = false;
            MouseUp( );
        }
    }
}
//Vector3[] corners = new Vector3[4];
//box.GetWorldCorners( corners );
//corners[ i ] = canvasRootRectTransform.InverseTransformPoint( corners[ i ] );
////var pos = box.localToWorldMatrix.MultiplyVector( box.rect.position );
////var siz = box.localToWorldMatrix.MultiplyVector( box.rect.size );
////Rect rect = new Rect( pos, siz );
//bool b = RectTransformUtility.ScreenPointToLocalPointInRectangle( 
//    box, 
//    Input.mousePosition, 
//    Camera.current, 
//    out Vector2 localPoint 
//);
//Debug.Log( box.rect );
//Debug.Log( rect ); // right size not position
//Debug.Log( Input.mousePosition );