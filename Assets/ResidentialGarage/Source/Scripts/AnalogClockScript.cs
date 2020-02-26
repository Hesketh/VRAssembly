using System;
using UnityEngine;

public class AnalogClockScript : MonoBehaviour {
	#region Private Constants

	private const float MAX_DEGREE_ANGLE = 360.0f;
	private const float SECONDS_PER_MINUTE = 60.0f;
	private const float MINUTES_PER_HOUR = 60.0f;
	private const float HOURS_PER_WHOLEDAY = 24.0f;
	private const float HOURS_PER_HALFDAY = 12.0f;
	private const float NORMAL_TIMERATE = 1.0f;
	private const float VECTOR_NOCHANGE = 0.0f;

	#endregion

	#region Public Fields

	/// <summary>
	/// Second Hand Game Object (Rotation will occur on z-axis)
	/// </summary>
	public GameObject secondHand;

	/// <summary>
	/// Minute Hand Game Object (Rotation will occur on z-axis)
	/// </summary>
	public GameObject minuteHand;

	/// <summary>
	/// Hour Hand Game Object (Rotation will occur on z-axis)
	/// </summary>
	public GameObject hourHand;

	/// <summary>
	/// Current Hour (Always 24 hour value, regardless)
	/// </summary>
	[Range(0, 23)]
	public int currentHour = 0;

	/// <summary>
	/// Current Minutes
	/// </summary>
	[Range(0, 59)]
	public int currentMinutes = 0;

	/// <summary>
	/// Current Seconds
	/// </summary>
	[Range(0, 59)]
	public int currentSeconds = 0;

	/// <summary>
	/// Current Time (Format based on use24HourFormat value)
	/// </summary>
	public string currentTime = "";

	/// <summary>
	/// Default value of 1.0 (Normal)
	/// </summary>
	[Range(0.1f, 4.0f)]
	public float currentTimeRate = NORMAL_TIMERATE;

	/// <summary>
	/// Display Format
	/// </summary>
	public bool use24HourFormat = false;

	#endregion

	#region Private Fields

	/// <summary>
	/// 
	/// </summary>
	private float updateTimeElapsed = 0.0f;

	#endregion

	#region Public Methods

	/// <summary>
	/// Returns thecurrent clock time, based on use24HourFormat;
	/// </summary>
	/// <returns></returns>
	public override string ToString() {
		var hours = currentHour.ToString().PadLeft(2, '0');
		if (!use24HourFormat && currentHour > 12)
			hours = (currentHour - 12).ToString().PadLeft(2, '0');

		var minutes = currentMinutes.ToString().PadLeft(2, '0');
		var seconds = currentSeconds.ToString().PadLeft(2, '0');

		return string.Format("{0}:{1}:{2}{3}",
			hours.Substring(hours.Length - 2, 2),
			minutes.Substring(minutes.Length - 2, 2),
			seconds.Substring(seconds.Length - 2, 2),
			(use24HourFormat) 
			? "" 
			: (currentHour > 12) 
			? " PM" 
			: " AM");
	}

	#endregion

	#region Private Unity Methods

	/// <summary>
	/// 
	/// </summary>
	void Start() {
		if (!secondHand)
			throw new UnityException("The Second Hand GameObject cannot be null");

		if (!minuteHand)
			throw new UnityException("The Minute Hand GameObject cannot be null");

		if (!hourHand)
			throw new UnityException("The Hour Hand GameObject cannot be null");

		var startTime = DateTime.Now;
		if (currentHour == 0 && currentMinutes == 0 && currentSeconds == 0) {
			currentHour = startTime.Hour;
			currentMinutes = startTime.Minute;
			currentSeconds = startTime.Second;
		}
	}

	/// <summary>
	/// 
	/// </summary>
	void Update() {    
		this.currentTime = this.ToString ();

		// Retrieve the amount of time elapsed, and mulitply it by the current time rate modifier
		updateTimeElapsed += (Time.deltaTime * currentTimeRate);
		if (updateTimeElapsed < NORMAL_TIMERATE) // If less than 1 second elsapsed, just return;
			return;

		updateTimeElapsed = 0.0f;
		currentSeconds += 1;    
		secondHand.transform.localEulerAngles = new Vector3 (
			VECTOR_NOCHANGE, 
			VECTOR_NOCHANGE, 
			(MAX_DEGREE_ANGLE / SECONDS_PER_MINUTE) * currentSeconds);

		if (currentSeconds >= SECONDS_PER_MINUTE) {
			currentSeconds = 0;
			currentMinutes += 1;
		}

		minuteHand.transform.localEulerAngles = new Vector3 (
			VECTOR_NOCHANGE,
			VECTOR_NOCHANGE,
			(MAX_DEGREE_ANGLE / MINUTES_PER_HOUR) * currentMinutes);      
	

		if (currentMinutes >= MINUTES_PER_HOUR) {
			currentMinutes = 0;
			currentHour += 1;
		}

		hourHand.transform.localEulerAngles = new Vector3(
			VECTOR_NOCHANGE, 
			VECTOR_NOCHANGE, 
			(MAX_DEGREE_ANGLE / HOURS_PER_HALFDAY) * currentHour);

		if (currentHour >= HOURS_PER_WHOLEDAY)
			currentHour = 0;
	}

	#endregion
}