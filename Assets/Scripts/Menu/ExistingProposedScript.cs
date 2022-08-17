using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExistingProposedScript : MonoBehaviour {

    epGameobjectsScript epParent;


    public int existingInt; // SPRITE
    public int proposedInt; // SPRITE

    public Sprite[] blueSprites; // SPRITE
    public Sprite[] checkSprites; // SPRITE

    public Image existingUI; // SPRITE
    public Image proposedUI; // SPRITE
    public Image existingcheckUI; // SPRITE
    public Image proposedcheckUI; // SPRITE

    // Use this for initialization
    void Start () {

        epParent = GameObject.Find("epGameObjects").GetComponent<epGameobjectsScript>();

        epParent.Proposed();
    }

    // Update is called once per frame
 //   void Update () {
		
	//}

    public void ToggleExisting()
    {
        epParent.Existing();

        // SPRITES
        existingInt = 1;
        proposedInt = 0;
        // BLUE
        existingUI.sprite = blueSprites[existingInt];
        proposedUI.sprite = blueSprites[proposedInt];
        // CHECKS
        existingcheckUI.sprite = checkSprites[existingInt];
        proposedcheckUI.sprite = checkSprites[proposedInt];
    }

    public void ToggleProposed()
    {
        epParent.Proposed();

        // SPRITES
        existingInt = 0;
        proposedInt = 1;
        // BLUE
        existingUI.sprite = blueSprites[existingInt];
        proposedUI.sprite = blueSprites[proposedInt];
        // CHECKS
        existingcheckUI.sprite = checkSprites[existingInt];
        proposedcheckUI.sprite = checkSprites[proposedInt];
    }
}
