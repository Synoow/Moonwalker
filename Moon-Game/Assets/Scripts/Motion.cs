using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.MoonDevs.MoonWalkers
{
    public class Motion : MonoBehaviourPunCallbacks
    {
        public float speed;
        public float sprintModifier;
        public float jumpForce;
        public Camera normalCam;
        public GameObject cameraParent;


        private Rigidbody rig;
        
        private float baseFOV;
        private float sprintFOVModifier = 1.5f;
        
        private void Start()
        {
            cameraParent.SetActive(photonView.IsMine);
            
            baseFOV = normalCam.fieldOfView;

            if (Camera.main)
            {
                Camera.main.enabled = false;
            }
            
            rig = GetComponent<Rigidbody>();
        }

        private void Update()
		{
            if (!photonView.IsMine) return;

            //Axles
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");
            
            
            //Controls
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKey(KeyCode.Space);
            

            //States
            bool isJumping = jump;
            bool isSprinting = sprint && t_vmove > 0 ;
            
            //Jumping
            if (isJumping)
            {
                rig.AddForce(Vector3.up * jumpForce);
            }


        }

        void FixedUpdate()
        {
            if (!photonView.IsMine) return;

            //Axles
            float t_hmove = Input.GetAxisRaw("Horizontal");
            float t_vmove = Input.GetAxisRaw("Vertical");
            
            
            //Controls
            bool sprint = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
            bool jump = Input.GetKey(KeyCode.Space);
            

            //States
            bool isJumping = jump;
            bool isSprinting = sprint && t_vmove > 0 ;
            
            //Jumping
            if (isJumping)
            {
                rig.AddForce(Vector3.up * jumpForce);
            }
            
            
            //Movement
            Vector3 t_direction = new Vector3(t_hmove,0,t_vmove);
            t_direction.Normalize();

            float t_adjustedSpeed = speed;
            if (isSprinting)
            {
                t_adjustedSpeed *= sprintModifier;
            }

            Vector3 t_targetVelocity = transform.TransformDirection(t_direction) * t_adjustedSpeed * Time.deltaTime;
            t_targetVelocity.y = rig.velocity.y;
            rig.velocity = t_targetVelocity;
            
            
            //Field of View
            if (isSprinting)
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView,baseFOV * sprintFOVModifier,Time.deltaTime * 8f);
            }
            else
            {
                normalCam.fieldOfView = Mathf.Lerp(normalCam.fieldOfView,baseFOV,Time.deltaTime * 8f);
            }
            
        }
    }
}
