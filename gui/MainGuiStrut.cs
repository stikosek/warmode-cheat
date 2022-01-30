using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using BepInEx.IL2CPP;
using UnityEngine;
using HarmonyLib;
using System.Threading.Tasks;


namespace warmode_cheat.gui
{
    public class MainGuiStrut : MonoBehaviour
    {
        //GameObject shit
        public MainGuiStrut(IntPtr ptr) : base(ptr) { }
        private Plugin loader;

        // Window (e)rects

        public Rect PlayerRect = new Rect(50, 20, 200, 345);
        public Rect AimbotRect = new Rect(260, 20, 200, 160);


        public static void CreateInstance(Plugin loader)
        {
            //Create GameObject
            MainGuiStrut obj = loader.AddComponent<MainGuiStrut>();

            obj.loader = loader;

            //Prevent Unity from deleting when a new Scene loads.
            DontDestroyOnLoad(obj.gameObject);
            obj.hideFlags |= HideFlags.HideAndDontSave;

        }
        bool stamina;
        public static string text = "WARMODE cheat (1.1)";

        public void AimbotWindow(int windowID)
        {
            if (cheats.aimbot.activated)
            {
                GUI.Label(new Rect(0,20, 200, 20), "Target Pos: " + cheats.aimbot.TargetVector.ToString());
                GUI.Label(new Rect(0,40, 200, 20), "RayHitLayer: " + cheats.aimbot.RHLN);
                GUI.Label(new Rect(0,60, 200, 20), "Player rot: " + FirstPersonPlayer.Transform.rotation.ToString());
                GUI.Label(new Rect(0,80, 200, 20), "TargetVisible: " + cheats.aimbot.IsVisable);
                GUI.Label(new Rect(0,100, 200, 20), "TargetActive: " + cheats.aimbot.latestTargetedPlayer.Active.ToString());
                GUI.Label(new Rect(0, 120, 200, 20), "Mode: " + cheats.aimbot.MODE);
                GUI.Label(new Rect(0, 140, 200, 20), "SQRDistance: " + cheats.aimbot.dist);




                //In-Game Position
                Vector3 pivotPos = cheats.aimbot.TargetVector; //Pivot point NOT at the feet, at the center
             
                Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y - 1f; //At the feet
                Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y + 2f; //At the head

                //Screen Position
                Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);

                    DrawLine(w2s_footpos, w2s_headpos, Color.blue, 3);


                





            }
            else
            {
                GUI.Label(new Rect(0, 20, 200, 40), "Activate aimbot to\nview debug window");
            }
            GUI.DragWindow(new Rect(0f, 0f, 10000f, 10000f));

        }

        public void OnGUI()
        {
            if (BEEG)
            {
                DrawLine(new Vector2(0, Screen.height / 2), new Vector2(Screen.width, Screen.height / 2),Color.yellow,5);
                DrawLine(new Vector2(Screen.width/2, 0), new Vector2(Screen.width /2, Screen.height), Color.yellow, 5);
            }

            

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                FirstPersonPlayer.go.transform.position = FirstPersonPlayer.go.transform.position + new Vector3(0, 20, 0);
            }
            UnityEngine.GUI.Label(new Rect(100, 100, 300, 100), "cheat");

            PlayerRect = GUI.Window(0, PlayerRect,(UnityEngine.GUI.WindowFunction)PlayerWindow,text);
            if (cheats.aimbot.activated)
            {
                AimbotRect = GUI.Window(1, AimbotRect, (UnityEngine.GUI.WindowFunction)AimbotWindow, "Aimbot debug window");
            }
            if (!esp) return;

          

           
           foreach(CPlayerData data in PlayerControll.Player)
            {
                if (data == null) continue;
               
                

                //In-Game Position
                Vector3 pivotPos = data.position; //Pivot point NOT at the feet, at the center
                Vector3 playerFootPos; playerFootPos.x = pivotPos.x; playerFootPos.z = pivotPos.z; playerFootPos.y = pivotPos.y - 1f; //At the feet
                Vector3 playerHeadPos; playerHeadPos.x = pivotPos.x; playerHeadPos.z = pivotPos.z; playerHeadPos.y = pivotPos.y + 2f; //At the head

                //Screen Position
                Vector3 w2s_footpos = Camera.main.WorldToScreenPoint(playerFootPos);
                Vector3 w2s_headpos = Camera.main.WorldToScreenPoint(playerHeadPos);

                if (w2s_footpos.z > 0f)
                {
                    GUIStyle style6 = new GUIStyle
                    {
                        alignment = TextAnchor.MiddleCenter
                    };
                    w2s_footpos.z = w2s_footpos.y + Screen.height;
                    GUI.Label(new Rect(w2s_headpos.x, (float)Screen.height - w2s_headpos.y, 0f, 0f), data.Name , style6);

                    if(FirstPersonPlayer.Team == data.Team)
                    {
                       
                            DrawBoxESP(w2s_footpos, w2s_headpos, Color.green, data.Name);
                    

                    }
                    else
                    {
                       
                            DrawBoxESP(w2s_footpos, w2s_headpos, Color.red, data.Name);
                        
                    }
                    
                    
                }
            }







        }

        public void DrawBoxESP(Vector3 footpos, Vector3 headpos, Color color,string name) //Rendering the ESP
        {
            float height = headpos.y - footpos.y;
            float widthOffset = 2f;
            float width = height / widthOffset;

            //ESP BOX
            DrawBox(footpos.x - (width / 2), (float)Screen.height - footpos.y - height, width, height, color, 2f,name);

            
        }


        public static Texture2D lineTex;
        public static void DrawLine(Vector2 pointA, Vector2 pointB, Color color, float width)
        {
            Matrix4x4 matrix = GUI.matrix;
            if (!lineTex)
                lineTex = new Texture2D(1, 1);

            Color color2 = GUI.color;
            GUI.color = color;
            float num = Vector3.Angle(pointB - pointA, Vector2.right);

            if (pointA.y > pointB.y)
                num = -num;

            GUIUtility.ScaleAroundPivot(new Vector2((pointB - pointA).magnitude, width), new Vector2(pointA.x, pointA.y + 0.5f));
            GUIUtility.RotateAroundPivot(num, pointA);
            GUI.DrawTexture(new Rect(pointA.x, pointA.y, 1f, 1f), lineTex);
            GUI.matrix = matrix;
            GUI.color = color2;
        }

        public static void DrawBox(float x, float y, float w, float h, Color color, float thickness,string name)
        {
            DrawLine(new Vector2(x, y), new Vector2(x + w, y), color, thickness);
            DrawLine(new Vector2(x, y), new Vector2(x, y + h), color, thickness);
            DrawLine(new Vector2(x + w, y), new Vector2(x + w, y + h), color, thickness);
            DrawLine(new Vector2(x, y + h), new Vector2(x + w, y + h), color, thickness);
           
        }

        public static bool esp = false;
        Color ButtonBorder = Color.black;
        Color ButtonRest = Color.green;
        Color WindowTop = Color.black;
        Color WindowRest = Color.gray;
        public static bool BEEG = false;
        public void PlayerWindow(int windowID)
        {
            //DrawWindowBackground(WindowTop, WindowRest, PlayerRect, 15, "WARMODE cheat");

            GUIStyle style = new GUIStyle();
            style.richText = true;
            if (GUI.Button(new Rect(5,15,190,30),getToggleText("ESP",esp)))
            {
                //DrawButtonToggle(new Rect(5, 15, 190, 30), ButtonBorder, ButtonRest, getToggleText("ESP", esp));
                esp = !esp;
            }
            if (GUI.Button(new Rect(5, 45, 190, 30), getToggleText("No Recoil", cheats.noshake.activated)))
            {
                //DrawButtonToggle(new Rect(5, 45, 190, 30), ButtonBorder, ButtonRest, getToggleText("No Recoil", cheats.noshake.activated));
                if (!cheats.noshake.activated && cheats.noshake.ractivated) cheats.noshake.ractivated = false;
                cheats.noshake.activated = !cheats.noshake.activated;
            }
            if (GUI.Button(new Rect(5, 75, 190, 30), getToggleText("No ammo usage", cheats.freeammo.activated)))
            {
               // DrawButtonToggle(new Rect(5, 75, 190, 30), ButtonBorder, ButtonRest, getToggleText("No ammo usage", cheats.freeammo.activated));
                cheats.freeammo.activated = !cheats.freeammo.activated;
            }
            if (GUI.Button(new Rect(5, 105, 190, 30), getToggleText("Infinite stamina", cheats.stamina.activated)))
            {
               // DrawButtonToggle(new Rect(5, 105, 190, 30), ButtonBorder, ButtonRest, getToggleText("Infinite stamina", cheats.stamina.activated));
                cheats.stamina.activated = !cheats.stamina.activated;
            }
            if (GUI.Button(new Rect(5, 135, 190, 30), getToggleText("Weird rapid fire", cheats.noshake.ractivated)))
            {
                //DrawButtonToggle(new Rect(5, 135, 190, 30), ButtonBorder, ButtonRest, getToggleText("Weird rapid fire", cheats.noshake.ractivated));
                if (cheats.noshake.activated && !cheats.noshake.ractivated) cheats.noshake.activated = false;

                cheats.noshake.ractivated = !cheats.noshake.ractivated;
            }
            if (GUI.Button(new Rect(5, 165, 190, 30), getToggleText("AimBot", cheats.aimbot.activated)))
            {
               
                

                cheats.aimbot.activated = !cheats.aimbot.activated;
            }
            if (GUI.Button(new Rect(5, 195, 190, 30), getToggleText("BEEG crosshair", BEEG)))
            {



                BEEG = !BEEG;
            }
            if (GUI.Button(new Rect(5, 225, 190, 30), getToggleText("Flight", cheats.flight.activated)))
            {

                if (!cheats.flight.activated && cheats.flight.Moonactivated) cheats.flight.Moonactivated = false;

                cheats.flight.activated = !cheats.flight.activated;
            }
            if (GUI.Button(new Rect(5, 255, 190, 30), getToggleText("Moon gravity", cheats.flight.Moonactivated)))
            {

                if (cheats.flight.activated && !cheats.flight.Moonactivated) cheats.flight.activated = false;

                cheats.flight.Moonactivated = !cheats.flight.Moonactivated;
            }
            if (GUI.Button(new Rect(5, 285, 190, 30),"Teleport to spawn [RSHIFT]") || Input.GetKeyDown(KeyCode.RightShift))
            {
                //DrawButtonToggle(new Rect(5, 165, 190, 30), ButtonBorder, ButtonRest, "Teleport to team spawn");
                FirstPersonPlayer.DevSpawn();
            }
            GUI.Label(new Rect(5, 315, 190, 20), "<color=lime>Made by stikosek#0761</color>");
            if(GUI.Button(new Rect(150, 315, 40, 20), "<color=cyan>DNT</color>"))
            {
                System.Diagnostics.Process.Start("https://ko-fi.com/stikosek");
            }

            GUI.DragWindow(new Rect(0f, 0f, 10000f, 10000f));


        }
        

        public string getToggleText(string name,bool state)
        {
            string text;
            if(state)
            {
                text = name + " <color=gray>[</color><color=lime>ENABLED</color><color=gray>]</color>";
            }
            else
            {
                text = name + " <color=gray>[</color><color=red>DISABLED</color><color=gray>]</color>";
            }
            

            return text;
        }

        public static void DrawColor(Color color, Rect rect)
        {
            Texture2D tex = new Texture2D(1, 1);

            tex.SetPixel(1, 1, color);

            tex.wrapMode = TextureWrapMode.Repeat;
            tex.Apply();

            GUI.DrawTexture(rect, tex);
        }

        public static void DrawWindowBackground(Color top, Color rest, Rect rect, int TopBarThickness, string Title)
        {


            //Draw main background
            DrawColor(rest, new Rect(0, TopBarThickness, rect.width, rect.height - TopBarThickness));

            //Draw top bar
           DrawColor(top, new Rect(0, 0, rect.width, TopBarThickness));

            DrawText(Title, new Rect(0, 0, rect.width, TopBarThickness), TopBarThickness - 3, Color.black);


        }

        public static void DrawButtonToggle(Rect rect,Color border,Color rest,string NormalText)
        {
            // Draw Button
            DrawColor(border,rect);
            // Draw Button text
           
            
            DrawText(NormalText, new Rect(rect.x + 5, rect.y + 5, rect.width - 10, rect.height - 10), 15, Color.white);
            
            
            // Draw toggle border
            DrawColor(border, new Rect(5,5, rect.width - 10, rect.height - 10));

        }
        public static void DrawText(string text, Rect pos, int fontSize, Color textColor)
        {
            GUIStyle style = GetTextStyle(fontSize, textColor);

            GUI.Label(pos, text, style);
        }

        public static GUIStyle GetTextStyle(int fontSize, Color textColor)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                font = Resources.GetBuiltinResource<Font>("Arial.ttf"),
                fontSize = fontSize,
                alignment = TextAnchor.MiddleLeft,
            };

            style.normal.textColor = textColor;

            return style;
        }






    }
}
