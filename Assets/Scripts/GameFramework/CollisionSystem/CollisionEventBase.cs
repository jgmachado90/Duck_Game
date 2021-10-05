using System;
using System.Collections.Generic;
using CoreInterfaces;
using UnityEngine;

namespace GameFramework
{
	public abstract class CollisionEventBase : MonoBehaviour
	{
		public CollisionPreset preset;
		public CollisionSettings collisionSettings;
		public MonoBehaviour[] actions;
		private List<IActionMonoBehaviour> _actionMonoBehaviour;

		protected List<IActionMonoBehaviour> ActionMonoBehaviour {
			get
			{
				if(_actionMonoBehaviour == null)
					_actionMonoBehaviour = actions.GetActionsOfType<IActionMonoBehaviour>();
				return _actionMonoBehaviour;
			}
		}
		
		public event Action<GameObject> OnCollisionEnter;
		
		private bool _presetUsed;

		public virtual void Start() {
			if (!_presetUsed && preset)
				SetCollisionPreset(preset);
		}

		public void SetCollisionPreset(CollisionPreset preset) {
			_presetUsed = true;
			this.preset = preset;
			collisionSettings = preset.collisionSettings;
		}

		protected virtual bool Collision(GameObject col)
		{
			if(col == null || !collisionSettings.CheckCollisionFilters(col,null)) return false;
			InvokeEvents(col);
			return true;
		}

		public virtual void InvokeEvents(GameObject obj)
		{
			foreach (var t in ActionMonoBehaviour) {
				t.InvokeAction();
			}
			OnCollisionEnter?.Invoke(obj);
		}
	}
}
