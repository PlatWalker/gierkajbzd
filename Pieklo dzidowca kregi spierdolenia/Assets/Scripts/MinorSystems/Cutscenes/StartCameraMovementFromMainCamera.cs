using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Zenject;

namespace jbzd.MinorSystems.Cutscenes
{
    [RequireComponent(typeof(PlayableDirector))]
    public class StartCameraMovementFromMainCamera: MonoBehaviour
    {
        [SerializeField]
        [TextArea(1,6)]
        private string comment = "Do dzialania tego skryptu nalezy pamietac, ze kamera glowna oraz kamera ktora bedzie uzyta" +
                                 "w cutscenie musza miec wartosci rotacji pomiedzy 0 a 360. W innym przypadku bedzie wam kamera robila obroty z dupy." +
                                 "Dodatkowo nie dodawaj keyframe'a w pierwszej klatce animacji";
        
        private CutscenesManager _cutscenesManager;
        private Camera _cutsceneCamera;
        private Camera _mainCamera;
        private PlayableDirector _playableDirector;
        private AnimationClip _animationClip;
        private const float TIME_AT_WHICH_TO_ADD_KEYFRAME = 0;

        [Inject]
        public void Constructor(CutscenesManager cutscenesManager)
        {
            _cutscenesManager = cutscenesManager;
        }

        private void Awake()
        {
            _cutscenesManager.OnCutscenePlayDemand += CutscenePlayDemand;
            _cutscenesManager.OnCutsceneEnded += CutsceneEnded;
            
            _playableDirector = GetComponent<PlayableDirector>();
            _mainCamera = Camera.main;
            
            Debug.Assert(_mainCamera, "Missing main camera object");
            
            _cutsceneCamera = GetComponentInChildren<Camera>();
            if (_cutsceneCamera is null)
            {
                for (var childIndex = 0; childIndex < transform.childCount; childIndex++)
                {
                    var child= transform.GetChild(childIndex);
                    if (child.TryGetComponent(typeof(Camera), out var component))
                    {
                        _cutsceneCamera = component as Camera;
                    }
                }
            }
            Debug.Assert(_cutsceneCamera, $"Missing {nameof(Camera)} component on {gameObject.name}");
        }

        private void CutsceneEnded(TimelineAsset timelineAsset)
        {
            if(timelineAsset != _playableDirector.playableAsset) return;
            DeleteAllKeyframesAtTime(_animationClip, TIME_AT_WHICH_TO_ADD_KEYFRAME);
        }

        private void CutscenePlayDemand(TimelineAsset timelineAsset)
        {
            if(timelineAsset != _playableDirector.playableAsset) return;
            
            foreach (var track in timelineAsset.GetOutputTracks())
            {
                if (track is not AnimationTrack animationTrack) continue;

                if (_playableDirector.GetGenericBinding(track) is not Animator animatorOfAnimatedObject)
                {
                    Debug.LogError($"Did not found animation game object on animation track of timeline asset: {timelineAsset.name}");
                    return;
                }
                
                if (animatorOfAnimatedObject.gameObject != _cutsceneCamera.gameObject) continue;
                if (!animationTrack.hasClips)
                {
                    Debug.LogError($"Animation track of {_cutsceneCamera.gameObject} in timeline asset {timelineAsset.name} has no track clips. You need at lease one!");
                    return;
                }
                foreach (var timelineClip in animationTrack.GetClips())
                {
                    if (_animationClip is not null) break;
                    _animationClip = timelineClip.animationClip;
                }
            }

            var parentLocalCoordinatesOfCutsceneCamera = _cutsceneCamera.transform.root.gameObject.transform.InverseTransformPoint(_mainCamera.transform.position);
            var startPosition = parentLocalCoordinatesOfCutsceneCamera - _cutsceneCamera.transform.position;
            AddPositionKeyframe(_animationClip, TIME_AT_WHICH_TO_ADD_KEYFRAME, startPosition);

            var startRotation = _mainCamera.transform.rotation;
            AddRotationKeyframe(_animationClip, TIME_AT_WHICH_TO_ADD_KEYFRAME, startRotation);
        }
        
        private void DeleteAllKeyframesAtTime(AnimationClip clip, float time)
        {
            var properties = new[]
            {
                "m_LocalPosition.x", "m_LocalPosition.y", "m_LocalPosition.z",
                "localEulerAnglesRaw.x", "localEulerAnglesRaw.y", "localEulerAnglesRaw.z",
                "m_LocalScale.x", "m_LocalScale.y", "m_LocalScale.z"
            };

            foreach (var property in properties)
            {
                var curve = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), property));

                if (curve == null) continue;
                
                var keyIndex = -1;
                for (var i = 0; i < curve.keys.Length; i++)
                {
                    if (!Mathf.Approximately(curve.keys[i].time, time)) continue;
                    
                    keyIndex = i;
                    break;
                }

                if (keyIndex == -1) continue;
                curve.RemoveKey(keyIndex);

                AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), property), curve);
            }
        }
        
        private void AddRotationKeyframe(AnimationClip clip, float time, Quaternion rotation)
        {
            var eulerRotation = rotation.eulerAngles;

            var curveX = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "localEulerAnglesRaw.x"));
            var curveY = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "localEulerAnglesRaw.y"));
            var curveZ = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "localEulerAnglesRaw.z"));

            if (curveX == null)
            {
                curveX = new AnimationCurve();
                curveY = new AnimationCurve();
                curveZ = new AnimationCurve();
            }
            
            curveX.AddKey(new Keyframe(time, eulerRotation.x));
            curveY.AddKey(new Keyframe(time, eulerRotation.y));
            curveZ.AddKey(new Keyframe(time, eulerRotation.z));
            
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "localEulerAnglesRaw.x"), curveX);
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "localEulerAnglesRaw.y"), curveY);
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "localEulerAnglesRaw.z"), curveZ);
        }
        
        private void AddPositionKeyframe(AnimationClip clip, float time, Vector3 position)
        {
            var curveX = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.x"));
            var curveY = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.y"));
            var curveZ = AnimationUtility.GetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.z"));

            if (curveX == null)
            {
                curveX = new AnimationCurve();
                curveY = new AnimationCurve();
                curveZ = new AnimationCurve();
            }
            
            curveX.AddKey(new Keyframe(time, position.x));
            curveY.AddKey(new Keyframe(time, position.y));
            curveZ.AddKey(new Keyframe(time, position.z));
            
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.x"), curveX);
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.y"), curveY);
            AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve("", typeof(Transform), "m_LocalPosition.z"), curveZ);
        }
        
        private void OnDestroy()
        {
            _cutscenesManager.OnCutscenePlayDemand -= CutscenePlayDemand;
            _cutscenesManager.OnCutsceneEnded -= CutsceneEnded;
        }
    }
}