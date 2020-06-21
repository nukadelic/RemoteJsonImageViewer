using UnityEngine;
using UnityEngine.UI;

public class PUIBottomSlideDrag : MonoBehaviour
{
    [SerializeField] PUIMouseTouch overlayArea;
    [SerializeField] PUIMouseTouch dragArea;
    [SerializeField] RectTransform popUp;
    [SerializeField] Image overlay;
    
    [SerializeField, Range(0f,1f)] 
    float overlayAlpha = 0.6f;
    
    [Tooltip("How much do we need to slide down of the height % to start the hide animation")]
    [SerializeField, Range(0.1f,0.9f)]
    float dragDownSnap = 0.5f;

    PUIEase ease;

    bool _dragging = false;

    [SerializeField]
    bool _visible = false;

    Vector2 _dragStartPoint;

    float yStart = 0f;
    float yEnd = 0f;

    Vector2 pos = Vector2.zero;

    float yPosVisible = 0f;
    float yPosHidden => - popUp.rect.size.y;
    bool IsAnimating => ease?.IsPlaying ?? false;
    float EaseValue => IsAnimating ? ease.value : 1f;

    public void Show( )
    {
        if(!this.SetIfNotEqual( ref _visible, true )) return;

        var c = overlay.color; c.a = 0; overlay.color = c;

        yStart = yPosHidden;
        yEnd = yPosVisible;

        KillEase( );
        ease = this.Ease( 0.4f );
        ease.OnEnd += OnShow;

        PostionAtStart( );

        gameObject.SetActive( true );
    }

    void PostionAtStart()
    {
        pos.y = yStart;
        popUp.anchoredPosition = pos;

    }


    void OnShow()
    {
        KillEase( );
    }

    void KillEase()
    {
        if( IsAnimating ) ease.Remove();
        ease = null;
    }

    public void Hide()
    {
        if( ! this.SetIfNotEqual( ref _visible, false )) return;

        yStart = yPosVisible;
        yEnd = yPosHidden;

        KillEase( );
        ease = this.Ease( 0.4f );
        ease.OnEnd += OnHide;
    }

    void OnHide()
    {
        gameObject.SetActive( false );

        KillEase( );
    }


    void OnEnable( )
    {
        dragArea.OnDown += DragBegin;
        dragArea.OnUp += DragEnd;
        overlayArea.OnDown += Hide;
    }

    void OnDisable( )
    {
        dragArea.OnDown -= DragBegin;
        dragArea.OnUp -= DragEnd;
        overlayArea.OnDown -= Hide;
    }

    void DragBegin( ) 
    {
        if( ! _visible || IsAnimating ) return;

        if( ! this.SetIfNotEqual( ref _dragging, true ) ) return;

        _dragStartPoint = dragArea.GetPoint( );
    }

    void DragEnd( ) 
    {
        if( ! this.SetIfNotEqual( ref _dragging, false )) return;

        bool above = pos.y > yStart + ( yEnd - yStart ) * dragDownSnap;

        float startAt = pos.y;

        if( above )
        {
            _visible = false; // override lock 
            Show( );
        }

        else
        {
            _visible = true; // override lock 
            Hide( );
        }

        if( ease != null ) ease.easeType = PUIEase.EaseType.OutQuad;

        yStart = startAt;

        PostionAtStart( );
    }

    void Update( )
    {
        if(_dragging)
        {
            var delta = dragArea.GetPoint() - _dragStartPoint;

            var deltaY = delta.y / popUp.lossyScale.y;

            pos.y = yEnd;
            pos.y += deltaY;

            if(pos.y > 0) pos.y = 0;
        }
        else
        {
            pos.y = yStart + ( yEnd - yStart ) * EaseValue;
        }

        popUp.anchoredPosition = pos;

        overlay.color = new Color( 0, 0, 0, overlayAlpha * ( 1f - pos.y / yPosHidden ) );
    }
}
