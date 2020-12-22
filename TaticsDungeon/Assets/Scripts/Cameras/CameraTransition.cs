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
        private TownCameraHandler handler;
        private Transform lookAtPoint;

        [System.Serializable]
        public class TweenHolder
        {
            public enum TravelMode { Linear, Bezier}
            public TravelMode travelMode = TravelMode.Linear;
            public LeanTweenType curve;
            public float ratio = 1;
            [ConditionalHide("travelMode", 0)]
            public Transform externalPosition;
            [ConditionalHide("travelMode", 1)]
            public BezierHandler bezier;

            public enum CameraMode { Linear, FocalPoint}
            public CameraMode cameraMode = CameraMode.Linear;
            [ConditionalHide("cameraMode", 0)]
            public LinearCamera linearCamera;
            [ConditionalHide("cameraMode", 1)]
            public FocalPointCamera focalPointCamera;
        }

        [System.Serializable]
        public class LinearCamera
        {
            public LeanTweenType curve;
        }

        [System.Serializable]
        public class FocalPointCamera
        {
            public Transform point;
            [Range(1f, 100.0f)]
            public float focusRate = 20f;
            public LeanTweenType focusCurve;
        }

        public void Transition(TownCameraHandler h)
        {
            ratioSum = 0;
            handler = h;
            foreach (TweenHolder tween in tweens)
            {
                ratioSum += tween.ratio;
            }
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

            TweenHolder holder = tweens[index];
            Transform destination;
            if (index + 1 >= tweens.Length)
                destination = transform;
            else
                destination = holder.externalPosition;

            float travelLength = tweens[index].ratio / ratioSum * length;

            //Movement handler
            switch (holder.travelMode)
            {
                case TweenHolder.TravelMode.Linear:
                    LeanTween.move(handler.gameObject, destination.position, travelLength).
                        setEase(holder.curve).
                        setOnComplete(IncrementTween);
                    LeanTween.rotate(handler.gameObject, destination.rotation.eulerAngles, travelLength).
                        setEase(holder.curve);
                    break;
                case TweenHolder.TravelMode.Bezier:
                    destination = holder.bezier.points[3].transform;
                    LTBezierPath ltbPath = new LTBezierPath(
                        new Vector3[] {
                            holder.bezier.points[0].transform.position,
                            holder.bezier.points[2].transform.position,
                            holder.bezier.points[1].transform.position,
                            destination.transform.position
                    });
                    LeanTween.move(handler.gameObject, ltbPath, travelLength).
                        setEase(holder.curve).
                        setOnComplete(IncrementTween);
                    break;
            }

            //Rotation Handler
            switch (holder.cameraMode)
            {
                case TweenHolder.CameraMode.Linear:
                    LeanTween.rotate(handler.gameObject, destination.rotation.eulerAngles, travelLength).
                        setEase(holder.linearCamera.curve);
                    break;
                case TweenHolder.CameraMode.FocalPoint:
                    LookAtFocalPoint();
                    break;
            }
            index++;
        }

        void LookAtFocalPoint()
        {
            handler.LookAtPoint(tweens[index].focalPointCamera.point, 1/tweens[index].focalPointCamera.focusRate);
        }
    }
}