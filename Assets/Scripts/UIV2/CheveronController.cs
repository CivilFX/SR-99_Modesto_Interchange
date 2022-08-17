using CivilFX.Generic2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CivilFX.UI2
{
    public class CheveronController : MonoBehaviour
    {

        private Coroutine routine;
        private GameObject lastObj;

        public bool isPlaying
        {
            get; private set;
        }

        bool isShowingTemp = false;

        public void StartCheveron(AnimatedCameraPathData asset, CustomButton previewButton)
        {
            if (routine != null)
            {
                Stop(asset, previewButton);
            }
            routine = StartCoroutine(CheveronRoutine(asset));
            isPlaying = true;

            // Material Swap
            var referencedObjs = new List<MaterialsSwapper>();

            foreach (var item in Resources.FindObjectsOfTypeAll<MaterialsSwapper>())
            {
                if (item.referencedName.Equals(asset.objName))
                {
                    referencedObjs.Add(item);
                    isShowingTemp = item.isShowingTemp;

                }

                else
                {
                    item.rend.material = item.mainMats[0];
                    item.isShowingTemp = false;
                }

            }

            foreach (var item in referencedObjs)
            {
                isShowingTemp = item.SwapMaterial();
            }


            // Eye Color
            previewButton.mainImage.color = asset.color;
        }

        public void Stop(AnimatedCameraPathData asset, CustomButton previewButton)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
            routine = null;

            //if (lastObj != null) {
            //    lastObj.SetActive(false);
            //}
            //lastObj = null;
            isPlaying = false;


            // Material Swap
            var referencedObjs = new List<MaterialsSwapper>();

            foreach (var item in Resources.FindObjectsOfTypeAll<MaterialsSwapper>())
            {
                if (item.referencedName.Equals(asset.objName))
                {
                    referencedObjs.Add(item);
                    isShowingTemp = item.isShowingTemp;

                }
            }

            foreach (var item in referencedObjs)
            {
                isShowingTemp = item.SwapMaterial();
            }
        }


        private IEnumerator CheveronRoutine(AnimatedCameraPathData asset)
        {
            //find the object
            GameObject go = null;
            foreach (var obj in Resources.FindObjectsOfTypeAll<SingleObjectReference>())
            {
                if (obj.name.Equals(asset.objName))
                {
                    go = obj.referencedObject;
                    break;
                }
            }

            if (go == null)
            {
                yield break;
            }
            go.SetActive(true);
            lastObj = go;
            //find mat
            var mat = asset.sequence.material;
            mat.color = asset.color;
            var sequences = asset.sequence.sprites;
            var currentIndex = 0;
            var timeStep = .1f;
            var currentTime = 0f;
            while (true)
            {
                if (currentTime <= 0)
                {
                    mat.mainTexture = sequences[currentIndex++];
                    currentIndex %= sequences.Length;
                    currentTime = timeStep;
                }
                currentTime -= Time.deltaTime;
                yield return null;
            }
        }
    }
}