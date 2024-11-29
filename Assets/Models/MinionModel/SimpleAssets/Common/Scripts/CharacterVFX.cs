using System.Collections.Generic;
using UnityEngine;

namespace SimpleAssets.Common.Character
{
    public class CharacterVFX : MonoBehaviour
    {
        [SerializeField] private List<GameObject> VFXList;

        public void SetVFXActivity(bool activity)
        {
            foreach (var vfx in VFXList)
            {
                vfx.SetActive(activity);
            }
        }
    }
}