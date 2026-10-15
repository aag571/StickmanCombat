////反编译自程序集： Assembly-CSharp.dll
//using System;
//using System.Diagnostics;
//using System.Runtime.CompilerServices;
//using System.Threading;
//using UnityEngine;

//public class InputManager : MonoBehaviour
//{
//    public delegate void AttackAction(int side);

//    public static InputManager instance;

//    private float width;

//    private float height;



//    public static event InputManager.AttackAction OnAttackPressed;

//    private void Awake()
//    {
//        InputManager.instance = this;
//        this.width = (float)Screen.width;
//        this.height = (float)Screen.height;
//    }

//    private void Update()
//    {
//        if (MenuManager.instance.isPaused || Time.timeScale <= 0f)
//        {
//            return;
//        }
//        if (SceneManager.instance.gameStarted)
//        {
//            bool flag = false;
//            bool flag2 = false;
//            //Touch[] touches = Input.touches;
//            //for (int i = 0; i < touches.Length; i++)
//            //{
//            //    Touch touch = touches[i];
//            //    if (touch.phase == TouchPhase.Began)
//            //    {
//            if (Input.GetMouseButtonDown(0))
//            {
//                //Vector2 position = touch.position;
//                Vector2 position = Input.mousePosition;
//                if ((double)position.x > (double)this.width / 1.3 && (double)position.y > (double)this.height / 1.3)
//                {
//                    if (!MenuManager.instance.isPaused)
//                    {
//                        MenuManager.instance.Pause();
//                    }
//                    return;
//                }
//                if (!SceneManager.instance.inputStarted)
//                {
//                    return;
//                }
//                if (position.x > this.width / 2f)
//                {
//                    if (!flag2)
//                    {
//                        flag2 = true;
//                        InputManager.OnAttackPressed(1);
//                    }
//                }
//                else if (!flag)
//                {
//                    flag = true;
//                    InputManager.OnAttackPressed(-1);
//                }
//            }
//            //    }
//            //}
//        }
//    }
//}
// 反编译自程序集： Assembly-CSharp.dll
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void AttackAction(int side);

    public static InputManager instance;

    private float width;

    private float height;



    public static event InputManager.AttackAction OnAttackPressed;

    private void Awake()
    {
        InputManager.instance = this;
        this.width = (float)Screen.width;
        this.height = (float)Screen.height;
    }

    private void Update()
    {
        this.width = Screen.width;
        this.height = Screen.height;
        var flow = StickmanCombat.Gameplay.GameFlowController.instance;
        if (flow != null && flow.TutorialAwaitingInput && !MenuManager.instance.isPaused)
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) flow.TutorialAttack(-1);
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) flow.TutorialAttack(1);
            if (!StickmanCombat.Gameplay.CombatInputPlatform.UsesTouchControls) foreach (Touch touch in Input.touches)
                if (touch.phase == TouchPhase.Began && (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))) flow.TutorialAttack(touch.position.x > Screen.width / 2f ? 1 : -1);
            if (!StickmanCombat.Gameplay.CombatInputPlatform.UsesTouchControls && Input.touchCount == 0 && Input.GetMouseButtonDown(0) && (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())) flow.TutorialAttack(Input.mousePosition.x > Screen.width / 2f ? 1 : -1);
            return;
        }
        if (MenuManager.instance.isPaused || Time.timeScale <= 0f)
        {
            return;
        }
        if (SceneManager.instance.gameStarted)
        {
            if (StickmanCombat.Gameplay.GameFlowController.instance != null)
            {
                if (!SceneManager.instance.inputStarted) return;
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) OnAttackPressed?.Invoke(-1);
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) OnAttackPressed?.Invoke(1);
                if (!StickmanCombat.Gameplay.CombatInputPlatform.UsesTouchControls) foreach (Touch touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began && (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId)))
                        OnAttackPressed?.Invoke(touch.position.x > Screen.width / 2f ? 1 : -1);
                }
                if (!StickmanCombat.Gameplay.CombatInputPlatform.UsesTouchControls && Input.touchCount == 0 && Input.GetMouseButtonDown(0) && (UnityEngine.EventSystems.EventSystem.current == null || !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()))
                    OnAttackPressed?.Invoke(Input.mousePosition.x > Screen.width / 2f ? 1 : -1);
                return;
            }
            bool flag = false;
            bool flag2 = false;
            Touch[] touches = Input.touches;
            for (int i = 0; i < touches.Length; i++)
            {
                Touch touch = touches[i];
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 position = touch.position;
                    if ((double)position.x > (double)this.width / 1.3 && (double)position.y > (double)this.height / 1.3)
                    {
                        if (!MenuManager.instance.isPaused)
                        {
                            MenuManager.instance.Pause();
                        }
                        return;
                    }
                    if (!SceneManager.instance.inputStarted)
                    {
                        return;
                    }
                    if (position.x > this.width / 2f)
                    {
                        if (!flag2)
                        {
                            flag2 = true;
                            InputManager.OnAttackPressed(1);
                        }
                    }
                    else if (!flag)
                    {
                        flag = true;
                        InputManager.OnAttackPressed(-1);
                    }
                }
            }
            if (SceneManager.instance.inputStarted)
            {
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    InputManager.OnAttackPressed?.Invoke(-1);
                }
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    InputManager.OnAttackPressed?.Invoke(1);
                }
                if (Input.touchCount == 0 && Input.GetMouseButtonDown(0))
                {
                    Vector2 position = Input.mousePosition;
                    if (position.x > this.width / 1.3f && position.y > this.height / 1.3f)
                        MenuManager.instance.Pause();
                    else
                        InputManager.OnAttackPressed?.Invoke(position.x > this.width / 2f ? 1 : -1);
                }
            }
        }
    }
    public void PressDirection(int side)
    {
        var flow = StickmanCombat.Gameplay.GameFlowController.instance;
        if (flow == null || flow.Session == null || MenuManager.instance.isPaused) return;
        if (flow.TutorialAwaitingInput) { flow.TutorialAttack(side); return; }
        if (Time.timeScale <= 0f || !SceneManager.instance.gameStarted || !SceneManager.instance.inputStarted) return;
        OnAttackPressed?.Invoke(side);
    }
}
