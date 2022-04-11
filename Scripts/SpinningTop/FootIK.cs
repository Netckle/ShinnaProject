using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SHINNAGame
{
    public class FootIK : MonoBehaviour
    {
        public Transform target;
        private Vector3 originEulerAngle;
        private Ray ray;
        private RaycastHit hit;
        public LayerMask whatIsGround;

        public void LateUpdate()
        {
            CheckGround();
        }
        public float height;
        public void CheckGround()
        {
            ray = new Ray(target.position, Vector3.down);

            if (Physics.Raycast(ray, out hit, 10f, whatIsGround, QueryTriggerInteraction.Ignore))
            {
                Vector3 normal = hit.normal;

                target.position = hit.point + Vector3.up * height;
                target.localRotation = Quaternion.LookRotation((hit.point + (-hit.normal)) - hit.point);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawRay(target.transform.position, Vector3.down * 10f);
        }
    }

    
}
