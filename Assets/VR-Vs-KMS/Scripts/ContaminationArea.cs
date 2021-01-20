using Photon.Pun;
using UnityEngine;

namespace vr_vs_kms
{
    public class ContaminationArea : MonoBehaviourPunCallbacks
    {
        [System.Serializable]
        public struct BelongToProperties
        {
            public Color mainColor;
            public Color secondColor;
            
        }

        public BelongToProperties nobody;
        public BelongToProperties virus;
        public BelongToProperties scientist;

        private float faerieSpeed;
        public float cullRadius = 5f;

        private float radius = 1f;
        private ParticleSystem pSystem;
        private WindZone windZone;
        private int remainingGrenades;
        public float inTimer = 0f;
        private CullingGroup cullGroup;

        private float Timer = 0;
        private int PlayersIn = 0;
        private GameObject TeamCatching;
        private GameObject TeamCatch;

        void Start()
        {
            populateParticleSystemCache();
            setupCullingGroup();

            photonView.RPC("BelongsToNobody", RpcTarget.All);
        }

        private void populateParticleSystemCache()
        {
            pSystem = this.GetComponentInChildren<ParticleSystem>();
        }

        /// <summary>
        /// This manage visibility of particle for the camera to optimize the rendering.
        /// </summary>
        private void setupCullingGroup()
        {
            Debug.Log($"setupCullingGroup {Camera.main}");
            cullGroup = new CullingGroup();
            cullGroup.targetCamera = Camera.main;
            cullGroup.SetBoundingSpheres(new BoundingSphere[] { new BoundingSphere(transform.position, cullRadius) });
            cullGroup.SetBoundingSphereCount(1);
            cullGroup.onStateChanged += OnStateChanged;
        }

        void OnStateChanged(CullingGroupEvent cullEvent)
        {
            Debug.Log($"cullEvent {cullEvent.isVisible}");
            if (cullEvent.isVisible)
            {
                pSystem.Play(true);
            }
            else
            {
                pSystem.Pause();
            }
        }

        void Update()
        {
            if (PlayersIn > 0)
            {
                Timer += Time.deltaTime;
                if (Timer >= GameConfig.Inst.TimeToAreaContamination)
                {
                    // CHECK IF IS A VR PLAYER OR A KMS PLAYER
                    DataGame.Inst.UpdateNbAreaContainer(TeamCatching.GetComponent<IsScientistPlayer>() != null,
                      TeamCatch != null);
                    
                    // ANIMATION IN FUNCTION OF TEAM PLAYER
                    if (TeamCatching.GetComponent<IsScientistPlayer>() != null) 
                    {
                        photonView.RPC("BelongsToScientists", RpcTarget.All);
                    } 
                    else if (TeamCatching.tag == "VRPlayer")
                    {
                        photonView.RPC("BelongsToVirus", RpcTarget.All);
                    }

                    TeamCatch = TeamCatching;
                    PlayersIn = 0;
                    Timer = 0;
                }
            }
        }

        private void ColorParticle(ParticleSystem pSys, Color mainColor, Color accentColor)
        {
            var main = pSys.main;
            main.startColor = new ParticleSystem.MinMaxGradient(mainColor, accentColor);
        }

        [PunRPC]
        void BelongsToNobody()
        {
            ColorParticle(pSystem, nobody.mainColor, nobody.secondColor);
        }

        [PunRPC]
        void BelongsToVirus()
        {
            ColorParticle(pSystem, virus.mainColor, virus.secondColor);
        }

        [PunRPC]
        void BelongsToScientists()
        {
            ColorParticle(pSystem, scientist.mainColor, scientist.secondColor);
        }

        void OnDestroy()
        {
            if (cullGroup != null)
                cullGroup.Dispose();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, cullRadius);
        }

        private void OnTriggerEnter(Collider coll)
        {
            if (coll.gameObject.tag == "VRPlayer" || coll.gameObject.GetComponent<IsScientistPlayer>() != null) {
                if (TeamCatch == null
                    || (TeamCatch.GetComponent<IsScientistPlayer>() != null && coll.gameObject.tag == "VRPlayer")
                    || (TeamCatch.tag == "VRPlayer" && coll.gameObject.GetComponent<IsScientistPlayer>() != null)) {
                    TeamCatching = coll.gameObject;
                    PlayersIn += 1;
                }
            }
        }

        void OnTriggerExit(Collider coll)
        {
            if (coll.gameObject.tag == "VRPlayer" || coll.gameObject.GetComponent<IsScientistPlayer>() != null) {
                if (TeamCatch == null
                    || (TeamCatch.GetComponent<IsScientistPlayer>() != null && coll.gameObject.tag == "VRPlayer")
                    || (TeamCatch.tag == "VRPlayer" && coll.gameObject.GetComponent<IsScientistPlayer>() != null)) {
                    PlayersIn--;
                }
            }

            if (PlayersIn == 0) { Timer = 0; }
        }
    }
}