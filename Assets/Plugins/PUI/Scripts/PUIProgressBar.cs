using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.ProceduralImage;

[ExecuteInEditMode]
public class PUIProgressBar : MonoBehaviour
{
    [Range(0,1)]
    [SerializeField] float progressValue = 0.5f;

    public Color backColor = Color.white;
    public Color frontColor = Color.red;

    public float Value { get => progressValue; set
    {
        progressValue = value;
        Validate( );
    } }

    public void EaseValue( float toValue, float duration )
    {
        PUIEase ez;
        if( ( ez = GetComponent<PUIEase>( ) ) != null) ez.End( );
        var ease = this.Ease( duration, PUIEase.EaseType.InCubic );
        ease.OnUpdate += v => Value = v;
    }

    RectTransform backRect = null;
    ProceduralImage backImg = null;
    RectTransform progressRect = null;
    ProceduralImage progressImg = null;
    RectTransform textRect = null;
    TextMeshProUGUI text = null;

    void Start( ) => Init( );
    void OnEnable( ) => Init( );
    void Init()
    {
        var back = transform.GetChild( 0 );
        backImg = back.GetComponent<ProceduralImage>( );
        backRect = back.GetComponent<RectTransform>( );
        var progress = back.GetChild( 0 );
        progressRect = progress.GetComponent<RectTransform>( );
        progressImg = progress.GetComponent<ProceduralImage>( );
        textRect = progress.GetChild( 0 ).GetComponent<RectTransform>( );
        text = progress.GetComponentInChildren<TextMeshProUGUI>( );

        Validate( );
    }

    public void OnValidate( ) => Validate( );

    void Validate()
    {
        if( text == null ) return;

        progressValue = progressValue < 0 ? 0 : ( progressValue > 1 ? 1 : progressValue );
        text.text = Mathf.RoundToInt( progressValue * 100 ) + "%";
        progressImg.fillAmount = progressValue;

        backImg.color = backColor;
        progressImg.color = frontColor;

        var w = progressRect.rect.width;
        bool right = w * progressValue < textRect.rect.width * 1.5f; // 1.5 times text width 
        var x = - ( w * ( 1 - progressValue ) ) / 2f;
        if( right ) x = ( w * 0.5f ) - ( w - ( w * progressValue ) ) * 0.5f;
        textRect.anchoredPosition = new Vector2( x , 0 );

        text.color = right ? frontColor : backColor;
    }
}
