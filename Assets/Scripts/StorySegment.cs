using UnityEngine;
using System.Collections;

[System.Serializable]
public class StorySegment {
	
	public enum ElementType { Story, Character, Thing, Location }
	public enum WordClass { singularNondeterminate, singularDeterminate, pluralNondeterminate, pluralDeterminate }
	public ElementType type;
	public WordClass wordClass;
	public int elementID;
	public string title;
}