using UnityEngine;
using System.Collections;

namespace YAProgressBar
{
    /// <summary>
    /// Use this class if you want to change progressbar value not in interval [0-1], but in some other values (e.g. [1-100] or [-1000, 5000])
    /// </summary>
    public class ProgressBarAdapter : MonoBehaviour
    {
        [SerializeField]
        private ProgressBarBaseMesh progressBar;
        [SerializeField]
        private float min;
        [SerializeField]
        private float max;
        [SerializeField]
        private float value;

        public float Min
        {
            get
            {
                return min;
            }

            set
            {
                min = value;
                UpdateProgressBar();
            }
        }

        public float Max
        {
            get
            {
                return max;
            }

            set
            {
                max = value;
                UpdateProgressBar();
            }
        }

        public float Value
        {
            get
            {
                return value;
            }

            set
            {
                this.value = value;
                UpdateProgressBar();
            }
        }

        void OnValidate()
        {
            if (!progressBar.IsInitialised)
            {
                return;
            }
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            progressBar.FillAmount = Mathf.InverseLerp(min, max, value);
        }
    }
}
