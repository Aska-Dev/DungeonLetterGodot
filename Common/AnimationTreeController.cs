using Godot;

public class AnimationTreeController
{
    public AnimationTree AnimTree { get; private set; }

    public AnimationTreeController(AnimationTree animTree)
    {
        AnimTree = animTree;
    }

    public void OneShot(string nodeName)
    {
        AnimTree.Set($"parameters/{nodeName}/request", (int)AnimationNodeOneShot.OneShotRequest.Fire);
    }

    public void CancelOneShot(string nodeName)
    {
        AnimTree.Set($"parameters/{nodeName}/request", (int)AnimationNodeOneShot.OneShotRequest.Abort);
    }

    public void SetState(string state)
    {
        AnimTree.Set("parameters/state/transition_request", state);
    }

    public bool Status(string nodeName)
    {
        var result = (bool)AnimTree.Get($"parameters/{nodeName}/active");
        return result;
    }
}