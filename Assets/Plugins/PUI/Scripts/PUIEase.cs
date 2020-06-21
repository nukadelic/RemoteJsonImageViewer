
// ease math functions from : https://gist.github.com/gre/1650294

using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PUIEase : MonoBehaviour
{
    public static List<PUIEase> inputs = new List<PUIEase>();

    /// <summary>
    /// can be changed OnEnd just before remaval
    /// </summary>
    public bool autoRemoveOnEnd { get; set; } = true;
    public bool IsPlaying { get; private set; } = false;
    public float value { get; private set; } = 0f;
    public EaseType easeType { get; set; }
    public float duration { get; private set; } = 0;

    public event System.Action OnEnd;

    public event System.Action<float> OnUpdate;

    float startTime = 0f;

    public void Begin()
    {
        if( IsPlaying ) return;

        IsPlaying = true;

        startTime = Time.time;
    }

    void Update( )
    {
        if( ! IsPlaying ) return; 

        var t = ( Time.time - startTime ) / duration;

        value = Functions.Ease( easeType, t );

        OnUpdate?.Invoke( value );

        if( t >= 1 ) End( );
    }

    public void End()
    {
        if( ! IsPlaying ) return;

        IsPlaying = false;

        value = 1;

        OnEnd?.Invoke( );

        if( autoRemoveOnEnd ) Remove();
    }

    public void Remove()
    {
        enabled = false;
        Destroy( this );
    }

    public static PUIEase Ease( GameObject go, float duration = 1f, PUIEase.EaseType ease = PUIEase.EaseType.InQuad )
    {
        var e = go.AddComponent<PUIEase>( );
        e.easeType = ease;
        e.duration = duration;
        e.Begin( );
        return e;
    }

    public enum EaseType
    {
        /// no easing, no acceleration
        Linear
        /// accelerating from zero velocity
        , InQuad
        /// decelerating to zero velocity
        , OutQuad
        /// acceleration until halfway, then deceleration
        , InOutQuad
        /// accelerating from zero velocity 
        , InCubic
        /// decelerating to zero velocity 
        , OutCubic
        /// acceleration until halfway, then deceleration 
        , InOutCubic
        /// accelerating from zero velocity 
        , InQuart
        /// decelerating to zero velocity 
        , OutQuart
        /// acceleration until halfway, then deceleration
        , InOutQuart
        /// accelerating from zero velocity
        , InQuint
        /// decelerating to zero velocity
        , OutQuint
        /// acceleration until halfway, then deceleration 
        , InOutQuint
    }
    public static class Functions
    {
        public static float Ease( EaseType ease, float t )
        {
            switch( ease )
            {
                default: case EaseType.Linear: return t;
                case EaseType.InQuad: return Functions.EaseInQuad( t );
                case EaseType.OutQuad: return Functions.EaseOutQuad( t );
                case EaseType.InOutQuad: return Functions.EaseInOutQuad( t );
                case EaseType.InCubic: return Functions.EaseInCubic( t );
                case EaseType.OutCubic: return Functions.EaseOutCubic( t );
                case EaseType.InOutCubic: return Functions.EaseInOutCubic( t );
                case EaseType.InQuart: return Functions.EaseInQuart( t );
                case EaseType.OutQuart: return Functions.EaseOutQuart( t );
                case EaseType.InOutQuart: return Functions.EaseInOutQuart( t );
                case EaseType.InQuint: return Functions.EaseInQuint( t );
                case EaseType.OutQuint: return Functions.EaseOutQuint( t );
                case EaseType.InOutQuint: return Functions.EaseInOutQuint( t );
            }
        }

        public static float EaseInQuad( float t ) => t * t;
        public static float EaseOutQuad( float t ) => t * ( 2 - t );
        public static float EaseInOutQuad( float t ) => t < .5 ? 2 * t * t : -1 + ( 4 - 2 * t ) * t;
        public static float EaseInCubic( float t ) => t * t * t;
        public static float EaseOutCubic( float t ) => ( --t ) * t * t + 1;
        public static float EaseInOutCubic( float t ) => t < .5 ? 4 * t * t * t : ( t - 1 ) * ( 2 * t - 2 ) * ( 2 * t - 2 ) + 1;
        public static float EaseInQuart( float t ) => t * t * t * t;
        public static float EaseOutQuart( float t ) => 1 - ( --t ) * t * t * t;
        public static float EaseInOutQuart( float t ) => t < .5 ? 8 * t * t * t * t : 1 - 8 * ( --t ) * t * t * t;
        public static float EaseInQuint( float t ) => t * t * t * t * t;
        public static float EaseOutQuint( float t ) => 1 + ( --t ) * t * t * t * t;
        public static float EaseInOutQuint( float t ) => t < .5 ? 16 * t * t * t * t * t : 1 + 16 * ( --t ) * t * t * t * t;
    }
 }

public static class PUIEaseEx
{
    public static PUIEase Ease( this GameObject go, float duration = 1f, PUIEase.EaseType ease = PUIEase.EaseType.InQuad )
        => PUIEase.Ease( go, duration, ease );
    public static PUIEase Ease( this Transform train, float duration = 1f, PUIEase.EaseType ease = PUIEase.EaseType.InQuad )
        => PUIEase.Ease( train.gameObject, duration, ease );
    public static PUIEase Ease( this MonoBehaviour mono, float duration = 1f, PUIEase.EaseType ease = PUIEase.EaseType.InQuad )
        => PUIEase.Ease( mono.gameObject, duration, ease );
}
