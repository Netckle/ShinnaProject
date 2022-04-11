using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHINNAGame
{
    public class SpinningTop : MonoBehaviour
    {
        // Gizmo
        public float sidesRadius = 1.0f;

        public float oneRotationTime = 0f;
        private float time = 0f;
        public float halfHorizontalDistance = 0f;
        public Transform[] sides = new Transform[2];
        public Transform modelParent = null;
        public float boostMaintainTime = 0f;

        public float startInterval = 0f;
        public float boostStartTime;

        public ParticleSystem spakle;

        public bool IsReversed
        {
            get { return isReversed; }
            set
            {
                StopAllCoroutines();
                StartCoroutine(SpeedUpTemporarily());
                isReversed = value;
                switch (value)
                { 
                    case true:                        
                        StartCoroutine(MoveReverse(interval));
                        break;
                    case false:
                        StartCoroutine(Move(interval));
                        break;
                }
            }
        }
        private bool isReversed = false;

        private float interval = 0f;

        private Vector3 start;
        private Vector3 startTop;
        private Vector3 endTop;
        private Vector3 end;

        Vector3 oppositeStartTop;
        Vector3 oppositeEndTop;

        private void Start()
        {
            interval = startInterval;
            time = oneRotationTime;
            SetCurvePoints();
            StartCoroutine(Move(interval));
        }

        private void Update()
        {
            SetCurvePoints();

            if (Input.GetButtonDown("Jump"))
            {
                IsReversed = !IsReversed;
            }
        }

        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.name == "SpinningTop")
            {
                spakle.Play();
                IsReversed = !IsReversed;
            }
        }

        private IEnumerator SpeedUpTemporarily()
        {
            float i = 0f;
            while (i < 1f)
            {
                time = Mathf.Lerp(boostStartTime, oneRotationTime, i);
                i += Time.deltaTime / boostMaintainTime;
                yield return null; 
            }
            time = oneRotationTime;
        }

        private void SetCurvePoints()
        {
            start = sides[0].position;
            end = sides[1].position;

            startTop = start + sides[0].right * halfHorizontalDistance;
            endTop = end + sides[1].right * halfHorizontalDistance;

            oppositeStartTop = start + sides[0].right * (-halfHorizontalDistance);
            oppositeEndTop = end + sides[1].right * (-halfHorizontalDistance);
        }

        private IEnumerator Move(float currentInterval)
        {
            interval = currentInterval;
            while (interval < 0.5f)
            {
                modelParent.position = BezierCurve.GetPointOnBezierCurve(start, startTop, endTop, end, interval * 2);
                interval += Time.deltaTime / (time / 2);
                yield return null;
            }
            
            while (interval < 1.0f)
            {
                modelParent.position = BezierCurve.GetPointOnBezierCurve(end, oppositeEndTop, oppositeStartTop, start, (interval - 0.5f) * 2);
                interval += Time.deltaTime / (time / 2);
                yield return null;
            }
            yield return Move(0f);
        }

        private IEnumerator MoveReverse(float currentInterval)
        {
            interval = currentInterval;
            while (interval > 0.5f)
            {
                modelParent.position = BezierCurve.GetPointOnBezierCurve(end, oppositeEndTop, oppositeStartTop, start, (interval - 0.5f) * 2);
                interval -= Time.deltaTime / (time / 2);
                yield return null;
            }

            while (interval > 0.0f)
            {
                modelParent.position = BezierCurve.GetPointOnBezierCurve(start, startTop, endTop, end, interval * 2);
                interval -= Time.deltaTime / (time / 2);
                yield return null;
            }
            yield return MoveReverse(1f);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(sides[0].position, sidesRadius);
            Gizmos.DrawWireSphere(sides[1].position, sidesRadius);

            Gizmos.color = Color.white;
            Gizmos.DrawLine(sides[0].position, sides[0].position + sides[0].right * halfHorizontalDistance);
            Gizmos.DrawLine(sides[0].position, sides[0].position + sides[0].right * (-halfHorizontalDistance));

            Gizmos.DrawLine(sides[1].position, sides[1].position + sides[1].right * halfHorizontalDistance);
            Gizmos.DrawLine(sides[1].position, sides[1].position + sides[1].right * (-halfHorizontalDistance));
        }
    }
}
