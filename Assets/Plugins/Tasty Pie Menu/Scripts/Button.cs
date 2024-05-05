using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.UI;

namespace Xamin
{
    [RequireComponent(typeof(Image))]
    public class Button : MonoBehaviour
    {
        [SerializeField] private GameObject _weapon;
        [SerializeField] private RadialMenu _radialMenu;

        public Item WeaponIn;

        [Tooltip("Your actions, that will be executed when the buttons is pressed")]
        public UnityEvent action;

        public UnityEvent Desecute;

        [Tooltip("The icon of this button")] public Sprite image;

        [Tooltip("If this button can be pressed or not. False = grayed out button")]
        public bool unlocked;

        [Tooltip("Can be used to reference the button via code.")]
        public string id;

        public Color customColor;
        public bool useCustomColor;

        private UnityEngine.UI.Image imageComponent;
        private bool _isimageComponentNotNull;

        void Start()
        {
            imageComponent = GetComponent<UnityEngine.UI.Image>();
            if (image)
                imageComponent.sprite = image;
            _isimageComponentNotNull =
                imageComponent != null; // This check avoids expensive not null comparisons at runtime.
        }

        public Color currentColor
        {
            get { return imageComponent.color; }
        }

        public void SetColor(Color c)
        {
            if (_isimageComponentNotNull)
                imageComponent.color = c;
        }

        public void SetButtonActive()
        {
            unlocked = true;
            imageComponent.sprite = WeaponIn.Icon;
            Color color = new Color(1, 1, 1, 1);
            imageComponent.color = color;
        }

        /// <summary>
        /// This method is responsible for handling the UnityEvent execution 
        /// </summary>
        public void ExecuteAction()
        {
            if (_weapon.activeSelf)
                return;
            if (unlocked)
            {
                action.Invoke();
            }
        }

        public void DesecuteAction()
        {
            if (unlocked)
            {
                Desecute?.Invoke();
            }
        }
    }
}