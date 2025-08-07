using CrowdControl.Common;
using JetBrains.Annotations;

namespace CrowdControl.Games.Packs.HoyleBook1;

[UsedImplicitly]
public class HoyleBook1 : RetroPCEffectPack
{
    public HoyleBook1(UserRecord player, Func<CrowdControlBlock, bool> responseHandler, Action<object> statusUpdateHandler) : base(player, responseHandler, statusUpdateHandler) { }
    
    public override Game Game { get; } = new("Hoyle's Official Book of Games Vol. 1", "HoyleBook1", "Retro", ConnectorType.RetroPCConnector);

    public override EffectList Effects
    {
        get
        {
            List<Effect> effects =
            [
                new("Test", "test"),
            ];
            return effects;
        }
    }
    
    protected override GameState GetGameState()
    {
        //if (!Connector.Read8(ADDR_INGAME, out byte g) || !Connector.Read8(ADDR_CONTROL, out byte c)) return GameState.Unknown;
        //if (g != 2) return GameState.WrongMode;
        //if (c is not (1 or 2)) return GameState.Cutscene;
        return GameState.Ready;
    }

    protected override void StartEffect(EffectRequest request)
    {
        string[] codeParams = FinalCode(request).Split('_');
        switch (codeParams[0])
        {
            case "test":
                {
                    RepeatAction(request,
                        () => true,
                        () =>
                        {
                            Span<byte> test = stackalloc byte[32];
                            Connector.Read(0, test);
                            //Log.Message(test.ToHexString());
                            return true;
                        },
                        TimeSpan.FromSeconds(5),
                        () => IsReady(request),
                        TimeSpan.FromSeconds(5),
                        () => true,
                        TimeSpan.FromSeconds(1), true);

                    return;
                }
            default:
                Respond(request, EffectStatus.FailPermanent, StandardErrors.UnknownEffect, request);
                return;
        }
    }


    public override bool StopAllEffects()
    {
        bool success = base.StopAllEffects();
        try
        {
            //success &= Connector.Write32(ADDR_LUIGI_PTR, 0);
        }
        catch { success = false; }

        return success;
    }

    protected override bool StopEffect(EffectRequest request)
    {
        switch (FinalCode(request))
        {
            case "ohko":
                {
                    //if (!Connector.Read32(ADDR_LUIGI_PTR, out uint ptr)) return false;
                    //if (!Connector.Write16(ptr + ADDR_HEALTH, oldhealth)) return false;
                    //oldhealth = 0;
                    break;
                }
                break;
        }
        return base.StopEffect(request);
    }
}