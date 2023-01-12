using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace HelloWorld
{
    public class ShowSelectedPiece : MonoBehaviour
    {
        public Button button1;
        public Button button2;
        public Button button3;
        public Button button4;
        public Button button5;
        public Button button6;

        public void HighLight()
        {
            var colors = GetComponent<Button>().colors;
            // Red
            colors.normalColor = new Color(0.8207547f, 0.1072389f, 0.08826979f, 1);
            colors.selectedColor = new Color(0.8490566f, 0.2992f, 0.2675329f, 1);
            var colors1 = GetComponent<Button>().colors;
            // Blue
            colors1.normalColor = new Color(0.04610179f, 0.1800396f, 0.6603774f, 1);
            colors1.selectedColor = new Color(0.4909042f, 0.5627947f, 0.8207547f, 1);
            if (HelloWorldManager.isHost)
            {
                button1.GetComponent<Button>().colors = colors1;
                button2.GetComponent<Button>().colors = colors1;
                button3.GetComponent<Button>().colors = colors1;
                button4.GetComponent<Button>().colors = colors1;
                button5.GetComponent<Button>().colors = colors1;
                button6.GetComponent<Button>().colors = colors1;
            }
            if (HelloWorldManager.isClient)
            {
                button1.GetComponent<Button>().colors = colors;
                button2.GetComponent<Button>().colors = colors;
                button3.GetComponent<Button>().colors = colors;
                button4.GetComponent<Button>().colors = colors;
                button5.GetComponent<Button>().colors = colors;
                button6.GetComponent<Button>().colors = colors;
            }
        }
    }
}
