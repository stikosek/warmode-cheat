using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using HarmonyLib;
using System.Runtime.InteropServices;

namespace warmode_cheat.cheats
{
    
    [HarmonyPatch]
    public class aimbot
    {


        
        public static Vector3 TargetVector = Vector3.zero;
        public static string RHLN;
        public static float dist = 0;
        public static string MODE = "HEAD";
        public static CPlayerData latestTargetedPlayer;
        public static bool IsVisable = false;
        public static bool activated = false;
        public static int smooth = 1;
        public static float minDist = 99999;
        
        [HarmonyPatch(typeof(vp_FPCamera), "Update")]
        [HarmonyPrefix]
        public static void Prefix(vp_FPCamera __instance)
        {
            if (activated)
            {

                TargetVector = GetClosestEnemy(PlayerControll.Player, FirstPersonPlayer.Transform,FirstPersonPlayer.Team,true);
                if(dist > 1700f)
                {
                    MODE = "FARBODY";
                }
                else if(dist > 700f)
                {
                    MODE = "BODY";
                }
                else {
                    MODE = "HEAD";
                }

                if (TargetVector != Vector3.zero)
                {
                    
                    Transform transform = new Transform();
                    transform = FirstPersonPlayer.Transform;
                    if (MODE == "BODY")
                    {
                        TargetVector.y = TargetVector.y - 1f;
                    }else if(MODE == "FARBODY")
                    {
                        TargetVector.y = TargetVector.y - 0.5f;
                    }
                    transform.LookAt(TargetVector);
                    IsVisable = IsPlayerVisable(latestTargetedPlayer.position);
                    if (Input.GetKey(KeyCode.Q)) __instance.SetRotation(transform.eulerAngles);
                }

            }

        }

        public static Vector3 GetClosestEnemy(CPlayerData[] data,Transform playertrans,int PlTeam,bool onlyVisiblePlayers)
        {

            CPlayerData bestTarget = null;
            float closestDistanceSqr = Mathf.Infinity;
            Vector3 currentPosition = playertrans.position;

            foreach (CPlayerData potentialtarget in data)
            {
                if (potentialtarget == null) continue;
                if (potentialtarget.Team == PlTeam) continue;
                
                Vector3 w2s = Camera.main.WorldToScreenPoint(potentialtarget.position);

                //if (w2s.z > 0) continue;


                Vector3 directionToTarget = potentialtarget.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    dist = closestDistanceSqr;
                    bestTarget = potentialtarget;
                }




            }

            latestTargetedPlayer = bestTarget;

            if (bestTarget != null) { return (bestTarget.position); } else { return (Vector3.zero); }
        }

        public static LayerMask mask = LayerMask.GetMask("Default", "Player3rd 21", "Map", "Player 25");
        public static bool IsPlayerVisable(Vector3 toCheck)
        {
            if(Physics.Raycast(Camera.main.transform.position,toCheck,out RaycastHit hit, 999999999999f))
            {
                RHLN = hit.collider.name;
                if (hit.collider.name == "LocalPlayer")
                {
                    return true;
                }
                
            }
            return false;
        }
    }
}
