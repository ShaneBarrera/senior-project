/* The following script is based on Code Monkey YouTube
 tutorials: 
 */

//#define SOUND_MANAGER // Has Sound_Manager in project
//#define CURSOR_MANAGER // Has Cursor_Manager in project

using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _Project._Scripts.Utilities {

    /*
     * UI Button Class for handling various user interactions
     */
    public class Button_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {

        // Click Events
        public Action ClickFunc;
        public Action MouseRightClickFunc;
        public Action MouseMiddleClickFunc;
        public Action MouseDownOnceFunc;
        public Action MouseUpFunc;
        public Action MouseOverOnceTooltipFunc;
        public Action MouseOutOnceTooltipFunc;
        public Action MouseOverOnceFunc;
        public Action MouseOutOnceFunc;
        public Action MouseOverFunc;
        public Action MouseOverPerSecFunc; // Triggers every second while mouse is over
        public Action MouseUpdate;
        public Action<PointerEventData> OnPointerClickFunc;

        public enum HoverBehaviour {
            Custom,
            Change_Color,
            Change_Image,
            Change_SetActive,
        }

        public HoverBehaviour hoverBehaviourType = HoverBehaviour.Custom;
        private Action hoverEnterAction, hoverExitAction;

        [Header("Hover Behavior Settings")]
        public Color hoverEnterColor, hoverExitColor;
        public Image hoverImage;
        public Sprite hoverEnterSprite, hoverExitSprite;
        public bool hoverMove = false;
        public Vector2 hoverMoveAmount = Vector2.zero;
        
        private Vector2 originalPosition, hoverPosition;
        public bool triggerMouseOutOnClick = false;
        private bool isMouseOver;
        private float mouseOverPerSecTimer;

        private Action onPointerEnterInternal, onPointerExitInternal, onPointerClickInternal;

#if SOUND_MANAGER
        public Sound_Manager.Sound mouseOverSound, mouseClickSound;
#endif
#if CURSOR_MANAGER
        public CursorManager.CursorType cursorMouseOver, cursorMouseOut;
#endif

        private void Awake() {
            originalPosition = transform.localPosition;
            hoverPosition = originalPosition + hoverMoveAmount;
            InitializeHoverBehaviour(hoverBehaviourType);

#if SOUND_MANAGER
            onPointerEnterInternal += () => { if (mouseOverSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseOverSound); };
            onPointerClickInternal += () => { if (mouseClickSound != Sound_Manager.Sound.None) Sound_Manager.PlaySound(mouseClickSound); };
#endif

#if CURSOR_MANAGER
            onPointerEnterInternal += () => { if (cursorMouseOver != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOver); };
            onPointerExitInternal += () => { if (cursorMouseOut != CursorManager.CursorType.None) CursorManager.SetCursor(cursorMouseOut); };
#endif
        }

        private void Update() {
            if (isMouseOver) {
                MouseOverFunc?.Invoke();

                mouseOverPerSecTimer -= Time.unscaledDeltaTime;
                if (mouseOverPerSecTimer <= 0) {
                    mouseOverPerSecTimer += 1f;
                    MouseOverPerSecFunc?.Invoke();
                }
            }
            MouseUpdate?.Invoke();
        }

        public void OnPointerEnter(PointerEventData eventData) {
            onPointerEnterInternal?.Invoke();
            if (hoverMove) transform.localPosition = hoverPosition;
            hoverEnterAction?.Invoke();
            MouseOverOnceFunc?.Invoke();
            MouseOverOnceTooltipFunc?.Invoke();
            isMouseOver = true;
            mouseOverPerSecTimer = 0f;
        }

        public void OnPointerExit(PointerEventData eventData) {
            onPointerExitInternal?.Invoke();
            if (hoverMove) transform.localPosition = originalPosition;
            hoverExitAction?.Invoke();
            MouseOutOnceFunc?.Invoke();
            MouseOutOnceTooltipFunc?.Invoke();
            isMouseOver = false;
        }

        public void OnPointerClick(PointerEventData eventData) {
            onPointerClickInternal?.Invoke();
            OnPointerClickFunc?.Invoke(eventData);

            switch (eventData.button) {
                case PointerEventData.InputButton.Left:
                    if (triggerMouseOutOnClick) OnPointerExit(eventData);
                    ClickFunc?.Invoke();
                    break;
                case PointerEventData.InputButton.Right:
                    MouseRightClickFunc?.Invoke();
                    break;
                case PointerEventData.InputButton.Middle:
                    MouseMiddleClickFunc?.Invoke();
                    break;
            }
        }

        public void OnPointerDown(PointerEventData eventData) => MouseDownOnceFunc?.Invoke();
        public void OnPointerUp(PointerEventData eventData) => MouseUpFunc?.Invoke();
        public void Manual_OnPointerExit() => OnPointerExit(null);
        public bool IsMouseOver() => isMouseOver;

        public void InitializeHoverBehaviour(HoverBehaviour type) {
            hoverBehaviourType = type;

            hoverEnterAction = type switch {
                HoverBehaviour.Change_Color => () => hoverImage.color = hoverEnterColor,
                HoverBehaviour.Change_Image => () => hoverImage.sprite = hoverEnterSprite,
                HoverBehaviour.Change_SetActive => () => hoverImage.gameObject.SetActive(true),
                _ => null,
            };

            hoverExitAction = type switch {
                HoverBehaviour.Change_Color => () => hoverImage.color = hoverExitColor,
                HoverBehaviour.Change_Image => () => hoverImage.sprite = hoverExitSprite,
                HoverBehaviour.Change_SetActive => () => hoverImage.gameObject.SetActive(false),
                _ => null,
            };
        }

        /*
         * Class for temporarily intercepting a button action
         * Useful for disabling buttons during tutorials
         */
        public class InterceptActionHandler {
            private readonly Action removeInterceptFunc;

            public InterceptActionHandler(Action removeInterceptFunc) {
                this.removeInterceptFunc = removeInterceptFunc;
            }

            public void RemoveIntercept() => removeInterceptFunc?.Invoke();
        }

        public InterceptActionHandler InterceptActionClick(Func<bool> testPassthroughFunc) {
            return InterceptAction(nameof(ClickFunc), testPassthroughFunc);
        }

        public InterceptActionHandler InterceptAction(string fieldName, Func<bool> testPassthroughFunc) {
            return InterceptAction(GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance), testPassthroughFunc);
        }

        public InterceptActionHandler InterceptAction(FieldInfo fieldInfo, Func<bool> testPassthroughFunc) {
            if (fieldInfo == null) return null;

            var originalFunc = fieldInfo.GetValue(this) as Action;
            var handler = new InterceptActionHandler(() => fieldInfo.SetValue(this, originalFunc));

            fieldInfo.SetValue(this, (Action)(() => {
                if (testPassthroughFunc()) {
                    handler.RemoveIntercept();
                    originalFunc?.Invoke();
                }
            }));

            return handler;
        }
    }
}
