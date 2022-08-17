using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class menuButtons : MonoBehaviour {

    public Animator menuAnim; // SLIDE OUT ANIM

    public int menusInt;
    // NAV BUTTON
    public int buttonNavInt;
    public Sprite[] navSprites;
    public Image navUI;
    // RENDER BUTTON
    public int buttonRenderInt;
    public Sprite[] renderSprites;
    public Image renderUI;
    // SETTINGS BUTTON
    public int buttonSettingsInt;
    public Sprite[] settingsSprites;
    public Image settingsUI;
    // INFO BUTTON
    public int buttonInfoInt;
    public Sprite[] infoSprites;
    public Image infoUI;

    public Text menuText;


    // Navigation Options
    public int curNav;

    public int nav1;
    public int nav2;
    public int nav3;

    // MENU PAGES
    public GameObject page1;
    public GameObject page2;
    public GameObject page3;
    public GameObject page4;


    // Use this for initialization
    void Start () {
        ButtonNav();
	}
	
	// Update is called once per frame
	//void Update () {
		
	//}




    // ANIM
    public void TriggerMenu() {
        menuAnim.SetTrigger("trigger");
    }





    public void ButtonNav() {
        menusInt = 1;
        menuText.text = "NAVIGATION";

        buttonNavInt = 1;
        buttonRenderInt = 0;
        buttonSettingsInt = 0;
        buttonInfoInt = 0;

        SwapSprites();
        ChangePage();
    }
    public void ButtonRender()
    {
        menusInt = 2;
        menuText.text = "RENDER";

        buttonNavInt = 0;
        buttonRenderInt = 1;
        buttonSettingsInt = 0;
        buttonInfoInt = 0;

        SwapSprites();
        ChangePage();
    }
    public void ButtonSett()
    {
        menusInt = 3;
        menuText.text = "SETTINGS";


        buttonNavInt = 0;
        buttonRenderInt = 0;
        buttonSettingsInt = 1;
        buttonInfoInt = 0;

        SwapSprites();
        ChangePage();

    }
    public void ButtonInfo()
    {
        menusInt = 4;
        menuText.text = "ABOUT";


        buttonNavInt = 0;
        buttonRenderInt = 0;
        buttonSettingsInt = 0;
        buttonInfoInt = 1;

        SwapSprites();
        ChangePage();

    }

    // CHANGE TOP PANEL IMAGES TO GREEN
    public void SwapSprites() {
        navUI.sprite = navSprites[buttonNavInt];
        renderUI.sprite = renderSprites[buttonRenderInt];
        settingsUI.sprite = settingsSprites[buttonSettingsInt];
        infoUI.sprite = infoSprites[buttonInfoInt];
    }

    public void ChangePage() {
        if (menusInt == 1) {
            page1.SetActive(true);
            page2.SetActive(false);
            page3.SetActive(false);
            page4.SetActive(false);
        }
        if (menusInt == 2)
        {
            page1.SetActive(false);
            page2.SetActive(true);
            page3.SetActive(false);
            page4.SetActive(false);
        }
        if (menusInt == 3)
        {
            page1.SetActive(false);
            page2.SetActive(false);
            page3.SetActive(true);
            page4.SetActive(false);
        }
        if (menusInt == 4)
        {
            page1.SetActive(false);
            page2.SetActive(false);
            page3.SetActive(false);
            page4.SetActive(true);
        }
    }


    public void Website() {
        Application.OpenURL("http://civilfx.com/");
    }
}
