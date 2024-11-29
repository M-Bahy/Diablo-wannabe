using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleAssets.Common.Preview
{
    public class AnimationList : MonoBehaviour
    {
        [SerializeField] private Transform AnimationRoot;
        [SerializeField] private GameObject Button;

        private CharacterLoader Loader { get; set; }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            Loader = GetComponent<CharacterLoader>();
            if (AnimationRoot == null || Button == null || Loader == null)
                return;
            SpawnAnimations();
        }

        private void SpawnAnimations()
        {
            var triggers = GetTriggersList();
            foreach (var trigger in triggers)
            {
                var buttonObject = Instantiate(Button, AnimationRoot);
                var textLabel = buttonObject.GetComponentInChildren<Text>();
                textLabel.text = trigger;

                var button = buttonObject.GetComponent<Button>();
                button.onClick.AddListener(() =>
                {
                    ResetAllTrigers();
                    Loader.CharacterAnimator.SetTrigger(trigger);
                });
            }
        }

        private void ResetAllTrigers()
        {
            var triggers = GetTriggersList();
            foreach (var trigger in triggers)
            {
                Loader.CharacterAnimator.ResetTrigger(trigger);
            }
        }

        private List<string> GetTriggersList()
        {
            if (Loader.CharacterAnimator == null)
                return null;
            
            var parameters = Loader.CharacterAnimator.parameters;
            return parameters.
                Where(x => x.type == AnimatorControllerParameterType.Trigger).
                Select(x=>x.name).
                ToList();
        }
    }
}