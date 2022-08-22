using System;
using System.Collections;
using UnityEngine;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class DelayedExecutionBehaviour : MonoBehaviour, IDelayedExecutionBehaviour
    {
        private IDisposable[] presenters_;
        
        public void Init(IDisposable[] presenters)
        {
            presenters_ = presenters;
        }
        
        public void ExecuteWithDelay(Action action, float delay)
        {
            StartCoroutine(WaitAndExecute());
                
            IEnumerator WaitAndExecute()
            {
                var remainingDelaytime = delay;
                while (remainingDelaytime > 0f)
                {
                    remainingDelaytime -= Time.unscaledDeltaTime;
                    yield return null;
                }
                action.Invoke();
            }
        }

        private void OnDestroy()
        {
            foreach (var presenter in presenters_)
                presenter.Dispose();
        }
    }
}
