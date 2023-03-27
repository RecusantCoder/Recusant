using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBoomBehaviour : MonoBehaviour
{
    //private CircleCollider2D circleCol;
    private SpriteRenderer _dustDevilColor;
    public GameObject dustDevil;
    private bool _triggered = false;
    private CircleCollider2D _forceBoom;
    
    // Start is called before the first frame update
    void Start()
    {
        // = dustDevil.GetComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        _dustDevilColor = dustDevil.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
        _forceBoom = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (circleCol.isTrigger && !triggered)
        {
            Debug.Log("OOHOHOHOHOHOHOHOHOHOHOHOHO");
            triggered = true;
        }*/

        if (_dustDevilColor.color == Color.white && !_triggered)
        {
            _forceBoom.radius += 0.1f;
            Debug.Log("OOHOHOHOHOHOHOHOHOHOHOHOHO");
            _triggered = true;
        }
    }
}
