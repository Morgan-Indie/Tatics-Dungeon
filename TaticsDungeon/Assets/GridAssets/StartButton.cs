using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class StartButton : MonoBehaviour
    {
        Button button;
        // Start is called before the first frame update
        void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(delegate { HandlePress(); });
        }

        void HandlePress()
        {
            GridManager.Instance.SetAndLoadNewMap(FloorManager.Instance.GetCurrentMap());
            GridManager.Instance.AddTransitions();
            Destroy(gameObject);
        }
    }
}

