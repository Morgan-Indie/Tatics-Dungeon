using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CameraTransition : MonoBehaviour
    {

        public TweenHolder[] tweens;
        public float length = 1f;
        private float ratioSum = 0;
        private int index = 0;
        private List<Transform> destinations;
        
        private TownCameraHandler handler;

        [System.Serializable]
        public class TweenHolder
        {
            public LeanTweenType curve;
            public float ratio = 1;
            public Transform externalPosition;
        }

        private void Start()
        {
            destinations = new List<Transform>();
        }

        public void Transition(TownCameraHandler h)
        {
            ratioSum = 0;
            handler = h;
            foreach (TweenHolder tween in tweens)
            {
                ratioSum += tween.ratio;
                destinations.Add(tween.externalPosition);
            }
            destinations[tweens.Length - 1] = transform;
            IncrementTween();
        }

        public void IncrementTween()
        {
            if (index >= tweens.Length)
            {
                index = 0;
                handler.CompleteTransition(transform);
                handler = null;
                return;
            }
            Transform destination = destinations[index];
            LeanTween.move(handler.gameObject, destination.position, tweens[index].ratio / ratioSum * length).
                setEase(tweens[index].curve).
                setOnComplete(IncrementTween);
            LeanTween.rotate(handler.gameObject, destination.rotation.eulerAngles, tweens[index].ratio / ratioSum * length).
                setEase(tweens[index].curve);
            index++;
        }
    }
}