using UnityEngine;
using UnityEngine.UI;

namespace SoundBoard
{
	public class ProgressBar : MonoBehaviour
	{
		private const string _percents = "{0}%";

		[SerializeField] private Image _imageBackground;
		[SerializeField] private Image _imageForeground;
		[SerializeField] private Text _text;
		
		private float _maxWidth;
		private RectTransform _fgRectTransform;

		public void SetProgress(float progress)
		{
			_fgRectTransform.sizeDelta = new Vector2(_maxWidth * progress, _fgRectTransform.sizeDelta.y);
			_text.text = string.Format(_percents, Mathf.RoundToInt(progress * 100f));
		}

		private void Awake()
		{
			_maxWidth = (_imageBackground.transform as RectTransform).sizeDelta.x;
			_fgRectTransform = _imageForeground.transform as RectTransform;
		}
	}
}
