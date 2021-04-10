using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milestones;
public class DummyMilestone : MilestoneBase
{
    public override bool CheckCompleteMilestone()
    {
        return false;
    }
}
