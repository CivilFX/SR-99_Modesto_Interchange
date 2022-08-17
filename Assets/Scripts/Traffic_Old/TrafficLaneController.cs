using UnityEngine;
using System.Collections;

namespace CivilFX
{
    [RequireComponent(typeof(Animator))]
    public class TrafficLaneController : MonoBehaviour
    {

        #region Fields
        private Animator _animator;
        private float _cachedPlaybackTime = 0.0f;
        private int _cachedStateName;
        private bool _streetExitA = false;
        private bool _streetExitB = false;
        private bool _alternate = false;
        private bool _cycleOffset = false;
        #endregion

        #region Behaviour Overrides
        private void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            PhaseManager.PhaseShifted += PhaseShifted;
        }

        private void OnEnable()
        {
            if (_animator != null)
            {
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                if (_streetExitA == true)
                {
                    _animator.SetBool("StreetExitA", true);
                }
                if (_streetExitB == true)
                {
                    _animator.SetBool("StreetExitB", true);
                }
                if (_alternate == true)
                {
                    _animator.SetTrigger("Alternate");
                }
                if (_cycleOffset == true)
                {
                    _animator.SetFloat("CycleOffset", _cachedPlaybackTime);
                }
                _animator.Play(_cachedStateName, -1, _cachedPlaybackTime);
                _animator.speed = 1.0f;
            }
        }

        void OnDestroy()
        {
            PhaseManager.PhaseShifted -= PhaseShifted;
        }
        #endregion

        public void PhaseShifted(Phase newPhase)
        {
            if (_animator != null && _animator.runtimeAnimatorController != null && gameObject.activeInHierarchy && _cachedPlaybackTime <= 0.0f)
            {
                AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                _cachedStateName = stateInfo.fullPathHash;
            }
        }

        public void SetCachedPlaybackTime(float playback_time)
        {
            _cachedPlaybackTime = playback_time;
        }

        public void SetStreetExitA()
        {
            _streetExitA = true;
        }

        public void SetStreetExitB()
        {
            _streetExitB = true;
        }

        public void SetAlternate()
        {
            _alternate = true;
        }

        public void SetCycleOffset()
        {
            _cycleOffset = true;
        }
    }
}