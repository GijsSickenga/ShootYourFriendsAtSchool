using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunUITest : MonoBehaviour
{
	public List<GunUIParent> gunUIParents = new List<GunUIParent>();

	private void Start()
	{
		foreach (GunUIParent parent in gunUIParents)
		{
			foreach (GunUIComponent component in parent.components.Values)
			{
				component.SetWeight(Random.Range(0, component.GetWeaponVariableType().maxedWeight));
			}
		}
	}
}
