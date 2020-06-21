using UnityEngine;

public class PUIOverlay : MonoBehaviour
{
    private void OnEnable( )
    {
        PUIMouseTouch.GlobalDepth ++ ;

        foreach( var input in GetComponentsInChildren<PUIMouseTouch>( true ) )
        {
            input.depth ++ ;
        }
    }

    private void OnDisable( )
    {
        PUIMouseTouch.GlobalDepth -- ;

        foreach(var input in GetComponentsInChildren<PUIMouseTouch>( true ))
        {
            input.depth -- ;
        }
    }
}
