using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class spriteSwapScript : MonoBehaviour {

    public int menuInt;

    public Sprite[] menuSprites;

    public Image menuUI;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void SwapSprite() {
        menuInt++;

        if (menuInt > 1) {
            menuInt = 0;
        }

        menuUI.sprite = menuSprites[menuInt]; 
    }


}

