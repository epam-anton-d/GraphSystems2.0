using System;
using System.Drawing;

namespace GraphSystems2._0
{
    internal class InterfaceMakeUp
    {
        public bool isLineButtonClicked;
        public bool isBezieButtonClicked;
        public bool isTriangleButtonClicked;
        public bool isStarButtonClicked;
        public bool isEditButtonClicked;

        public Color upButtonColor;
        public Color downButtonColor;

        /// <summary>
        /// Конструктор по-умолчанию.
        /// </summary>
        public InterfaceMakeUp()
        {
            isLineButtonClicked = false;
            isBezieButtonClicked = false;
            isTriangleButtonClicked = false;
            isStarButtonClicked = false;
            isEditButtonClicked = false;

            upButtonColor = SystemColors.Info;
            downButtonColor = SystemColors.ActiveCaption;
        }

        /// <summary>
        /// Возвращает цвет клавиши в зависимости от того нажата ли она.
        /// </summary>
        /// <param name="isButtonClicked">Значение - нажата ли клавиша.</param>
        /// <returns></returns>
        public Color ChooseButtonColor(bool isButtonClicked)
        {
            if (isButtonClicked)
            {
                return downButtonColor;
            }
            else
            {
                return upButtonColor;
            }
        }

        public void MakeButtonsFalse()
        {
            isEditButtonClicked = false;
            isLineButtonClicked = false;
            isBezieButtonClicked = false;
            isTriangleButtonClicked = false;
            isStarButtonClicked = false;
        }
    }
}
