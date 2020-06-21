
using UnityEngine;

public static class PUIEx
{
    public static Color ColorGS( float grayScale = 0.5f, float alpha = 1f ) 
    {
        var c = Color.HSVToRGB( 0, 0, grayScale ); 
        c.a = alpha; 
        return c;
    }

    public static T GetAddComp<T>( this MonoBehaviour self ) where T : Component
    {
        return self.GetComponent<T>( ) ?? self.gameObject.AddComponent<T>( );
    }

    /// Return true if values were different and value was set to be equal to target , return false if value already equals target 
    public static bool SetIfNotEqual<T>( this MonoBehaviour self, ref T value, T target ) => SetIfNotEqual( ref value, target );
    
    /// Return true if values were different and value was set to be equal to target , return false if value already equals target 
    public static bool SetIfNotEqual<T>( ref T value, T target )
    {
        if( value.Equals( target ) ) return false;
        else value = target;
        return true;
    }

}

