using System;
using Trails;
using UnityEngine;

namespace Character
{
    [Serializable]
    public enum Who
    {
        Seeker,
        Hider,
    }
    
    [Serializable]
    public class CharactersComponent
    {
        [Header("WHO ARE YOU :")]
        public Who who;
        [Space(5)][Header("SEEKER FOV :")]
        public FieldOfView.FieldOfView fieldOfView;
        [Space(5)][Header("SKILLS AND EFFECTS :")]
        public TrailManager trailManager;
        public GameObject shield;
        public GameObject phrase;
    }

    [Serializable]
    public class CameraConfiguration
    {
        [Space(5)][Header("CAMERA CULLING MASK :")]
        public LayerMask seekerMask;
        public LayerMask hiderMask;
    }
    
    public class CharacterConfiguration : MonoBehaviour
    {
        public CharactersComponent charactersComponent;
        public CameraConfiguration cameraConfiguration;

        private void Start()
        {
            switch (charactersComponent.who)
            {
                case Who.Seeker:
                    charactersComponent.fieldOfView.gameObject.SetActive(true);
                    GameManager.instance.ChangeCamera(cameraConfiguration.seekerMask);
                    break;
                case Who.Hider:
                    charactersComponent.fieldOfView.gameObject.SetActive(false);
                    // GameManager.instance.ChangeCamera(cameraConfiguration.hiderMask);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
