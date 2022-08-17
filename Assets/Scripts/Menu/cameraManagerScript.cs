using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

public class cameraManagerScript : MonoBehaviour {


    public PlayableDirector director1;
    public PlayableDirector director2;
    public PlayableDirector director3;
    public PlayableDirector director4;
    public PlayableDirector director5;
    public PlayableDirector director6;



    //public CinemachineVirtualCamera freeCam;

    public GameObject mainCam;

    public GameObject freeCamText;

    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject cam5;
    public GameObject cam6;

    public GameObject freeCam;

    public GameObject stillCam1;
    public GameObject stillCam2;
    public GameObject stillCam3;
    public GameObject stillCam4;
    public GameObject stillCam5;
    public GameObject stillCam6;
    public GameObject stillCam7;
    public GameObject stillCam8;
    public GameObject stillCam9;
    public GameObject stillCam10;
    public GameObject stillCam11;
    public GameObject stillCam12;

    public GameObject playpauseButtons;

    // Use this for initialization
    void Start()
    {
        freeCamText.SetActive(false);

        cam2.SetActive(false);
        cam3.SetActive(false);
        cam4.SetActive(false);
        cam5.SetActive(false);
        cam6.SetActive(false);

        freeCam.SetActive(false);

        stillCam1.SetActive(false);
        stillCam2.SetActive(false);
        stillCam3.SetActive(false);
        stillCam4.SetActive(false);
        stillCam5.SetActive(false);
        stillCam6.SetActive(false);
        stillCam7.SetActive(false);
        stillCam8.SetActive(false);
        stillCam9.SetActive(false);
        stillCam10.SetActive(false);
        stillCam11.SetActive(false);
        stillCam12.SetActive(false);
    }

    // Update is called once per frame
    //void Update()
    //{
    //}

    public void Button1() {
        FalseAll();
        cam1.SetActive(true);

        EnableButtons();
    }

    public void Button2()
    {
        FalseAll();
        cam2.SetActive(true);

        EnableButtons();
    }

    public void Button3()
    {
        FalseAll();
        cam3.SetActive(true);

        EnableButtons();
    }

    public void Button4()
    {
        FalseAll();
        cam4.SetActive(true);

        EnableButtons();
    }

    public void Button5()
    {
        FalseAll();
        cam5.SetActive(true);

        EnableButtons();
    }

    public void Button6()
    {
        FalseAll();
        cam6.SetActive(true);

        EnableButtons();
    }

    public void View1()
    {
        FalseAll();
        stillCam1.SetActive(true);

    }

    public void View2()
    {
        FalseAll();
        stillCam2.SetActive(true);

    }

    public void View3()
    {
        FalseAll();
        stillCam3.SetActive(true);

    }

    public void View4()
    {
        FalseAll();
        stillCam4.SetActive(true);

    }

    public void View5()
    {
        FalseAll();
        stillCam5.SetActive(true);

    }

    public void View6()
    {
        FalseAll();
        stillCam6.SetActive(true);

    }

    public void View7()
    {
        FalseAll();
        stillCam7.SetActive(true);

    }

    public void View8()
    {
        FalseAll();
        stillCam8.SetActive(true);

    }

    public void View9()
    {
        FalseAll();
        stillCam9.SetActive(true);

    }

    public void View10()
    {
        FalseAll();
        stillCam10.SetActive(true);

    }

    public void View11()
    {
        FalseAll();
        stillCam11.SetActive(true);

    }

    public void View12()
    {
        FalseAll();
        stillCam12.SetActive(true);

    }


    public void FalseAll() {

        cam1.SetActive(false);
        cam2.SetActive(false);
        cam3.SetActive(false);
        cam4.SetActive(false);
        cam5.SetActive(false);
        cam6.SetActive(false);

        freeCam.SetActive(false);
        freeCamText.SetActive(false);

        stillCam1.SetActive(false);
        stillCam2.SetActive(false);
        stillCam3.SetActive(false);
        stillCam4.SetActive(false);
        stillCam5.SetActive(false);
        stillCam6.SetActive(false);
        stillCam7.SetActive(false);
        stillCam8.SetActive(false);
        stillCam9.SetActive(false);
        stillCam10.SetActive(false);
        stillCam11.SetActive(false);
        stillCam12.SetActive(false);


        Play();
        FalseButtons();
    }



    // FREE CAM
    public void FreeCam()
    {

        FalseAll();
        freeCamText.SetActive(true);

        freeCam.SetActive(true);
    }


    public void Play()
    {
        director1.enabled = true;
        director2.enabled = true;
        director3.enabled = true;
        director4.enabled = true;
        director5.enabled = true;
        director6.enabled = true;

    }
    public void Pause() {
        director1.enabled = false;
        director2.enabled = false;
        director3.enabled = false;
        director4.enabled = false;
        director5.enabled = false;
        director6.enabled = false;
    }

    public void FalseButtons() {
        playpauseButtons.SetActive(false);
    }

    public void EnableButtons()
    {
        playpauseButtons.SetActive(true);
    }


}
