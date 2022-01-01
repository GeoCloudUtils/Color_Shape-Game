using System;
using UnityEngine.Events;
using UnityEngine;

namespace ScriptUtils.GameUtils
{
    /// <summary>
    /// Interface to be implemented by all LoadingScreen codes, one and exactly one implementation of this interface must be attached to the loading screen passed to Navigator in order for it to work!
    /// </summary>
    public interface ILoadingScreen
    {
        /// <summary>
        /// Event dispathed when a fade operation is complete (either fade in or fade out)
        /// </summary>
        event UnityAction<ILoadingScreen,int> FadeDone;
        /// <summary>
        /// Start fading in
        /// </summary>
        void startFadeIn();
        /// <summary>
        /// Start fade out when op is completre
        /// </summary>
        /// <param name="op"></param>
        void startFadeOut(AsyncOperation op);
        /// <summary>
        /// gets the duration of the fade
        /// </summary>
        /// <returns></returns>
        float getDuration();
        /// <summary>
        /// Force the loadign screen to either be completly faded in(true) or out(false)
        /// </summary>
        /// <param name="visible"></param>
        void forceVisible(bool visible);
    }
}