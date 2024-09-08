using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventList", menuName = "Purrcifer/EventList")]
public class PurrciferEventListSO : ScriptableObject
{
    public List<PurrciferEventData> events;
}
