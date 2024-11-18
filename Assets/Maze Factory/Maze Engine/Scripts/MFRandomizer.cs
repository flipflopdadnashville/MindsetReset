using UnityEngine;
using System.Collections;

/// <summary>
/// Wrapper for a randomizer. Currently using the System randomizer.
/// But you can easily replace this by unity's or roll your own.
/// </summary>
public class MFRandomizer {
	private System.Random random;
	
	/// <summary>
	/// Initializes a new instance of the <see cref="MFRandomizer"/> class. 
	/// </summary>
	public MFRandomizer() : this(null){}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="MFRandomizer"/> class.
	/// </summary>
	/// <param name='seed'>
	/// Seed.
	/// </param>
	public MFRandomizer(int? seed)
	{
		if (seed == null)
			random = new System.Random();
		else 
		{
			random = new System.Random((int)seed);			
		}
	}
	
	/// <summary>
	/// Get a pseudo random value between 0 and maxValue.
	/// </summary>
	/// <param name='maxValue'>
	/// Max value.
	/// </param>
	public int Next(int maxValue)
	{
		return random.Next(maxValue);		
	}
}
