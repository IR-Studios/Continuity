using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Arctic.Objectives
{

    public class ObjectiveManager : MonoBehaviour
    {
        HUDManager HUD;
        public Animator UIAnimator;
        public int CurrentObjective = 0;
        public List<Objective> Objectives = new List<Objective>();

        public void Start()
        {
            HUD = GameObject.Find("Player").GetComponent<HUDManager>();
            Objectives[CurrentObjective].isActive = true;
            
        }

        public void Update()
        {
            UpdateCurrentObjective();
            UpdateNextObjective();
        }

        public void UpdateCurrentObjective()
        {
            if (Objectives[CurrentObjective].isActive)
            {
                UpdateHUD();
            }
        }

        public void UpdateNextObjective()
        {
            if (Objectives[CurrentObjective].isComplete)
            {
                Objectives[CurrentObjective].isActive = false;
                CurrentObjective++;
                Objectives[CurrentObjective].isActive = true;
                UpdateHUD();
                StartCoroutine("DisplayNotification");
            }
        }

        public void CompletedObjective()
        {
            Objectives[CurrentObjective].isComplete = true;

        }

        public void UpdateHUD()
        {
            HUD.ObjText.text = Objectives[CurrentObjective].ObjectiveDescription;
            
        }

        IEnumerator DisplayNotification()
        {
            HUD.ObjNotif.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            TextMeshProUGUI text = HUD.ObjNotif.GetComponent<TextMeshProUGUI>();
            while (text.alpha > 0)
            {
                text.alpha -= 1 * Time.deltaTime;
                yield return null;

            }
            if (text.alpha <= 0)
            {
                text.alpha = 100;
                HUD.ObjNotif.SetActive(false);

            }

            

        }
    }
    [System.Serializable]
    public class Objective
    {
        public int ObjNum;
        public string ObjectiveName;
        [TextArea]
        public string ObjectiveDescription;
        public bool isActive;
        public bool isComplete;
    }
}
