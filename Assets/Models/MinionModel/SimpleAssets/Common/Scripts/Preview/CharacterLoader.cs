using System.Collections.Generic;
using SimpleAssets.Common.Character;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleAssets.Common.Preview
{
    public class CharacterLoader : MonoBehaviour
    {
        [SerializeField] private List<GameObject> Prefabs;
        [SerializeField] private Toggle VFXToggle;

        public int CharacterIndex { get; private set; }
        public Animator CharacterAnimator { get; private set; }
        public Transform CharacterTranform { get; private set; }
        public CharacterVFX CharacterVFX { get; private set; }

        private void Awake() => 
            Initialize();

        private void OnDestroy() => 
            VFXToggle.onValueChanged.RemoveListener(VFXValueChanged);

        private void Initialize()
        {
            LoadCharacterAt(CharacterIndex);
            VFXToggle.onValueChanged.AddListener(VFXValueChanged);
        }

        public void LoadCharacterAt(int index)
        {
            CharacterIndex = index;
            if (CharacterTranform != null)
                Destroy(CharacterTranform.gameObject);
            var characterPrefab = Prefabs[index];
            var characterModel = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);
            CharacterTranform = characterModel.transform;
            CharacterAnimator = characterModel.GetComponent<Animator>();
            CharacterVFX = characterModel.GetComponent<CharacterVFX>();
            if (CharacterVFX != null)
                CharacterVFX.SetVFXActivity(VFXToggle.isOn);
        }

        private void VFXValueChanged(bool val)
        {
            if (CharacterVFX != null)
                CharacterVFX.SetVFXActivity(val);
        }
    }
}