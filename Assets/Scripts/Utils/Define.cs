using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Define
{
	public const int KILL_SCORE = 1;
	public const int DANCE_SCORE = 2;
	
	public enum UIEvent
	{
		Click,
		Pressed,
		PointerDown,
		PointerUp,
	}

	public enum Scene
	{
		Unknown,
		Lobby,
		Game,
		AR,
	}

	public enum Sound
	{
		Bgm,
		Effect,
		Speech,
		Effect2,
		Effect3,
		Max,
	}

	public enum Role
	{
		Cloaker,
		Observer,
		Ray,
		Defender,
		Mirror,
		Max
	}

	public enum ContentType
	{
		Animation,
		Puzzle,
		Room,
		DragAndDrop
	}
}
