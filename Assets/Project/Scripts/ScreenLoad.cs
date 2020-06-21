using BestHTTP;
using System;
using UnityEngine;
using TMPro;

public class ScreenLoad : MonoBehaviour
{
    [SerializeField] PUIProgressBar progressBar = null;
    [SerializeField] TextMeshProUGUI header = null;
    [SerializeField] GameObject mainScreen = null;

    void Start( )
    {
        HTTPRequest request = new HTTPRequest(new Uri( JsonUrl.Value ), OnRequestFinished);
        request.Send( );
        request.OnDownloadProgress = OnProgress;
    }

    void OnProgress( HTTPRequest originalRequest, long downloaded, long downloadLength )
    {
        float progress = downloaded / ( ( float ) downloadLength );
        progressBar.Value = progress;
    }

    void Error( string txt )
    {
        progressBar.gameObject.SetActive( false );
        header.text = "Error\n" + txt;
    }

    private void OnRequestFinished( HTTPRequest originalRequest, HTTPResponse response )
    {
        if( ! response.IsSuccess )
        {
            Error( "Bad URL" );
            return;
        }
        try
        {
            SessionData.instance.data1 = JsonUtility.FromJson<JsonData1>( response.DataAsText );
            //Debug.Log( data.outline_images.Length );
            //Debug.Log( data.outline_images[ 0 ].svg.original.url );
        }
        catch( System.Exception ex )
        {
            Error( "Bad Data" + "\n" + ex.Message );
            return;
        }

        var MCR = mainScreen.GetComponent<RectTransform>( );

        header.text = "Loaded";
        MCR.anchoredPosition = new Vector2( 500, 0 );
        mainScreen.SetActive( true );

        var ease = this.Ease( 0.6f );
        ease.OnUpdate += x => {

            GetComponent<RectTransform>( ).anchoredPosition = new Vector2( -500 * x , 0 );
            MCR.anchoredPosition = new Vector2( 500 * ( 1 - x ), 0 );
        };

        ease.OnEnd += ( ) =>
        {
            gameObject.SetActive( false );
            MCR.anchoredPosition = Vector2.zero;
        };

    }
}
