using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using UnityEngine.UI.ProceduralImage;
using System.Collections.Generic;

public class ImageContainer : MonoBehaviour
{
    public Image image;
    public Transform spinIcon;

    float spinValue = 0;

    bool loaded = false;

    public bool debug = false;

    private void Update( )
    {
        if( debug )
        {
            var rect = GetComponent<RectTransform>();


            debug = false;
        }

        if( loaded ) return;

        spinValue += 13;
        spinIcon.rotation = Quaternion.Euler( 0, 0, spinValue );
    }

    [HideInInspector] public JsonData1.Image data;

    public static Queue<ImageContainer> pool = new Queue<ImageContainer>();
    public static int parallel_load_limit = 4;
    public static int in_load = 0;

    public static void Next()
    {
        if( in_load < parallel_load_limit && pool.Count > 0 )
        {
            in_load ++ ;
            var item = pool.Dequeue( );
            new HTTPRequest( new System.Uri( item.data.svg.th150.url ), item.OnLoad ).Send( );
        }
    }

    public void Load( JsonData1.Image data )
    {
        this.data = data;
        pool.Enqueue( this );
        Next( );
    }

    void OnLoad( HTTPRequest originalRequest, HTTPResponse response  )
    {
        //image.material.mainTexture = response.DataAsTexture2D;
        var sprite = Sprite.Create( response.DataAsTexture2D, new Rect( 0, 0, response.DataAsTexture2D.width, response.DataAsTexture2D.height ), Vector2.zero );
        image.overrideSprite = sprite;

        loaded = true;
        image.gameObject.SetActive( true );
        spinIcon.gameObject.SetActive( false );
        var color = GetComponent<ProceduralImage>( ).color;
        var button = GetComponent<PUIButton>();
        button.enabled = true;
        button.colorUp = color;


        in_load--; Next( );
    }
}
