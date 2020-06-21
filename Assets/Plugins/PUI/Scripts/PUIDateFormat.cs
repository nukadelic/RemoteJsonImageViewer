using TMPro;
using UnityEngine;
using Date = System.DateTime;
using Regex = System.Text.RegularExpressions.Regex;

[RequireComponent(typeof(TextMeshProUGUI))]
public class PUIDateFormat : MonoBehaviour
{
    public bool dateIsNow = true;

    [Tooltip("Date string in dd-mm-yyyy format ( seperator isn't required )")]
    public string ddmmyyyy = "01/01/1991";

    TextMeshProUGUI tmp = null;

    Regex digits;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>( );

        digits = new Regex( "^[0-9]*$" );
    }

    Date GetDate()
    {
        if( dateIsNow  ) return new Date( );

        var match = digits.Match( ddmmyyyy );

        var d = match.Value;
        match.NextMatch( );
        var m = match.Value;
        match.NextMatch( );
        var y = match.Value;

        bool parseOk = true;

        parseOk &= int.TryParse( d, out int _d );
        parseOk &= int.TryParse( m, out int _m );
        parseOk &= int.TryParse( y, out int _y );

        if( ! parseOk )
        {
            Debug.LogError( "Bad date input value" );
            return new Date();
        }

        return new Date( _y, _m, _d );
    }

    void Update()
    {
        var date = GetDate( );

        tmp.text = date.ToString( "d" );
    }
}