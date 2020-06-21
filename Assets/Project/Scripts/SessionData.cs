using UnityEngine;

public class SessionData : MonoBehaviour
{
    public static SessionData instance;

    private void Awake() => DontDestroyOnLoad( this );

    private void OnEnable( ) => instance = this;

    public JsonData1 data1;
}
