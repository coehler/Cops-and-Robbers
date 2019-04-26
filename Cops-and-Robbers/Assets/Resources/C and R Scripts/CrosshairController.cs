using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour{

    public RectTransform top, bottom, left, right;
    public RaycastBulletController bulletController;

    private readonly float maxLocation = 100.0f;
    private readonly float minLocation = 10.0f;

    // Start is called before the first frame update.
    void Start(){

    }

    // Update is called once per frame.
    void Update(){

        top.anchoredPosition = new Vector2(0, minLocation + (bulletController.spreadCurrent * maxLocation));
        bottom.anchoredPosition = new Vector2(0, -minLocation - (bulletController.spreadCurrent * maxLocation));
        left.anchoredPosition = new Vector2(-minLocation - (bulletController.spreadCurrent * maxLocation), 0);
        right.anchoredPosition = new Vector2(minLocation + (bulletController.spreadCurrent * maxLocation), 0);

    }
}
