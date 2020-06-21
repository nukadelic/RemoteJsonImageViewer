using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI.ProceduralImage;

[RequireComponent( typeof(ProceduralImage) )]
public class PUIButton : MonoBehaviour
{
    void OnValidate( ) => GetComponent<ProceduralImage>( ).color = colorUp;

    public Color colorUp = PUIEx.ColorGS( 0.4f );
    public Color colorDown = PUIEx.ColorGS( 0.72f );

    public UnityEvent EventClick;

    PUIEase ease;
    ProceduralImage img;
    PUIMouseTouch input;


    void Start( )
    {
        img = GetComponent<ProceduralImage>( );
        input = this.GetAddComp<PUIMouseTouch>( );

        input.OnClick += MouseClick;
        input.OnDown += EaseStart;
        input.OnUp += EaseStart;
    }

    void Update( )
    {
        if( ease == null ) return;

        //if(!( ease?.IsPlaying ?? false )) return;

        bool isDown = input.IsMouseDown;
        
        img.color = Color.Lerp(
            isDown ? colorUp : colorDown, 
            isDown ? colorDown : colorUp,
            // half way to down 
            ( isDown ? 0.5f : 1 ) * ease.value 
        );

        if(!ease.IsPlaying)
        {
            ease.End( );
            ease = null;
        }
    }

    void EaseStart()
    {
        if( ease != null ) ease.End();
        ease = this.Ease( input.IsMouseDown ? 0.05f : 0.25f );
    }

    void MouseClick()
    {
        Debug.Log( "CLick!" );

        EventClick?.Invoke( );
    }
}
