using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CivilFX
{
    public class PhasesController : MonoBehaviour
    {
        [SerializeField]
        private Button _existingOnlyButton;
        [SerializeField]
        private Image _existingOnlyFill;

        [SerializeField]
        private Button _designButton;
        [SerializeField]
        private Image _designFill;

        [SerializeField]
        private Button _phase1Button;
        [SerializeField]
        private Image _phase1Fill;

        [SerializeField]
        private Button _phase2Button;
        [SerializeField]
        private Image _phase2Fill;


        [SerializeField]
        private Button _phase3Button;
        [SerializeField]
        private Image _phase3Fill;

        [SerializeField]
        private Button _phase4Button;
        [SerializeField]
        private Image _phase4Fill;

        private void Start()
        {
            _designFill.enabled = false;
            _phase1Fill.enabled = false;
            _phase2Fill.enabled = false;
            _phase3Fill.enabled = false;
            _phase4Fill.enabled = false;
            DesignClicked();

            _existingOnlyButton.onClick.AddListener(ExistingClicked);
            _designButton.onClick.AddListener(DesignClicked);
            _phase1Button.onClick.AddListener(Phase1Clicked);
            _phase2Button.onClick.AddListener(Phase2Clicked);
            _phase3Button.onClick.AddListener(Phase3Clicked);
            _phase4Button.onClick.AddListener(Phase4Clicked);
        }

        private void OnDestroy()
        {
            _existingOnlyButton.onClick.RemoveListener(ExistingClicked);
            _existingOnlyButton.onClick.RemoveListener(DesignClicked);
            _phase1Button.onClick.RemoveListener(Phase1Clicked);
            _phase2Button.onClick.RemoveListener(Phase2Clicked);
            _phase3Button.onClick.RemoveListener(Phase3Clicked);
            _phase4Button.onClick.RemoveListener(Phase4Clicked);
        }

        private void TogglePhase(Phase toggle, Image image)
        {
            Phase phase = PhaseManager.CurrentPhase ^ toggle;

            PhaseManager.RaisePhaseShift(phase);

            if ((phase & toggle) == toggle)
            {
                if (image != null)
                    image.enabled = true;
            }
            else
            {
                if (image != null)
                    image.enabled = false;
            }
        }

        #region Button Functions
        private void ExistingClicked()
        {
            PhaseManager.RaisePhaseShift(Phase.Existing);
            _designFill.enabled = false;
            _phase1Fill.enabled = false;
            _phase2Fill.enabled = false;
            _phase3Fill.enabled = false;
            _phase4Fill.enabled = false;
        }
        private void DesignClicked()
        {
            TogglePhase(Phase.Ultimate, _designFill);
        }
        private void Phase1Clicked()
        {
            TogglePhase(Phase.Phase1, _phase1Fill);
        }
        private void Phase2Clicked()
        {
            TogglePhase(Phase.Phase2, _phase2Fill);
        }
        private void Phase3Clicked()
        {
            TogglePhase(Phase.Phase3, _phase3Fill);
        }
        private void Phase4Clicked()
        {
            TogglePhase(Phase.Phase4, _phase4Fill);
        }
        #endregion
    }
}