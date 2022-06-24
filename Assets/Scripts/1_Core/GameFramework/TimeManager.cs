using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Serialization;

namespace GameFramework
{

	public class TimeManager : MonoBehaviour
	{
		public PauseEntitiesRegister pauseEntities;
		private bool _pause;
		private float _timeScale = 1;
		private float _scaleFactor = 1;
		private AudioMixerSnapshot _returnSnapshot;
		private List<Coroutine> _stopCoroutines = new List<Coroutine>();
		private float _defaultFixedDeltaTime = 0.02f;

		private float TimeScale
		{
			get => _timeScale;
			set
			{
				_timeScale = value;
				UpdateTimeScale();
			}
		}

		private float ScaleFactor
		{
			set
			{
				_scaleFactor = value;
				UpdateTimeScale();
			}
		}

		private float PauseFloat => _pause ? 0 : 1;

		private bool PauseBool
		{
			get => _pause;
			set
			{
				Physics.autoSyncTransforms = value;
				_pause = value;
				AudioListener.pause = _pause;
				UpdateTimeScale();
			}
		}

		public void RegisterPause(Entity entity)
		{
			pauseEntities.RegisterPause(entity);
		}

		public void UnregisterPause(Entity entity)
		{
			pauseEntities.UnregisterPause(entity);
		}

		private void Awake()
		{
			_defaultFixedDeltaTime = Time.fixedDeltaTime;
			ResetTimeScale();
		}

		private void OnDestroy()
		{
			if (_returnSnapshot) _returnSnapshot.TransitionTo(0);
			StopAllCoroutines();
			ResetTimeScale();
			UpdateFixedDeltaTime();
			AudioListener.pause = false;
		}

		private void ResetTimeScale()
		{
			UnPause();
			ScaleFactor = 1;
			TimeScale = 1;
		}

		private void UpdateTimeScale() => Time.timeScale = _timeScale * _scaleFactor * PauseFloat;

		private void UpdateFixedDeltaTime()
		{
			if (Time.timeScale <= 0) return;
			Time.fixedDeltaTime = Time.timeScale * _defaultFixedDeltaTime;
		}

		private IEnumerator SlowMotion(float slowFactor, float slowLength, float timeIni)
		{
			if (timeIni > 0)
				yield return StartCoroutine(WaitForSecondsRealtimeWithPause(timeIni));
			TimeScale = slowFactor;
			while (TimeScale < 1)
			{
				TimeScale = Mathf.MoveTowards(TimeScale, 1, (1f / slowLength) * Time.unscaledDeltaTime * PauseFloat);
				TimeScale = Mathf.Clamp(TimeScale, 0, 1);
				UpdateFixedDeltaTime();
				yield return null;
			}
		}

		private IEnumerator SlowMotion(AnimationCurve curve)
		{
			TimeScale = curve.Evaluate(0);
			float progress = 0;
			while (progress < curve.keys[curve.length - 1].time)
			{
				TimeScale = curve.Evaluate(progress);
				TimeScale = Mathf.Clamp(TimeScale, 0, 1);
				UpdateFixedDeltaTime();
				progress += Time.unscaledDeltaTime * PauseFloat;
				yield return null;
			}
			TimeScale = 1;
			UpdateFixedDeltaTime();
		}

		private IEnumerator FramePause(float timeIni, float timePause, float newScale = 0)
		{
			yield return StartCoroutine(WaitForSecondsRealtimeWithPause(timeIni));
			ScaleFactor = newScale;
			yield return new WaitWhile(() => Time.timeScale == 0);
			UpdateFixedDeltaTime();

			yield return StartCoroutine(WaitForSecondsRealtimeWithPause(timePause));
			ScaleFactor = 1;
			yield return new WaitWhile(() => Time.timeScale == 0);
			UpdateFixedDeltaTime();
		}

		private IEnumerator WaitForSecondsRealtimeWithPause(float time)
		{
			float t = 0;
			while (t < time)
			{
				t += Time.unscaledDeltaTime * PauseFloat;
				yield return null;
			}
		}

		public bool IsPaused()
		{
			return PauseBool;
		}

		public void Pause()
		{
			PauseBool = true;
			Time.fixedDeltaTime = _defaultFixedDeltaTime;
			pauseEntities.Pause();
		}

		public void UnPause()
		{
			PauseBool = false;
			Time.fixedDeltaTime = _defaultFixedDeltaTime;
			pauseEntities.Unpause();
		}

		public void TimeManagerStopCoroutine(Coroutine coroutine, bool returnTimeScale = false)
		{
			StopCoroutine(coroutine);
			if (returnTimeScale)
			{
				TimeScale = 1;
				UpdateFixedDeltaTime();
			}
		}

		public void DoSlowMotion(float slowFactor = 0.5f, float slowLength = 0.5f, float timeIni = 0)
		{
			if (slowLength <= 0) return;
			StartCoroutine(SlowMotion(slowFactor, slowLength, timeIni));
		}

		public Coroutine DoSlowMotion(AnimationCurve curve)
		{
			Coroutine coroutine = StartCoroutine(SlowMotion(curve));
			_stopCoroutines.Add(coroutine);
			return coroutine;
		}

		public void DoFramePause(float timeIni = 0.03f, float timePause = 0.08f, float newScale = 0)
		{
			StartCoroutine(FramePause(timeIni, timePause, newScale));
		}
	}
}