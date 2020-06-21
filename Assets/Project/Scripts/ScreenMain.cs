using TMPro;
using UnityEngine;

public class ScreenMain : MonoBehaviour
{
    [SerializeField] GameObject ImageContainerPrefab = null;
    [SerializeField] GameObject scrollContainer = null;
    [SerializeField] PUIBottomSlideDrag popUp = null;
    [SerializeField] TextMeshProUGUI text = null;

    void Start()
    {
        var data = SessionData.instance.data1;

        GameObject item = null;
        int count = 0;

        foreach( var img in data.outline_images )
        {
            item = Instantiate( ImageContainerPrefab );
            item.name = "Item(" + ( count++ ) + ")";
            var imgCont = item.GetComponent<ImageContainer>( );
            item.transform.SetParent( scrollContainer.transform );
            item.transform.localScale = Vector3.one;
            item.GetComponent<PUIButton>( ).OnClick += OnClick;
            imgCont.Load( img );
        }
    }

    void OnClick( PUIButton btn )
    {
        popUp.Show( );
        var data = btn.gameObject.GetComponent<ImageContainer>( ).data;
        text.text = "";
        text.text += "<b>" + data.title + "</b>\n";
        text.text += "Date: " + data.created_at.Substring( 0, 10 ) + "\n";
        text.text += "Favorites: " + data.favorites_count + "\n";
        text.text += "Likes: " + data.stat.likes_count + "\n";
        text.text += "Comments: " + data.stat.comments_count + "\n";
    }
}
