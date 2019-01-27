using FMOD;
using FMOD.Studio;
using JetBrains.Annotations;
using UnityEngine;

public class SoundManager: MonoBehaviour
{
    static SoundManager _instance = null;
    public static SoundManager Instance => (SoundManager)_instance;

    protected void Awake() {
        _instance = this;
    }

    // Used to play single sound clips.
    public EventInstance PlaySingle(string eventName, Vector2 position,
        bool randomPitch = false, GameObject gameObject = null, bool doRelease = true) {
        if(string.IsNullOrEmpty(eventName))
            return new EventInstance();
        var instance = FMODUnity.RuntimeManager.CreateInstance(eventName);
        if(randomPitch) {
            instance.setPitch(Random.Range(0.75f, 1.25f));
        }

        instance.set3DAttributes(gameObject != null
            ? FMODUnity.RuntimeUtils.To3DAttributes(gameObject)
            : FMODUnity.RuntimeUtils.To3DAttributes(position));
        instance.start();
        if(doRelease)
            instance.release();
        return instance;
    }
}