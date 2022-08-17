using UnityEngine;
using System.Collections;

public class RenderElement : MonoBehaviour {

    #region Inspector Fields
    #endregion

    #region Properties
    public bool RenderingEnabled
    {
        get { return _renderingEnabled;  }
        private set { }
    }
    #endregion

    #region Fields
    bool _renderingEnabled = false;
    #endregion

    #region Inspector Overrides
    void Awake()
    {
        RenderElementManager.GetInstance();
        RenderElementManager.REToggle += RenderingToggled;
        Debug.Log("Registered for Render Toggle");
        RenderingToggled(_renderingEnabled);
    }

    void OnDestroy()
    {
        RenderElementManager.REToggle -= RenderingToggled;
    }
    #endregion

    #region Callbacks
    public void RenderingToggled(bool displayFeature)
    {
        _renderingEnabled = displayFeature;
        gameObject.SetActive(displayFeature);
    }
    #endregion
}
