using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;

namespace warmode_cheat.cheats
{
    [HarmonyPatch]
    public class flight
    {
        public static bool activated = false;
        public static bool Moonactivated = false;

        [HarmonyPatch(typeof(vp_FPController), "FixedUpdate")]
        [HarmonyPrefix]
        public static void Prefix(ref bool __runOriginal,vp_FPController __instance)
        {
            gui.MainGuiStrut.text = __instance.PhysicsGravityModifier.ToString();

            if (activated)
            {
                __instance.m_Grounded = true;
               __instance.PhysicsGravityModifier = 0.0001f;
               
                float speed = Input.GetKey(KeyCode.LeftControl) ? 0.5f : (Input.GetKey(KeyCode.LeftShift) ? 1f : 0.5f);
                if (Input.GetKey(KeyCode.Space))
                {
                    FirstPersonPlayer.go.gameObject.transform.position = new Vector3(FirstPersonPlayer.go.gameObject.transform.position.x, FirstPersonPlayer.go.gameObject.transform.position.y + speed, FirstPersonPlayer.go.gameObject.transform.position.z);
                }
                Vector3 playerTransformPosVec = FirstPersonPlayer.go.gameObject.transform.position;
                if (Input.GetKey(KeyCode.W))
                {
                    FirstPersonPlayer.go.gameObject.transform.position = new Vector3(playerTransformPosVec.x + Camera.main.transform.forward.x * Camera.main.transform.up.y * speed, playerTransformPosVec.y + Camera.main.transform.forward.y * speed, playerTransformPosVec.z + Camera.main.transform.forward.z * Camera.main.transform.up.y * speed);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    FirstPersonPlayer.go.gameObject.transform.position = new Vector3(playerTransformPosVec.x - Camera.main.transform.forward.x * Camera.main.transform.up.y * speed, playerTransformPosVec.y - Camera.main.transform.forward.y * speed, playerTransformPosVec.z - Camera.main.transform.forward.z * Camera.main.transform.up.y * speed);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    FirstPersonPlayer.go.gameObject.transform.position = new Vector3(playerTransformPosVec.x + Camera.main.transform.right.x * speed, playerTransformPosVec.y, playerTransformPosVec.z + Camera.main.transform.right.z * speed);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    FirstPersonPlayer.go.gameObject.transform.position = new Vector3(playerTransformPosVec.x - Camera.main.transform.right.x * speed, playerTransformPosVec.y, playerTransformPosVec.z - Camera.main.transform.right.z * speed);
                }






            }
            else if (Moonactivated)
            {
                __instance.m_Grounded = true;
                __instance.PhysicsGravityModifier = 0.04f;
            }
            else
            {
                __instance.PhysicsGravityModifier = 0.2f;
            }
       
        }

       
    }
}
